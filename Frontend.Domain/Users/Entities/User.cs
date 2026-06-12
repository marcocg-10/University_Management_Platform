using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Entities;

/// <summary>
///  Represents a university campus user in the frontend.
/// </summary>
public class User
{
    /// <summary>
    /// Constructor for the user class
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="isActive"></param>
    /// <param name="email"></param>
    public User(
        UserId id,
        UserName name,
        bool isActive,
        Email email
        )
    {
        Id = id;
        Name = name;
        Email = email;
        IsActive = isActive;
    }

    /// <summary>
    /// Constructor for the user class with Azure Object Identifier
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="isActive"></param>
    /// <param name="email"></param>
    /// <param name="azureObjectIdentifier"></param>
    public User(
        UserId id,
        UserName name,
        bool isActive,
        Email email,
        string? azureObjectIdentifier)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        Email = email;
        AzureObjectIdentifier = azureObjectIdentifier;
    }

    /// <summary>
    /// Constructor for the user class
    /// </summary>
    /// <param name="idKey"></param>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="isActive"></param>
    /// <param name="email"></param>
    public User(
        int idKey,
        UserId id,
        UserName name,
        bool isActive,
        Email email)
    {
        IdKey = idKey;
        Id = id;
        Name = name;
        IsActive = isActive;
        Email = email;
    }

    /// <summary>
    /// Constructor for the user class with IdKey and Azure Object Identifier
    /// </summary>
    /// <param name="idKey"></param>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="isActive"></param>
    /// <param name="email"></param>
    /// <param name="azureObjectIdentifier"></param>
    public User(
        int idKey,
        UserId id,
        UserName name,
        bool isActive,
        Email email,
        string? azureObjectIdentifier)
    {
        IdKey = idKey;
        Id = id;
        Name = name;
        IsActive = isActive;
        Email = email;
        AzureObjectIdentifier = azureObjectIdentifier;
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
    public bool IsActive { get; }

    /// <summary>
    /// The internal key for identifying the user
    /// </summary>
    public int IdKey { get; private set; }

    /// <summary>
    /// The roles assigned to the user.
    /// </summary>
    public List<Role> Roles { get; } = [];

    /// <summary>
    /// Azure Entra ID Object Identifier for the user.
    /// Used for Entra ID authentication and authorization.
    /// </summary>
    /// <remarks>
    /// This is an internal property that stores the Azure AD Object ID (OID) claim.
    /// </remarks>
    public string? AzureObjectIdentifier { get; }
}
