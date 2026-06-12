using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when the provided coordinates for an InteractiveComponent are invalid.
/// Inherits from <see cref="ValidationException"/> to represent a validation error.
/// </summary>
public class InvalidCoordinatesException : ValidationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCoordinatesException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message describing the reason for the exception.</param>
    public InvalidCoordinatesException(string message)
        : base(message)
    {
    }
}
