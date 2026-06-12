using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

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
    /// <param name="color">Color of the classroom.</param>
    /// <param name="texture">Texture of the classroom.</param>"
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
    /// <param name="color">Color of the classroom.</param>
    /// <param name="texture">Texture of the classroom.</param>"
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

    /// <summary>
    /// Updates the classroom properties, including base learning space properties.
    /// </summary>
    /// <remarks>
    /// Calls the base Update method and then handles classroom-specific updates.
    /// </remarks>
    public override void Update(
        int? buildingId = null,
        int? floorLevel = null,
        string? roomId = null,
        LearningSpaceColor? color = null,
        LearningSpaceTexture? texture = null,
        LearningSpaceDimensions? dimensions = null,
        LearningSpaceCoordinates? coordinates = null,
        bool updateBuildingId = true,
        bool updateFloorLevel = true)
    {
        // Call base class Update to handle common properties.
        base.Update(buildingId, floorLevel, roomId, color, texture, dimensions, coordinates, updateBuildingId, updateFloorLevel);
    }

    /// <summary>
    /// Ctor for EF Core.
    /// </summary>
    protected Classroom(){ }
}
