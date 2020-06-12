using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace DeadMosquito.IosGoodies.Example
{
	public class IGHardwareExample : MonoBehaviour
	{
#pragma warning disable
		[SerializeField] Slider _lightIntensitySlider;
#pragma warning restore 

		void Awake()
		{
#if UNITY_IOS
			_lightIntensitySlider.onValueChanged.AddListener(val =>
			{
				if (!IGFlashlight.HasTorch)
				{
					return;
				}

				IGFlashlight.SetFlashlightIntensity(val);
			});
#endif
		}

#if UNITY_IOS
		bool _torchLightEnabled;

		[UsedImplicitly]
		public void OnEnableFlashlight()
		{
			if (IGFlashlight.HasTorch)
			{
				_torchLightEnabled = !_torchLightEnabled;
				IGFlashlight.EnableFlashlight(_torchLightEnabled);
			}
			else
			{
				Debug.Log("This device does not have a flashlight");
			}
		}

		[UsedImplicitly]
		public void OnNotificationError()
		{
			IGHapticFeedback.NotificationOccurred(IGHapticFeedback.NotificationType.Error);
		}

		[UsedImplicitly]
		public void OnNotificationWarning()
		{
			IGHapticFeedback.NotificationOccurred(IGHapticFeedback.NotificationType.Warning);
		}

		[UsedImplicitly]
		public void OnNotificationSuccess()
		{
			IGHapticFeedback.NotificationOccurred(IGHapticFeedback.NotificationType.Success);
		}

		[UsedImplicitly]
		public void OnSelected()
		{
			IGHapticFeedback.SelectionChanged();
		}

		[UsedImplicitly]
		public void OnImpactLight()
		{
			IGHapticFeedback.ImpactOccurred(IGHapticFeedback.ImpactType.Light);
		}

		[UsedImplicitly]
		public void OnImpactMedium()
		{
			IGHapticFeedback.ImpactOccurred(IGHapticFeedback.ImpactType.Medium);
		}

		[UsedImplicitly]
		public void OnImpactHeavy()
		{
			IGHapticFeedback.ImpactOccurred(IGHapticFeedback.ImpactType.Heavy);
		}

		[UsedImplicitly]
		public void OnBiometricAuthentication()
		{
#if !DISABLE_BIOMETRIC_AUTH
			if (IGLocalAuthentication.IsLocalAuthenticationAvailable)
			{
				const IGLocalAuthentication.Policy policy = IGLocalAuthentication.Policy.DeviceOwnerAuthenticationWithBiometrics;
				IGLocalAuthentication.AuthenticateWithBiometrics("Please, confirm your identity", policy,
					() => { Debug.Log("Authentication was successful"); }, 
					error => Debug.Log("Authentication failed: " + error));
			}
			else
			{
				Debug.Log("Device does not support biometric authentication.");
			}
#else
			ExampleUtil.LogFeatureDisabled();
#endif
		}
#endif
	}
}