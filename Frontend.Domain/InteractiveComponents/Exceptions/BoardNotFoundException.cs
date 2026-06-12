using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when a board id is not found.
/// Inherits from <see cref="ValidationException"/> to represent a validation error.
/// </summary>
[Serializable]
public class BoardNotFoundException : ValidationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BoardNotFoundException"/> 
    /// class with a specified error message.
    /// </summary>
    /// <param name="message">Message shown to the user about the exception</param>
    public BoardNotFoundException(string message)
        : base(message)
    {
    }
}
