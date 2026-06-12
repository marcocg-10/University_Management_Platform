using System.Net; 
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Middlewares;
internal class UsersExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Determines whether this handler can process the given exception.
    /// </summary>
    /// <param name="ex">The exception to evaluate.</param>
    /// <returns>True if the exception is a <see cref="UserException"/>; otherwise, false.</returns>
    public bool CanHandle(Exception ex)
    {
        return ex is UserException;
    }

    public ExceptionResult Handle(Exception ex)
    {
        return ex switch
        {
            UserDataException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "ValidationError",
                "Bad Request",
                ex.Message
            ),
            DuplicateEmailException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "ConflictError",
                "Existing User",
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

