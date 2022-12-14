namespace MRTKUtilities.Application
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

#if WINDOWS_UWP
    /// <summary>
    /// A helper class for dispatching actions to run on various Unity threads.
    /// </summary>
    public static class UnityDispatcher
    {
        /// <summary>
        /// Schedules the specified action to be run on Unity's main thread.
        /// </summary>
        /// <param name="action">
        /// The action to run.
        /// </param>
        public static void InvokeOnAppThread(Action action)
        {
            if (action == null)
            {
                return;
            }

            if (UnityEngine.WSA.Application.RunningOnAppThread())
            {
                // Already on app thread, just run inline
                action();
            }
            else
            {
                // Schedule
                UnityEngine.WSA.Application.InvokeOnAppThread(() => action(), false);
            }
        }
    }
#else
    /// <summary>
    /// A helper class for dispatching actions to run on various Unity threads.
    /// </summary>
    public class UnityDispatcher : MonoBehaviour
    {
        static private UnityDispatcher instance;
        static private Queue<Action> queue = new Queue<Action>(8);
        static private volatile bool queued = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static private void Initialize()
        {
            if (instance == null)
            {
                instance = new GameObject("Dispatcher").AddComponent<UnityDispatcher>();
                DontDestroyOnLoad(instance.gameObject);
            }
        }

        protected virtual void Update()
        {
            // Action placeholder
            Action action = null;

            // Do this as long as there's something in the queue
            while (queued)
            {
                // Lock only long enough to take an item
                lock (queue)
                {
                    // Get the next action
                    action = queue.Dequeue();

                    // Have we exhausted the queue?
                    if (queue.Count == 0) { queued = false; }
                }

                // Execute the action outside of the lock
                action();
            }
        }

        /// <summary>
        /// Schedules the specified action to be run on Unity's main thread.
        /// </summary>
        /// <param name="action">
        /// The action to run.
        /// </param>
        static public void InvokeOnAppThread(Action action)
        {
            // Validate
            if (action == null) throw new ArgumentNullException(nameof(action));

            // Lock to be thread-safe
            lock (queue)
            {
                // Add the action
                queue.Enqueue(action);

                // Action is in the queue
                queued = true;
            }
        }
    }
#endif
}