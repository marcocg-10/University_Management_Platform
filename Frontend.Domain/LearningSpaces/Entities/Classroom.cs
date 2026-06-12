using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Entities;

/// <summary>
/// Represents a classroom learning space within a building.
/// </summary>
/// <remarks>Inherits from LearningSpace.</remarks>
public class Classroom : LearningSpace
{
    /// <summary>
    /// Creates an instance of a classroom with basic properties.
    /// </summary>
    /// <param name="id">Unique Id of classroom.</param>
    /// <param name="buildingId">Building's Id the classroom may be located in.</param>
    /// <param name="floorLevel">Floor level inside building.</param>
    /// <param name="roomId">Identifier of room inside building.</param>
    /// <param name="color">Color of the learning space in hexadecimal format.</param>
    /// <param name="texture">Texture of the learning space.</param>
    /// <param name="dimensions">Dimensions (Width, Length, Height) of room.</param>
    /// <param name="coordinates">Coordinates (X-axis, Y-axis, Z-axis) of room.</param>
    public Classroom(
        int id,
        int? buildingId,
        int? floorLevel,
        string roomId,
        LearningSpaceColor color,
        LearningSpaceTexture texture,
        LearningSpaceDimensions dimensions,
        LearningSpaceCoordinates coordinates) : base(id, buildingId, floorLevel, roomId, color, texture, dimensions, coordinates)
    {
    }

    /// <summary>
    /// Creates an instance of a classroom with basic properties.
    /// </summary>
    /// <param name="buildingId">Building's Id the classroom may be located in.</param>
    /// <param name="floorLevel">Floor level inside building.</param>
    /// <param name="roomId">Identifier of room inside building.</param>
    /// <param name="color">Color of the learning space in hexadecimal format.</param>
    /// <param name="texture">Texture of the learning space.</param>
    /// <param name="dimensions">Dimensions (Width, Length, Height) of room.</param>
    /// <param name="coordinates">Coordinates (X-axis, Y-axis, Z-axis) of room.</param>
    public Classroom(
        int? buildingId,
        int? floorLevel,
        string roomId,
        LearningSpaceColor color,
        LearningSpaceTexture texture,
        LearningSpaceDimensions dimensions,
        LearningSpaceCoordinates coordinates) : base(buildingId, floorLevel, roomId, color, texture, dimensions, coordinates)
    {
    }
}
