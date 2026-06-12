using System;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.LearningSpaces.Mappers
{
    internal static class LaboratoryDtoMapper
    {
        public static Laboratory ToEntity(this LaboratoryDto dto)
        {
            var id = dto.Id ?? throw new ArgumentNullException(nameof(dto.Id), "LaboratoryDto.Id is null");
            var buildingId = dto.BuildingId;
            var floorLevel = dto.FloorLevel;
            var colorValue = dto.Color ?? throw new ArgumentNullException(nameof(dto.Color), "LaboratoryDto.Color is null");
            var textureValue = dto.Texture;
            var roomId = dto.RoomId ?? throw new ArgumentNullException(nameof(dto.RoomId), "LaboratoryDto.RoomId is null");
            var width = dto.Width ?? throw new ArgumentNullException(nameof(dto.Width), "LaboratoryDto.Width is null");
            var length = dto.Length ?? throw new ArgumentNullException(nameof(dto.Length), "LaboratoryDto.Length is null");
            var height = dto.Height ?? throw new ArgumentNullException(nameof(dto.Height), "LaboratoryDto.Height is null");
            var xCoordinate = dto.XCoordinate ?? throw new ArgumentNullException(nameof(dto.XCoordinate), "LaboratoryDto.XCoordinate is null");
            var yCoordinate = dto.YCoordinate ?? throw new ArgumentNullException(nameof(dto.YCoordinate), "LaboratoryDto.YCoordinate is null");
            var zCoordinate = dto.ZCoordinate ?? throw new ArgumentNullException(nameof(dto.ZCoordinate), "LaboratoryDto.ZCoordinate is null");

            var color = LearningSpaceColor.Create(colorValue);
            var texture = LearningSpaceTexture.Create(textureValue);
            var dimensions = LearningSpaceDimensions.Create(width, length, height);
            var coordinates = LearningSpaceCoordinates.Create(xCoordinate, yCoordinate, zCoordinate);

            return new Laboratory(
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

}
