using UnityEngine;
using ReadyPlayerMe.AvatarCreator;

/// <summary>
/// Controls the visibility of the save button based on the user's authentication state.
/// </summary>
/// <remarks>This class listens to authentication events from the <see cref="AuthManager"/> to determine whether
/// the save button should be shown or hidden. The button is visible when the user is signed in and hidden when the user
/// is signed out. It also updates the button's visibility when the session is refreshed.</remarks>
public class SaveButtonVisibilityController : MonoBehaviour
{
    /// <summary>
    /// Represents the button GameObject used to save data, which can be shown or hidden as needed.
    /// </summary>
    [SerializeField] private GameObject saveButton; // the button GameObject to show/hide

    /// <summary>
    /// Subscribes to authentication events and updates the visibility of the button based on the current sign-in state.
    /// </summary>
    /// <remarks>This method is called when the component is enabled. It attaches event handlers to
    /// authentication events  to manage the visibility of the button dynamically. The initial visibility is set based
    /// on the current  sign-in state.</remarks>
    private void OnEnable()
    {
        AuthManager.OnSignedIn += ShowOnSignIn;
        AuthManager.OnSessionRefreshed += ShowOnSignIn; // treat refresh as logged-in
        AuthManager.OnSignedOut += HideButton;

        // Initial state when entering customization UI
        SetVisible(AuthManager.IsSignedIn);
    }

    /// <summary>
    /// Unsubscribes from authentication-related events when the object is disabled.
    /// </summary>
    /// <remarks>This method ensures that event handlers for authentication events, such as sign-in, session
    /// refresh,  and sign-out, are detached when the object is disabled. This helps prevent memory leaks and unintended
    /// behavior caused by lingering event subscriptions.</remarks>
    private void OnDisable()
    {
        AuthManager.OnSignedIn -= ShowOnSignIn;
        AuthManager.OnSessionRefreshed -= ShowOnSignIn;
        AuthManager.OnSignedOut -= HideButton;
    }

    /// <summary>
    /// Displays the user interface element associated with the sign-in process.
    /// </summary>
    /// <param name="_">The current user session. This parameter is not used in the method.</param>
    private void ShowOnSignIn(UserSession _)
    {
        SetVisible(true);
    }

    /// <summary>
    /// Hides the button by setting its visibility to false.
    /// </summary>
    /// <remarks>This method ensures that the button is no longer visible to the user.  It is typically used
    /// to programmatically control the button's visibility.</remarks>
    private void HideButton()
    {
        SetVisible(false);
    }

    /// <summary>
    /// Sets the visibility of the save button.
    /// </summary>
    /// <param name="visible">A value indicating whether the save button should be visible.  <see langword="true"/> to make the save button
    /// visible; otherwise, <see langword="false"/>.</param>
    private void SetVisible(bool visible)
    {
        if (saveButton != null)
            saveButton.SetActive(visible);
    }
}
