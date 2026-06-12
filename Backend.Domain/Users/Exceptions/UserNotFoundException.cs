using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a specified role cannot be found.
/// </summary>
/// <remarks>This exception is typically used to indicate that an operation failed because the requested role does
/// not exist.</remarks>
public class UserNotFoundException : UserException
{
    /// <summary>
    /// Represents an exception that is thrown when a specified role cannot be found.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UserNotFoundException(UserName userName)
        : base($"The user with the name '{userName?.Value}' does not exist.")
    {
    }

    public UserNotFoundException(UserId id)
        : base($"The user with official id '{id.Value}' does not exist.")
    {
    }

    public UserNotFoundException(int id)
        : base($"The user with internal id '{id}' does not exist.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified error message and a
    /// reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
    /// specified.</param>
    public UserNotFoundException(UserName userName, Exception innerException)
        : base($"The user with the name '{userName?.Value}' does not exist.", innerException)
    {
    }
}