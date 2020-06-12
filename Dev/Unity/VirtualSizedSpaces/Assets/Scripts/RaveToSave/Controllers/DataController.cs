using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DataControllerEvent : UnityEvent<DataController.DataTypes> { }

public class DataController : MonoBehaviour
{

    public DataControllerEvent Updated = new DataControllerEvent();
    private static DataController _instance;
    private int _secsBetween = 2;
    private int _newsecsBetween = 30;
    private Coroutine _coroutine;
    public enum DataTypes {MATCHES};
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("DataController").AddComponent<DataController>();
                _instance.Init();
            }
            
            return _instance;
        }
    }

    private void Init()
    {

    }


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //android onPause()
        }
        else
        {
           
        }
    }

   
}
