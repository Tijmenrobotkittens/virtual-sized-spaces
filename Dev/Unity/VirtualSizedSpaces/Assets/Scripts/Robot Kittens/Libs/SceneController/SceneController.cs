using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobotKittens;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    private SceneAnimator.Animations _forceDirection = SceneAnimator.Animations.NONE;
    private List<States.State> _waitingStates = new List<States.State>();


    public static SceneController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("SceneController").AddComponent<SceneController>();
                _instance.Init();
            }
            return _instance;
        }
    }

    public void ForceAnimation(SceneAnimator.Animations dir)
    {
        _forceDirection = dir;
    }

    public void Init()
    {
        RKLog.Log("init scenecontroller");
        RKLog.Log(GlobalController.StateManager.stateChanged.ToString());

        GlobalController.StateManager.stateChanged.AddListener(stateChanged);
        GlobalController.StateManager.stateAgain.AddListener(stateAgain);
        SceneLoader.Instance.sceneloaded.AddListener(SceneLoaded);
    }

    private void SceneLoaded(States.State scenename)
    {
        List<States.State> removestates = new List<States.State>();
        foreach (States.State state in _waitingStates)
        {
            if (scenename == state)
            {
                removestates.Add(state);
                stateChanged(state);
            }
        }

        foreach (States.State state in removestates)
        {
            _waitingStates.Remove(state);
        }
    }

    private void stateAgain(States.State state)
    {
        if (SceneLoader.Instance.GetScene(state) != null)
        {
            SceneBaseClass sb = SceneLoader.Instance.GetScene(state);
            if (sb)
            {
                sb.TappedAgain();
            }
        }
    }

    private bool animate(States.State newstate, SceneAnimator.Animations animation = SceneAnimator.Animations.LEFT)
    {



        List<string> states2 = new List<string>();

        States.State previousstate = GlobalController.StateManager.previous;

        animation = SceneTransitionController.Instance.GetTransition(previousstate,newstate);
        float time = SceneTransitionController.Instance.GetSpeed(previousstate, newstate);

        bool ret = false;
        RKLog.Log("change state from: " + previousstate + " to " + newstate);
        if (_forceDirection != SceneAnimator.Animations.NONE)
        {
            animation = _forceDirection;
            _forceDirection = SceneAnimator.Animations.NONE;
        }



        SceneAnimator.Instance.animateIn(SceneLoader.Instance.Scene(previousstate), SceneLoader.Instance.Scene(newstate), animation, time);
        ret = true;
       
        return ret;
    }



    private void stateChanged(States.State s)
    {
        RKLog.Log("Scenecontroller, statechanged " + s);

        if (!SceneLoader.Instance.IsSceneLoaded(s))
        {
            RKLog.Log("Not loaded, load " + s);
            _waitingStates.Add(s);
            SceneLoader.Instance.LoadScene(s);
            return;
        }
        else
        {
            animate(s, SceneAnimator.Animations.LEFT);
        }
    }

}