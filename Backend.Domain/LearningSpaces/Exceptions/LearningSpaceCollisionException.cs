namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;

/// <summary>
/// Exception thrown when a collision occurs between learning spaces.
/// </summary>
public class LearningSpaceCollisionException : LearningSpaceException
{
    /// <summary>
    /// Represents an exception that is thrown when a collision occurs between learning spaces.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public LearningSpaceCollisionException(string message)
        : base(message)
    {
    }
}
