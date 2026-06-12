using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an attempt is made to assign a permission to a role, but the permission
/// is already assigned.
/// </summary>
/// <remarks>This exception is typically used to indicate that a permission assignment operation cannot proceed
/// because the specified permission is already associated with the specified role.</remarks>
public class PermissionAlreadyAssignedException : RoleException
{
    /// <summary>
    /// Gets the name of the permission associated with this instance.
    /// </summary>
    public PermissionName PermissionName { get; }

    /// <summary>
    /// Gets the name of the role associated with the current context.
    /// </summary>
    public RoleName RoleName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionAlreadyAssignedException"/> class with the specified
    /// permission and role.
    /// </summary>
    /// <remarks>This exception is typically thrown when an attempt is made to assign a permission to a role,
    /// but the permission is already assigned.</remarks>
    /// <param name="permissionName">The name of the permission that is already assigned.</param>
    /// <param name="roleName">The name of the role to which the permission is already assigned.</param>
    public PermissionAlreadyAssignedException(PermissionName permissionName, RoleName roleName)
        : base($"The permission '{permissionName?.Value}' is already assigned to the role '{roleName?.Value}'.")
    {
        PermissionName = permissionName;
        RoleName = roleName;
    }
}
