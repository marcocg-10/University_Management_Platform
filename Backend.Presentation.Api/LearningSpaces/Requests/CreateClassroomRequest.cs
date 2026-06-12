namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Requests;

/// <summary>
/// Represents the payload for creating a new <see cref="Classroom"/> 
/// via the API.
/// </summary>
/// <remarks>
/// All properties are received as strings because they originate from HTTP requests.
/// Numeric values such as coordinates, dimensions, and building/floor information are validated
/// and converted by the API handlers before being passed to the service layer.
/// </remarks>
public record CreateClassroomRequest(
    /// <summary>
    /// The identifier of the building where the classroom is located.
    /// Expected to be a string representing an integer, or null/empty for standalone classrooms.
    /// </summary>
    string? BuildingId,

    /// <summary>
    /// The floor level of the classroom within the building.
    /// Expected to be a string representing an integer, or null/empty for ground level or standalone classrooms.
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
    /// The texture of the classroom.
    /// This is a required field that describes the classroom's texture.
    /// </summary>
    string Texture,

    /// <summary>
    /// The width of the classroom in meters.
    /// Expected to be a string representing a positive number (e.g., "10.5").
    /// </summary>
    string Width,

    /// <summary>
    /// The length of the classroom in meters.
    /// Expected to be a string representing a positive number (e.g., "15.0").
    /// </summary>
    string Length,

    /// <summary>
    /// The height of the classroom in meters.
    /// Expected to be a string representing a positive number (e.g., "3.5").
    /// </summary>
    string Height,

    /// <summary>
    /// The X coordinate of the classroom's location within the building.
    /// Expected to be a string representing a non-negative number (e.g., "25.75").
    /// </summary>
    string XCoordinate,

    /// <summary>
    /// The Y coordinate of the classroom's location within the building.
    /// Expected to be a string representing a non-negative number (e.g., "42.30").
    /// </summary>
    string YCoordinate,

    /// <summary>
    /// The Z coordinate of the classroom's location within the building.
    /// Expected to be a string representing a non-negative number (e.g., "0.0").
    /// </summary>
    string ZCoordinate);
