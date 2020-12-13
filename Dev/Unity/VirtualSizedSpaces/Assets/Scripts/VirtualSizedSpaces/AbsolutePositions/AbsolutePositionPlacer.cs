using System.Linq;
using Antilatency.Alt.Tracking;
using Antilatency.InterfaceContract;
using UnityEngine;


public class AbsolutePositionPlacer : Antilatency.InterfaceContract.InterfacedObject, Antilatency.Alt.Tracking.IEnvironment
{

    protected override void Destroy()
    {
     
    }

    Bool IEnvironment.isMutable()
    {
        return false;
    }

   
    Vector3[] IEnvironment.GetUsers()
    {
        return new Vector3[0];
    }

   
    Bool IEnvironment.filterRay(Vector3 up, Vector3 ray)
    {
        return true;
    }

   
    Bool IEnvironment.match(Vector3[] raysUpSpace, out MarkerIndex[] markersIndices, out Pose poseOfUpSpace)
    {
        markersIndices = Enumerable.Repeat(MarkerIndex.Unknown, raysUpSpace.Length).ToArray();
        poseOfUpSpace = new Pose();
        return false;
    }

    MarkerIndex[] IEnvironment.matchByPosition(Vector3[] rays, Vector3 origin)
    {
        return Enumerable.Repeat(MarkerIndex.Unknown, rays.Length).ToArray();
    }
}