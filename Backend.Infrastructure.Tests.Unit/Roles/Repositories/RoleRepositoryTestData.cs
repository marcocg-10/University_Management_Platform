using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Roles.Repositories;

public class RoleRepositoryTestData
{
    public List<Role> EmptyData { get; } = [];
    public List<Role> SingleEntryData { get; } = [
        new Role(
            RoleName.Create("Administrator"))];
    public List<Role> MultipleEntryData { get; } = [
        new Role(
             RoleName.Create("Administrator")),
        new Role(
             RoleName.Create("User")),
         new Role(
             RoleName.Create("Manager")),
         new Role(
             RoleName.Create("Guest"))
    ];
}