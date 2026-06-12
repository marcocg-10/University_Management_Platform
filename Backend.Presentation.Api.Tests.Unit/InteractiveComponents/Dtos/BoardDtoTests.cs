using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Tests.Unit.InteractiveComponents.Dtos;

/// <summary>
/// Unit tests for the <see cref="BoardDto"/> record.
/// These tests validate that the DTO correctly stores values,
/// supports equality comparisons, and handles edge cases.
/// </summary>
public class BoardDtoTests
{
    /// <summary>
    /// Ensures all properties are correctly assigned through the constructor.
    /// </summary>
    [Fact]
    public void Ctor_Should_Assign_Properties_Correctly()
    {
        // Arrange
        var dto = new BoardDto(
            "#FFF",
            "#000",
            "Smooth",
            "WBS-123",
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
        dto.MarkerColor.Should().Be("#000");
        dto.Texture.Should().Be("Smooth");
        dto.PlateId.Should().Be("WBS-123");
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
        var dto1 = new BoardDto("#FFF", "#000", "Smooth", "WBS-123", 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);
        var dto2 = new BoardDto("#FFF", "#000", "Smooth", "WBS-123", 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);

        dto1.Should().Be(dto2);
        dto1.GetHashCode().Should().Be(dto2.GetHashCode());
    }

    /// <summary>
    /// Ensures two DTOs with different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_Should_Return_False_When_Values_Are_Different()
    {
        var dto1 = new BoardDto("#FFF", "#000", "Smooth", "WBS-123", 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);
        var dto2 = new BoardDto("#000", "#FFF", "Rough", "WBS-321", 10, 20, 30, 40, 50, 60, 0, 0, 0, 1);

        dto1.Should().NotBe(dto2);
    }

    /// <summary>
    /// Ensures record immutability by verifying properties cannot be changed after creation.
    /// </summary>
    [Fact]
    public void BoardDto_Should_Be_Immutable()
    {
        var dto = new BoardDto("#FFF", "#000", "Smooth", "WBS-123", 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);

        // Records are immutable; new instance must be created to "modify"
        var modified = dto with { Color = "#000" };

        modified.Color.Should().Be("#000");
        dto.Color.Should().Be("#FFF");
    }
}
