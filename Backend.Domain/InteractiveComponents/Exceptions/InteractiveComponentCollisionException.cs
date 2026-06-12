namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when a collision occurs in an interactive component.
/// </summary>
public class InteractiveComponentCollisionException : InteractiveComponentException
{
    /// <summary>
    /// Represents an exception that is thrown when a collision occurs in an interactive component.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InteractiveComponentCollisionException(string message)
        : base(message)
    {
    }
}
