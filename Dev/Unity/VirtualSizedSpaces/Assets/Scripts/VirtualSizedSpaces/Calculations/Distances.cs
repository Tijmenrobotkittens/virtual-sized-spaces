using System;
public class Distances
{
    public static float ClosestPointOnLineT(NewVector3 p0, NewVector3 dir, NewVector3 pt)
    {
        float t = (pt - p0).Dot(dir);
        return t;
    }
}
