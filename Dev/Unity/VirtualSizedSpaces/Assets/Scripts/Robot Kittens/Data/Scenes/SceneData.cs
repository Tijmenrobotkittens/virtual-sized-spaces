using System.Collections.Generic;
using UnityEngine;

public class SceneData
{
    private string _name;
    private GameObject _target;
    private SceneBaseClass _scene;
    private GameObject _gameObject;
    private bool _hideOnStart;
    private bool _loaded = false;
    private bool _loading = false;
    private bool _load = false;
    private bool _destroyOnMoveOut = false;
    public GenericData tempdata = null;
    private States.State _state;

    public string name
    {
        get { return _name; }
        set
        {
            _name = value;
        }
    }

    public States.State State
    {
        get { return _state; }
        set
        {
            _state = value;
        }
    }

    public SceneBaseClass Scene
    {
        get { return _scene; }
        set
        {
            _scene = value;
        }
    }

    public bool hideOnStart
    {
        get { return _hideOnStart; }
        set
        {
            _hideOnStart = value;
        }
    }

    public bool loaded
    {
        get { return _loaded; }
        set
        {
            _loaded = value;
        }
    }

    public bool loading
    {
        get { return _loading; }
        set
        {
            _loading = value;
        }
    }

    public bool load
    {
        get { return _load; }
        set
        {
            _load = value;
        }
    }

    public bool DestroyOnMoveOut
    {
        get { return _destroyOnMoveOut; }
        set
        {
            _destroyOnMoveOut = value;
        }
    }



    public GameObject target
    {
        get { return _target; }
        set
        {
            _target = value;
        }
    }

    public GameObject gameobject
    {
        get { return _gameObject; }
        set
        {
            _gameObject = value;
            if (_gameObject != null)
            {
                _gameObject.transform.SetParent(_target.transform, false);
                if (hideOnStart)
                {
                    _gameObject.SetActive(false);
                }
            }
           
        }
    }

}