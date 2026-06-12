namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Represents an exception that occurs when an interactive component containment operation fails.
/// </summary>
/// <remarks>This exception is typically thrown when there is an issue related to the containment, for example:
/// If an Interactive Component is not fully inside a Learning Space</remarks>
public class InteractiveComponentContainmentException : InteractiveComponentException
{
    /// <summary>
    /// Represents an exception that occurs when there is an issue with the containment of an interactive component.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InteractiveComponentContainmentException(string message)
        : base(message)
    {
    }
}
