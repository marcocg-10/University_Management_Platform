using System.Net;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Middleware;

/// <summary>
/// Handles exceptions specific to the <c>InteractiveComponents</c> domain, mapping domain exceptions
/// to structured <see cref="ExceptionResult"/> responses that are serialized as JSON and returned to clients.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IExceptionHandler"/> contract and is registered in the global
/// <c>ExceptionHandlerMiddleware</c>.  
/// </remarks>
internal class InteractiveComponentExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Determines whether this handler is responsible for handling the provided exception.
    /// </summary>
    /// <param name="ex">The exception to evaluate.</param>
    /// <returns>
    /// <c>true</c> if the exception is an <see cref="InteractiveComponentException"/> or a derived type;
    /// otherwise, <c>false</c>.
    /// </returns>
    public bool CanHandle(Exception ex)
    {
        return ex is InteractiveComponentException;
    }

    /// <summary>
    /// Maps a specific <see cref="InteractiveComponentException"/> to an HTTP response
    /// using a standardized <see cref="ExceptionResult"/> object.
    /// </summary>
    /// <param name="ex">The domain exception to handle.</param>
    /// <returns>
    /// An <see cref="ExceptionResult"/> containing:
    /// <list type="bullet">
    ///   <item><description>HTTP status code corresponding to the exception type</description></item>
    ///   <item><description>Machine-readable <c>type</c> identifier (e.g., <c>InvalidColorError</c>)</description></item>
    ///   <item><description>User-friendly <c>title</c> (e.g., “Invalid Color”)</description></item>
    ///   <item><description>Detailed message from the exception itself (<see cref="Exception.Message"/>)</description></item>
    /// </list>
    /// </returns>
    public ExceptionResult Handle(Exception ex)
    {
        return ex switch
        {
            BoardDeletionException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "BoardDeletionError",
                "Board Deletion Error",
                ex.Message
            ),

            BoardNotFoundException => new ExceptionResult(
                (int)HttpStatusCode.NotFound,
                "BoardNotFoundError",
                "Board Not Found",
                ex.Message
            ),

            InteractiveComponentCollisionException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "InteractiveComponentCollisionError",
                "Interactive Component Collision Error",
                ex.Message
            ),

            InteractiveComponentContainmentException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InteractiveComponentContainmentError",
                "Interactive Component Containment Error",
                ex.Message
            ),

            InteractiveComponentNotFoundException => new ExceptionResult(
                (int)HttpStatusCode.NotFound,
                "InteractiveComponentNotFoundError",
                "Interactive Component Not Found",
                ex.Message
            ),

            InvalidBrightnessException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InvalidBrightnessError",
                "Invalid Brightness",
                ex.Message
            ),

            InvalidColorException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InvalidColorError",
                "Invalid Color",
                ex.Message
            ),

            InvalidCoordinatesException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InvalidCoordinatesError",
                "Invalid Coordinates",
                ex.Message
            ),

            InvalidDimensionsException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InvalidDimensionsError",
                "Invalid Dimensions",
                ex.Message
            ),

            InvalidLearningSpaceIdException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InvalidLearningSpaceIdError",
                "Invalid Learning Space ID",
                ex.Message
            ),

            InvalidPlateIdException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InvalidPlateIdError",
                "Invalid Plate ID",
                ex.Message
            ),

            InvalidResolutionException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InvalidResolutionError",
                "Invalid Resolution",
                ex.Message
            ),

            LearningSpaceIdDoesNotExistException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "LearningSpaceIdDoesNotExistError",
                "Learning Space ID Does Not Exist",
                ex.Message
            ),

            PlateIdAlreadyExistsException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "PlateIdAlreadyExistsError",
                "Plate ID Already Exists",
                ex.Message
            ),

            InvalidRotationsException => new ExceptionResult(
                (int)HttpStatusCode.BadRequest,
                "InvalidRotationsError",
                "Invalid Rotations",
                ex.Message
            ),

            _ => new ExceptionResult(
                (int)HttpStatusCode.InternalServerError,
                "UnexpectedError",
                "Internal Server Error",
                "An unexpected error occurred"
            ),
        };
    }
}
