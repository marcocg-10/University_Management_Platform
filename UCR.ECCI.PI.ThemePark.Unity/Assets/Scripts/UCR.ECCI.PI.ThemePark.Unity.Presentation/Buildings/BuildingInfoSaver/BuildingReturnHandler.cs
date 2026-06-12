using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Listens for a specific key press and returns to the previous scene.
/// Useful for exiting a building or returning to the main map.
/// </summary>
public class BuildingReturnHandler : MonoBehaviour
{
    /// <summary>
    /// Name of the scene to return to.
    /// This should match the name of the target scene in your build settings.
    /// </summary>
    [Tooltip("Name of the scene to return to when Escape is pressed.")]
    public string ReturnSceneName = "ThemePark";

    /// <summary>
    /// Unity lifecycle method called once per frame.
    /// Checks for Escape key press and triggers scene change.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log($"Returning to scene: {ReturnSceneName}");
            SceneManager.LoadScene(0);
        }
    }
}