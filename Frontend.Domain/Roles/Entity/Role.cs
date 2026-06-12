using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;

/// <summary>
/// Represents a role entity in the system.
/// </summary>
public class Role
{
    /// <summary>
    /// Creates a new instance of the <see cref="Role"/>.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    public Role(RoleName name)
    {
        Name = name;
    }

    /// <summary>
    ///  Creates a new instance of the <see cref="Role"/> with specified <see cref="Permission"/>.  
    /// </summary>
    /// <param name="name"> The name of the role</param>
    /// <param name="permissions"> The permissions associated with the role</param>
    public Role(RoleName name, List<Permission> permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Role"/> with the specified identifier, name, and permissions.
    /// </summary>
    /// <param name="id">The internal database identifier for the role.</param>
    /// <param name="name">The name of the role.</param>
    /// <param name="permissions">The permissions associated with the role.</param>
    public Role(int id, RoleName name, List<Permission> permissions)
    {
        Id = id;
        Name = name;
        Permissions = permissions;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Role"/> with the specified identifier and name.
    /// </summary>
    public Role(int id, RoleName name)
    {
        Id = id;
        Name = name;
    }


    /// <summary>
    /// The internal database identifier for the role.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// The name of the role.
    /// </summary>
    public RoleName Name { get; }

    /// <summary>
    /// The list of permissions associated with the role.
    /// </summary>
    public List<Permission> Permissions { get; } = [];
}

