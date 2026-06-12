using Microsoft.EntityFrameworkCore;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

/// <summary>
/// Exception thrown when a Board interactive component could not be deleted.
/// Inherits from <see cref="InteractiveComponentException"/> to represent a validation-level error.
/// </summary>
public class BoardDeletionException : InteractiveComponentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BoardDeletionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message describing the reason for the exception.</param>
    public BoardDeletionException(string message, DbUpdateException ex)
        : base(message)
    {
    }
}
