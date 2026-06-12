using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Mappers;

/// <summary>
/// Provides extension methods to map <see cref="BoardDto"/> objects
/// to their corresponding <see cref="Board"/> entity representations.
/// </summary>
internal static class BoardDtoMapper
{
    /// <summary>
    /// Converts a <see cref="BoardDto"/> data transfer object into a <see cref="Board"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="BoardDto"/> instance containing board data.</param>
    /// <returns>
    /// A new <see cref="Board"/> entity populated with the values provided by the <paramref name="dto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any required property in <paramref name="dto"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// Any <c>null</c> values for numeric properties (such as coordinates or dimensions)
    /// are replaced with zero (0) to ensure valid default initialization.
    /// </remarks>
    internal static Board ToEntity(this BoardDto dto)
    {
        var texture = dto.Texture
            ?? throw new ArgumentNullException(nameof(BoardDto.Texture), "BoardDto.Texture is null");

        var learningSpaceId = dto.LearningSpaceId
            ?? throw new ArgumentNullException(nameof(BoardDto.LearningSpaceId), "BoardDto.LearningSpaceId is null");

        var colorValue = dto.Color
            ?? throw new ArgumentNullException(nameof(BoardDto.Color), "BoardDto.Color is null");
        var color = new Color(colorValue);

        var markerColorValue = dto.MarkerColor
            ?? throw new ArgumentNullException(nameof(BoardDto.MarkerColor), "BoardDto.MarkerColor is null");
        var markerColor = new Color(markerColorValue);

        var plateIdValue = dto.PlateId
            ?? throw new ArgumentNullException(nameof(BoardDto.PlateId), "BoardDto.PlateId is null");
        var plateId = new PlateId(plateIdValue);

        var coordinates = new Coordinates((dto.X ?? 0), (dto.Y ?? 0), (dto.Z ?? 0));

        var dimensions = new Dimensions((dto.Width ?? 0), (dto.Height ?? 0), (dto.Depth ?? 0));

        var rotations = new Rotations((dto.XAxisRotation ?? 0), (dto.YAxisRotation ?? 0), (dto.ZAxisRotation ?? 0));

        return new Board(
            color,
            markerColor,
            texture,
            plateId,
            coordinates,
            dimensions,
            rotations,
            learningSpaceId);
    }

    internal static BoardDto ToDto(this Board entity)
    {
        return new BoardDto
        {
            Color = entity.Color.Value,
            MarkerColor = entity.MarkerColor.Value,
            Texture = entity.Texture,
            PlateId = entity.PlateId.Value,
            X = entity.Coordinates.X,
            Y = entity.Coordinates.Y,
            Z = entity.Coordinates.Z,
            Width = entity.Dimensions.Width,
            Height = entity.Dimensions.Height,
            Depth = entity.Dimensions.Depth,
            XAxisRotation = entity.Rotations.XAxisRotation,
            YAxisRotation = entity.Rotations.YAxisRotation,
            ZAxisRotation = entity.Rotations.ZAxisRotation,
            LearningSpaceId = entity.LearningSpaceId
        };
    }
}
