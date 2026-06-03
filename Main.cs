using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aegis
{
    public static class AegisRuntime
    {
        private static readonly Queue<Action> unityQueue = new();

        private static bool initialized;

        public static event Action OnRender;
        public static event Action OnUpdate;

        public static void Initialize()
        {
            if (initialized)
                return;

            var obj = new GameObject("Aegis");

            UnityEngine.Object.DontDestroyOnLoad(obj);

            obj.AddComponent<AegisRunner>();

            initialized = true;
        }

        public static void RunOnUnityThread(Action action)
        {
            if (action == null)
                return;

            lock (unityQueue)
            {
                unityQueue.Enqueue(action);
            }
        }

        internal static void ProcessQueue()
        {
            lock (unityQueue)
            {
                while (unityQueue.Count > 0)
                {
                    unityQueue.Dequeue()?.Invoke();
                }
            }
        }

        internal static void Update()
        {
            OnUpdate?.Invoke();
        }

        internal static void Render()
        {
            OnRender?.Invoke();
        }
    }
}