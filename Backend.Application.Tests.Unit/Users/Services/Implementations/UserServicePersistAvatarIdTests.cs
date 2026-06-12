using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Users.Services.Implementations;

public class UserServicePersistAvatarIdTests
{
    [Fact]
    public async Task SaveAvatarId_Should_Invoke_Repository_With_Same_Arguments()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();
        var sut = new UserService(repo.Object);
        var userId = 42;
        var avatarId = AvatarId.Create("rpm-42");

        // Act
        await sut.SaveAvatarId(userId, avatarId);

        // Assert
        repo.Verify(r => r.SaveAvatarId(userId, avatarId), Times.Once);
    }
}
