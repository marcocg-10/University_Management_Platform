namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when attempting to create or update an InteractiveComponent
/// with a PlateId that already exists in the system.
/// Inherits from <see cref="InteractiveComponentException"/> to represent a validation error.
/// </summary>
public class PlateIdAlreadyExistsException : InteractiveComponentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlateIdAlreadyExistsException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message describing the reason for the exception.</param>
    public PlateIdAlreadyExistsException(string message)
        : base(message)
    {
    }
}
