using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace RobotKittens
{
    // Employs queue strategy, lazy load at end of frame.
    public class Queue<T>
    {
        // Queue settings
        private bool isWaiting = false;
        private int maxConcurrentRequests = 1;
        private List<T> queuedRequests = new List<T>();
        private MonoBehaviour context;

        private QueueAction queueAction;

        public delegate void QueueAction(T item);

        public Queue(MonoBehaviour context, QueueAction queueAction, int maxConcurrentRequests = 1)
        {
            this.context = context;
            this.queueAction = queueAction;
            this.maxConcurrentRequests = maxConcurrentRequests;
        }

        public void Process(T item) 
        {
            queuedRequests.Add(item);
            ThrottleQueue();
        }

        public List<T> GetQueuedRequests()
        {
            return queuedRequests;
        }

        public void DeleteRequest(T request)
        {
            queuedRequests.Remove(request);
        }

        private void ThrottleQueue()
        {
            if (isWaiting) { return; }
            if(queuedRequests.Count <= 0) { return; }

            context.StartCoroutine(WaitForNextFrameRoutine());
        }

        private IEnumerator<WaitForEndOfFrame> WaitForNextFrameRoutine()
        {
            isWaiting = true;
            yield return new WaitForEndOfFrame();

            int currentConcurrentRequests = 0;
            List<T> pendingItems = new List<T>();
            foreach (T queueItem in queuedRequests)
            {
                if (currentConcurrentRequests < maxConcurrentRequests)
                {
                    currentConcurrentRequests++;
                    this.queueAction(queueItem);
                } else {
                    pendingItems.Add(queueItem);
                }
            }

            //Debug.Log("Finished with WaitForNextFrameRoutine");

            this.queuedRequests = pendingItems;
            isWaiting = false;
            this.ThrottleQueue();
        }

    }
}
