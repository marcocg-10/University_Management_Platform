﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Middlewares;

/// <summary>
/// Middleware responsible for handling exceptions thrown during HTTP request processing.
/// Provides centralized exception handling for building-related operations and general exceptions,
/// converting them into appropriate HTTP responses with proper status codes and error details.
/// </summary>
public class ExceptionHandlerMiddleware : IMiddleware
{

    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    /// <summary>
    /// Collection of exception handlers to process specific exception types.
    /// </summary>
    private readonly IEnumerable<IExceptionHandler> _handlers;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for recording errors and diagnostic information.</param>

    public ExceptionHandlerMiddleware(
        ILogger<ExceptionHandlerMiddleware> logger,
        IEnumerable<IExceptionHandler> handlers
     )
    {
        _logger = logger;
        _handlers = handlers;
    }

    /// <summary>
    /// Processes the HTTP context and catches any exceptions that occur during request processing.
    /// Converts exceptions into appropriate HTTP responses with proper status codes and error details.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
    /// <param name="next">The next middleware delegate in the pipeline.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during request processing");

            var problem = HandleException(ex);

            context.Response.StatusCode = problem.StatusCode;
            context.Response.ContentType = "application/json";

            if (problem.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                await context.Response.WriteAsJsonAsync(
                    new ValidationErrorResponse(problem.Detail)
                );
            }
            else if (problem.StatusCode == (int)HttpStatusCode.Conflict)
            {
                await context.Response.WriteAsJsonAsync(
                    new ConflictErrorResponse(problem.Detail)
                );
            }
            else
            {
                await context.Response.WriteAsJsonAsync(
                    new ErrorResponse(problem.Detail)
                );
            }
        }
    }

    /// <summary>
    /// Handles any exception and determines the appropriate response based on the exception type.
    /// Attempts to handle the exception using registered handlers, then falls back to generic error handling.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>An <see cref="ExceptionResult"/> containing response details for the exception.</returns>
    protected ExceptionResult HandleException(Exception ex)
    {
       foreach (var handler in _handlers)
        {
            if (handler.CanHandle(ex))
            {
                return handler.Handle(ex);
            }
        }
        // Generic exception handling
        return ex switch
        {
            // Handle known exceptions here if needed
            _ => new ExceptionResult(
                (int)HttpStatusCode.InternalServerError,
                "Server Error",
                "Internal Server Error",
                "An unexpected error occurred."

            ),
        };
    }
}