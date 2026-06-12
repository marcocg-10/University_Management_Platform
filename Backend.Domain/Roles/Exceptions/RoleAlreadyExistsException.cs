namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an attempt is made to create a role that already exists.
/// </summary>
/// <remarks>This exception is typically thrown during role creation operations to indicate that a role with the
/// specified name already exists in the system. It provides the name of the conflicting role for further
/// context.</remarks>
public class RoleAlreadyExistsException : RoleException
{

    /// <summary>
    /// Represents an exception that is thrown when attempting to create a role that already exists.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RoleAlreadyExistsException(string message) : base(message) { }
}
