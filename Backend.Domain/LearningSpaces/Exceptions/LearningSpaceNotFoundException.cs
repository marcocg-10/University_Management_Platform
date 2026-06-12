namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;

public class LearningSpaceNotFoundException : LearningSpaceException
{
    /// <summary>
    /// Gets the identifier of the learning space that was not found.
    /// </summary>
    public int LearningSpaceId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceNotFoundException"/> class.
    /// </summary>
    /// <param name="learningSpaceId">The identifier of the learning space that was not found.</param>
    public LearningSpaceNotFoundException(int learningSpaceId)
        : base($"Learning space with ID {learningSpaceId} was not found.")
    {
        LearningSpaceId = learningSpaceId;
    }
}
