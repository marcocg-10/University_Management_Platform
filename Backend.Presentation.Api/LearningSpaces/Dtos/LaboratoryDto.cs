namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

/// <summary>
/// Represents a data transfer object (DTO) for a laboratory.
/// <remarks>
/// Inherits from LearningSpaceDto.
/// </remarks>
/// </summary>
/// <param name="Id">The unique identifier of the laboratory.</param>
/// <param name="BuildingId">The identifier of the building where the laboratory is located.</param>
/// <param name="FloorLevel">The floor level of the laboratory within the building.</param>
/// <param name="RoomId">The identifier of the room for the laboratory.</param>
/// <param name="Width">The width of the laboratory in meters.</param>
/// <param name="Length">The length of the laboratory in meters.</param>
/// <param name="Height">The height of the laboratory in meters.</param>
/// <param name="XCoordinate">The X coordinate of the laboratory's location.</param>
/// <param name="YCoordinate">The Y coordinate of the laboratory's location.</param>
/// <param name="ZCoordinate">The Z coordinate of the laboratory's location.</param>
public record LaboratoryDto(
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
