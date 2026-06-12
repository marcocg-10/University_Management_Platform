using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

/// <summary>
/// Represents the successful response when retrieving a user by Azure Object Identifier.
/// </summary>
/// <param name="User">The user information including IdKey and other details.</param>
public record GetUserByAzureObjectIdentifierResponse(UserIdDto User);

/// <summary>
/// Represents the error response when retrieving a user by Azure Object Identifier fails.
/// </summary>
/// <param name="ErrorMessage">A descriptive error message explaining what went wrong.</param>
/// <param name="ErrorCode">A code that categorizes the type of error that occurred.</param>
public record GetUserByAzureObjectIdentifierErrorResponse(string ErrorMessage, string ErrorCode);
