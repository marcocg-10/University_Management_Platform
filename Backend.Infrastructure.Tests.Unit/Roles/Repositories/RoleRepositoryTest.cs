using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Roles.Repositories;

public class RoleRepositoryTest :
    IClassFixture<RoleRepositoryTestData>
{
    private readonly RoleRepositoryTestData _testRoleData;

    public RoleRepositoryTest(
        RoleRepositoryTestData testRoleData)
    {
        _testRoleData = testRoleData;
    }

    [Fact]
    public async Task CreateRoleAsync_WithValidRole_AddsAndSavesChanges()
    {
        // Arrange
        var rolesDbSetData = _testRoleData.EmptyData;
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new RoleRepository(dbContextMock.Object);

        var role = new Role(RoleName.Create("Administrator"));

        // Act
        var createdRole = await sut.CreateRoleAsync(role);

        // Assert
        createdRole.Should().NotBeNull(because: "a valid role should be returned");
        createdRole.Should().BeEquivalentTo(role, because: "the created role should be the same as the input");
        rolesDbSetMock.Verify(db => db.AddAsync(It.IsAny<Role>(), It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateRoleAsync_WhenSaveChangesThrowsOtherDbUpdateException_Rethrows()
    {
        // Arrange
        var rolesDbSetData = _testRoleData.EmptyData;
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);

        // Throw a DbUpdateException without a SqlException inner -> should propagate
        var dbUpdateEx = new DbUpdateException("Some other DB error");
        dbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(dbUpdateEx);

        var sut = new RoleRepository(dbContextMock.Object);
        var role = new Role(RoleName.Create("Manager"));

        // Act
        var act = async () => await sut.CreateRoleAsync(role);

        // Assert
        await act.Should().ThrowAsync<DbUpdateException>();
        rolesDbSetMock.Verify(db => db.AddAsync(It.IsAny<Role>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateRoleAsync_WithMultipleRolesInDatabase_CreatesNewUniqueRole()
    {
        // Arrange
        var rolesDbSetData = _testRoleData.MultipleEntryData;
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new RoleRepository(dbContextMock.Object);

        var newRole = new Role(RoleName.Create("SuperAdmin"));

        // Act
        var createdRole = await sut.CreateRoleAsync(newRole);

        // Assert
        createdRole.Should().NotBeNull(because: "a valid unique role should be created");
        createdRole.Should().BeEquivalentTo(newRole);
        rolesDbSetMock.Verify(db => db.AddAsync(It.IsAny<Role>(), It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

}