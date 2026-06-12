using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Tests.Unit.InteractiveComponents.Dtos;

/// <summary>
/// Unit tests for the <see cref="ProjectorDto"/> record.
/// These tests validate that the DTO correctly stores values,
/// supports equality comparisons, and handles edge cases.
/// </summary>
public class ProjectorDtoTest
{
    /// <summary>
    /// Ensures all properties are correctly assigned through the constructor.
    /// </summary>
    [Fact]
    public void Ctor_Should_Assign_Properties_Correctly()
    {
        // Arrange
        var dto = new ProjectorDto(
            "#FFF",
            "Matte",
            100,
            "PRJ-456",
            1920,
            1080,
            1.0,
            2.0,
            3.0,
            4.0,
            5.0,
            6.0,
            0,
            0,
            0,
            1
        );
        // Assert
        dto.Color.Should().Be("#FFF");
        dto.Texture.Should().Be("Matte");
        dto.Brightness.Should().Be(100);
        dto.PlateId.Should().Be("PRJ-456");
        dto.ResWidth.Should().Be(1920);
        dto.ResHeight.Should().Be(1080);
        dto.X.Should().Be(1.0);
        dto.Y.Should().Be(2.0);
        dto.Z.Should().Be(3.0);
        dto.Width.Should().Be(4.0);
        dto.Height.Should().Be(5.0);
        dto.Depth.Should().Be(6.0);
        dto.LearningSpaceId.Should().Be(1);
    }

    /// <summary>
    /// Ensures two DTOs with the same values are equal (record equality).
    /// </summary>
    [Fact]
    public void Equals_Should_Return_True_When_Values_Are_Equal()
    {
        var dto1 = new ProjectorDto("#FFF", "Matte", 100, "PRJ-456", 1920, 1080, 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);
        var dto2 = new ProjectorDto("#FFF", "Matte", 100, "PRJ-456", 1920, 1080, 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);
        dto1.Should().Be(dto2);
        dto1.GetHashCode().Should().Be(dto2.GetHashCode());
    }

    /// <summary>
    /// Ensures two DTOs with different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_Return_False_When_Values_Are_Different()
    {
        var dto1 = new ProjectorDto("#FFF", "Matte", 100, "PRJ-456", 1920, 1080, 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);
        var dto2 = new ProjectorDto("#000", "Glossy", 200, "PRJ-789", 1280, 720, 7, 8, 9, 10, 11, 12, 0, 0, 0, 2);
        dto1.Should().NotBe(dto2);
        dto1.GetHashCode().Should().NotBe(dto2.GetHashCode());
    }

    /// <summary>
    /// Ensures record immutability by verifying properties cannot be changed after creation.
    /// </summary>
    [Fact]
    public void ProjectorDto_Should_Be_Immutable()
    {
        var dto = new ProjectorDto("#FFF", "Matte", 100, "PRJ-456", 1920, 1080, 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);
        // Records are immutable; new instance must be created to "modify"
        var modified = dto with { Color = "#000" };

        modified.Color.Should().Be("#000");
        dto.Color.Should().Be("#FFF");
    }
}
