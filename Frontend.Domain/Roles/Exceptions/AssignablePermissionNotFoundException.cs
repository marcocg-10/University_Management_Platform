using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a specific permission cannot be found.
/// </summary>
/// <remarks>This exception is typically used to indicate that an operation requiring a specific permission failed
/// because the requested permission does not exist or is not recognized.</remarks>
public class AssignablePermissionNotFoundException : RoleException
{

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignablePermissionNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// </summary>
    /// <param name="message"></param>
    public AssignablePermissionNotFoundException(PermissionName name)
        : base($"The permission with the name '{name?.Value}' does not exist.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignablePermissionNotFoundException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
    /// specified.</param>
    public AssignablePermissionNotFoundException(PermissionName name, Exception innerException)
        : base($"The permission with the name '{name?.Value}' does not exist.", innerException)
    {
    }
}
