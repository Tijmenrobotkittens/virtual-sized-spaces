﻿using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
  
 public class UserController2 : MonoBehaviour {
    public float speed = 1;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    private Vector3 _lastPosition;
    float rotationX = 0F;
    float rotationY = 0F;
    public float total = 0;
    Quaternion originalRotation;
    float height = 2.14f;
    private TestingResult _currentTest;
    private Quaternion _offset =  Quaternion.Euler(0,0,0);
    private Quaternion _lastRotation = Quaternion.Euler(0,0,0);
    private SceneRotaterController _rotator;

    public void setRotator(SceneRotaterController rotator) {
        _rotator = rotator;
    }

    private void Start()
    {
        originalRotation = transform.localRotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
         angle += 360F;
        if (angle > 360F)
         angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void setSettings(TestingResult testingResult) {
        Debug.Log("adding testresults" + testingResult);
        _currentTest = testingResult;
    }

    void Update()
    {

        if (axes == RotationAxes.MouseXAndY)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right) ;
            transform.localRotation = originalRotation * xQuaternion * yQuaternion * _offset;
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
            transform.localRotation = originalRotation * yQuaternion * _offset;
        }

        transform.TransformDirection(transform.localRotation.eulerAngles);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
          //  transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
           // transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
           
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, 0, -speed * Time.deltaTime, Space.Self);
        }
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        if (_currentTest != null && _lastPosition != null)
        {
            float distance = Vector3.Distance(_lastPosition, transform.position);
            if (distance > 0)
            {
                float change = (distance / _currentTest.Distance) * _currentTest.MaxAngle;
                ApplyManipulation(change);
            }
            else {
               // Debug.Log("No distance");
            }
        }
        else {
          //  Debug.Log("noopo");
        }
     
        total = _offset.eulerAngles.y;
        _rotator.setRotation(transform.rotation, _lastRotation);
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
        

    }

    private void ApplyManipulation(float val)
    {
        //Debug.Log("Apply manipulation "+val);
        _offset *= Quaternion.Euler(0, val, 0);
    }
}