using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player movement and input processing in a 3D environment.
/// </summary>
/// <remarks>This class is responsible for interpreting player input and applying movement to the player
/// character. It uses Unity's physics system to move the player based on input from the associated input action. The
/// movement direction is calculated relative to the camera's orientation.</remarks>
public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    private Vector2 inputMovement;
    private float defaultSpeed = 5;
    private bool runPressed = false;
    private bool helloPressed = false;

    /// <summary>
    /// Initializes the component by retrieving the attached <see cref="Rigidbody"/> instance.
    /// </summary>
    /// <remarks>This method is called automatically by Unity during the initialization phase of the
    /// GameObject. It ensures that the <see cref="Rigidbody"/> component required for physics interactions is
    /// assigned.</remarks>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Handles movement input by reading the current value from the input context.
    /// </summary>
    /// <param name="context">The input action callback context containing the movement data.  Must provide a valid <see cref="Vector2"/>
    /// value.</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles interaction input when the interact action is performed.
    /// </summary>
    /// <param name="context"></param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("E pressed in PlayerHandler");
            PlayerInteractionManager.Instance.Interact();
        }
    }

    /// <summary>
    /// Handles the "Hello" input action, updating the animator state based on the input context.
    /// </summary>
    /// <remarks>When the input action is performed, the "Hello" parameter in the animator is set to <see
    /// langword="true"/>. When the input action is canceled, the "Hello" parameter is set to <see
    /// langword="false"/>.</remarks>
    /// <param name="context">The input action context that provides information about the current state of the input, such as whether it was
    /// performed or canceled.</param>
    public void OnHello(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            helloPressed = true;
        }
        else if (context.canceled)
        {
            helloPressed = false;
        }

    }

    /// <summary>
    /// Handles the player's running state based on the input action context.
    /// </summary>
    /// <remarks>When the input action is performed, the player's running animation is triggered, and the
    /// speed is increased. When the input action is canceled, the running animation is stopped, and the speed is reset
    /// to its default value.</remarks>
    /// <param name="context">The input action context that provides information about the current state of the input,  such as whether the
    /// action was performed or canceled.</param>
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            runPressed = true;
            playerSpeed = defaultSpeed * 1.8f;
        }
        else if (context.canceled)
        {
            runPressed = false;
            playerSpeed = defaultSpeed;
        }

    }

    /// <summary>
    /// Updates the player's movement and animation state at a fixed time interval.
    /// </summary>
    /// <remarks>This method calculates the player's movement direction based on the camera's orientation  and
    /// the input values, then applies the calculated velocity to the player's rigidbody.  It also updates the animation
    /// parameter to reflect the player's movement speed.</remarks>
    private void FixedUpdate()
    {
        // Prevent movement while a board is focused
        if (PlayerInteractionManager.Instance != null && PlayerInteractionManager.Instance.IsBoardFocused)
        {
            rb.velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * inputMovement.y + camRight * inputMovement.x;

        rb.velocity = moveDirection * playerSpeed;

        float horizontalSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        animator.SetFloat("Speed", horizontalSpeed);

        animator.SetBool("Run", runPressed && horizontalSpeed > 0.1f);
        animator.SetBool("Hello", helloPressed && horizontalSpeed < 0.1f);
    }
}
