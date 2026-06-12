using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Permissions.Repositories;

public class PermissionRepositoryTestData
{
    public List<Permission> EmptyData { get; } = [];

    public List<Permission> SingleEntryData { get; } = [
        new Permission(
            PermissionName.Create("CreateBuildings"))];

    public List<Permission> MultipleEntryData { get; } = [
        new Permission(
             PermissionName.Create("CreateBuildings")),
        new Permission(
             PermissionName.Create("CreateUsers")),
         new Permission(
             PermissionName.Create("ReadUsers")),
         new Permission(
             PermissionName.Create("DeleteComponents"))
    ];

}
