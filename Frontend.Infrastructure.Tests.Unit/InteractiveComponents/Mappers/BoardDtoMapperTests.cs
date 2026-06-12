using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Tests.Unit.InteractiveComponents.Mappers;

/// <summary>
/// Contains unit tests that validate the mapping logic in <see cref="BoardDtoMapper"/>.
/// </summary>
public class BoardDtoMapperTests
{
    /// <summary>
    /// Verifies that all non-null properties from a valid <see cref="BoardDto"/>
    /// are accurately mapped to their corresponding fields in the domain entity.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        // Prepare a complete and valid DTO representing a Board.
        var dto = new BoardDto
        {
            PlateId = "123456",
            Color = "#FFF",
            MarkerColor = "#000",
            Texture = "Smooth",
            LearningSpaceId = 10,
            X = 1.1,
            Y = 2.2,
            Z = 3.3,
            Width = 10,
            Height = 20,
            Depth = 30
        };

        // Act
        // Convert the DTO into the corresponding domain entity.
        var result = dto.ToEntity();

        // Assert
        // Validate that all domain properties match the DTO values.
        result.Should().NotBeNull();
        result.PlateId.Value.Should().Be("123456");
        result.Color.Value.Should().Be("#FFF");
        result.MarkerColor.Value.Should().Be("#000");
        result.Texture.Should().Be("Smooth");
        result.LearningSpaceId.Should().Be(10);

        // Validate spatial and dimensional properties.
        result.Coordinates.X.Should().Be(1.1);
        result.Coordinates.Y.Should().Be(2.2);
        result.Coordinates.Z.Should().Be(3.3);

        result.Dimensions.Width.Should().Be(10);
        result.Dimensions.Height.Should().Be(20);
        result.Dimensions.Depth.Should().Be(30);
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="BoardDto.Texture"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_Texture_Is_Null()
    {
        // Arrange
        var dto = new BoardDto
        {
            PlateId = "123456",
            Color = "#FFF",
            MarkerColor = "#000",
            LearningSpaceId = 1,
            Texture = null
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*BoardDto.Texture is null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="BoardDto.LearningSpaceId"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_LearningSpaceId_Is_Null()
    {
        // Arrange
        var dto = new BoardDto
        {
            PlateId = "123456",
            Color = "#FFF",
            MarkerColor = "#000",
            Texture = "Smooth",
            LearningSpaceId = null
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*BoardDto.LearningSpaceId is null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="BoardDto.Color"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_Color_Is_Null()
    {
        // Arrange
        var dto = new BoardDto
        {
            PlateId = "123456",
            Color = null,
            MarkerColor = "#000",
            Texture = "Smooth",
            LearningSpaceId = 1
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*BoardDto.Color is null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="BoardDto.MarkerColor"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_MarkerColor_Is_Null()
    {
        // Arrange
        var dto = new BoardDto
        {
            PlateId = "123456",
            Color = "#FFF",
            MarkerColor = null,
            Texture = "Smooth",
            LearningSpaceId = 1
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*BoardDto.MarkerColor is null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentNullException"/> is thrown when
    /// the <see cref="BoardDto.PlateId"/> property is <c>null</c>.
    /// </summary>
    [Fact]
    public void ToEntity_Should_Throw_When_PlateId_Is_Null()
    {
        // Arrange
        var dto = new BoardDto
        {
            PlateId = null,
            Color = "#FFF",
            MarkerColor = "#000",
            Texture = "Smooth",
            LearningSpaceId = 1
        };

        // Act
        Action act = () => dto.ToEntity();

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*BoardDto.PlateId is null*");
    }
}
