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
    private MatchVisualization _matchViz = new MatchVisualization();
    private MatchByPositionVisualization _machPos = new MatchByPositionVisualization();


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

    Bool IEnvironment.match(Vector3[] Rays, out MarkerIndex[] Markers, out Pose PositionofUp)
    {
        const float projectionsMatchTolerance = 0.05f;

        Markers = Enumerable.Repeat(MarkerIndex.Unknown, Rays.Length).ToArray();
        PositionofUp = new Pose();

       
        if (Rays.Length != 3)
            return false;

        var projecties = projectRaysOnFloor(Rays);

       
        var M = new float[6, 4];
        var b = new float[6];
        for (int i = 0; i < 3; ++i)
        {
            M[2 * i, 0] = projecties[i].x;
            M[2 * i, 1] = -projecties[i].y;
            M[2 * i, 2] = 1;
            M[2 * i + 1, 0] = projecties[i].y;
            M[2 * i + 1, 1] = projecties[i].x;
            M[2 * i + 1, 3] = 1;
            b[2 * i] = _markers[i].x;
            b[2 * i + 1] = _markers[i].y;
        }

       
        var invM = M.transpose().multiply(M).inverse().multiply(M.transpose());


        int BestMatch = -1;
        float bestError = float.MaxValue;
        var bestTransform2d = new ConvertTo2D();
        for (int idPermutation = 0; idPermutation < 6; ++idPermutation)
        {
            var curB = new float[6];
            for (int i = 0; i < 3; ++i)
            {
                int j = permutations[idPermutation][i];
                curB[2 * i] = b[2 * j];
                curB[2 * i + 1] = b[2 * j + 1];
            }

            var transformParams = invM.multiply(curB); 
            var transform2d = new ConvertTo2D(transformParams[0], transformParams[1], transformParams[2], transformParams[3]);
            float error = 0; 
            for (int idMarker = 0; idMarker < 3; ++idMarker)
            {
                error += (transform2d.apply(projecties[idMarker]) - _markers[permutations[idPermutation][idMarker]]).sqrMagnitude;
            }

            if (error < bestError)
            {
                bestError = error;
                BestMatch = idPermutation;
                bestTransform2d = transform2d;
            }
        }

        if (bestError > projectionsMatchTolerance)
            return false;

        for (int i = 0; i < Rays.Length; ++i)
        {
            Markers[i].value = (uint)permutations[BestMatch][i];
        }

        var position = new Vector3(bestTransform2d.Translate.x, bestTransform2d.Scale, bestTransform2d.Translate.y);
        var rotation = Quaternion.AxisAngle(Vector3.up, -bestTransform2d.Angle);
        PositionofUp = new Pose(position, rotation);

        lock (_visobject)
        {
            _matchViz = new MatchVisualization(
                Rays.ToList(),
                PositionofUp,
                projecties.Select(p => bestTransform2d.apply(p)).ToList(),
                Markers);
        }

        return true;
    }
}