using System;

public class SceneAnimatorPerSceneData {
    public float Xfrom = 0;
    public float Yfrom = 0;
    public float Xto = 0;
    public float Yto = 0;
    public float AlphaFrom = 1;
    public float AlphaTo = 1;
    public float ScaleFrom = 1;
    public float ScaleTo = 1;
    public bool ForceToTop = false;
}

public class SceneAnimatorData
{
    public SceneAnimatorPerSceneData InObject = new SceneAnimatorPerSceneData();
    public SceneAnimatorPerSceneData OutObject = new SceneAnimatorPerSceneData();
}
