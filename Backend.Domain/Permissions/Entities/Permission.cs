using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
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
