using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;

/// <summary>
/// Provides mapping utilities to convert <see cref="Board"/> domain entities
/// into <see cref="BoardDto"/> data transfer objects (DTOs) for API responses.
/// </summary>
/// <remarks>
/// This mapper is used to transform the domain model into a DTO that can be safely
/// exposed via the presentation layer. It extracts the necessary properties from
/// the <see cref="Board"/> entity, including value objects such as <see cref="Color"/>,
/// <see cref="PlateId"/>, <see cref="Coordinates"/>, and <see cref="Dimensions"/>.
/// </remarks>
internal static class BoardDtoMapper
{
    /// <summary>
    /// Converts a <see cref="Board"/> entity into a <see cref="BoardDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Board"/> entity to convert. Must not be null.</param>
    /// <returns>
    /// A <see cref="BoardDto"/> containing all the properties of the board, including:
    /// - <see cref="BoardDto.Color"/>
    /// - <see cref="BoardDto.MarkerColor"/>
    /// - <see cref="BoardDto.Texture"/>
    /// - <see cref="BoardDto.PlateId"/>
    /// - <see cref="BoardDto.X"/>
    /// - <see cref="BoardDto.Y"/>
    /// - <see cref="BoardDto.Z"/>
    /// - <see cref="BoardDto.Width"/>
    /// - <see cref="BoardDto.Height"/>
    /// - <see cref="BoardDto.Depth"/>
    /// - <see cref="BoardDto.LearningSpaceId"/>
    /// </returns>
    internal static BoardDto ToDto(this Board entity)
    {
        return new BoardDto(
            entity.Color.Value,
            entity.MarkerColor.Value,
            entity.Texture,
            entity.PlateId.Value,
            entity.Coordinates.X,
            entity.Coordinates.Y,
            entity.Coordinates.Z,
            entity.Dimensions.Width,
            entity.Dimensions.Height,
            entity.Dimensions.Depth,
            entity.Rotations.XAxisRotation,
            entity.Rotations.YAxisRotation,
            entity.Rotations.ZAxisRotation,
            entity.LearningSpaceId);
    }

    /// <summary>
    /// Transforms a <see cref="BoardDto"/> back into a <see cref="Board"/> entity.
    /// </summary>
    /// <param name="dto">The data transfer object to convert. Must not be null.</param>
    /// <returns></returns>
    internal static Board ToEntity(this BoardDto dto)
    {
        return new Board(
            new Color(dto.Color),
            new Color(dto.MarkerColor),
            dto.Texture,
            new PlateId(dto.PlateId),
            new Coordinates(dto.X, dto.Y, dto.Z),
            new Dimensions(dto.Width, dto.Height, dto.Depth),
            new Rotations(dto.XAxisRotation, dto.YAxisRotation, dto.ZAxisRotation),
            dto.LearningSpaceId);
    }
}
