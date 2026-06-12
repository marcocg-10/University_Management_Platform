using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Roles.Repositories;

/// <summary>
/// Unit tests for <see cref="RoleRepository"/>'s search functionality.
/// </summary>
public class RoleRepositorySearchRole : IClassFixture<RoleRepositoryTestData>
{
    private readonly RoleRepositoryTestData _testData;

    public RoleRepositorySearchRole(RoleRepositoryTestData testData)
    {
        _testData = testData;
    }

    [Fact]
    public async Task SearchRolesAsync_WhenGivenNoData_ReturnsEmptyEnumerable()
    {
        // Arrange
        var rolesDbSetMock = _testData.EmptyData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);

        // Act
        var result = await sut.SearchRolesAsync("Doesn't matter", 1, 10);

        // Assert
        result.Roles.Should().BeEmpty(because: "there are no roles in the database");
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task SearchRolesAsync_WhenOneMatch_ReturnsData()
    {
        // Arrange
        var rolesDbSetMock = _testData.MultipleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);

        // Act
        var result = await sut.SearchRolesAsync("Admin", 1, 10);

        // Assert
        result.Roles.Should().BeEquivalentTo(
            new[] { _testData.MultipleEntryData[0] },
            because: "only 'Administrator' contains 'Admin'");
        result.TotalCount.Should().Be(_testData.MultipleEntryData.Count);
    }

    [Fact]
    public async Task SearchRolesAsync_WhenMultipleMatches_ReturnsData()
    {
        // Arrange
        var rolesDbSetMock = _testData.MultipleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);

        // Act
        var result = await sut.SearchRolesAsync("a", 1, 10);

        // Assert
        result.Roles.Should().BeEquivalentTo(
            new[] { _testData.MultipleEntryData[0], _testData.MultipleEntryData[2] },
            because: "both 'Administrator' and 'Manager' contain 'a' and repository orders by name");
        result.TotalCount.Should().Be(_testData.MultipleEntryData.Count);
    }

    [Fact]
    public async Task SearchRolesAsync_WhenNoMatch_ReturnsEmpty()
    {
        // Arrange
        var rolesDbSetMock = _testData.MultipleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Roles)
            .Returns(rolesDbSetMock.Object);
        var sut = new RoleRepository(dbContextMock.Object);

        // Act
        var result = await sut.SearchRolesAsync("ඞ", 1, 10);

        // Assert
        result.Roles.Should().BeEmpty(because: "no role contains that character");
        result.TotalCount.Should().Be(_testData.MultipleEntryData.Count);
    }
}