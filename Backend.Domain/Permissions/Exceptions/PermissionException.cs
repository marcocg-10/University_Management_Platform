
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a permission-related error occurs.
/// </summary>
/// <remarks>This exception is typically used to indicate that an operation could not be completed due to
/// insufficient permissions or access rights. It provides constructors to specify an error message and optionally an
/// inner exception that caused the current exception.</remarks>
public class PermissionException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PermissionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionException"/> class with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
    /// specified.</param>
    public PermissionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
