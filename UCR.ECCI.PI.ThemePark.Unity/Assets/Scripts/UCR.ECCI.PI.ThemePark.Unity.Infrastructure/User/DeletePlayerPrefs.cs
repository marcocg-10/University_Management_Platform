using UnityEngine;

/// <summary>
/// Deletes all stored player preferences and disables the associated GameObject at the start of the session
/// </summary>
/// <remarks>This method clears all data stored in Unity's <see cref="PlayerPrefs"/> and saves the changes. After
/// clearing the preferences, the GameObject to which this script is attached is deactivated. This is needed for the customize
/// feature because it often has errors with the content of playerprefs if it is not deleted </remarks>
public class ClearPrefs : MonoBehaviour
{
    private static bool alreadyCleared = false;

    void Start()
    {
        if (alreadyCleared)
        {
            this.enabled = false;
            return;
        }

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs Deleted");

        alreadyCleared = true;
        this.enabled = false;
    }
}

