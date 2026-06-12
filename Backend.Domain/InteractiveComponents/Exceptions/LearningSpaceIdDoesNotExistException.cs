namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when the specified LearningSpaceId does not exist in the system.
/// Inherits from <see cref="InteractiveComponentException"/> to represent a validation error.
/// </summary>
public class LearningSpaceIdDoesNotExistException : InteractiveComponentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceIdDoesNotExistException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="learningSpaceId">The learning space id that caused the exception.</param>
    public LearningSpaceIdDoesNotExistException(int learningSpaceId)
        : base($"Learning space with ID {learningSpaceId} does not exist.")
    {
    }
}
