using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Tests.Unit.InteractiveComponents.Mappers;

/// <summary>
/// Unit tests for verifying the correctness of the mapping from <see cref="Board"/> entities
/// to <see cref="BoardDto"/> objects.
/// These tests ensure that all properties, including edge cases, are mapped accurately.
/// </summary>
public class BoardDtoMapperTests
{
    /// <summary>
    /// Ensures that mapping copies all values correctly.
    /// </summary>
    [Fact]
    public void ToDto_WithValidBoard_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange: create a sample Board
        var board = new Board(
            color: new Color("#FFF"),
            markerColor: new Color("#000"),
            texture: "smooth",
            plateId: new PlateId("123456"),
            coordinates: new Coordinates(10.5, 20.3, 5.7),
            dimensions: new Dimensions(100.0, 200.0, 50.0),
            rotations: new Rotations(0, 0, 0),
            learningSpaceId: 1
        );

        // Act: map to DTO
        var result = board.ToDto();

        // Assert: properties match between entity and DTO
        result.Should().NotBeNull();
        result.Color.Should().Be(board.Color.Value);
        result.Texture.Should().Be(board.Texture);
        result.PlateId.Should().Be(board.PlateId.Value);
        result.X.Should().Be(board.Coordinates.X);
        result.Y.Should().Be(board.Coordinates.Y);
        result.Z.Should().Be(board.Coordinates.Z);
        result.Width.Should().Be(board.Dimensions.Width);
        result.Height.Should().Be(board.Dimensions.Height);
        result.Depth.Should().Be(board.Dimensions.Depth);
        result.LearningSpaceId.Should().Be(board.LearningSpaceId);
    }
}
