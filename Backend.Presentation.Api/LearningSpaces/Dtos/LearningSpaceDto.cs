namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

/// <summary>
/// Represents a data transfer object (DTO) for a learning space.
/// </summary>
/// <param name="BuildingId">The unique identifier of the building where the learning space is located.</param>
/// <param name="FloorLevel">The floor level within the building where the learning space is situated.</param>
/// <param name="RoomId">The identifier of the room representing the learning space.</param>
/// <param name="Width">The width of the learning space in meters.</param>
/// <param name="Length">The length of the learning space in meters.</param>
/// <param name="Height">The height of the learning space in meters.</param>
/// <param name="XCoordinate">The X coordinate of the learning space within the building.</param>
/// <param name="YCoordinate">The Y coordinate of the learning space within the building.</param>
/// <param name="ZCoordinate">The Z coordinate (elevation) of the learning space within the building.</param>
public record LearningSpaceDto(
	int Id,
    int? BuildingId,
    int? FloorLevel,
    string RoomId,
    string Color,
    string Texture,
    float Width,
    float Length,
    float Height,
    float XCoordinate,
    float YCoordinate,
    float ZCoordinate);
