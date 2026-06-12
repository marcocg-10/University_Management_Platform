using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Mappers;

/// <summary>
/// Provides extension methods for mapping between <see cref="Role"/> and <see cref="RoleDto"/> objects.
/// </summary>
/// <remarks>This class contains methods to convert a <see cref="Role"/> entity to a <see cref="RoleDto"/>  and
/// vice versa. The mapping ensures that the essential properties of the objects are transferred  while validating the
/// data where necessary.</remarks>
internal static class RoleDtoMapper
{
    /// <summary>
    /// Converts a <see cref="Role"/> entity to its corresponding <see cref="RoleDto"/> representation.
    /// </summary>
    internal static RoleDto ToDto(this Role role) =>
        new RoleDto(
            Name: role.Name.Value);

    /// <summary>
    /// Converts a <see cref="Role"/> entity to its corresponding <see cref="RoleIdDto"/> representation.
    /// </summary>
    internal static RoleIdDto ToIdDto(this Role role) =>
        new RoleIdDto(
            Id: role.Id,
            Name: role.Name.Value);

    /// <summary>
    /// Converts the current <see cref="RoleDto"/> instance to a <see cref="Role"/> entity.
    /// </summary>
    internal static Role ToEntity(this RoleDto roleDto)
    {          
        var roleName = RoleName.Create(roleDto.Name);
        
        return new Role(roleName);
        
    }
}
