using UnityEngine;
using System.Collections;

namespace RobotKittens
{

    /// <summary>
    /// Dumping ground for all monobehaviour extentions
    /// </summary>
    public static class MonoExt
    {

        public delegate void MethodToCall();

        /// <summary>
        /// Calls OnComplete method after [ticks] seconds.
        /// </summary>
        /// <param name="obj">Reference to the monobehaviour (not passed).</param>
        /// <param name="ticks">How long to wait.</param>
        /// <param name="onComplete">Call after timeout.</param>
        public static void WaitAndCall(this MonoBehaviour obj, float ticks, MethodToCall onComplete)
        {

            obj.StartCoroutine(Wait(ticks, onComplete));

        }

        private static IEnumerator Wait(float time, MethodToCall onComplete)
        {

            yield return new WaitForSeconds(time);

            onComplete?.Invoke();

        }

    }

}