using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using RobotKittens;

public class SceneAnimatorMoveEvent : UnityEvent<float, float> { }

public class SceneAnimator :MonoBehaviour {
    private float _height = 0;
    private bool _animating = false;
    private List<SceneAnimatorObject> animations = new List<SceneAnimatorObject>();
    private static SceneAnimator _instance;
    public UnityEvent complete = new UnityEvent();
    private XYID xyid = new XYID(0,0,0);
    private SceneAnimatorObject CurrentAnimation;
    public SceneAnimatorMoveEvent moveEvent = new SceneAnimatorMoveEvent();
    public enum Animations { NONE, LEFT,RIGHT,UP,DOWN,SCALEFROMTOP,FADEOUT,FADEIN}


    public static SceneAnimator Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("SceneAnimator").AddComponent<SceneAnimator>();
            }
            return _instance;
        }
    }


    public float width {
        get {
            return Config.width;
        }
      
    }

    public void singleIn(SceneData sceneIn, SceneAnimator.Animations animation = SceneAnimator.Animations.LEFT, float speed = 0.5f, Ease ease = Ease.InOutQuad){
        SceneAnimatorObject so = new SceneAnimatorObject(sceneIn, null, animation, speed,ease);
        animations.Add(so);
        if (!_animating)
        {
            animate();
        }
    }

    public void singleOut(SceneData sceneOut, SceneAnimator.Animations animation = SceneAnimator.Animations.LEFT, float speed = 0.5f, Ease ease = Ease.InOutQuad)
    {
         SceneAnimatorObject so = new SceneAnimatorObject(null, sceneOut, animation, speed, ease);
        animations.Add(so);
        if (!_animating)
        {
          //  Debug.Log("SceneAnimator singleOut4: " + sceneOut.name);
            animate();
        }
    }

    public void animateIn(SceneData sceneOut, SceneData sceneIn, SceneAnimator.Animations animation = SceneAnimator.Animations.LEFT, float speed = 0.5f, Ease ease = Ease.InOutQuad) {
        if (sceneIn != null && sceneOut != null)
        {
            RKLog.Log("SceneAnimator Animatein: " + sceneIn.name + " out: " + sceneOut.name + " speed " + speed + " dir ","sceneanimator");
        }
        SceneAnimatorObject so = new SceneAnimatorObject(sceneIn, sceneOut, animation, speed,ease);
        animations.Add(so);
        if (!_animating)
        {
            animate();
        }
    }

    private void UpdateTween() {
        float newx = CurrentAnimation.inScene.gameobject.GetComponent<RectTransform>().anchoredPosition.x;
        float newy = CurrentAnimation.inScene.gameobject.GetComponent<RectTransform>().anchoredPosition.y;

        float diffx = xyid.x - newx;
        float diffy = xyid.y - newy;

        moveEvent.Invoke(diffx, diffy);

        xyid.x = newx;
        xyid.y = newy;


    }


    private void runAnimation(SceneAnimatorObject animation, SceneAnimatorData sad) {
//        Debug.Log("animate! " +xtarget+"x"+ytarget);
        _animating = true;
        CurrentAnimation = animation;

        if (animation.inScene != null && sad.InObject != null && animation.inScene.gameobject)
        {
            //   RKLog.Log(animation.inScene.ToString());
           // RKLog.Log("SceneAnimator runanimation inscene: " + animation.inScene, "sceneanimator");

          
            SceneLoader.Instance.Show(animation.inScene.State);
            animation.inScene.gameobject.absolutePosition(new Vector2(sad.InObject.Xfrom, sad.InObject.Yfrom));
            beforeTransitionIn(animation.inScene.State);
            xyid.x = animation.inScene.gameobject.GetComponent<RectTransform>().anchoredPosition.x;
            xyid.y = animation.inScene.gameobject.GetComponent<RectTransform>().anchoredPosition.y;

            DOTween.Kill(animation.inScene.gameobject.GetComponent<RectTransform>());

            animation.inScene.gameobject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(sad.InObject.Xto, sad.InObject.Yto), animation.speed).OnComplete(completeAnimation).SetEase(animation.ease).OnUpdate(UpdateTween);

           // RKLog.Log("SceneAnimator runanimation inscene: x" + sad.InObject.Xfrom +" x2: "+ sad.InObject.Xto, "sceneanimator");
            //return;
            if (sad.InObject.ForceToTop)
            {
                animation.inScene.gameobject.transform.SetAsLastSibling();
            }


            if (sad.InObject.ScaleFrom != sad.InObject.ScaleTo)
            {
                animation.inScene.gameobject.transform.localScale = new Vector2(sad.InObject.ScaleFrom, sad.InObject.ScaleFrom);
                animation.inScene.gameobject.transform.DOScale(sad.InObject.ScaleTo, animation.speed).SetEase(Ease.OutExpo);
            }

            CanvasGroup cg = animation.inScene.gameobject.GetComponent<CanvasGroup>();
            if (sad.InObject.AlphaFrom != sad.InObject.AlphaTo)
            {


                if (cg)
                {
                    cg.alpha = sad.InObject.AlphaFrom;
                    cg.DOFade(sad.InObject.AlphaTo, animation.speed);
                }
            }
            else
            {
                if (cg)
                {
                    cg.alpha = 1;
                }
            }
        }
        else {
            completeAnimation();
        }

        if (animation.outScene != null && animation.outScene.gameobject != null)
        {
            float x = sad.OutObject.Xto;
            float y = sad.OutObject.Yto;
          //  RKLog.Log("SceneAnimator runanimation outscene: " + animation.outScene, "sceneanimator");

            if (sad.OutObject.ForceToTop)
            {
                animation.outScene.gameobject.transform.SetAsLastSibling();
            }

            if (animation.inScene == null)
            {
                DOTween.Kill(animation.outScene.gameobject.GetComponent<RectTransform>());
                animation.outScene.gameobject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(x, y), animation.speed).SetEase(animation.ease).OnComplete(completeAnimation);
            }
            else
            {
                DOTween.Kill(animation.outScene.gameobject.GetComponent<RectTransform>());
                animation.outScene.gameobject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(x, y), animation.speed).SetEase(animation.ease);
            }

            if (sad.OutObject.ScaleFrom != sad.OutObject.ScaleTo)
            {
                animation.outScene.gameobject.transform.localScale = new Vector2(sad.OutObject.ScaleFrom, sad.OutObject.ScaleFrom);
                animation.outScene.gameobject.transform.DOScale(sad.OutObject.ScaleTo, animation.speed);
            }

            CanvasGroup cg = animation.outScene.gameobject.GetComponent<CanvasGroup>();
            if (sad.OutObject.AlphaFrom != sad.OutObject.AlphaTo)
            {


                if (cg)
                {
                    cg.alpha = sad.OutObject.AlphaFrom;
                    cg.DOFade(sad.OutObject.AlphaTo, animation.speed);
                }
            }
            else
            {
                if (cg)
                {
                    cg.alpha = 1;
                }
            }
        }
        else {
            completeAnimation();
        }
    }

    private void animate(){

		_height = HelperFunctions.GetHeight((int)width);
//        Debug.LogError("animate "+ width);
 //       Debug.LogError("animate " + Config.width + " - "+ width);

        SceneAnimatorObject animation = new SceneAnimatorObject(null,null,SceneAnimator.Animations.LEFT,0);
        bool animationFound = false;
        if (animations.Count > 0) {
            animation = animations[0];
            animationFound = true;
            //Debug.Log("animate found!");
        }
        else {
            //Debug.Log("animate not found!");
        }

        if (animationFound)
        {
            //Debug.Log("animationfound "+animation.dir);
            SceneAnimatorData sad = new SceneAnimatorData();
            switch (animation.animation)
            {
                case Animations.LEFT:
                    
                    sad.InObject.Xto = 0;
                    sad.InObject.Yto = 0;
                    sad.InObject.Xfrom = width;
                    sad.InObject.Yfrom = 0;
                    sad.OutObject.Xto = 0 - width;
                    sad.OutObject.Yto = 0;
     


                    runAnimation(animation,sad);
                    break;
                case Animations.RIGHT:

                    sad.InObject.Xto = 0;
                    sad.InObject.Yto = 0;
                    sad.InObject.Xfrom = 0-width;
                    sad.InObject.Yfrom = 0;
                    sad.OutObject.Xto = width;
                    sad.OutObject.Yto = 0;

                    runAnimation(animation, sad);
                    break;
                case Animations.UP:
                    
                    sad.InObject.Xto = 0;
                    sad.InObject.Yto = 0;
                    sad.InObject.Xfrom = 0;
                    sad.InObject.Yfrom = 0-_height;
                    sad.OutObject.Xto = 0;
                    sad.OutObject.Yto = _height;

                    runAnimation(animation, sad);
                    break;
                case Animations.DOWN:
                    sad.InObject.Xto = 0;
                    sad.InObject.Yto = 0;
                    sad.InObject.Xfrom = 0;
                    sad.InObject.Yfrom =  _height;
                    sad.OutObject.Xto = 0;
                    sad.OutObject.Yto = 0-_height;
                    runAnimation(animation,sad);
                    break;

                case Animations.SCALEFROMTOP:
                    sad.InObject.AlphaFrom = 0;
                    sad.InObject.AlphaTo = 1;
                    sad.InObject.ScaleFrom = 2;
                    sad.InObject.ScaleTo = 1;
                    sad.InObject.ForceToTop = true;

                    runAnimation(animation, sad);
                    break;
                case Animations.FADEOUT:
                    sad.OutObject.AlphaFrom = 1;
                    sad.OutObject.AlphaTo = 0;
                    sad.OutObject.ForceToTop = true;
                    runAnimation(animation, sad);
                    break;

                case Animations.FADEIN:
                    sad.InObject.AlphaFrom = 0;
                    sad.OutObject.AlphaTo = 1;
                    sad.InObject.ForceToTop = true;
                    runAnimation(animation, sad);
                    break;
            }
        }
    }

    private void completeAnimation(){
       
        SceneAnimatorObject animation = null;
        if (animations.Count > 0) {
            animation = animations[0];
        }
        //Debug.LogError("complete animation ");
        if (animation != null && animation.outScene != null)
        {

            SceneBaseClass scene = SceneLoader.Instance.GetScene(animation.outScene.State);
    
            if (scene != null) {
                scene.AfterTransitionOut();
            }
            SceneLoader.Instance.Hide(animation.outScene.State);

        }
        if (animation != null && animation.inScene != null)
        {
            SceneBaseClass scene = SceneLoader.Instance.GetScene(animation.inScene.State);
            if (scene != null)
            {

                scene.AfterTransitionIn();
            }

        }
        if (animations.Count > 0)
        {
            animations.RemoveAt(0);
        }
        _animating = false;
        GlobalController.StateManager.Unlock();
        animate();
        //Debug.LogError("complete animation end");
        complete.Invoke();
        
    }

    public void beforeTransitionIn(States.State state) {
        SceneBaseClass scene = SceneLoader.Instance.GetScene(state);
        if (scene != null)
        {

            scene.BeforeTransitionIn();
        }
    }


}