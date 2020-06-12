using System;
using RobotKittens;
using UnityEngine.Events;

public class SceneBaseClass : UiElement
{
   // protected VerticalUIList _verticallist;
    protected GenericData ReceivedData;
    public Action Preloaded;

    public void Preload(Action callback) {
        Preloaded = callback;
        DoPreload();
    }

    public virtual void DoPreload()
    {
        
    }

    public virtual void TappedAgain()
    {
//        Debug.LogError("baseclass tappedAgain, replace me");
    }

    public virtual void AfterTransitionIn()
    {
//        Debug.LogError("baseclass afterTransitionIn, replace me");
    }

    public virtual void AfterTransitionOut()
    {
//        Debug.LogError("baseclass afterTransitionOut, replace me");
    }

    public virtual void BeforeTransitionIn()
    {
//        Debug.LogError("baseclass beforeTransitionIn, replace me");
    }

    void Start()
    {
        if (Config.StartedIsolated) {
           
            BeforeTransitionIn();
            AfterTransitionIn();
        }
        else {

        }
    }
       
    protected override void Init()
    {
        base.Init();
    }
    
    public float GetScrollLength(){
        return 0;
    }
}