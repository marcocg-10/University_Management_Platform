using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Roles.Repositories;

public class RepositoryGetRolesTest : IClassFixture<RoleRepositoryTestData>
{
    private readonly RoleRepositoryTestData _testRoleData;
 
    public RepositoryGetRolesTest(
        RoleRepositoryTestData testRoleData)
    {
        _testRoleData = testRoleData;
    }

    /// <summary>
    /// Verifies that GetRolesAsync returns all roles from the database
    /// </summary>
    [Fact]
    public async Task GetRolesAsync_WhenCalled_ReturnsAllRoles()
    {
        // Arrange
        var rolesDbSetData = _testRoleData.MultipleEntryData;
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);
        // Act
        var roles = await sut.GetRolesAsync();
        // Assert
        roles.Should().NotBeNull(because: "roles should be returned");
        roles!.Should().HaveCount(rolesDbSetData.Count(), because: "all roles should be retrieved");
    }
    /// <summary>
    /// Verifies that GetRolesAsync handles exceptions and returns an appropriate error message
    /// </summary>
    [Fact]
    public async Task GetRolesAsync_WhenExceptionOccurs_ReturnsErrorMessage()
    {
        // Arrange
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Throws(new Exception("Database error"));
        var sut = new RoleRepository(dbContextMock.Object);
        // Act
        var roles = async () => await sut.GetRolesAsync();
        // Assert
        await roles.Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Verifies that GetRolesAsync returns an empty list when no roles exist in the database
    /// </summary>
    [Fact]
    public async Task GetRolesAsync_WhenNoRolesExist_ReturnsEmptyList()
    {
        // Arrange
        var rolesDbSetData = _testRoleData.EmptyData;
        var rolesDbSetMock = rolesDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);
        // Act
        var roles = await sut.GetRolesAsync();
        // Assert
        roles.Should().NotBeNull(because: "an empty list should be returned when no roles exist");
        roles!.Should().BeEmpty(because: "there are no roles in the database");
    }
}