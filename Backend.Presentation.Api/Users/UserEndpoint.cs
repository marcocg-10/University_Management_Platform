using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users;

/// <summary>
/// Provides extension methods for mapping user-related endpoints to an <see cref="IEndpointRouteBuilder"/>.
/// </summary>
/// <remarks>This class includes methods for registering endpoints that handle user-related operations, such as
/// retrieving active users and creating new users. The endpoints are mapped to their respective handlers and configured
/// with appropriate route names.</remarks>
internal static class UserEndpoints
{

    /// <summary>
    /// Maps user-related endpoints to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <remarks>This method registers the following endpoints: <list type="bullet"> <item>
    /// <description><c>GET /listUsers</c>: Retrieves a list of active users.</description> </item> <item>
    /// <description><c>GET /listUsers-protected</c>: Require authentication, then retrieves a list of active users.</description> </item> <item>
    /// <description><c>POST /createUsers</c>: Creates new users based on the provided data.</description> </item>
    /// </list></remarks>
    /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to which the endpoints will be added.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> instance with the user-related endpoints mapped.</returns>
    internal static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        routes
            .MapGet("/users", GetActiveUsersHandler.HandleAsync)
            .WithName("GetActiveUsers")
            .RequireAuthorization("ListUsers");

        routes
            .MapGet("/searchUsers", SearchUsersByNameHandler.HandleAsync)
            .WithName("SearchUsers")
            .RequireAuthorization("ListUsers");

        routes
            .MapPost("/users", CreateUsersHandler.HandleAsync)
            .WithName("CreateUsers")
            .Produces<CreateUserResponse>(StatusCodes.Status201Created)
            .Produces<CreateUserResponse>(StatusCodes.Status409Conflict)
            .Produces<CreateUserResponse>(StatusCodes.Status400BadRequest)
            .Produces<ExceptionResult>(StatusCodes.Status500InternalServerError);

        routes
            .MapPut("/asignRoleToUser/{userId}", Handlers.AssignRoleHandler.HandleAsync)
            .WithName("AssignRoleToUser").RequireAuthorization("AssignRole")
            .Produces<AssignRoleToUserResponse>(StatusCodes.Status200OK)
            .Produces<AssignRoleToUserResponse>(StatusCodes.Status404NotFound)
            .Produces<AssignRoleToUserResponse>(StatusCodes.Status409Conflict)
            .Produces<AssignRoleToUserResponse>(StatusCodes.Status400BadRequest)
            .Produces<ExceptionResult>(StatusCodes.Status500InternalServerError);

        routes
            .MapPut("/register", RegisterUserFromClaimsHandler.HandleAsync)
            .WithName("RegisterUserUsingClaims");

        routes
            .MapGet("/users/{userId}/roles", ListUserRolesHandler.HandleAsync)
            .WithName("ListUserRoles")
            .Produces<ListUserRolesResponse>(StatusCodes.Status200OK)
            .Produces<ListUserRolesErrorResponse>(StatusCodes.Status500InternalServerError).RequireAuthorization("ManageRoles");

        routes
            .MapGet("/users/oid/{azureObjectIdentifier}", GetUserByAzureObjectIdentifierHandler.HandleAsync)
            .WithName("GetUserByAzureObjectIdentifier")
            .Produces<GetUserByAzureObjectIdentifierResponse>(StatusCodes.Status200OK)
            .Produces<GetUserByAzureObjectIdentifierErrorResponse>(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        routes
            .MapGet("/users/{azureObjectIdentifier}/permissions", GetCurrentUserPermissionsHandler.HandleAsync)
            .WithName("GetCurrentUserPermissions")
            .Produces<GetCurrentUserPermissionsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        routes
            .MapGet("/users/paginated", ListActiveUsersPagedHandler.HandleAsync)
            .WithName("GetActiveUsersPaged")
            .RequireAuthorization("ListUsers");

        routes
            .MapPut("/users/avatar", PersistAvatarIdHandler.HandleAsync)
            .WithName("PersistAvatarId")
            .RequireAuthorization();

        return routes;
    }
}