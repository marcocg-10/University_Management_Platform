namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Requests;

/// <summary>
/// Represents the payload for updating an existing <see cref="UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities.Classroom"/> 
/// via the API.
/// </summary>
/// <remarks>
/// The classroom ID is not included as it comes from the route parameter.
/// All numeric properties can be provided as their native types since this follows
/// the simpler DTO pattern rather than the string-based parsing approach.
/// </remarks>
public record UpdateClassroomRequest(
    /// <summary>
    /// The identifier of the building where the classroom is located.
    /// Can be null for standalone classrooms.
    /// </summary>
    string? BuildingId,

    /// <summary>
    /// The floor level of the classroom within the building.
    /// Can be null for ground level or standalone classrooms.
    /// </summary>
    string? FloorLevel,

    /// <summary>
    /// The identifier of the room for the classroom.
    /// This is a required field that identifies the classroom within the building/floor.
    /// </summary>
    string RoomId,

    /// <summary>
    /// The color of the classroom.
    /// This is a required field that describes the classroom's color.
    /// </summary>
    string Color,

    /// <summary>
    /// The Texture of the classroom.
    /// </summary>
    string Texture,

    /// <summary>
    /// The width of the classroom in meters.
    /// Must be a positive number.
    /// </summary>
    string Width,

    /// <summary>
    /// The length of the classroom in meters.
    /// Must be a positive number.
    /// </summary>
    string Length,

    /// <summary>
    /// The height of the classroom in meters.
    /// Must be a positive number.
    /// </summary>
    string Height,

    /// <summary>
    /// The X coordinate of the classroom's location within the building.
    /// Must be non-negative.
    /// </summary>
    string XCoordinate,

    /// <summary>
    /// The Y coordinate of the classroom's location within the building.
    /// Must be non-negative.
    /// </summary>
    string YCoordinate,

    /// <summary>
    /// The Z coordinate of the classroom's location within the building.
    /// Must be non-negative.
    /// </summary>
    string ZCoordinate);