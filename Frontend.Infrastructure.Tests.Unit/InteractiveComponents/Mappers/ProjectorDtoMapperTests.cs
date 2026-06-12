using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Tests.Unit.InteractiveComponents.Mappers;

/// <summary>
/// Contains unit tests that validate the mapping logic in <see cref="ProjectorDtoMapper"/>.
/// </summary>
public class ProjectorDtoMapperTests
{
    /// <summary>
    /// Verifies that all non-null properties from a valid <see cref="ProjectorDto"/>
    /// are correctly mapped to their corresponding fields in the <c>Projector</c> entity.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        // Create a valid DTO representing a complete projector definition.
        var dto = new ProjectorDto
        {
            PlateId = "123456",
            Color = "#FFF",
            Texture = "Matte",
            Brightness = 200,
            LearningSpaceId = 5,
            ResWidth = 1920,
            ResHeight = 1080,
            X = 1.5,
            Y = 2.5,
            Z = 3.5,
            Width = 10,
            Height = 5,
            Depth = 3
        };

        // Act
        // Map the DTO into a domain entity.
        var result = dto.ToEntity();

        // Assert
        // Verify that all fields were correctly mapped and initialized.
        result.Should().NotBeNull();
        result.PlateId.Value.Should().Be("123456");
        result.Color.Value.Should().Be("#FFF");
        result.Texture.Should().Be("Matte");
        result.Brightness.Should().Be(200);
        result.LearningSpaceId.Should().Be(5);

        // Validate nested Resolution, Coordinates, and Dimensions.
        result.ProjectionResolution.Width.Should().Be(1920);
        result.ProjectionResolution.Height.Should().Be(1080);

        result.Coordinates.X.Should().Be(1.5);
        result.Coordinates.Y.Should().Be(2.5);
        result.Coordinates.Z.Should().Be(3.5);

        result.Dimensions.Width.Should().Be(10);
        result.Dimensions.Height.Should().Be(5);
        result.Dimensions.Depth.Should().Be(3);
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="ProjectorDto.Texture"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_Texture_Is_Null()
    {
        // Arrange
        var dto = new ProjectorDto
        {
            PlateId = "123456",
            Color = "#FFF",
            Brightness = 100,
            LearningSpaceId = 1,
            Texture = null
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*ProjectorDto.Texture is null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="ProjectorDto.Brightness"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_Brightness_Is_Null()
    {
        // Arrange
        var dto = new ProjectorDto
        {
            PlateId = "123456",
            Color = "#FFF",
            Texture = "Matte",
            Brightness = null,
            LearningSpaceId = 1
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*ProjectorDto.Brightness is null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="ProjectorDto.LearningSpaceId"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_LearningSpaceId_Is_Null()
    {
        // Arrange
        var dto = new ProjectorDto
        {
            PlateId = "123456",
            Color = "#FFF",
            Texture = "Matte",
            Brightness = 100,
            LearningSpaceId = null
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*ProjectorDto.LearningSpaceId is null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="ProjectorDto.Color"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_Color_Is_Null()
    {
        // Arrange
        var dto = new ProjectorDto
        {
            PlateId = "123456",
            Color = null,
            Texture = "Matte",
            Brightness = 100,
            LearningSpaceId = 1
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*ProjectorDto.Color is null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="ProjectorDto.PlateId"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_PlateId_Is_Null()
    {
        // Arrange
        var dto = new ProjectorDto
        {
            PlateId = null,
            Color = "#FFF",
            Texture = "Matte",
            Brightness = 100,
            LearningSpaceId = 1
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*ProjectorDto.PlateId is null*");
    }
}
