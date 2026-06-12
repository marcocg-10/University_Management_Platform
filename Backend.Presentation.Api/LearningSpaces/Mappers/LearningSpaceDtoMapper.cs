using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;

/// <summary>
/// Provides extension methods to map domain LearningSpace
/// and Laboratory entities to their corresponding DTOs.
/// </summary>
internal static class LearningSpaceDtoMapper
{
    /// <summary>
    /// Maps a LearningSpace domain entity to a LearningSpaceDto.
    /// </summary>
    /// <param name="entity"> The LearningSpace entity to map. </param>
    /// <returns> A LearningSpaceDto containing the mapped data. </returns>
    internal static LearningSpaceDto ToDto(this LearningSpace entity)
    {
        return new LearningSpaceDto(
            entity.Id,
            entity.BuildingId,
            entity.FloorLevel,
            entity.RoomId,
            entity.Color.Value,
            entity.Texture.Value,
            entity.Dimensions.Width,
            entity.Dimensions.Length,
            entity.Dimensions.Height,
            entity.Coordinates.XCoordinate,
            entity.Coordinates.YCoordinate,
            entity.Coordinates.ZCoordinate);
    }

    /// <summary>
    /// Maps a Laboratory domain entity to a LaboratoryDto.
    /// </summary>
    /// <param name="entity"> The Laboratory entity to map. </param>
    /// <returns> A LaboratoryDto containing the mapped data. </returns>
    internal static LaboratoryDto ToDto(this Laboratory entity)
    {
        return new LaboratoryDto(
            entity.Id,
            entity.BuildingId,
            entity.FloorLevel,
            entity.RoomId,
            entity.Color.Value,
            entity.Texture.Value,
            entity.Dimensions.Width,
            entity.Dimensions.Length,
            entity.Dimensions.Height,
            entity.Coordinates.XCoordinate,
            entity.Coordinates.YCoordinate,
            entity.Coordinates.ZCoordinate);
    }

    /// <summary>
    /// Maps a Classroom domain entity to a ClassroomDto.
    /// </summary>
    /// <param name="entity"> The Classroom entity to map. </param>
    /// <returns> A ClassroomDto containing the mapped data. </returns>
    internal static ClassroomDto ToDto(this Classroom entity)
    {
        return new ClassroomDto(
            entity.Id,
            entity.BuildingId,
            entity.FloorLevel,
            entity.RoomId,
            entity.Color.Value,
            entity.Texture.Value,
            entity.Dimensions.Width,
            entity.Dimensions.Length,
            entity.Dimensions.Height,
            entity.Coordinates.XCoordinate,
            entity.Coordinates.YCoordinate,
            entity.Coordinates.ZCoordinate);
    }
}
