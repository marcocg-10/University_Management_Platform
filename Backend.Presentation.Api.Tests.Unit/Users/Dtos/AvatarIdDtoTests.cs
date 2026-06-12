using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Tests.Unit.Users.Dtos;

public class AvatarIdDtoTests
{
    [Fact]
    public void Constructor_Should_Set_Value()
    {
        // Arrange
        var value = "rpm-123";
        // Act
        var dto = new AvatarIdDto(value);
        // Assert
        dto.AvatarId.Should().Be(value);
    }
}
