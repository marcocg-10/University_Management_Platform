using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene transitions triggered by UI button clicks.
/// </summary>
public class ReturnSceneButton : MonoBehaviour
{
    [Tooltip("Scene name to return to.")]
    public string ReturnSceneName = "ThemePark";

    /// <summary>
    /// Called by the UI Button to trigger scene change.
    /// </summary>
    public void OnReturnButtonPressed()
    {
        Debug.Log($"Returning to scene: {ReturnSceneName}");
        SceneManager.LoadScene(0);
    }
}
