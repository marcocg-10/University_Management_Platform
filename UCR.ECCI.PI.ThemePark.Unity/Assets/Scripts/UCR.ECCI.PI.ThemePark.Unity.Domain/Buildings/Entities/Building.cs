using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities
{
    /// <summary>
    /// Represents a building in the frontend domain layer.
    /// </summary>
    public class Building
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        /// <param name="officialId">The official identifier of the building.</param>
        /// <param name="name">The name of the building.</param>
        /// <param name="renderInfo">The rendering information used to visually represent the building.</param>
        public Building(BuildingOfficialId officialId, BuildingName name, FloorCount floorCount, BuildingRenderInfo renderInfo)
        {
            OfficialId = officialId;
            Name = name;
            FloorCount = floorCount;
            RenderInfo = renderInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        /// <param name="id">The system identifier of the building.</param>
        /// <param name="officialId">The official identifier of the building.</param>
        /// <param name="name">The name of the building.</param>
        /// <param name="renderInfo">The rendering information used to visually represent the building.</param>
        public Building(int id, BuildingOfficialId officialId, BuildingName name, FloorCount floorCount, BuildingRenderInfo renderInfo)
        {
            Id = id;
            OfficialId = officialId;
            Name = name;
            FloorCount = floorCount;
            RenderInfo = renderInfo;
        }

        /// <summary>
        /// Gets the system identifier of the building.
        /// This value should match the ID provided by the backend.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Gets the official identifier of the building.
        /// This value should match the ID provided by the backend.
        /// </summary>
        public BuildingOfficialId OfficialId { get; }

        /// <summary>
        /// Gets the display name of the building.
        /// </summary>
        public BuildingName Name { get; }

        /// <summary>
        /// Gets the floor count of the building.
        /// </summary>
        public FloorCount FloorCount { get; }

        /// <summary>
        /// Rendering information used to visually represent the building.
        /// </summary>
        public BuildingRenderInfo RenderInfo { get; }
    }
}

