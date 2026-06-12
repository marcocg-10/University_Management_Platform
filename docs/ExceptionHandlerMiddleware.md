# Exception Handler Middleware Documentation

## Overview

The `ExceptionHandlerMiddleware` provides centralized exception handling for ASP.NET Core applications. It catches unhandled exceptions across your application and converts them into structured HTTP responses with appropriate status codes.

## Features

- Centralized exception handling
- Custom exception handler support
- Consistent error response format
- Automatic logging of exceptions
- RFC 7807 Problem Details compliant responses

## Setup Instructions

### 1. Register the Middleware

Add the following code in the file **DependencyInjectionExtensions.cs** and call it from your `Program.cs`:

```csharp
// DependencyInjectionExtensions.cs
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCleanArchitectureExceptions(this IServiceCollection services)
    {
        // Register the global exception handling middleware
        services.AddTransient<ExceptionHandlerMiddleware>();

        // Register your domain-specific exception handlers
        services.AddTransient<IExceptionHandler, BuildingExceptionHandler>();

        // Add more custom handlers here
        // services.AddTransient<IExceptionHandler, UserExceptionHandler>();
        // services.AddTransient<IExceptionHandler, LearningSpaceExceptionHandler>();
        // services.AddTransient<IExceptionHandler, InteractiveComponentsExceptionHandler>();

        return services;
    }
}
```

### 2. Create Custom Exception Handlers

Implement the `IExceptionHandler` interface for your custom exceptions:

```csharp
public class YourCustomExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception)
    {
        return exception is YourCustomException;
    }

    public ExceptionResult Handle(Exception exception)
    {
        var customException = (YourCustomException)exception;
        return new ExceptionResult(
            StatusCodes.Status400BadRequest,  // Or appropriate status code
            "Error Type",                     // Type of the error
            "Error Title",                    // Short title
            "Detailed error message"          // User-friendly error description
        );
    }
}
```

## Response Format

The middleware returns errors in the RFC 7807 Problem Details format:

```json
{
    "type": "Error Type",
    "title": "Error Title",
    "detail": "Detailed error message",
    "status": 400  // HTTP status code
}
```

## Error Handling Priority

1. Custom Exception Handlers: The middleware first tries to find a matching custom handler
2. Default Handler: If no custom handler is found, returns a generic 500 Internal Server Error

## Best Practices

1. **Custom Exceptions**
   - Create specific exception types for different error scenarios
   - Include relevant error information in your custom exceptions

2. **Exception Handlers**
   - Keep handlers focused and single-purpose
   - Use appropriate HTTP status codes
   - Provide clear, user-friendly error messages
   - Don't expose sensitive information in error responses

3. **Logging**
   - The middleware automatically logs exceptions
   - Add contextual information in your custom handlers if needed

## Example Usage

### Custom Exception
```csharp
public class ResourceNotFoundException : Exception
{
    public string ResourceId { get; }

    public ResourceNotFoundException(string resourceId)
        : base($"Resource with ID {resourceId} was not found")
    {
        ResourceId = resourceId;
    }
}
```

### Custom Handler
```csharp
public class ResourceNotFoundExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception)
    {
        return exception is ResourceNotFoundException;
    }

    public ExceptionResult Handle(Exception exception)
    {
        var notFoundEx = (ResourceNotFoundException)exception;
        return new ExceptionResult(
            StatusCodes.Status404NotFound,
            "Resource.NotFound",
            "Resource Not Found",
            exception.Message
        );
    }
}
```

## Security Considerations

1. Don't expose sensitive information in error messages
2. Use appropriate status codes to avoid information leakage
3. Consider implementing rate limiting for error endpoints
4. Log security-related exceptions appropriately

## Troubleshooting

### Common Issues

1. **Multiple Handlers for Same Exception**
   - Handlers are evaluated in registration order
   - First matching handler is used

2. **Middleware Order**
   - Ensure ExceptionHandlerMiddleware is registered early in the pipeline
   - Place it before routing and other middleware that might throw exceptions

3. **Missing Handler Registration**
   - Verify all handlers are properly registered in DI container
   - Check handler scope (Transient/Scoped/Singleton)

### Debugging

- Enable detailed logging in development
- Check the logs for unhandled exceptions
- Verify custom handlers are being called as expected

## Contributing

When adding new exception handlers:

1. Create a new class implementing `IExceptionHandler`
2. Register it in the DI container
3. Add appropriate unit tests
4. Update documentation if needed

## Support

For questions or issues:
1. Check the existing documentation
2. Review the logs for detailed error information
3. Contact the development team