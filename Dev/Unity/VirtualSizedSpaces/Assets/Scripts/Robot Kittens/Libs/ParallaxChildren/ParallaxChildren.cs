using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxChildren : MonoBehaviour {
    public ScrollRect scrollRect;
    private ParallaxChild[] _objects;
    private bool started = false;
    public bool onGyro = false;

	// Use this for initialization
	void Start () {

        if (scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(ScrollValueChanged);
        }
        getObjects();
        started = true;
        if (onGyro == true) {
            Input.gyro.enabled = true;
        }
    }

    private void getObjects(){
        if (_objects == null) {
            _objects = GetComponentsInChildren<ParallaxChild>();
        }
    }

    public void reset()
    {
        if (started)
        {
            foreach (ParallaxChild child in _objects)
            {
                child.update(0);
            }
        }
    }

    private void ScrollValueChanged(Vector2 value) {
        getObjects();
        //Debug.Log(scrollRect.content.localPosition);

        //foreach (ParallaxChild child in _objects)
        //{
        //    child.update(scrollRect.content.localPosition);
        //}
    }
	
	// Update is called once per frame
	void Update () {

        if (onGyro) {
            //Debug.LogError("x: " + Input.gyro.attitude.x + "y: " + Input.gyro.attitude.y + " z: " + Input.gyro.attitude.z + " - "+ _objects.Length);
            if (_objects != null)
            {
                foreach (ParallaxChild child in _objects)
                {
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        child.update(-Input.gyro.attitude.x);
                    }
                    else {
                        child.update(-Input.gyro.attitude.y);
                    }
               }
            }
            
            //Quaternion q = Quaternion.Euler(90.0f, 0.0f, 0.0f) * new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
            //transform.parent.rotation = q;

        }
	}
}
