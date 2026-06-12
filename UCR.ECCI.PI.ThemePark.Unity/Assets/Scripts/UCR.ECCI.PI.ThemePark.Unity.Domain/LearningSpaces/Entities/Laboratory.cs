using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities
{
    /// <summary>
    /// Represents a laboratory learning space within a building.
    /// </summary>
    /// <remarks>Inherits from LearningSpace.</remarks>
    public class Laboratory : LearningSpace
    {
        /// <summary>
        /// Creates an instance of a laboratory with basic properties.
        /// </summary>
        /// <param name="id">Unique Id of laboratory.</param>
        /// <param name="buildingId">Building's Id the laboratory may be located in.</param>
        /// <param name="floorLevel">Floor level inside building.</param>
        /// <param name="roomId">Identifier of room inside building.</param>
        /// <param name="color">Color of the laboratory.</param>
        /// <param name="texture">Texture of the laboratory.</param>"
        /// <param name="dimensions">Dimensions (Width, Length, Height) of room.</param>
        /// <param name="coordinates">Coordinates (X-axis, Y-axis, Z-axis) of room.</param>
        public Laboratory(
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
        /// Creates an instance of a laboratory with basic properties.
        /// </summary>
        /// <param name="buildingId">Building's Id the laboratory may be located in.</param>
        /// <param name="floorLevel">Floor level inside building.</param>
        /// <param name="roomId">Identifier of room inside building.</param>
        /// <param name="color">Color of the laboratory.</param>
        /// <param name="texture">Texture of the laboratory.</param>""
        /// <param name="dimensions">Dimensions (Width, Length, Height) of room.</param>
        /// <param name="coordinates">Coordinates (X-axis, Y-axis, Z-axis) of room.</param>
        public Laboratory(
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
        /// Updates the laboratory properties, including base learning space properties.
        /// </summary>
        /// <remarks>
        /// Calls the base Update method and then handles laboratory-specific updates.
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
        protected Laboratory() { }
    }
}

