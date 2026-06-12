using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Mappers;

/// <summary>
/// Provides extension methods to map <see cref="ProjectorDto"/> objects.
/// </summary>
internal static class ProjectorDtoMapper
{
    /// <summary>
    /// Transforms a <see cref="ProjectorDto"/> into a <see cref="Projector"/> entity.
    /// </summary>
    /// <param name="dto"> The <see cref="ProjectorDto"/> to be transformed.</param>
    /// <returns>
    /// Returns a <see cref="Projector"/> entity populated with data from the provided <paramref name="dto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static Projector ToEntity(this ProjectorDto dto)
    {
        var texture = dto.Texture
            ?? throw new ArgumentNullException(nameof(ProjectorDto.Texture), "ProjectorDto.Texture is null");

        var brightness = dto.Brightness
            ?? throw new ArgumentNullException(nameof(ProjectorDto.Brightness), "ProjectorDto.Brightness is null");

        var learningSpaceId = dto.LearningSpaceId
            ?? throw new ArgumentNullException(nameof(ProjectorDto.LearningSpaceId), "ProjectorDto.LearningSpaceId is null");

        var colorValue = dto.Color
            ?? throw new ArgumentNullException(nameof(ProjectorDto.Color), "ProjectorDto.Color is null");
        var color = new Color(colorValue);

        var plateIdValue = dto.PlateId
            ?? throw new ArgumentNullException(nameof(ProjectorDto.PlateId), "ProjectorDto.PlateId is null");
        var plateId = new PlateId(plateIdValue);

        var resolution = new Resolution((int)(dto.ResWidth ?? 0), (int)(dto.ResHeight ?? 0));
        var coordinates = new Coordinates((dto.X ?? 0), (dto.Y ?? 0), (dto.Z ?? 0));
        var dimensions = new Dimensions((dto.Width ?? 0), (dto.Height ?? 0), (dto.Depth ?? 0));
        var rotations = new Rotations((dto.XAxisRotation ?? 0), (dto.YAxisRotation ?? 0), (dto.ZAxisRotation ?? 0));

        return new Projector(
            color,
            texture,
            brightness,
            plateId,
            resolution,
            coordinates,
            dimensions,
            rotations,
            learningSpaceId);
    }

    internal static ProjectorDto ToDto(this Projector entity)
    {
        return new ProjectorDto
        {
            Color = entity.Color.Value,
            Texture = entity.Texture,
            PlateId = entity.PlateId.Value,
            Brightness = entity.Brightness,
            ResWidth = entity.ProjectionResolution.Width,
            ResHeight = entity.ProjectionResolution.Height,
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
