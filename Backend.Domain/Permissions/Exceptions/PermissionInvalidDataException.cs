
namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Exceptions;

/// <summary>
/// Represents an exception that is thrown when invalid data is encountered in the context of a permission operation.
/// </summary>
/// <remarks>This exception is typically used to indicate that the data provided for a permission-related
/// operation is invalid  or does not meet the expected format or requirements. It extends <see
/// cref="PermissionException"/> to provide  more specific error information.</remarks>
public class PermissionInvalidDataException : PermissionException
{
    /// <summary>
    /// Represents an exception that is thrown when invalid data is encountered in the context of a permission
    /// operation.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PermissionInvalidDataException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when invalid data is encountered in the context of permission handling.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
    /// specified.</param>
    public PermissionInvalidDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

}