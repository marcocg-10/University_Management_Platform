using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Dtos;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

/// <summary>
/// Represents the response containing a collection of users.
/// </summary>
/// <remarks>This record is typically used as the result of an operation that retrieves users.</remarks>
/// <param name="Users">A collection of <see cref="UserIdDto"/> objects representing the users. The collection may be empty if no
/// users are found.</param>
public record SearchRolesResponse(
    IEnumerable<RoleDto> Roles,
    PaginationMetadata Metadata);

