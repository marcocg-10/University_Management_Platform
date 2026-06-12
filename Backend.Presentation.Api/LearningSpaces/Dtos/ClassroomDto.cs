namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

/// <summary>
/// Represents a data transfer object (DTO) for a classroom.
/// <remarks>
/// Inherits from LearningSpaceDto.
/// </remarks>
/// </summary>
/// <param name="Id">The unique identifier of the classroom.</param>
/// <param name="BuildingId">The identifier of the building where the classroom is located.</param>
/// <param name="FloorLevel">The floor level of the classroom within the building.</param>
/// <param name="RoomId">The identifier of the room for the classroom.</param>
/// <param name="Width">The width of the classroom in meters.</param>
/// <param name="Length">The length of the classroom in meters.</param>
/// <param name="Height">The height of the classroom in meters.</param>
/// <param name="XCoordinate">The X coordinate of the classroom's location.</param>
/// <param name="YCoordinate">The Y coordinate of the classroom's location.</param>
/// <param name="ZCoordinate">The Z coordinate of the classroom's location.</param>
public record ClassroomDto(
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
    float ZCoordinate) : LearningSpaceDto(
        Id,
        BuildingId,
        FloorLevel,
        RoomId,
        Color,
        Texture,
        Width,
        Length,
        Height,
        XCoordinate,
        YCoordinate,
        ZCoordinate);
