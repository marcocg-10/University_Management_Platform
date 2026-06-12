using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.Roles.Entity;

public class RoleTests
{
    /// <summary>
    ///  Tests the constructor of the Role class with valid parameters.
    ///  If I gave valid parameters, then it should create a Role instance with the correct properties set.
    /// </summary>
    [Fact]
    public void Constructor_ValidParameters_CreatesRole()
    {
        // Arrange
        var roleName = RoleName.Create("Administrator");
        // Act
        var role = new Role(roleName);
        // Assert
        role.Name.Should().Be(roleName, because: "the Name property should match the provided RoleName");
        role.Id.Should().Be(0, because: "Id should be default (0) before being set by the database");
        role.Permissions.Should().BeEmpty(because: "No list of properties was provided");
    }

    /// <summary>
    /// If I gave valid parameters with empty permissions, they returns a role without permissions.
    /// </summary>
    [Fact]
    public void Constructor_ValidParametersWithEmptyPermissions_CreatesRoleWithoutPermissions()
    {
        // Arrange
        var roleName = RoleName.Create("Administrator");
        List<Permission> permissionList = [];
        // Act
        var role = new Role(roleName, permissionList);
        // Assert
        role.Name.Should().Be(roleName, because: "the Name property should match the provided RoleName");
        role.Id.Should().Be(0, because: "Id should be default (0) before being set by the database");
        role.Permissions.Should().BeEmpty(because: "An empty list was provided");
    }


    /// <summary>
    /// If I gave valid parameters with a single permission, it returns a role with that permission.
    /// </summary>
    [Fact]
    public void Constructor_ValidParametersWithSinglePermission_CreatesRoleWithSinglePermission()
    {
        // Arrange
        var roleName = RoleName.Create("Administrator");
        List<Permission> permissionList = [
            new Permission(PermissionName.Create("Administrator"))
            ];
        // Act
        var role = new Role(roleName, permissionList);
        // Assert
        role.Name.Should().Be(roleName, because: "the Name property should match the provided RoleName");
        role.Id.Should().Be(0, because: "Id should be default (0) before being set by the database");
        role.Permissions.Should().BeEquivalentTo(permissionList, because: "A single permission was provided");
    }

    /// <summary>
    /// If I gave valid parameters with multiple permissions, it returns a role with those permissions.
    /// </summary>
    [Fact]
    public void Constructor_ValidParametersWithMultiplePermissions_CreatesRoleWithMultiplePermissions()
    {
        // Arrange
        var roleName = RoleName.Create("Administrator");
        List<Permission> permissionList = [
            new Permission(PermissionName.Create("Administrator1")),
            new Permission(PermissionName.Create("Administrator2"))
            ];
        // Act
        var role = new Role(roleName, permissionList);
        // Assert
        role.Name.Should().Be(roleName, because: "the Name property should match the provided RoleName");
        role.Id.Should().Be(0, because: "Id should be default (0) before being set by the database");
        role.Permissions.Should().BeEquivalentTo(permissionList, because: "Multiple permissions were provided");
    }
}