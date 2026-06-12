using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;
public class AssociateRolesTests :
IClassFixture<UserRepositoryTestData>,
IClassFixture<RoleRepositoryTestData>
{

    private readonly UserRepositoryTestData _testUserData;
    private readonly RoleRepositoryTestData _testRoleData;

    public AssociateRolesTests(
        UserRepositoryTestData testUserData,
        RoleRepositoryTestData testRoleData)
    {
        _testUserData = testUserData;
        _testRoleData = testRoleData;
    }

    /// <summary>
    /// Verifies that when a user and a role are not associated, they become
    /// associated by adding the latter to a the former's list.
    /// </summary>
    [Fact]
    public async Task AssociateRoleAsync_WhenNotAlreadyAssociated_AddsRole()
    {
        var usersDbSetData = new List<User>(_testUserData.SingleEntryData);
        var usersDbSetMock = usersDbSetData.BuildMockDbSet();

        var rolesDbSetData = new List<Role>(_testRoleData.SingleEntryData);
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new UserRepository(dbContextMock.Object);

        var duplicateUser = _testUserData.SingleEntryData.First();
        var duplicateRole = _testRoleData.SingleEntryData.First();

        // Act
        var (resultUser, resultRole) = await sut.AssociateRoleAsync(duplicateUser, duplicateRole);

        // Assert
        resultUser.Should().NotBeNull(because: "a user should be returned");
        resultRole.Should().NotBeNull(because: "a role should be returned");

        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        usersDbSetData.First().Roles.Should().ContainSingle(
            because: "We associated the only role with the only user");

    }

    /// <summary>
    /// Verifies that when the user and the role are already associated,
    /// it returns an error message.
    /// </summary>
    [Fact]
    public async Task AssociateRoleAsync_WhenAlreadyAssociated_ThrowsUserException()
    {
        // Arrange
        var usersDbSetData = new List<User>(_testUserData.SingleEntryData);
        // set up existing association
        usersDbSetData.First().Roles.Add(_testRoleData.SingleEntryData.First());
        
        var usersDbSetMock = usersDbSetData.BuildMockDbSet();

        var rolesDbSetData = new List<Role>(_testRoleData.SingleEntryData);
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new UserRepository(dbContextMock.Object);

        usersDbSetData.First().Roles.Add(rolesDbSetData.First());

        var duplicateUser = _testUserData.SingleEntryData.First();
        var duplicateRole = _testRoleData.SingleEntryData.First();

        // Act & Assert: repository throws a generic UserException for this implementation
        var exception = await Assert.ThrowsAsync<RoleAlreadyAssignedException>(
            () => sut.AssociateRoleAsync(duplicateUser, duplicateRole));

        // Assert
        exception.RoleName.Should().Be(duplicateRole.Name, because: "the exception should contain the role name");
        exception.UserId.Should().Be(duplicateUser.Id, because: "the exception should contain the user ID");
       
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }



    /// <summary>
    /// Verifies that when the user to associate with doesn't exist,
    /// it returns an error message.
    /// </summary>
    [Fact]
    public async Task AssociateRoleAsync_WhenNoUsers_ReturnsError()
    {
        // Arrange
        var rolesDbSetData = new List<User>(_testUserData.EmptyData);
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();

        var permissionsDbSetData = new List<Role>(_testRoleData.SingleEntryData);
        var permissionsDbSetMock = permissionsDbSetData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(rolesDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(permissionsDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new UserRepository(dbContextMock.Object);

        var duplicateUser = _testUserData.SingleEntryData.First();
        var duplicateRole = _testRoleData.SingleEntryData.First();

       


        // Act
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(
            () => sut.AssociateRoleAsync(duplicateUser, duplicateRole));

        // Assert
        exception.Message.Should().Contain($"{duplicateUser.Id.Value}", because: "the exception should contain the user ID");

        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    /// <summary>
    /// Verifies that when the role to associate to the user doesn't exist,
    /// it returns an error message.
    /// </summary>
    [Fact]
    public async Task AssociateRoleAsync_WhenNoRoles_ReturnsError()
    {
        // Arrange
        var usersDbSetData = new List<User>(_testUserData.SingleEntryData);
        var usersDbSetMock = usersDbSetData.BuildMockDbSet();

        var rolesDbSetData = new List<Role>(_testRoleData.EmptyData);
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new UserRepository(dbContextMock.Object);

        var duplicateUser = _testUserData.SingleEntryData.First();
        var duplicateRole = _testRoleData.SingleEntryData.First();

        
        // Act
        var exception = await Assert.ThrowsAsync<AssignableRoleNotFoundException>(
            () => sut.AssociateRoleAsync(duplicateUser, duplicateRole));
        
        // Assert
        exception.Message.Should().Contain($"{duplicateRole.Name.Value}", because: "the exception should contain the user ID");

        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}