using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    public static class Awaiters
    {
        public static MainThreadAwaiter UnityMainThread => new MainThreadAwaiter();

        public class MainThreadAwaiter : System.Runtime.CompilerServices.INotifyCompletion
        {
            public bool IsCompleted => UnityMainThreadDispatcher.IsMainThread;

            public void OnCompleted(Action continuation)
            {
                UnityMainThreadDispatcher.RunOnMainThread(continuation);
            }

            public void GetResult() { }

            // ✅ Required for `await Awaiters.UnityMainThread`
            public MainThreadAwaiter GetAwaiter() => this;
        }
    }

    public static class UnityMainThreadDispatcher
    {
        private static readonly System.Collections.Concurrent.ConcurrentQueue<Action> _actions = new();
        private static int _mainThreadId;

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            _mainThreadId = Environment.CurrentManagedThreadId;
            UnityEngine.Application.quitting += () => _actions.Clear();
        }

        public static bool IsMainThread => Environment.CurrentManagedThreadId == _mainThreadId;

        public static void RunOnMainThread(Action action)
        {
            _actions.Enqueue(action);
        }

        [RuntimeInitializeOnLoadMethod]
        static void StartUpdater()
        {
            var go = new GameObject("MainThreadDispatcher");
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<Updater>();
        }

        private class Updater : MonoBehaviour
        {
            void Update()
            {
                while (_actions.TryDequeue(out var a))
                {
                    try { a(); } catch (Exception e) { Debug.LogException(e); }
                }
            }
        }
    }
}