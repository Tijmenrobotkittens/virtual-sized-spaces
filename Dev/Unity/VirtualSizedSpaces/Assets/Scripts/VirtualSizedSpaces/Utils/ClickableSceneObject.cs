using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    [System.Serializable]
    public class ClickableSceneObjectEvent : UnityEvent<ClickableSceneObject> { }

    public class ClickableSceneObject : MonoBehaviour
    {

        public Camera Camera;

        public float ScreenSpaceTollerance = 0.05f; // 1 is whole screen away from the object
        public bool CollidersBlockButtonPress = true;

        public UnityEvent<ClickableSceneObject> onClick = new ClickableSceneObjectEvent();
        public static UnityEvent<ClickableSceneObject> onClicked = new ClickableSceneObjectEvent();
        public bool DebugEnabled = false;

        void Start()
        {

            if (Camera == null) Camera = Camera.main;
        }

        void Update()
        {

            bool _pressDetected = false;
            Vector3 _pressPosition = Vector3.zero;

            if (Application.isMobilePlatform)
            {

                if (Input.touchCount > 0)
                {
                    _pressDetected = Input.GetTouch(0).phase == TouchPhase.Began;
                    _pressPosition = Input.GetTouch(0).position;
                }

            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _pressDetected = true;
                    _pressPosition = Input.mousePosition;
                }

            }

            if (Camera == null)
                Camera = Camera.main;

            if (Camera == null)
            {
                Debug.LogError("There is no camera assigned and Camera.main returns no camera!");
                enabled = false;
            }

            if (!_pressDetected)
                return;

            Vector3 _screenPosition = Camera.main.WorldToScreenPoint(transform.position);

            float _pxDistanceX = _screenPosition.x - _pressPosition.x;
            float _pxDistanceY = _screenPosition.y - _pressPosition.y;

            if (_pxDistanceX < 0) _pxDistanceX = -_pxDistanceX;
            if (_pxDistanceY < 0) _pxDistanceY = -_pxDistanceY;

            float _x = 1f / (Screen.width / _pxDistanceX);
            float _y = 1f / (Screen.height / _pxDistanceY);

            if (!TestForColliderHit(_pressPosition, _x, _y) && 
                _x < ScreenSpaceTollerance && _y < ScreenSpaceTollerance)
            {
                if (!CollidersBlockButtonPress)
                {

                    onClick?.Invoke(this);
                    onClicked?.Invoke(this);

                    return;

                }
            }

        }

        private bool TestForColliderHit(Vector3 pressPosition, float x, float y)
        {
            Ray _rayCast = Camera.ScreenPointToRay(pressPosition);

            if (Physics.Raycast(_rayCast, out var _rayCastHit))
            {
                if (_rayCastHit.collider.gameObject != gameObject)
                    return false;
            }
            else
            {
                return false;
            }
               

            onClick?.Invoke(this);
            onClicked?.Invoke(this);

            return true;
        }
    }
