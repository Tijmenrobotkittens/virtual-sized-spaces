using System.Collections.Generic;
using System.Linq;
using Antilatency.Alt.Tracking;
using Antilatency.InterfaceContract;
using UnityEngine;

public class DynamicUserEnvironment : Antilatency.InterfaceContract.InterfacedObject, Antilatency.Alt.Tracking.IEnvironment
{

    static readonly int[][] permutations = new int[][] {
        new int[] {0, 1, 2},
        new int[] {0, 2, 1},
        new int[] {1, 0, 2},
        new int[] {1, 2, 0},
        new int[] {2, 0, 1},
        new int[] {2, 1, 0}
    };

    private object _visobject = new object();
   


    private List<Vector2> _markers;

    public DynamicUserEnvironment(IList<Vector2> markers)
    {
        if (markers.Count != 3)
            throw new System.Exception("Geen users gevonden!");

        _markers = markers.ToList();
    }

    protected override void Destroy()
    {
        // Ehhh, deze moeten we nooit aanroepen!
    }

    Bool IEnvironment.isMutable()
    {
        return false;
    }

    Vector3[] IEnvironment.GetUsers()
    {
        return _markers.Select(m => new Vector3(m.x, 0, m.y)).ToArray();
    }

    Bool IEnvironment.filterRay(Vector3 up, Vector3 ray)
    {
        return Vector3.Dot(up, ray) < 0;
    }

    
}