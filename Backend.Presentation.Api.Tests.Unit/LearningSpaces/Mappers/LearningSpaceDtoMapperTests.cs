using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Tests.Unit.LearningSpaces.Mappers;

/// <summary>
/// Unit tests for LearningSpaceDtoMapper.
/// Ensures that entities LearningSpace and Laboratory are correctly mapped to their DTO representations.
/// </summary>
public class LearningSpaceDtoMapperTests
{
    /// <summary>
    /// Verifies that LearningSpace.ToDto() maps all properties
    /// correctly for a valid LearningSpace instance.
    /// </summary>
    [Fact]
    public void ToDto_GivenValidLearningSpace_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var dimensions = LearningSpaceDimensions.TryCreate(10, 20, 3, out var dims) ? dims : null;
        var coordinates = LearningSpaceCoordinates.TryCreate(5, 7, 8, out var coords) ? coords : null;
        var color = LearningSpaceColor.TryCreate("#FFFFFF", out var col) ? col : null;
        var texture = LearningSpaceTexture.TryCreate("Concrete", out var tex) ? tex : null;
        var entity = new LearningSpace(1, 1, "101", color!, texture, dimensions!, coordinates!);

        // Act
        var dto = entity.ToDto();

        // Assert
        dto.BuildingId.Should().Be(1);
        dto.FloorLevel.Should().Be(1);
        dto.RoomId.Should().Be("101");
        dto.Color.Should().Be("#FFFFFF");
        dto.Texture.Should().Be("Concrete");
        dto.Width.Should().Be(10);
        dto.Length.Should().Be(20);
        dto.Height.Should().Be(3);
        dto.XCoordinate.Should().Be(5);
        dto.YCoordinate.Should().Be(7);
        dto.ZCoordinate.Should().Be(8);
    }

    /// <summary>
    /// Verifies that Laboratory.ToDto() maps all properties
    /// correctly for a valid Laboratory instance.
    /// </summary>
    [Fact]
    public void ToDto_GivenValidLaboratory_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var dimensions = LearningSpaceDimensions.TryCreate(15, 25, 4, out var dims) ? dims : null;
        var coordinates = LearningSpaceCoordinates.TryCreate(8, 12, 8, out var coords) ? coords : null;
        var color = LearningSpaceColor.TryCreate("#FF5733", out var col) ? col : null;
        var texture = LearningSpaceTexture.TryCreate("Concrete", out var tex) ? tex : null;
        var entity = new Laboratory(2, 2, "202", color!, texture!,  dimensions!, coordinates!);

        // Act
        var dto = entity.ToDto();

        // Assert
        dto.BuildingId.Should().Be(2);
        dto.FloorLevel.Should().Be(2);
        dto.RoomId.Should().Be("202");
        dto.Color.Should().Be("#FF5733");
        dto.Texture.Should().Be("Concrete");
        dto.Width.Should().Be(15);
        dto.Length.Should().Be(25);
        dto.Height.Should().Be(4);
        dto.XCoordinate.Should().Be(8);
        dto.YCoordinate.Should().Be(12);
        dto.ZCoordinate.Should().Be(8);
    }
}
