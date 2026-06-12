using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Tests.Unit.InteractiveComponents.Mappers;

/// <summary>
/// Provides unit tests for verifying the functionality of the ProjectorDtoMapper class.
/// </summary>
/// <remarks>This class contains test methods to ensure that the ProjectorDtoMapper correctly maps data between
/// Projector and DTO objects. It is intended to validate the mapping logic and ensure data integrity during the
/// transformation process.</remarks>
public class ProjectorDtoMapperTest
{
    /// <summary>
    /// Ensures that the mapping from Projector to ProjectorDto is performed correctly.
    /// </summary>
    [Fact]
    public void ToDto_WithValidProjector_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange: create a sample Projector
        var projector = new Projector(
            color: new Color("#FFF"),
            texture: "Matte",
            brightness: 100,
            plateId: new PlateId("123456"),
            resolution: new Resolution(1920, 1080),
            coordinates: new Coordinates(1.0, 2.0, 3.0),
            dimensions: new Dimensions(4.0, 5.0, 6.0),
            rotations: new Rotations(0, 0, 0),
            learningSpaceId: 1
        );
        // Act: map to DTO
        var result = projector.ToDto();
        // Assert: properties match between entity and DTO
        result.Should().NotBeNull();
        result.Color.Should().Be(projector.Color.Value);
        result.Texture.Should().Be(projector.Texture);
        result.Brightness.Should().Be(projector.Brightness);
        result.PlateId.Should().Be(projector.PlateId.Value);
        result.ResWidth.Should().Be(projector.ProjectionResolution.Width);
        result.ResHeight.Should().Be(projector.ProjectionResolution.Height);
        result.X.Should().Be(projector.Coordinates.X);
        result.Y.Should().Be(projector.Coordinates.Y);
        result.Z.Should().Be(projector.Coordinates.Z);
        result.Width.Should().Be(projector.Dimensions.Width);
        result.Height.Should().Be(projector.Dimensions.Height);
        result.Depth.Should().Be(projector.Dimensions.Depth);
        result.LearningSpaceId.Should().Be(projector.LearningSpaceId);
    }
}
