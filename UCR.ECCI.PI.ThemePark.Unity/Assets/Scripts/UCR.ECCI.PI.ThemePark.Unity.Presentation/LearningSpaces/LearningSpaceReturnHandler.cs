using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UCR.ECCI.PI.ThemePark.Unity.Services;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;

/// <summary>
/// Listens for a specific key press and returns to the previous scene.
/// Useful for exiting a learning space or returning to the main map.
/// </summary>

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces
{
    public class LearningSpaceReturnHandler : MonoBehaviour
    {
        [Inject]
        private ISceneTransitionService _sceneTransitionService;

        /// <summary>
        /// Name of the scene to return to.
        /// This should match the name of the target scene in your build settings.
        /// </summary>
        [Tooltip("Name of the scene to return to when Escape is pressed.")]
        public string ReturnSceneName = "BuildingSelectScene";

        /// <summary>
        /// Unity lifecycle method called once per frame.
        /// Checks for Escape key press and triggers scene change.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log($"Returning to scene: {ReturnSceneName}");
                if (_sceneTransitionService == null)
                {
                    var sceneContext = FindObjectOfType<SceneContext>();
                    if (sceneContext != null)
                    {
                        Debug.Log("[LearningSpaceReturnEsc] Resolving ISceneTransitionService manually from SceneContext.");
                        _sceneTransitionService = sceneContext.Container.Resolve<ISceneTransitionService>();
                    }
                    else
                    {
                        Debug.LogError("[LearningSpaceReturnEsc] No SceneContext found to manually resolve ISceneTransitionService.");
                    }
                }
                //SceneManager.LoadScene(1);
                _sceneTransitionService.TransitionTo(1);
            }
        }
    }
}