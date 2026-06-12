using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;

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
    ///  Creates a new instance of the <see cref="Roles"/> with specified <see cref="Permissions"/>.  
    /// </summary>
    /// <param name="name"> The name of the role</param>
    /// <param name="permissions"> The permissions associated with the role</param>
    public Role(RoleName name, List<Permission> permissions)
    {
        Name = name;
        Permissions = permissions;
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

