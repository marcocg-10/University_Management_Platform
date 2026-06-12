using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.LearningSpaces.Mappers;

internal static class ClassroomDtoMapper
{
    public static Classroom ToEntity(this ClassroomDto dto)
    {
        var id = dto.Id ?? throw new ArgumentNullException(nameof(dto.Id), "ClassroomDto.Id is null");
        var buildingId = dto.BuildingId;
        var floorLevel = dto.FloorLevel;
        var roomId = dto.RoomId ?? throw new ArgumentNullException(nameof(dto.RoomId), "ClassroomDto.RoomId is null");
        var colorValue = dto.Color ?? throw new ArgumentNullException(nameof(dto.Color), "ClassroomDto.Color is null");
        var textureValue = dto.Texture ?? throw new ArgumentNullException(nameof(dto.Texture), "ClassroomDto.Texture is null");
        var width = dto.Width ?? throw new ArgumentNullException(nameof(dto.Width), "ClassroomDto.Width is null");
        var length = dto.Length ?? throw new ArgumentNullException(nameof(dto.Length), "ClassroomDto.Length is null");
        var height = dto.Height ?? throw new ArgumentNullException(nameof(dto.Height), "ClassroomDto.Height is null");
        var xCoordinate = dto.XCoordinate ?? throw new ArgumentNullException(nameof(dto.XCoordinate), "ClassroomDto.XCoordinate is null");
        var yCoordinate = dto.YCoordinate ?? throw new ArgumentNullException(nameof(dto.YCoordinate), "ClassroomDto.YCoordinate is null");
        var zCoordinate = dto.ZCoordinate ?? throw new ArgumentNullException(nameof(dto.ZCoordinate), "ClassroomDto.ZCoordinate is null");

        var color = LearningSpaceColor.Create(colorValue);
        var texture = LearningSpaceTexture.Create(textureValue);
        var dimensions = LearningSpaceDimensions.Create(width, length, height);
        var coordinates = LearningSpaceCoordinates.Create(xCoordinate, yCoordinate, zCoordinate);

        return new Classroom(
            id,
            buildingId,
            floorLevel,
            roomId,
            color,
            texture,
            dimensions,
            coordinates);
    }
}
