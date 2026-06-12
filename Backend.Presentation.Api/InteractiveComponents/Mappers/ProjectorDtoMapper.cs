using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;

/// <summary>
/// Provides mapping utilities to convert <see cref="Projector"/> domain entities.
/// into <see cref="ProjectorDto"/> data transfer objects (DTOs) for API responses.
/// </summary>
/// <remarks>
/// This mapper is used to transform the domain model into a DTO that can be safely
/// exposed via the presentation layer. It extracts the necessary properties from
/// the <see cref="Projector"/> entity, including value objects such as <see cref="Color"/>,
/// <see cref="Resolution"/>, <see cref="PlateId"/>, <see cref="Coordinates"/>, and 
/// <see cref="Dimensions"/>.
/// </remarks>
internal static class ProjectorDtoMapper
{
    /// <summary>
    /// Converts a <see cref="Projector"/> entity into a <see cref="ProjectorDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Projector"/> entity to convert. Must not be null.
    /// </param>
    /// <returns>
    /// A <see cref="ProjectorDto"/> containing all the properties of the projector, including:
    /// - <see cref="ProjectorDto.Color"/>
    /// - <see cref="ProjectorDto.Texture"/>
    /// - <see cref="ProjectorDto.Brightness"/>
    /// - <see cref="ProjectorDto.PlateId"/>
    /// - <see cref="ProjectorDto.ResWidth"/>
    /// - <see cref="ProjectorDto.ResHeight"/>
    /// - <see cref="ProjectorDto.X"/>
    /// - <see cref="ProjectorDto.Y"/>
    /// - <see cref="ProjectorDto.Z"/>
    /// - <see cref="ProjectorDto.Width"/>
    /// - <see cref="ProjectorDto.Height"/>
    /// - <see cref="ProjectorDto.Depth"/>
    /// - <see cref="ProjectorDto.XAxisRotation"/>
    /// - <see cref="ProjectorDto.YAxisRotation"/>
    /// - <see cref="ProjectorDto.ZAxisRotation"/>
    /// </returns>
    internal static ProjectorDto ToDto(this Projector entity)
    {
        return new ProjectorDto(
            entity.Color.Value,
            entity.Texture,
            entity.Brightness,
            entity.PlateId.Value,
            entity.ProjectionResolution.Width,
            entity.ProjectionResolution.Height,
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
    /// Transforms a <see cref="ProjectorDto"/> back into a <see cref="Projector"/> entity.
    /// </summary>
    /// <param name="dto">The data transfer object to convert. Must not be null.</param>
    /// <returns></returns>
    internal static Projector ToEntity(this ProjectorDto dto)
    {
        return new Projector(
            new Color(dto.Color),
            dto.Texture,
            dto.Brightness,
            new PlateId(dto.PlateId),
            new Resolution(dto.ResWidth, dto.ResHeight),
            new Coordinates(dto.X, dto.Y, dto.Z),
            new Dimensions(dto.Width, dto.Height, dto.Depth),
            new Rotations(dto.XAxisRotation, dto.YAxisRotation, dto.ZAxisRotation),
            dto.LearningSpaceId);
    }
}
