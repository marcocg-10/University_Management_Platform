namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when the provided dimensions for an InteractiveComponent are invalid.
/// Inherits from <see cref="InteractiveComponentException"/> to represent a validation error.
/// </summary>
public class InvalidDimensionsException : InteractiveComponentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidDimensionsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message describing the reason for the exception.</param>
    public InvalidDimensionsException(string message)
        : base(message)
    {
    }
}