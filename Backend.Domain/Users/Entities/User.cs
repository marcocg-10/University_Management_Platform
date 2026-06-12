using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;

public class User
{
    /// <summary>
    /// The Ready Player Me avatar identifier linked to the user profile.
    /// </summary>
    /// <remarks>
    /// By default, this is set to a generic avatar ID created before.
    /// </remarks>
    public AvatarId? AvatarId { get; set; } = new AvatarId("690289f98c926a605ec5e5fb");

    /// <summary>
    ///  Represents a university campus user.
    /// </summary>
    /// <remarks>
    /// Keeping as a single class for now, but can be extracted
    /// into a person base class in the future based on needs.
    /// This constructor does not specify any roles.
    /// </remarks>
    public User(
        UserId id, 
        UserName name,
        bool isActive,
        Email email,
        string? azureObjectIdentifier = null)
    {
        Id = id;
        Name = name;
        Email = email;
        IsActive = isActive;
        AzureObjectIdentifier = azureObjectIdentifier;
    }

    /// <summary>
    ///  Represents a university campus user, with roles.
    /// </summary>
    /// <remarks>
    /// Keeping as a single class for now, but can be extracted
    /// into a person base class in the future based on needs.
    /// This constructor does specify a list of roles
    /// </remarks>
    public User(
        UserId id,
        UserName name,
        bool isActive,
        Email email,
        List<Role> roles)
    {
        Id = id;
        Name = name;
        Email = email;
        IsActive = isActive;
        Roles = roles;
    }

    /// <summary>
    /// User's identifier
    /// </summary>

    public UserId Id { get; }

    /// <summary>
    /// User's name
    /// </summary>
    public UserName Name { get; }

    /// <summary>
    /// User's Email address
    /// </summary>
    public Email Email { get; }

    /// <summary>
    /// Whether the user is active.
    /// </summary>
    public bool IsActive { get;  }

    /// <summary>
    /// The internal key for identifying the user
    /// </summary>
    public int IdKey { get; private set; }

    /// <summary>
    /// Azure Active Directory Object Identifier for the user.
    /// Used for Azure AD authentication and authorization.
    /// </summary>
    /// <remarks>
    /// This is an internal property that stores the Azure AD Object ID (OID) claim.
    /// </remarks>
    public string? AzureObjectIdentifier { get; }
    /// The list of roles associated with the user.
    /// </summary>
    public List<Role> Roles { get; } = [];
}
