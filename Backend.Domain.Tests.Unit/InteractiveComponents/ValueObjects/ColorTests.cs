using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.InteractiveComponents.ValueObjects;

public class ColorTests
{
    [Theory]
    [InlineData("#FFFFFF")]
    [InlineData("#ffffff")]
    [InlineData("#ABC123")]
    [InlineData("#abc")]
    [InlineData("#123")]
    public void Constructor_ShouldCreateColor_WhenValidHex(string validHex)
    {
        // Act
        var color = new Color(validHex);

        // Assert
        color.Should().NotBeNull();
        color.Value.Should().Be(validHex.ToUpperInvariant());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowColorValidationException_WhenNullOrEmpty(string invalidHex)
    {
        // Act
        Action act = () => new Color(invalidHex);

        // Assert
        var exception = act.Should().Throw<InvalidColorException>().Which;
        exception.Message.Should().MatchRegex("Color cannot be null.*|Color cannot be empty or whitespace.*");
    }


    [Theory]
    [InlineData("123456")]
    [InlineData("ZZZZZZ")]
    [InlineData("#12345G")]
    [InlineData("#12")]
    [InlineData("##FFFFFF")]
    [InlineData("#FFFFF")] // 5 chars, invalid
    [InlineData("FFFFFF")] // missing #
    public void Constructor_ShouldThrowColorValidationException_WhenInvalidFormat(string invalidHex)
    {
        // Act
        Action act = () => new Color(invalidHex);

        // Assert
        act.Should().Throw<InvalidColorException>()
            .WithMessage($"Color format is invalid.*{invalidHex}*");
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameColorValues()
    {
        // Arrange
        var color1 = new Color("#abc123");
        var color2 = new Color("#ABC123");

        // Act
        bool areEqual = color1.Equals(color2);

        // Assert
        areEqual.Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentColorValues()
    {
        // Arrange
        var color1 = new Color("#abc123");
        var color2 = new Color("#ffffff");

        // Act
        bool areEqual = color1.Equals(color2);

        // Assert
        areEqual.Should().BeFalse();
    }
}
