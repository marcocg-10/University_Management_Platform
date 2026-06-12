namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;

/// <summary>
/// Exception thrown when a database operation fails for a learning space.
/// </summary>
public class LearningSpaceDataException : LearningSpaceException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceDataException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public LearningSpaceDataException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceDataException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public LearningSpaceDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
