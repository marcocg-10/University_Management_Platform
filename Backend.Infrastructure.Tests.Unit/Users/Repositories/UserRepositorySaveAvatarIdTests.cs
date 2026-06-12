using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;

public class UserRepositorySaveAvatarIdTests : IClassFixture<UserRepositoryTestData>
{
    private readonly UserRepositoryTestData _testData;

    public UserRepositorySaveAvatarIdTests(UserRepositoryTestData testData)
    {
        _testData = testData;
    }

    [Fact]
    public async Task SaveAvatarId_WhenUserExists_Should_Update_Field_And_SaveChanges()
    {
        // Arrange
        var usersDbSetData = new List<User>(_testData.SingleEntryData);
        var usersDbSetMock = usersDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);
        dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var sut = new UserRepository(dbContextMock.Object);
        var idKey = usersDbSetData[0].IdKey;
        var avatarId = AvatarId.Create("rpm-abc");

        // Act
        await sut.SaveAvatarId(idKey, avatarId);

        // Assert
        usersDbSetData[0].AvatarId.Should().Be(avatarId);
        Mock.Get(dbContextMock.Object).Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SaveAvatarId_WhenUserMissing_Should_Throw_UserNotFoundException()
    {
        // Arrange
        var usersDbSetMock = _testData.EmptyData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(c => c.Users).Returns(usersDbSetMock.Object);

        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var act = async () => await sut.SaveAvatarId(999, AvatarId.Create("rpm-xyz"));

        // Assert
        await Assert.ThrowsAsync<UserNotFoundException>(act);
    }
}
