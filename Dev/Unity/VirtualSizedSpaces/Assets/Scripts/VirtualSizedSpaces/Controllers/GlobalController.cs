using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobotKittens;
using System;
using Newtonsoft.Json.Linq;

public class UserAppData
{
    public string name;
    public string email;
    public string size;
}

public class GlobalController : MonoBehaviour
{
    private static GlobalController _instance;
    public static StateManager StateManager;
    private SceneController _sceneController;

    public static GlobalController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("GlobalController").AddComponent<GlobalController>();
                _instance.Init();
            }
            return _instance;
        }
    }


    private void Init()
    {
        RKLog.Log("Init");
        StateManager = new StateManager();
        StateManager.stateChanged.AddListener(StateChanged);
        _sceneController = SceneController.Instance;
    }

    private void StateChanged(States.State state)
    {
        RKLog.Log("state changed to " + state.ToString());
    }

}
