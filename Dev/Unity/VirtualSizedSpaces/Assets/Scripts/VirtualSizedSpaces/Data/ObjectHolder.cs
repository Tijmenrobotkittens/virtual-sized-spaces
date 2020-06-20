using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobotKittens;

public class ObjectHolder : UiElement
{
    private static ObjectHolder _instance;
    private GameObject _topCanvas;
    private GameObject _middleCanvas;
    private GameObject _bottomCanvas;
    private GameObject _recordingCanvas;
    private GameObject _holders;
    private GameObject _3dHolder;

    public static ObjectHolder Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("ObjectHolder").AddComponent<ObjectHolder>();
                _instance.Init();
            }
            return _instance;
        }
    }

    protected override void Init()
    {
        _holders = GameObject.Find("Holders");
        if (_holders != null)
        {
            _bottomCanvas = _holders.transform.Find("Bottom").gameObject;
            _middleCanvas = _holders.transform.Find("Middle").gameObject;
            _topCanvas = _holders.transform.Find("Top").gameObject;
            _recordingCanvas = _holders.transform.Find("Recording").gameObject;
            _3dHolder = _holders.transform.Find("3D").gameObject;
        }
    }

    public GameObject Holder3D
    {
        get
        {
            BaseInit();
            return _3dHolder;
        }

        set
        {
            _3dHolder = value;
        }
    }

    public GameObject TopCanvas
    {
        get
        {
            BaseInit();
            return _topCanvas;
        }

        set
        {
            _topCanvas = value;
        }
    }

    public GameObject MiddleCanvas
    {
        get
        {
            BaseInit();
            return _middleCanvas;
        }

        set
        {
            _middleCanvas = value;
        }
    }

    public GameObject BottomCanvas
    {
        get
        {
            BaseInit();
            return _bottomCanvas;
        }

        set
        {
            _bottomCanvas = value;
        }
    }

    public GameObject RecordingCanvas
    {
        get
        {
            BaseInit();
            return _recordingCanvas;
        }

        set
        {
            _recordingCanvas = value;
        }
    }
}
