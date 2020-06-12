using UnityEngine;

namespace RobotKittens
{
    public static class VectorExtensions
    {
        public static Vector3 Random(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            return new Vector3(
                UnityEngine.Random.Range(xMin, xMax),
                UnityEngine.Random.Range(yMin, yMax),
                UnityEngine.Random.Range(zMin, zMax));
        }
        
        public static Vector3 Random(float min, float max)
        {
            return new Vector3(
                UnityEngine.Random.Range(min, max),
                UnityEngine.Random.Range(min, max),
                UnityEngine.Random.Range(min, max));
        }
    }
}
