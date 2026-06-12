﻿using System.Net;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Middlewares;

/// <summary>
/// Handles exceptions related to building operations and maps them to appropriate HTTP responses.
/// </summary>
internal class BuildingExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Determines whether this handler can process the given exception.
    /// </summary>
    /// <param name="ex">The exception to evaluate.</param>
    /// <returns>True if the exception is a <see cref="BuildingException"/>; otherwise, false.</returns>
    public bool CanHandle(Exception ex)
    {
        return ex is BuildingException;
    }

    /// <summary>
    /// Converts a building-related exception into a standardized <see cref="ExceptionResult"/> response.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>An <see cref="ExceptionResult"/> containing status code, error type, title, and message.</returns>
    public ExceptionResult Handle(Exception ex)
    {
        Console.WriteLine(ex.GetType());
        return ex switch
        {
            BuildingDataException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "ValidationError",
                "Bad Request",
                ex.Message
            ),

            DuplicateBuildingException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "ConflictError",
                "Existing Building",
                ex.Message
            ),
            BuildingCollisionException => new ExceptionResult(
                (int)HttpStatusCode.Conflict,
                "ConflictError",
                "Collision Detected",
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