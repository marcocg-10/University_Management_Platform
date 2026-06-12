using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Exceptions.Classroom;

/// <summary>
/// Exception thrown when a classroom with the specified identifier is not found.
/// </summary>
public class ClassroomNotFoundException : DomainException
{
    /// <summary>
    /// Gets the identifier of the classroom that was not found.
    /// </summary>
    public int ClassroomId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClassroomNotFoundException"/> class.
    /// </summary>
    /// <param name="classroomId">The identifier of the classroom that was not found.</param>
    public ClassroomNotFoundException(int classroomId)
        : base($"Classroom with ID {classroomId} was not found.")
    {
        ClassroomId = classroomId;
    }
}
