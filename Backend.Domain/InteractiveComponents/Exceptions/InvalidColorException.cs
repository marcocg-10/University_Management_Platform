namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when a provided color for an InteractiveComponent is invalid.
/// Inherits from <see cref="InteractiveComponentException"/> to represent a validation-level error.
/// </summary>
public class InvalidColorException : InteractiveComponentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidColorException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message describing the reason for the exception.</param>
    public InvalidColorException(string message)
        : base(message)
    {
    }
}
