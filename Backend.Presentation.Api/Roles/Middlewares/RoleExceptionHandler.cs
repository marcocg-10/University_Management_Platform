using System.Net;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Middlewares;

/// <summary>
/// Provides functionality to evaluate and handle exceptions related to role operations.
/// </summary>
/// <remarks>This class implements the <see cref="IExceptionHandler"/> interface to determine whether an exception
/// is of type <see cref="RoleException"/> and to map such exceptions to appropriate HTTP responses.</remarks>
internal class RoleExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Determines whether the specified exception is of type <see cref="RoleException"/>.
    /// </summary>
    /// <param name="ex">The exception to evaluate. Cannot be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the exception is of type <see cref="RoleException"/>; otherwise, <see
    /// langword="false"/>.</returns>
    public bool CanHandle(Exception ex)
    {
        return ex is RoleException;
    }

    /// <summary>
    /// Handles the specified exception and maps it to an appropriate <see cref="ExceptionResult"/>.
    /// </summary>
    /// <param name="ex">The exception to handle. Must not be <see langword="null"/>.</param>
    /// <returns>An <see cref="ExceptionResult"/> representing the HTTP status code, error type, title, and message corresponding
    /// to the provided exception.  <list type="bullet"> <item> <description>Returns a result with status code <see
    /// cref="HttpStatusCode.BadRequest"/> for <see cref="RoleInvalidEntryException"/>.</description> </item> <item>
    /// <description>Returns a result with status code <see cref="HttpStatusCode.Conflict"/> for <see
    /// cref="RoleAlreadyExistsException"/>.</description> </item> <item> <description>Returns a result with status code
    /// <see cref="HttpStatusCode.InternalServerError"/> for all other exceptions.</description> </item> </list></returns>
    public ExceptionResult Handle(Exception ex)
    {
        return ex switch
        {
            RoleInvalidDataException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "ValidationError",
                "Bad Request",
                ex.Message
            ),
            RoleAlreadyExistsException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "ConflictError",
                "Existing Role",
                ex.Message
            ),
            PermissionAlreadyAssignedException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "ConflictError",
                "Permission Already Assigned",
                ex.Message
            ),
            AssignablePermissionNotFoundException => new ExceptionResult(
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
