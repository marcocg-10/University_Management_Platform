using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;

/// <summary>
/// Defines a contract for handling exceptions and converting them into standardized results.
/// </summary>
public interface IExceptionHandler
{
    /// <summary>
    /// Determines whether the handler can process the specified exception.
    /// </summary>
    /// <param name="ex">The exception to evaluate.</param>
    /// <returns>True if the handler can handle the exception; otherwise, false.</returns>
    bool CanHandle(Exception ex);

    /// <summary>
    /// Handles the specified exception and returns a structured <see cref="ExceptionResult"/>.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>An <see cref="ExceptionResult"/> representing the error response.</returns>
    ExceptionResult Handle(Exception ex);
}
