using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an invalid rotation value is encountered.
/// </summary>
/// <remarks>This exception is typically used to indicate that a rotation value provided to an interactive
/// component is outside the acceptable range or is otherwise invalid.</remarks>
public class InvalidRotationsException : ValidationException
{
    /// <summary>
    /// Represents an exception that is thrown when an invalid rotation operation is attempted.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidRotationsException(string message)
        : base(message)
    {
    }
}