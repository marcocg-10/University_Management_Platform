namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;

/// <summary>
/// Represents the result of exception handling containing HTTP response details.
/// </summary>
/// <param name="StatusCode">The HTTP status code to return</param>
/// <param name="Type">The type of error that occurred</param>
/// <param name="Title">A brief title describing the error</param>
/// <param name="Detail">Detailed error message</param>
public record struct ExceptionResult(int StatusCode, string Type, string Title, string Detail);