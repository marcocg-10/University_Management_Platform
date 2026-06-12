using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
//using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

/// <summary>
/// Provides functionality to handle the retrieval of active users.
/// </summary>
/// <remarks>This class contains a method to asynchronously retrieve a list of active users from the provided user
/// service and map them to a response object.</remarks>
public static class GetActiveUsersHandler
{

    /// <summary>
    /// Retrieves the list of active users and maps them to a response object.
    /// </summary>
    /// <param name="usersService">The service used to retrieve active users.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a  A <see
    /// cref="GetActiveUsersResponse"/> object with the mapped active users.</returns>
    public static async Task<GetActiveUsersResponse> HandleAsync(IUserService usersService)
    {
        var users = await usersService.GetActiveUsersAsync();

        return new GetActiveUsersResponse(
            users.Select(UserDtoMapper.ToIdDto));
    }
}
