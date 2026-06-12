using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an attempt is made to assign a Role to a role, but the Role
/// is already assigned.
/// </summary>
/// <remarks>This exception is typically used to indicate that a Role assignment operation cannot proceed
/// because the specified Role is already associated with the specified role.</remarks>
public class RoleAlreadyAssignedException : UserException
{
    /// <summary>
    /// Gets the name of the Role associated with this instance.
    /// </summary>
    public RoleName RoleName { get; }

    /// <summary>
    /// Gets the name of the role associated with the current context.
    /// </summary>
    public UserId UserId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleAlreadyAssignedException"/> class with the specified
    /// Role and role.
    /// </summary>
    /// <remarks>This exception is typically thrown when an attempt is made to assign a Role to a role,
    /// but the Role is already assigned.</remarks>
    /// <param name="RoleName">The name of the Role that is already assigned.</param>
    /// <param name="roleName">The name of the role to which the Role is already assigned.</param>
    public RoleAlreadyAssignedException(UserId userId, RoleName roleName)
        : base($"The Role '{roleName?.Value}' is already assigned to the user '{userId?.Value}'.")
    {
        RoleName = RoleName;
        UserId = userId;
    }
}
