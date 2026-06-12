using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Exceptions.Laboratory;

/// <summary>
/// Exception thrown when a laboratory with the specified identifier is not found.
/// </summary>
public class LaboratoryNotFoundException : DomainException
{
    /// <summary>
    /// Gets the identifier of the laboratory that was not found.
    /// </summary>
    public int LaboratoryId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LaboratoryNotFoundException"/> class.
    /// </summary>
    /// <param name="laboratoryId">The identifier of the laboratory that was not found.</param>
    public LaboratoryNotFoundException(int laboratoryId)
        : base($"Laboratory with ID {laboratoryId} was not found.")
    {
        LaboratoryId = laboratoryId;
    }
}
