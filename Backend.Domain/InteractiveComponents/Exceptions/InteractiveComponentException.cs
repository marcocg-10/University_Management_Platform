using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when a collision occurs in an interactive component.
/// </summary>
public class InteractiveComponentException : DomainException
{
    /// <summary>
    /// Represents an exception that is thrown when a collision occurs in an interactive component.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InteractiveComponentException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when a collision occurs in an interactive component.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The inner exception of the error.</param>
    public InteractiveComponentException(string message, Exception innerException)
        : base(message, innerException) { }
}
