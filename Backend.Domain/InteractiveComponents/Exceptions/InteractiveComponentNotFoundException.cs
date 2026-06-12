namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when a Interactive Component is not found in the system.
/// Inherits from <see cref="InteractiveComponentException"/> to represent a validation-level error.
/// </summary>
public class InteractiveComponentNotFoundException : InteractiveComponentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InteractiveComponentNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message describing the reason for the exception.</param>
    public InteractiveComponentNotFoundException(string message)
        : base(message)
    {
    }
}
