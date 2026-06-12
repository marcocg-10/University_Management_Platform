using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Tests.Unit.Users.Mappers;

public class AvatarIdDtoMapperTests
{
    [Fact]
    public void ToDto_Should_Return_Dto_With_Same_Value()
    {
        // Arrange
        var value = "rpm-abc";
        // Act
        var dto = AvatarIdDtoMapper.ToDto(value);
        // Assert
        dto.AvatarId.Should().Be(value);
    }

    [Fact]
    public void ToEntity_Should_Return_AvatarId_When_Valid()
    {
        // Arrange
        var dto = new AvatarIdDto("rpm-def");
        // Act
        var result = AvatarIdDtoMapper.ToEntity(dto, out var entity, out var error);
        // Assert
        result.Should().BeTrue();
        error.Should().BeNull();
        entity.Should().NotBeNull();
        entity!.Value.Should().Be("rpm-def");
    }

    [Fact]
    public void ToEntity_Should_Return_False_And_Error_When_Invalid()
    {
        // Arrange
        var dto = new AvatarIdDto("");
        // Act
        var result = AvatarIdDtoMapper.ToEntity(dto, out var entity, out var error);
        // Assert
        result.Should().BeFalse();
        entity.Should().BeNull();
        error.Should().NotBeNull();
    }
}
