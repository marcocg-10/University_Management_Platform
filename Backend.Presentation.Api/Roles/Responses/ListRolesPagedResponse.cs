using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

/// <summary>
/// Represents a paged response containing a collection of users and associated pagination metadata.
/// </summary>
/// <param name="Users">The collection of users included in the current page of the response.</param>
/// <param name="Metadata">The pagination metadata providing details about the current page, total items, and other pagination-related
/// information.</param>
public record ListRolesPagedResponse(
    IEnumerable<RoleDto> Role,
    PaginationMetadata Metadata);
