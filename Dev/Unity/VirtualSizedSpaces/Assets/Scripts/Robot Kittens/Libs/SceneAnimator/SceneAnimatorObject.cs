using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SceneAnimatorObject
{
    private SceneData _in;
    private SceneData _out;
    private float _speed = 0.5f;
    private string _description = "";
    private Ease _ease;
    SceneAnimator.Animations _animation;


    public SceneAnimatorObject(SceneData inScene,SceneData outScene, SceneAnimator.Animations animation = SceneAnimator.Animations.LEFT, float speed = 1, Ease ease = Ease.InOutQuint)
    {
        _ease = ease;
        _in = inScene;
        _out = outScene;
        _speed = speed;
        
        _animation = animation;
    }

    public SceneData inScene
    {
        get { return _in; }
        set
        {
            _in = value;
        }
    }

    public SceneData outScene
    {
        get { return _out; }
        set
        {
            _out = value;
        }
    }

    public float speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
        }
    }

    public SceneAnimator.Animations animation
    {
        get { return _animation; }
        set
        {
            _animation = value;
        }
    }

    public Ease ease
    {
        get { return _ease; }
        set
        {
            _ease = value;
        }
    }
}