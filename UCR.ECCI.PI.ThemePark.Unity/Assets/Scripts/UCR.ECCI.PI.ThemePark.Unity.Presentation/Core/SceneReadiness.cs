using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.Core
{
    public static class SceneReadiness
    {
        private static int _pendingTasks;

        public static bool IsReady => _pendingTasks <= 0;

        public static void Reset()
        {
            _pendingTasks = 0;
            Debug.Log("[SceneReadiness] Reset. Pending = 0");
        }

        public static IEnumerator DelayedAction(float delayTime)
        {
            Debug.Log("Action before delay.");

            // Pause execution for the specified time
            yield return new WaitForSeconds(delayTime);

            Debug.Log("Action after delay.");
            // Add any other code you want to execute after the delay here
        }

        public static void RegisterTask(string name = null)
        {
            _pendingTasks++;
            Debug.Log($"[SceneReadiness] RegisterTask {name ?? ""}. Pending = {_pendingTasks}");
        }

        public static void TaskDone(string name = null)
        {
            _pendingTasks = Mathf.Max(0, _pendingTasks - 1);
            Debug.Log($"[SceneReadiness] TaskDone {name ?? ""}. Pending = {_pendingTasks}");
        }
    }
}