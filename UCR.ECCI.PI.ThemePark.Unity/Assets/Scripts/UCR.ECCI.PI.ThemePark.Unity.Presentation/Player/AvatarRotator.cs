using UnityEngine;

/// <summary>
/// Rotates the avatar's visual representation to face the direction of movement.
/// </summary>
/// <remarks>The rotation is smoothed using spherical interpolation (Slerp) to ensure a gradual transition. The
/// rotation speed can be adjusted using the <see cref="rotationSpeed"/> field. This class should be attached to a
/// GameObject with a Transform component, and the <see cref="avatarVisual"/> field must be assigned to the Transform
/// representing the avatar's visual appearance.</remarks>
public class AvatarRotator : MonoBehaviour
{
    public Transform avatarVisual;
    public float rotationSpeed = 10f;

    private Vector3 lastPosition;

    /// <summary>
    /// Initializes the starting position of the object.
    /// </summary>
    /// <remarks>This method sets the initial value of the object's position to its current position in the
    /// scene.</remarks>
    void Start()
    {
        lastPosition = transform.position;
    }

    /// <summary>
    /// Updates the rotation of the avatar to face the direction of movement.
    /// </summary>
    /// <remarks>This method calculates the movement direction based on the change in position since the last
    /// update. If the movement is significant, the avatar's rotation is smoothly interpolated toward the target
    /// direction using spherical linear interpolation (Slerp). The rotation speed is influenced by the <see
    /// cref="rotationSpeed"/>  and the elapsed time since the last frame.</remarks>
    void Update()
    {
        Vector3 movementDirection = transform.position - lastPosition;

        if (movementDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection.normalized);
            avatarVisual.rotation = Quaternion.Slerp(avatarVisual.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        lastPosition = transform.position;
    }
}
