using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ToggleImageEvent : UnityEvent<bool> { }

public class ToggleImage : MonoBehaviour, IPointerClickHandler
{
    public bool activeOnStart = false;
    private bool _active = true;
    private GameObject _imageActive;
    private GameObject _imageInactive;
    public bool enabled = true;
    public ToggleImageEvent clicked = new ToggleImageEvent();

    private void GetObjects()
    {
        if (_imageActive == false)
        {
            _imageActive = transform.Find("active").gameObject;
            _imageInactive = transform.Find("inactive").gameObject;
            if (activeOnStart) {
                _active = true;
            }
            else {
                _active = false;
            }
            UpdateState();
        }
    }
    public bool active { get { return _active; }set { _active = value; UpdateState(); } }
    private void UpdateState()
    {
        GetObjects();
        if (_active)
        {
            _imageActive.SetActive(true);
            _imageInactive.SetActive(false);
        }
        else
        {
            _imageActive.SetActive(false);
            _imageInactive.SetActive(true);
        }
    }

    public void Set(bool active)
    {
        _active = active;
        UpdateState();
    }

    public void Toggle(){
        
        if (_active) {
            _active = false;
        }
        else {
            _active = true;
        }
        UpdateState();
    }



	// Use this for initialization
	void Start () {
        GetObjects();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (enabled)
        {
            Toggle();
        }

        clicked.Invoke(_active);
    }
}
