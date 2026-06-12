namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when a provided Brightness for an InteractiveComponent is invalid.
/// Inherits from <see cref="InteractiveComponentException"/> to represent a validation error.
/// </summary>
public class InvalidBrightnessException : InteractiveComponentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidBrightnessException"/> 
    /// class with a specified error message.
    /// </summary>
    /// <param name="message">The message describing the reason for the exception.</param>
    public InvalidBrightnessException(string message)
        : base(message)
    {
    }
}
