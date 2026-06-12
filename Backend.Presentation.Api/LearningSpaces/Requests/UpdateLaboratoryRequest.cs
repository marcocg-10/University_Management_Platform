namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Requests;

/// <summary>
/// Represents the payload for updating an existing <see cref="UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities.Laboratory"/> 
/// via the API.
/// </summary>
/// <remarks>
/// The laboratory ID is not included as it comes from the route parameter.
/// All numeric properties can be provided as their native types since this follows
/// the simpler DTO pattern rather than the string-based parsing approach.
/// </remarks>
public record UpdateLaboratoryRequest(
    /// <summary>
    /// The identifier of the building where the laboratory is located.
    /// Can be null for standalone laboratories.
    /// </summary>
    string? BuildingId,

    /// <summary>
    /// The floor level of the laboratory within the building.
    /// Can be null for ground level or standalone laboratories.
    /// </summary>
    string? FloorLevel,

    /// <summary>
    /// The identifier of the room for the laboratory.
    /// This is a required field that identifies the laboratory within the building/floor.
    /// </summary>
    string RoomId,

    /// <summary>
    /// The color of the laboratory.
    /// This is a required field that describes the laboratory's color.
    /// </summary>
    string Color,

    /// <summary>
    /// The Texture of the laboratory.
    /// </summary>
    string Texture,

    /// <summary>
    /// The width of the laboratory in meters.
    /// Must be a positive number.
    /// </summary>
    string Width,

    /// <summary>
    /// The length of the laboratory in meters.
    /// Must be a positive number.
    /// </summary>
    string Length,

    /// <summary>
    /// The height of the laboratory in meters.
    /// Must be a positive number.
    /// </summary>
    string Height,

    /// <summary>
    /// The X coordinate of the laboratory's location within the building.
    /// Must be non-negative.
    /// </summary>
    string XCoordinate,

    /// <summary>
    /// The Y coordinate of the laboratory's location within the building.
    /// Must be non-negative.
    /// </summary>
    string YCoordinate,

    /// <summary>
    /// The Z coordinate of the laboratory's location within the building.
    /// Must be non-negative.
    /// </summary>
    string ZCoordinate);