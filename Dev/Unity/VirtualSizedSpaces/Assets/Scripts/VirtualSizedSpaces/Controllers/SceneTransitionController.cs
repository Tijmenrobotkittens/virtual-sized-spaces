using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobotKittens;

public class SceneTransitionController : MonoBehaviour
{
    private static SceneTransitionController _instance;
   
    public static SceneTransitionController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("SceneTransitionController").AddComponent<SceneTransitionController>();
                //_instance.Init();
            }
            return _instance;
        }
    }

    //\Getters

    public SceneAnimator.Animations GetTransition(States.State fromState, States.State toState)
    {
        SceneAnimator.Animations ts = SceneAnimator.Animations.LEFT;

        //if (fromState == States.State.sp && toState == States.State.ONBOARDING)
        //{
        //    ts = SceneAnimator.Animations.FADEIN;
        //}


       
       ts = SceneAnimator.Animations.FADEOUT;
       

        return ts;
    }

    public float GetSpeed(States.State fromState, States.State toState) {
        return 0.3f;
    }
}
