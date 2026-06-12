using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Entities;

/// <summary>
/// Represents a learning space within a building, such as a classroom or laboratory.
/// </summary>
public class LearningSpace
{
    /// <summary>
    /// Unique identifier for the learning space.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier of the building where the learning space is located.
    /// </summary>
    public int? BuildingId { get; set; }

    /// <summary>
    /// Floor level where the learning space is located.
    /// </summary>
    public int? FloorLevel { get; set; }

    /// <summary>
    /// Room identifier of the learning space.
    /// </summary>
    public string RoomId { get; set; }

    /// <summary>
    /// Gets the color of the Learning Space.
    /// </summary>
    public LearningSpaceColor Color { get; private set; }

    /// <summary>
    /// Gets the color of the Learning Space.
    /// </summary>
    public LearningSpaceTexture Texture { get; private set; }

    /// <summary>
    /// Represents the dimensions of a room within a floor in a building.
    /// </summary>
    public LearningSpaceDimensions Dimensions { get; private set; }

    /// <summary>
    /// Represents the coordinates where this learning space is located within
    /// a floor level in a building.
    /// </summary>
    public LearningSpaceCoordinates Coordinates { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpace"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the learning space.</param>
    /// <param name="buildingId">Identifier of the building where the learning space is located.</param>
    /// <param name="floorLevel">Floor level where the learning space is located</param>
    /// <param name="roomId">Room identifier of the learning space.</param>
    /// <param name="color">Color of the learning space in hexadecimal format.</param>
    /// <param name="texture">Texture of the learning space.</param>
    /// <param name="width">Width of the learning space in meters.</param>
    /// <param name="length">Length of the learning space in meters.</param>
    /// <param name="height">Height of the learning space in meters.</param>
    /// <param name="xCoordinate">X-coordinate of the learning space's location.</param
    /// <param name="yCoordinate">Y-coordinate of the learning space's location.</param>
    /// <param name="zCoordinate">Z-coordinate of the learning space's location.</param
    /// <summary>
    /// Creates an instance of a learning space with basic properties.
    /// </summary>
    /// <param name="id">Unique Id of learning space.</param>
    /// <param name="buildingId">Building Id the learning space may be located in.</param>
    /// <param name="floorLevel">Floor level inside building.</param>
    /// <param name="roomId">Identifier of room inside building.</param>
    /// <param name="color">Color of the learning space.</param>
    /// <param name="texture">Texture of the learning space.</param>
    /// <param name="dimensions">Dimensions (Width, Length, Height) of room.</param>
    /// <param name="coordinates">Coordinates (X-axis, Y-axis, Z-axis) of room.</param>
    public LearningSpace(
        int id,
        int? buildingId,
        int? floorLevel,
        string roomId,
        LearningSpaceColor color,
        LearningSpaceTexture texture,
        LearningSpaceDimensions dimensions,
        LearningSpaceCoordinates coordinates)
    {
        Id = id;
        BuildingId = buildingId;
        FloorLevel = floorLevel;
        RoomId = roomId;
        Color = color;
        Texture = texture;
        Dimensions = dimensions;
        Coordinates = coordinates;
    }

    /// <summary>
    /// Creates an instance of a learning space with basic properties, with no Id as
    /// the user is creating one that does not exist yet in the database.
    /// </summary>
    /// <param name="buildingId">Building Id the learning space may be located in.</param>
    /// <param name="floorLevel">Floor level inside building.</param>
    /// <param name="roomId">Identifier of room inside building.</param>
    /// <param name="color">Color of the learning space.</param>
    /// <param name="texture">Texture of the learning space.</param>
    /// <param name="dimensions">Dimensions (Width, Length, Height) of room.</param>
    /// <param name="coordinates">Coordinates (X-axis, Y-axis, Z-axis) of room.</param>
    public LearningSpace(
        int? buildingId,
        int? floorLevel,
        string roomId,
        LearningSpaceColor color,
        LearningSpaceTexture texture,
        LearningSpaceDimensions dimensions,
        LearningSpaceCoordinates coordinates)
    {
        BuildingId = buildingId;
        FloorLevel = floorLevel;
        RoomId = roomId;
        Color = color;
        Texture = texture;
        Dimensions = dimensions;
        Coordinates = coordinates;
    }
}
