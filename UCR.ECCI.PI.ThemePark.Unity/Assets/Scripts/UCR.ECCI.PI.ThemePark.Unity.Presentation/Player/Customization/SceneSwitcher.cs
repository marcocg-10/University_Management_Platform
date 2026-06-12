using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Detect if the esc key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentScene == "ThemeParkScene")
            {
                // Load the customize avatar scene
                SceneManager.LoadScene("AvatarCreatorElements");
            } else
            {
                // Load the themepark scene
                SceneManager.LoadScene("ThemeParkScene");
            }
            
        }
    }
}
