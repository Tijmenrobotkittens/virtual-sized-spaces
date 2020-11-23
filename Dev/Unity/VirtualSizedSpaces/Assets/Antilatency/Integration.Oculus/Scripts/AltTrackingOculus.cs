// Copyright 2020, ALT LLC. All Rights Reserved.
// This file is part of Antilatency SDK.
// It is subject to the license terms in the LICENSE file found in the top-level directory
// of this distribution and at http://www.antilatency.com/eula
// You may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.XR;

using Antilatency.Integration;
using Antilatency.DeviceNetwork;

/// <summary>
/// %Antilatency %Alt tracking components and scripts specific for %Oculus HMD devices.
/// </summary>
namespace Antilatency.IntegrationOculus {
    /// <summary>
    /// %Antilatency %Alt tracking implementation for %Oculus headsets.
    /// </summary>
    public class AltTrackingOculus : AltTracking {
        private enum OculusDeviceType {
            Undefined,
            GearVR,
            Rift,
            Go,
            Quest,
            RiftS
        }

        /// <summary>
        /// Link to OVRCameraRig component.
        /// </summary>
        protected OVRCameraRig _rig;

        /// <summary>
        /// Controls how quickly %Alt rotation data will be applied.
        /// </summary>
        protected float _k = 20.0f;

        private bool _checkDeviceRequiered = false;
        private OculusDeviceType _deviceType = OculusDeviceType.Undefined;

        /// <summary>
        /// Get node (ALT tracker device) to start tracking task.
        /// </summary>
        /// <returns>First idle ALT tracker node connected via USB.</returns>
        protected override NodeHandle GetAvailableTrackingNode() {
            return GetUsbConnectedFirstIdleTrackerNode();
        }

        protected override void OnEnable() {
            base.OnEnable();

            _checkDeviceRequiered = true;

            _rig = GetComponent<OVRCameraRig>();

            if (_rig != null) {
                _rig.UpdatedAnchors += OnUpdatedAnchors;
            } else {
                Debug.LogError("OVRCameraRig not found");
            }

            if (OVRManager.boundary != null) {
                OVRManager.boundary.SetVisible(false);
            }
        }

        protected override void OnDisable() {
            base.OnDisable();

            if (_deviceType == OculusDeviceType.Rift) {
#if UNITY_2017_1_OR_NEWER
                Application.onBeforeRender -= OnBeforeRender;
#else
                Camera.onPreCull -= OnCameraPreCull;
#endif
            }

            _rig.UpdatedAnchors -= OnUpdatedAnchors;

            //OVRManager.boundary.SetVisible(true);
        }

        private OculusDeviceType GetDeviceType() {
            var result = OculusDeviceType.Undefined;
            var curDevice = OVRPlugin.GetSystemHeadsetType().ToString().ToLower();

            if (curDevice.Contains("gearvr")) {
                result = OculusDeviceType.GearVR;
            } else if (curDevice.Contains("rift")) {
                result = curDevice == "rift_s" ? OculusDeviceType.RiftS : OculusDeviceType.Rift;
            } else if (curDevice.Contains("oculus_go")) {
                result = OculusDeviceType.Go;
            } else if (curDevice.Contains("quest")) {
                result = OculusDeviceType.Quest;
            }

            return result;
        }

#if UNITY_2017_1_OR_NEWER
        private void OnBeforeRender() {
#else
        private void OnCameraPreCull(Camera cam) {
#endif
            if (_rig == null) {
                Debug.LogWarning("OVRCameraRig not found");
                return;
            }
            //Override position, we don't need hardcoded user height
            _rig.leftEyeAnchor.localPosition = Vector3.zero;
            _rig.rightEyeAnchor.localPosition = Vector3.zero;
            _rig.centerEyeAnchor.localPosition = Vector3.zero;
        }

        protected override void Update() {
            base.Update();

            if (!_checkDeviceRequiered) {
                return;
            }

            _deviceType = GetDeviceType();

            if (_deviceType != OculusDeviceType.Undefined) {
                _checkDeviceRequiered = false;
                if (_deviceType == OculusDeviceType.Rift) {
#if UNITY_2017_1_OR_NEWER
                    Application.onBeforeRender += OnBeforeRender;
#else
                Camera.onPreCull += OnCameraPreCull;
#endif
                }
            }
        }

        /// <summary>
        /// Applies tracking data to CameraRig. We used %Oculus native rotation as base and then smoothly correct it with our tracking data 
        /// to avoid glitches that can be seen because %Oculus Asynchronous TimeWarp system uses only native rotation data provided by headset.
        /// </summary>
        /// <param name="rig">Pointer to OVRCameraRig</param>
        protected virtual void OnUpdatedAnchors(OVRCameraRig rig) {
            if (_rig != rig) {
                return;
            }

            Alt.Tracking.State trackingState;
            var trackingActive = GetTrackingState(out trackingState);

            if (trackingActive) {
                var oculusRotation = _rig.centerEyeAnchor.localRotation;

                var altEnvSpaceRotation = trackingState.pose.rotation;
                var pa = altEnvSpaceRotation * Quaternion.Inverse(oculusRotation);
                var diff = Quaternion.Inverse(_rig.trackingSpace.localRotation) * pa;
                diff = Quaternion.Lerp(Quaternion.identity, diff, 1.0f - Fx(Quaternion.Angle(Quaternion.identity, diff), _k));
                _rig.trackingSpace.localRotation = _rig.trackingSpace.localRotation * diff;

                if (_deviceType == OculusDeviceType.Quest || _deviceType == OculusDeviceType.RiftS) {
                    var oculusPosition = _rig.centerEyeAnchor.position;
                    var altEnvSpacePosition = transform.TransformPoint(trackingState.pose.position);

                    var altToOculus = altEnvSpacePosition - oculusPosition;
                    var posDiff = Vector3.Lerp(Vector3.zero, altToOculus, 1.0f - Fx(altToOculus.magnitude, 0.05f));

                    _rig.trackingSpace.position = _rig.trackingSpace.position + posDiff;
                } else {
                    _rig.trackingSpace.localPosition = trackingState.pose.position;
                }
            }
        }

        private float Fx(float x, float k) {
            var xDk = x / k;
            return 1.0f / (xDk * xDk + 1);
        }

        protected override Pose GetPlacement() {
            var result = Pose.identity;

            using (var localStorage = Integration.StorageClient.GetLocalStorage()) {

                if (localStorage == null) {
                    return result;
                }

                var placementCode = localStorage.read("placement", "default");

                if (string.IsNullOrEmpty(placementCode)) {
                    Debug.LogError("Failed to get placement code");
                } else {
                    result = _trackingLibrary.createPlacement(placementCode);
                }

                return result;
            }
        }
    }
}