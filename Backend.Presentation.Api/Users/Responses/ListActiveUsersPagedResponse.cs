using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

/// <summary>
/// Represents a paged response containing a collection of users and associated pagination metadata.
/// </summary>
/// <param name="Users">The collection of users included in the current page of the response.</param>
/// <param name="Metadata">The pagination metadata providing details about the current page, total items, and other pagination-related
/// information.</param>
public record ListActiveusersPagedResponse(
    IEnumerable<UserDto> Users,
    PaginationMetadata Metadata);
