using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;

/// <summary>
/// Handles the retrieval of a paginated list of users.
/// </summary>
/// <remarks>This method retrieves a specific page of users from the data source, along with metadata about the
/// pagination state. The users are mapped to DTOs before being returned in the response.</remarks>
internal class ListActiveUsersPagedHandler
{
    /// <summary>
    /// Retrieves a paginated list of users and associated pagination metadata.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be 1 or greater.</param>
    /// <param name="pageSize">The number of items per page. Must be between 1 and 100, inclusive.</param>
    /// <param name="userService">The service used to retrieve the users. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="ListBoardsPagedResponse"/> containing the list of users for the specified page and the associated
    /// pagination metadata.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> is less than 1, or if <paramref name="pageSize"/> is not between 1 and
    /// 100.</exception>
    public static async Task<ListActiveusersPagedResponse> HandleAsync(
        [FromServices] IUserService userService,
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

        var (users, totalCount) = await userService
            .ListActiveUsersPagedAsync(pageNumber, pageSize);

        var userDtos = users
            .Select(user => user.ToDto());

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var paginationMetadata = new PaginationMetadata(
            CurrentPage: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        );

        return new ListActiveusersPagedResponse(userDtos, paginationMetadata);
    }
}