

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Request;

public record CreateUserRequest(
    /// <<summary>
    /// The unique identifier for the user.
    /// <</summary>
    string Id,
    /// <summary>
    /// The full name of the user.
    /// </summary>
    string Name,
    /// <summary>
    /// The email address of the user.
    /// </summary>
    string Email,
    /// <summary>
    /// The Azure Object Identifier of the user.
    /// </summary>
    string AzureObjectId,
    /// <summary>
    /// Indicates whether the user is active.
    /// </summary>  
    bool IsActive,
    ///<summary>
    /// The role identifier to assign to the user.
    ///</summary>
    string RoleId
);