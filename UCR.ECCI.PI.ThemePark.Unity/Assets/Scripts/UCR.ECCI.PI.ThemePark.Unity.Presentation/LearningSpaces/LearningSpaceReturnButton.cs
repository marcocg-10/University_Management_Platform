using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UCR.ECCI.PI.ThemePark.Unity.Services;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;

/// <summary>
/// Handles scene transitions triggered by UI button clicks.
/// </summary>

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces
{
    public class ReturnSceneButton : MonoBehaviour
    {
        [Inject]
        private ISceneTransitionService _sceneTransitionService;

        [Tooltip("Scene name to return to.")]
        public string ReturnSceneName = "BuildingSelectScene";

        /// <summary>
        /// Called by the UI Button to trigger scene change.
        /// </summary>
        public void OnReturnButtonPressed()
        {
            Debug.Log($"Returning to scene: {ReturnSceneName}");
            if (_sceneTransitionService == null)
                {
                    var sceneContext = FindObjectOfType<SceneContext>();
                    if (sceneContext != null)
                    {
                        Debug.Log("[LearningSpaceReturnButton] Resolving ISceneTransitionService manually from SceneContext.");
                        _sceneTransitionService = sceneContext.Container.Resolve<ISceneTransitionService>();
                    }
                    else
                    {
                        Debug.LogError("[LearningSpaceReturnButton] No SceneContext found to manually resolve ISceneTransitionService.");
                    }
                }
            // SceneManager.LoadScene(1);
            _sceneTransitionService.TransitionTo(1);
        }
    }
}