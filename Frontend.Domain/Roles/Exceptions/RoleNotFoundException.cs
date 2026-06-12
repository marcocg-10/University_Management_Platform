using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a specified role cannot be found.
/// </summary>
/// <remarks>This exception is typically used to indicate that an operation failed because the requested role does
/// not exist.</remarks>
public class RoleNotFoundException : RoleException
{
    /// <summary>
    /// Represents an exception that is thrown when a specified role cannot be found.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RoleNotFoundException(RoleName roleName)
        : base($"The role with the name '{roleName?.Value}' does not exist.")
    {
    }

    public RoleNotFoundException(int id)
        : base($"The role with the id '{id}' does not exist.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleNotFoundException"/> class with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
    /// specified.</param>
    public RoleNotFoundException(RoleName roleName, Exception innerException) 
        : base($"The role with the name '{roleName?.Value}' does not exist.", innerException)
    {
    }
}
