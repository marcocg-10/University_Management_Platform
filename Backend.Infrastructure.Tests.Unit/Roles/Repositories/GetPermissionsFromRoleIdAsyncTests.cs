using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Permissions.Repositories;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Roles.Repositories;
public class GetPermissionsFromRoleIdAsyncTests :
IClassFixture<RoleRepositoryTestData>,
IClassFixture<PermissionRepositoryTestData>
{

    private readonly RoleRepositoryTestData _testRoleData;
    private readonly PermissionRepositoryTestData _testPermissionData;

    public GetPermissionsFromRoleIdAsyncTests(
        RoleRepositoryTestData testRoleData,
        PermissionRepositoryTestData testPermissionData)
    {
        _testRoleData = testRoleData;
        _testPermissionData = testPermissionData;
    }
    
    [Fact]
    public async Task GetPermissionsFromRoleIdAsync_WhenCalled_ShouldReturnAllPermissionsForRole()
    {
        // Arrange
        var role = new Role(RoleName.Create("Admin"));
        // Use permissions from test data
        role.Permissions.Add(_testPermissionData.MultipleEntryData[0]); // CreateBuildings
        role.Permissions.Add(_testPermissionData.MultipleEntryData[1]); // CreateUsers
        
        var rolesDbSetData = new List<Role> { role };
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);
        
        // Act
        var (permissions, errorMessage) = await sut.GetPermissionsFromRoleIdAsync(role.Id);
        
        // Assert
        permissions.Should().NotBeNull(because: "there should be a list of permissions");
        permissions!.Should().HaveCount(2, because: "two permissions were associated with the role");
        permissions.Should().Contain(_testPermissionData.MultipleEntryData[0], because: "CreateBuildings permission was added");
        permissions.Should().Contain(_testPermissionData.MultipleEntryData[1], because: "CreateUsers permission was added");
        errorMessage.Should().BeNull(because: "there should be no error message");
    }

    [Fact]
    public async Task GetPermissionsFromRoleIdAsync_WhenRepositoryReturnsError_ShouldReturnErrorMessage()
    {
        // Arrange
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Throws(new Exception("Database error"));
        var sut = new RoleRepository(dbContextMock.Object);
        
        // Act
        var (permissions, errorMessage) = await sut.GetPermissionsFromRoleIdAsync(1);
        
        // Assert
        permissions.Should().BeNull(because: "no permissions should be returned upon failure");
        errorMessage.Should().NotBeNull(because: "there should be an error message for a failure");
        errorMessage.Should().Be("Error searching for role with id 1", because: "the error message should indicate a search error");
    }

    [Fact]
    public async Task GetPermissionsFromRoleIdAsync_WhenNoPermissionsExist_ShouldReturnEmptyList()
    {
        // Arrange
        // Use a role from test data without permissions
        var role = _testRoleData.SingleEntryData[0]; // Administrator role with no permissions
        var rolesDbSetData = new List<Role> { role };
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);
        
        // Act
        var (permissions, errorMessage) = await sut.GetPermissionsFromRoleIdAsync(role.Id);
        
        // Assert
        permissions.Should().NotBeNull(because: "there should be a list of permissions");
        permissions!.Should().BeEmpty(because: "no permissions were associated with the role");
        errorMessage.Should().BeNull(because: "there should be no error message");
    }

    [Fact]
    public async Task GetPermissionsFromRoleIdAsync_WhenRoleHasSinglePermission_ShouldReturnSinglePermission()
    {
        // Arrange
        var role = _testRoleData.MultipleEntryData[0]; // Administrator role
        // Add single permission from test data
        role.Permissions.Add(_testPermissionData.SingleEntryData[0]); // CreateBuildings
        
        var rolesDbSetData = new List<Role> { role };
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);
        
        // Act
        var (permissions, errorMessage) = await sut.GetPermissionsFromRoleIdAsync(role.Id);
        
        // Assert
        permissions.Should().NotBeNull(because: "there should be a list of permissions");
        permissions!.Should().HaveCount(1, because: "one permission was associated with the role");
        permissions.Should().Contain(_testPermissionData.SingleEntryData[0], because: "the CreateBuildings permission was added");
        errorMessage.Should().BeNull(because: "there should be no error message");
    }

    [Fact]
    public async Task GetPermissionsFromRoleIdAsync_WhenRoleHasMultiplePermissions_ShouldReturnAllPermissions()
    {
        // Arrange
        var role = _testRoleData.MultipleEntryData[1]; // User role
        // Add all permissions from test data
        foreach (var permission in _testPermissionData.MultipleEntryData)
        {
            role.Permissions.Add(permission);
        }
        
        var rolesDbSetData = new List<Role> { role };
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);
        
        // Act
        var (permissions, errorMessage) = await sut.GetPermissionsFromRoleIdAsync(role.Id);
        
        // Assert
        permissions.Should().NotBeNull(because: "there should be a list of permissions");
        permissions!.Should().HaveCount(4, because: "four permissions were associated with the role");
        permissions.Should().BeEquivalentTo(_testPermissionData.MultipleEntryData, because: "all test permissions were added to the role");
        errorMessage.Should().BeNull(because: "there should be no error message");
    }
}
