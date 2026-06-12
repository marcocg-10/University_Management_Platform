namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when a Board interactive component is not found in the system.
/// Inherits from <see cref="InteractiveComponentException"/> to represent a validation-level error.
/// </summary>
public class BoardNotFoundException : InteractiveComponentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BoardNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="plateId">The plateId that caused the exception.</param>
    public BoardNotFoundException(string plateId)
        : base($"Board with Plate ID {plateId} was not found.")
    {
    }
}
