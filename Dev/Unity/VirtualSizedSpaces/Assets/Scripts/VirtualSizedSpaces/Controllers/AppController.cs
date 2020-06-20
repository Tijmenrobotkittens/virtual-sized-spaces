using System;
using System.Collections;
using System.Collections.Generic;
using RobotKittens;
using UnityEngine;

public class AppController : UiElement
{

    public enum Environments { None,Local, Staging, Production };
    public Environments Environment;
    private GlobalController _globalController;
    private GameObject SplashObject;
    private SocketController _socketController;  
    private States.State _startState;


    void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Config.Environment = Environment;
        Config.StartedIsolated = false;
        Config.SafetyY = HelperFunctions.GetTopSafeArea();
        Config.SafetyBottom = HelperFunctions.GetBottomSafeArea() / 2;
    }

    private void LoadScenesAndStart()
    {
        _globalController = GlobalController.Instance;

        //SceneLoader.Instance.AddScene(States.State.HOME, "Prefabs/Scenes/Onboarding", ObjectHolder.Instance.BottomCanvas, true, false, true);
        //SceneLoader.Instance.AddScene(States.State.EVENT, "Prefabs/Scenes/RegisterPhone", ObjectHolder.Instance.BottomCanvas, true, false, true);

        SceneLoader.Instance.Load();
        

        _startState = States.State.HOME;

        _socketController = SocketController.Instance;
        SplashObject.SetActive(false);
    }    
}
