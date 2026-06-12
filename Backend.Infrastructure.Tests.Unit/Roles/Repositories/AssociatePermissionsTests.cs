using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Permissions.Repositories;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Roles.Repositories;
    public class AssociatePermissionsTests :
    IClassFixture<RoleRepositoryTestData>,
    IClassFixture<PermissionRepositoryTestData>
    {

    private readonly RoleRepositoryTestData _testRoleData;
    private readonly PermissionRepositoryTestData _testPermissionData;

    public AssociatePermissionsTests(
        RoleRepositoryTestData testRoleData,
        PermissionRepositoryTestData testPermissionData)
    {
        _testRoleData = testRoleData;
        _testPermissionData = testPermissionData;
    }
    [Fact]
    public async Task AssociatePermissionAsync_WhenNotAlreadyAssociated_AddsPermission()
    {
        // Arrange
        var rolesDbSetData = new List<Role>(_testRoleData.SingleEntryData);
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();

        var permissionsDbSetData = new List<Permission>(_testPermissionData.SingleEntryData);
        var permissionsDbSetMock = permissionsDbSetData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Permissions)
            .Returns(permissionsDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new RoleRepository(dbContextMock.Object);

        var duplicateRole = _testRoleData.SingleEntryData.First();
        var duplicatePermission = _testPermissionData.SingleEntryData.First();

        // Act
        var (role, permission) = await sut.AssociatePermissionAsync(duplicateRole, duplicatePermission);

        // Assert
        role.Should().NotBeNull(because: "a change should be saved");
        permission.Should().NotBeNull(because: "a change should be saved");
        role.Should().Be(duplicateRole, because: "the returned value should be the role");
        permission.Should().Be(duplicatePermission, because: "the returned value should be the assigned permission");

        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        rolesDbSetData.First().Permissions.Should().BeEquivalentTo(
            permissionsDbSetData,
            because: "We associated the only role with the only permission");

    }

    [Fact]
    public async Task AssociatePermissionAsync_WhenAlreadyAssociated_ReturnsError()
    {
        // Arrange
        var rolesDbSetData = new List<Role>(_testRoleData.SingleEntryData);
        // simulate assignment of first permission to first role
        rolesDbSetData.First().Permissions.Add(_testPermissionData.SingleEntryData.First());

        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();

        var permissionsDbSetData = new List<Permission>(_testPermissionData.SingleEntryData);
        var permissionsDbSetMock = permissionsDbSetData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Permissions)
            .Returns(permissionsDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new RoleRepository(dbContextMock.Object);

        rolesDbSetData.First().Permissions.Add(permissionsDbSetData.First());

        var duplicateRole = _testRoleData.SingleEntryData.First();
        var duplicatePermission = _testPermissionData.SingleEntryData.First();

        // Act
        var exception = await Assert.ThrowsAsync<PermissionAlreadyAssignedException>(
           () => sut.AssociatePermissionAsync(duplicateRole, duplicatePermission));

        // Assert
        exception.PermissionName.Should().Be(duplicatePermission.Name, because: "the exception should contain the permission name");
        exception.RoleName.Should().Be(duplicateRole.Name, because: "the exception should contain the role name");

        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
        public async Task AssociatePermissionAsync_WhenNoRoles_ReturnsError()
        {
            // Arrange
            var rolesDbSetData = new List<Role>(_testRoleData.EmptyData);
            var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();


            var permissionsDbSetData = new List<Permission>(_testPermissionData.SingleEntryData);
            var permissionsDbSetMock = permissionsDbSetData.BuildMockDbSet();

            var dbContextMock = new Mock<AppDbContext>();
            dbContextMock
                .Setup(dbContext => dbContext.Roles)
                .Returns(rolesDbSetMock.Object);
            dbContextMock
                .Setup(dbContext => dbContext.Permissions)
                .Returns(permissionsDbSetMock.Object);
            dbContextMock
                .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var sut = new RoleRepository(dbContextMock.Object);

            var duplicateRole = _testRoleData.SingleEntryData.First();
            var duplicatePermission = _testPermissionData.SingleEntryData.First();

            
            var expectedErrorMessage = $"The role with the name '{duplicateRole.Name.Value}' does not exist.";

            // Act
            var exception = await Assert.ThrowsAsync<RoleNotFoundException>(
               () => sut.AssociatePermissionAsync(duplicateRole, duplicatePermission));
            // Assert
            exception.Message.Should().Be(expectedErrorMessage, because: "the exception should contain that role was not found");

            dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AssociatePermissionAsync_WhenNoPermissions_ReturnsError()
        {
            // Arrange
            var rolesDbSetData = new List<Role>(_testRoleData.SingleEntryData);
            var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();

            var permissionsDbSetData = new List<Permission>(_testPermissionData.EmptyData);
            var permissionsDbSetMock = permissionsDbSetData.BuildMockDbSet();

            var dbContextMock = new Mock<AppDbContext>();
            dbContextMock
                .Setup(dbContext => dbContext.Roles)
                .Returns(rolesDbSetMock.Object);
            dbContextMock
                .Setup(dbContext => dbContext.Permissions)
                .Returns(permissionsDbSetMock.Object);
            dbContextMock
                .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var sut = new RoleRepository(dbContextMock.Object);

            var duplicateRole = _testRoleData.SingleEntryData.First();
            var duplicatePermission = _testPermissionData.SingleEntryData.First();

            var expectedErrorMessage = $"The permission with the name '{duplicatePermission.Name.Value}' does not exist.";

            // Act
            
            var exception = await Assert.ThrowsAsync<AssignablePermissionNotFoundException>(
                () => sut.AssociatePermissionAsync(duplicateRole, duplicatePermission));
            // Assert
             exception.Message.Should().Be(expectedErrorMessage, because: "the exception should contain that role was not found");
            dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }      
    }

