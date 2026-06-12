namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;

/// <summary>
/// Exception thrown when getting active users (listUsers) fails.
/// </summary>
public class ListUsersException : UserException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ListUsersException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ListUsersException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListUsersException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public ListUsersException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}