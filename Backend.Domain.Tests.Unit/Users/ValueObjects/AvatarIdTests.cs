using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Users.ValueObjects;

public class AvatarIdTests
{
    [Theory]
    [InlineData("rpm-12345")]
    [InlineData("avatar_abcdef012345")]
    [InlineData("XyZ-7890")]
    public void TryCreate_GivenValidAvatarId_ReturnsTrue(string input)
    {
        // Act
        var result = AvatarId.TryCreate(input, out var avatarId, out var error);
        // Assert
        result.Should().BeTrue();
        error.Should().BeNull();
        avatarId!.Value.Should().Be(input);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void TryCreate_GivenEmptyAvatarId_ReturnsFalse(string input)
    {
        // Act
        var result = AvatarId.TryCreate(input, out var avatarId, out var error);
        // Assert
        result.Should().BeFalse();
        error.Should().Be("AvatarId cannot be empty.");
        avatarId.Should().BeNull();
    }

    [Fact]
    public void TryCreate_GivenTooLongAvatarId_ReturnsFalse()
    {
        // Arrange
        var input = new string('a', 51);
        // Act
        var result = AvatarId.TryCreate(input, out var avatarId, out var error);
        // Assert
        result.Should().BeFalse();
        error.Should().Be("AvatarId cannot exceed 50 characters.");
        avatarId.Should().BeNull();
    }

    [Fact]
    public void Create_GivenInvalid_ThrowsValidationException()
    {
        // Arrange
        var input = "";
        // Act
        FluentActions.Invoking(() => AvatarId.Create(input))
            .Should().ThrowExactly<ValidationException>()
            .WithMessage("*AvatarId cannot be empty.*");
    }

    [Fact]
    public void EqualityComponents_GivenSameValue_AreEqual()
    {
        // Arrange
        var a1 = AvatarId.Create("rpm-1");
        var a2 = AvatarId.Create("rpm-1");
        // Assert
        a1.Should().Be(a2);
    }

    [Fact]
    public void EqualityComponents_GivenDifferentValue_AreNotEqual()
    {
        // Arrange
        var a1 = AvatarId.Create("rpm-1");
        var a2 = AvatarId.Create("rpm-2");
        // Assert
        a1.Should().NotBe(a2);
    }
}
