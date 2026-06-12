using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UCR.ECCI.PI.ThemePark.Unity.Services;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.Core
{
    public class SceneTransitionService : ISceneTransitionService
    {
        private readonly LoadingScreenView _loadingScreen;
        private readonly SceneTransitionRunner _runner;

        public SceneTransitionService(
            LoadingScreenView loadingScreen,
            SceneTransitionRunner runner)
        {
            _loadingScreen = loadingScreen;
            _runner = runner;
        }

        public void TransitionTo(string sceneName)
        {
            SceneReadiness.Reset();

            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            _runner.StartCoroutine(TransitionRoutine(op));
        }

        public void TransitionTo(int sceneIndex)
        {
            SceneReadiness.Reset();

            var op = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            _runner.StartCoroutine(TransitionRoutine(op));
        }

        private IEnumerator TransitionRoutine(AsyncOperation op)
        {
            _loadingScreen.ShowImmediate();
            float minDisplayTime = 2f;
            float startTime = Time.realtimeSinceStartup;
            yield return null; // let UI render

            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                float normalized = Mathf.Clamp01(op.progress / 0.9f);
                _loadingScreen.SetProgress(normalized);
                yield return null;
            }

            _loadingScreen.SetProgress(1f);
            op.allowSceneActivation = true;

            while (!op.isDone)
                yield return null;

            yield return null; // one more frame so Awake/Start in new scene run

            yield return new WaitUntil(() => SceneReadiness.IsReady);

            float elapsed = Time.realtimeSinceStartup - startTime;
            if (elapsed < minDisplayTime)
            {
                float remaining = minDisplayTime - elapsed;
                // Realtime so timeScale changes don't affect it
                yield return new WaitForSecondsRealtime(remaining);
            }

            yield return _loadingScreen.FadeOut();
        }
    }
}