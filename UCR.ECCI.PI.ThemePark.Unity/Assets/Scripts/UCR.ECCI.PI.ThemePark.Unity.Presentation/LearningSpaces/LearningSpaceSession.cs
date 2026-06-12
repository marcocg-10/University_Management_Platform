using UnityEngine;

/// <summary>
/// Manages the state and behavior of the current learning space session.
/// </summary>
/// <remarks>This class follows the singleton pattern, ensuring that only one instance exists throughout the
/// application's lifecycle. The instance persists across scene loads. Use the <see cref="Instance"/> property to access
/// the singleton instance.</remarks>

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces
{
    public class LearningSpaceSession : MonoBehaviour
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="LearningSpaceSession"/> class.
        /// </summary>
        public static LearningSpaceSession Instance { get; private set; }

        /// <summary>
        /// Gets or sets the identifier of the currently selected learning space.
        /// </summary>
        public int SelectedLearningSpaceId { get; set; }  // Set when clicking on a learning space

        public string SelectedLearningSpaceType { get; set; } // Set when clicking on a learning space

        /// <summary>
        /// Initializes the singleton instance of the class and ensures it persists across scene loads.
        /// </summary>
        /// <remarks>If an instance of this class already exists and is not the current object, the existing
        /// object is destroyed.  Otherwise, this object is set as the singleton instance and marked to not be destroyed
        /// when loading new scenes.</remarks>
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}