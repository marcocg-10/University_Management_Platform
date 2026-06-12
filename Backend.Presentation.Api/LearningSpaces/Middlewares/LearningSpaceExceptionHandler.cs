using System.Net;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Middlewares;

/// <summary>
/// Provides exception handling logic specific to exceptions related to learning spaces.
/// </summary>
/// <remarks>This class implements the <see cref="IExceptionHandler"/> interface to handle exceptions of type <see
/// cref="LearningSpaceException"/> and its derived types. It maps these exceptions to appropriate HTTP status codes and
/// error messages for consistent error responses.</remarks>
internal class LearningSpaceExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Determines whether the specified exception can be handled by this handler.
    /// </summary>
    /// <param name="ex">The exception to evaluate. Must not be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the exception is of type <see cref="LearningSpaceException"/>; otherwise, <see
    /// langword="false"/>.</returns>
    public bool CanHandle(Exception ex)
    {
        return ex is LearningSpaceException;
    }

    /// <summary>
    /// Maps an exception to a corresponding <see cref="ExceptionResult"/> object that represents an HTTP response with
    /// an appropriate status code, error type,  and message.
    /// </summary>
    /// <returns>An <see cref="ExceptionResult"/> object containing the HTTP status code,  error type, title, and message
    /// corresponding to the provided exception.</returns>
    public ExceptionResult Handle(Exception ex)
    {
        return ex switch
        {
            // Handle data exception (invalid data provided).
            LearningSpaceDataException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "ValidationError",
                "Bad Request",
                ex.Message
            ),

            // Handle duplicated learning space exception.
            LearningSpaceAlreadyExistsException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "ConflictError",
                "Learning Space Already Exists",
                ex.Message
            ),

            // Handle a not found learning space exception.
            LearningSpaceNotFoundException => new ExceptionResult(
                (int)HttpStatusCode.NotFound,
                "NotFoundError",
                "Learning Space Not Found",
                ex.Message
            ),

            // Handle generic exception.
            _ => new ExceptionResult(
                (int)HttpStatusCode.InternalServerError,
                "UnexpectedError",
                "Internal Server Error",
                "An unexpected error occurred"
            )
        };
    }
}
