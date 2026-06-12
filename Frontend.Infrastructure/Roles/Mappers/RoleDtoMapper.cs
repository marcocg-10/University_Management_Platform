using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Roles.Mappers;

internal static class RoleDtoMapper
{
    public static Role toRolIdEntity(this RoleIdDto dto)
    {
        RoleName? roleName = null;
        string? stringError = null;
        var created = RoleName.TryCreate(dto.Name, out roleName, out stringError);

        if (!created || roleName is null)
        {
            throw new ArgumentException(stringError, nameof(dto.Name));
        }

        return new Role(
            id: dto.Id ?? throw new ArgumentException("Role ID cannot be null", nameof(dto.Id)),
            name: roleName
        );
    }

    public static Role ToEntity(this RoleDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        RoleName? roleName = null;
        string? stringError = null;
        var created = RoleName.TryCreate(dto.Name, out roleName, out stringError);

        if (!created || roleName is null)
            throw new ArgumentException(stringError, nameof(dto.Name));

        return new Role(roleName);
    }
}
