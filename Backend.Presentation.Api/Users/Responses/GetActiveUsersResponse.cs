using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

/// <summary>
/// Represents the response containing a collection of active users.
/// </summary>
/// <remarks>This record is typically used as the result of an operation that retrieves active users.</remarks>
public record GetActiveUsersResponse(IEnumerable<UserIdDto> Users);

