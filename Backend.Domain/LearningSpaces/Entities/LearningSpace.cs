using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

/// <summary>
/// Represents a learning space. 
/// </summary>
/// <remarks>
/// Every particular learning space should inherit from this class.
/// Coordinates are based on a 3D plane (X, Y and Z axis) where the origin (0,0,0) is
/// located at the corner of the building or general plane.
/// </remarks>
public class LearningSpace
{
    /// <summary>
    /// Unique identifier of the learning space, generated automatically.
    /// </summary>
    /// <remarks>
    /// Used in SQL Server, but included here as well as it is useful for
    /// telling multiple learning spaces with same RoomId apart.
    /// </remarks>
    public int Id { get; private set; }

    /// <summary>
    /// Represents the Id of the building where this learning space is located.
    /// </summary>
    /// <remarks>
    /// Can be null if the building is not registered. This means that a learning space
    /// can exist without any relation to a building.
    /// </remarks>
    public int? BuildingId { get; private set; }

    /// <summary>
    /// Represents the floor level where this learning space is 
    /// located inside the building.
    /// </summary>
    /// <remarks>
    /// Can be null if the building is not registered as well. The learning space may not
    /// be related to a floor level if the building is not registered.
    /// </remarks>
    public int? FloorLevel { get; private set; }

    /// <summary>
    /// Represents the identifier of the learning space within the building as a room.
    /// </summary>
    /// <remarks>
    /// This does not refer to the learning space directly, but to the room the learning
    /// space is located in. Should be unique, but only within the same building.
    /// </remarks>
    public string RoomId { get; private set; }

    /// <summary>
    /// Gets the color of the Learning Space.
    /// </summary>
    /// <remarks>
    /// Has default value if not specified.
    /// </remarks>
    public LearningSpaceColor Color { get; private set; }

    /// <summary>
    /// Gets the texture associated with the learning space.
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
    /// Gets the current building associated with this instance.
    /// </summary>
    public Building? Building { get; private set; }

    /// <summary>
    /// Collection of interactive components located in this learning space.
    /// </summary>
    public ICollection<InteractiveComponent> InteractiveComponents { get; } = new List<InteractiveComponent>();

    // TODO: Add a list of non-interactive components when implemented.

    /// <summary>
    /// Creates an instance of a learning space with basic properties.
    /// </summary>
    /// <param name="id">Unique Id of learning space.</param>
    /// <param name="buildingId">Building Id the learning space may be located in.</param>
    /// <param name="floorLevel">Floor level inside building.</param>
    /// <param name="roomId">Identifier of room inside building.</param>
    /// <param name="color">Color of the learning space.</param>
    /// <param name="texture">Texture of the learning space.</param>"
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

    /// <summary>
    /// Updates the learning space properties with provided values. 
    /// For buildingId and floorLevel, null values are explicitly set when provided.
    /// For other parameters, only non-null values will be updated, allowing partial updates.
    /// Subclasses can override this method to provide specialized update behavior.
    /// </summary>
    /// <param name="buildingId">New building ID. Null values are explicitly set.</param>
    /// <param name="floorLevel">New floor level. Null values are explicitly set.</param>
    /// <param name="roomId">New room identifier. If null, the current value is preserved.</param>
    /// <param name="color">New color. If null, the current value is preserved.</param>
    /// <param name="dimensions">New dimensions. If null, the current value is preserved.</param>
    /// <param name="coordinates">New coordinates. If null, the current value is preserved.</param>
    /// <param name="updateBuildingId">Indicates whether to update buildingId. Defaults to true.</param>
    /// <param name="updateFloorLevel">Indicates whether to update floorLevel. Defaults to true.</param>
    /// <exception cref="ArgumentException">Thrown when roomId is provided but is empty or whitespace.</exception>
    public virtual void Update(
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
        // Update BuildingId with explicit null support
        if (updateBuildingId)
        {
            BuildingId = buildingId;
            // If removing from building, also remove floor level
            if (buildingId == null)
                FloorLevel = null;
        }

        // Update FloorLevel with explicit null support (only if not already cleared by BuildingId update)
        if (updateFloorLevel && !(updateBuildingId && buildingId == null))
            FloorLevel = floorLevel;

        // Update RoomId if provided (with validation)
        if (roomId != null)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                throw new ValidationException("Room ID cannot be empty or whitespace when provided.");
            RoomId = roomId;
        }

        // Update Color if provided
        if (color != null)
            Color = color;

        // Update Texture if provided
        if (texture != null)
            Texture = texture;

        // Update Dimensions if provided
        if (dimensions != null)
            Dimensions = dimensions;

        // Update Coordinates if provided
        if (coordinates != null)
            Coordinates = coordinates;
    }

    /// <summary>
    /// Ctor for EF Core.
    /// </summary>
    protected LearningSpace() { }
}
