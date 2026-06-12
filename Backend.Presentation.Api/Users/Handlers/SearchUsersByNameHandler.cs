using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

internal class SearchUsersByNameHandler
{
    public static async Task<SearchUsersByNameResponse> HandleAsync(
        [FromServices] IUserService userService,
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

        var (users, totalCount) = await userService.SearchUsersAsync(name, pageNumber, pageSize);

        var userDtos = users
            .Select(user => user.ToDto());

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var paginationMetadata = new PaginationMetadata(
            CurrentPage: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        );

        return new SearchUsersByNameResponse(userDtos, paginationMetadata);
    }
}