using UnityEngine;

/// <summary>
/// Singleton session component used to persist building-related data across scenes.
/// - Stores the currently selected BuildingId.
/// - Ensures only one instance exists at runtime.
/// - Survives scene changes using DontDestroyOnLoad.
/// </summary>
public class BuildingSession : MonoBehaviour
{
    /// <summary>
    /// Global singleton instance of the BuildingSession.
    /// Accessible from anywhere in the project.
    /// </summary>
    public static BuildingSession Instance { get; private set; }

    /// <summary>
    /// Identifier of the building currently selected or entered by the player.
    /// Stored as a string for flexibility (can represent numeric or alphanumeric IDs).
    /// </summary>
    public int BuildingId { get; set; }

    /// <summary>
    /// Unity lifecycle method called when the object is first created.
    /// Ensures that only one instance of BuildingSession exists:
    /// - If another instance already exists, this duplicate is destroyed.
    /// - Otherwise, this instance is set as the global singleton and marked
    ///   to persist across scene loads.
    /// </summary>
    private void Start()
    {
        // If an instance already exists and it's not this one, destroy this duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign this as the singleton instance
        Instance = this;

        // Prevent this object from being destroyed when loading new scenes
        DontDestroyOnLoad(gameObject);
    }
}
