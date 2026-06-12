
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Exceptions;

/// <summary>
/// Represents an exception that is thrown when attempting to create or add a permission with a name that already
/// exists.
/// </summary>
/// <remarks>This exception is typically used to indicate a conflict when a permission with the specified name
/// already exists in the system.  Ensure that the permission name is unique before attempting the operation.</remarks>
public class PermissionAlreadyExistsException : PermissionException
{
    /// <summary>
    /// Gets the VO for permission name that caused the exception.
    /// </summary>
    public PermissionName PermissionName { get; }

    /// <summary>
    /// Represents an exception that is thrown when attempting to create or add a permission  with a name that already
    /// exists.
    /// </summary>
    /// <remarks>This exception is typically used to indicate a conflict when a permission with the  specified
    /// name already exists in the system. Ensure that the permission name is unique  before attempting the
    /// operation.</remarks>
    /// <param name="permissionName">The name of the permission that caused the exception.  This value cannot be null.</param>
    public PermissionAlreadyExistsException(PermissionName permissionName)
        : base($"A permission with the name '{permissionName.Value}' already exists.")
    {
        PermissionName = permissionName;
    }
}
