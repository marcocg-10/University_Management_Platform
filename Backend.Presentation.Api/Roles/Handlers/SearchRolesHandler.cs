using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Handlers;

internal class SearchRolesHandler
{
    public static async Task<SearchRolesResponse> HandleAsync(
        [FromServices] IRoleService roleService,
        [FromQuery] string? name,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be at least 1.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 100.");
        }

        name ??= "";

        var (roles, totalCount) = await roleService.SearchRolesAsync(name, pageNumber, pageSize);

        var roleDtos = roles
            .Select(x => x.ToDto());

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var paginationMetadata = new PaginationMetadata(
            CurrentPage: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        );

        return new SearchRolesResponse(roleDtos, paginationMetadata);
    }
}