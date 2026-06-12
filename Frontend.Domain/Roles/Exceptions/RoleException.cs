using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Exceptions;

/// <summary>
/// Represents an exception that occurs when an error related to roles is encountered in the domain layer.
/// </summary>
/// <remarks>This exception is typically thrown to indicate a domain-specific issue related to roles, such as
/// invalid role assignments or operations that violate role-related business rules.</remarks>
public class RoleException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RoleException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleException"/> class with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
    /// specified.</param>
    public RoleException(string message, Exception innerException) : base(message, innerException) { }
}
