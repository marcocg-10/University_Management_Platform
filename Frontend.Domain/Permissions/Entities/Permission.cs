using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;

/// <summary>
/// Represents a permission that can be assigned to a user or role within the system.
/// </summary>
/// <remarks>A permission typically defines a specific action or set of actions that can be performed. Each
/// permission is uniquely identified by its <see cref="Id"/> and has a descriptive <see cref="Name"/>.</remarks>
public class Permission
{
    /// <summary>
    /// Constructor of the permission class
    /// </summary>
    /// <param name="Name"></param>
    public Permission(
        PermissionName name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the permission.
    /// </summary>
    public PermissionName Name { get; }
}

