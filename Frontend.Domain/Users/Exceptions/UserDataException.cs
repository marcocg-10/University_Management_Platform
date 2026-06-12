namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;

/// <summary>
/// Exception thrown when a database operation fails for a User.
/// </summary>
public class UserDataException : UserException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDataException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public UserDataException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDataException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public UserDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}