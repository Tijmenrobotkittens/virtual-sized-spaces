using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRotaterController : MonoBehaviour
{
    private GameObject _user;
    private GameObject _stage;
    public float _threshold = 4;
    private float _angle = 0;
    public float _factor = 500;
    private float _totalAngleManipulated = 0;
    
    public void set(GameObject user, GameObject stage) {
        _user = user;
        _stage = stage;

    }

    public void setRotation(Quaternion rotate, Quaternion lastrotate) {
        Vector3 movement = rotate.eulerAngles - lastrotate.eulerAngles;
        float totalmovement = Mathf.Abs(movement.x) + Mathf.Abs(movement.y) + Mathf.Abs(movement.z);
        float move = 0f;
        if (totalmovement > _threshold) {
            move = totalmovement - _threshold;
            float moveangle = move / _factor;
            UpdateAngle(moveangle);
            _totalAngleManipulated += moveangle;
            Debug.Log("Manipulated angle "+_totalAngleManipulated);

        }
    }

    private void UpdateAngle(float angle)
    {
        
        _stage.transform.RotateAround(_user.transform.position, Vector3.up, angle);
    }

    private void Update()
    {
       
    }
}