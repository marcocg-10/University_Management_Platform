using System.Net;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Middlewares;

/// <summary>
/// Provides functionality to handle exceptions related to permission operations and map them to appropriate HTTP
/// responses.
/// </summary>
/// <remarks>This class is responsible for identifying and processing exceptions of type <see
/// cref="PermissionException"/> and its derived types. It maps specific exceptions to predefined HTTP status codes and
/// error details: <list type="bullet"> <item> <description><see cref="PermissionInvalidDataException"/> results in a
/// 400 Bad Request response.</description> </item> <item> <description><see cref="PermissionAlreadyExistsException"/>
/// results in a 409 Conflict response.</description> </item> <item> <description><see
/// cref="PermissionAlreadyAssignedException"/> results in a 409 Conflict response.</description> </item> </list> For
/// all other exceptions, a generic 500 Internal Server Error response is returned.</remarks>
internal class PermissionExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Determines whether the specified exception is a <see cref="PermissionException"/>.
    /// </summary>
    /// <param name="ex">The exception to evaluate. Cannot be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the exception is a <see cref="PermissionException"/>; otherwise, <see
    /// langword="false"/>.</returns>
    public bool CanHandle(Exception ex)
    {
        return ex is PermissionException;
    }

    /// <summary>
    /// Handles the provided exception and maps it to a corresponding <see cref="ExceptionResult"/>.
    /// </summary>
    /// <remarks>The method maps specific exception types to predefined HTTP status codes and error details:
    /// <list type="bullet"> <item> <description><see cref="PermissionInvalidDataException"/> results in a 400 Bad
    /// Request response.</description> </item> <item> <description><see cref="PermissionAlreadyExistsException"/>
    /// results in a 409 Conflict response.</description> </item> <item> <description><see
    /// cref="PermissionAlreadyAssignedException"/> results in a 409 Conflict response.</description> </item> </list>
    /// For all other exception types, the method returns a 500 Internal Server Error response with a generic
    /// message.</remarks>
    /// <param name="ex">The exception to handle. Must not be <see langword="null"/>.</param>
    /// <returns>An <see cref="ExceptionResult"/> representing the HTTP status code, error type, title, and message associated
    /// with the provided exception. If the exception type is not explicitly handled, a generic internal server error
    /// result is returned.</returns>
    public ExceptionResult Handle(Exception ex)
    {
        return ex switch
        {
            PermissionInvalidDataException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "ValidationError",
                "Bad Request",
                ex.Message
            ),
            PermissionAlreadyExistsException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "ConflictError",
                "Existing Permission",
                ex.Message
            ),
            PermissionNotFoundException => new ExceptionResult(
                (int)HttpStatusCode.NotFound,
                "NotFoundError",
                "Permission Not Found",
                ex.Message
            ),
            _ => new ExceptionResult(
                (int)HttpStatusCode.InternalServerError,
                "UnexpectedError",
                "Internal Server Error",
                "An unexpected error occurred"
            )
        };
    }   
}
