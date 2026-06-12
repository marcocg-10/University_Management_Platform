using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;

/// <summary>
/// Represents a building entity in the theme park domain.
/// </summary>
public class Building
{
    private BuildingOfficialId buildingOfficialId;
    private BuildingName buildingName;
    private FloorCount floorCount;
    private object value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Building"/> class with official ID, name, and rendering information.
    /// </summary>
    /// <param name="officialId">Official ID of the building.</param>
    /// <param name="name">Official name of the building.</param>
    /// <param name="floorCount">Number of floors in the building.</param>
    /// <param name="buildingRenderInfo">Rendering information of the building.</param>
    public Building(
        BuildingOfficialId officialId,
        BuildingName name,
        FloorCount floorCount,
        BuildingRenderInfo buildingRenderInfo
        )
    {
        OfficialId = officialId;
        Name = name;
        FloorCount = floorCount;
        RenderInfo = buildingRenderInfo;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Building"/> class with system ID, official ID, and name.
    /// </summary>
    /// <param name="id">System unique identifier for the building.</param>
    /// <param name="officialId">Official ID of the building.</param>
    /// <param name="name">Official name of the building.</param>
    public Building(
        int id,
        BuildingOfficialId officialId,
        BuildingName name
        )
    {   
        Id = id;
        OfficialId = officialId;
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Building"/> class with official ID, name, and a custom value.
    /// </summary>
    /// <param name="buildingOfficialId">Official ID of the building.</param>
    /// <param name="buildingName">Official name of the building.</param>
    /// <param name="value">Custom value associated with the building.</param>
    public Building(BuildingOfficialId buildingOfficialId, BuildingName buildingName, object value)
    {
        this.buildingOfficialId = buildingOfficialId;
        this.buildingName = buildingName;
        this.value = value;
    }

    /// <summary>
    /// Updates the building's properties with those from another building instance.
    /// </summary>
    /// <param name="building">The <see cref="Building"/> instance to copy properties from.</param>
    public void UpdateBuilding(Building building)
    {
        this.Name = building.Name;
        this.FloorCount = building.FloorCount;
        if (this.RenderInfo != null && building.RenderInfo != null)
        {
            this.RenderInfo.UpdateRenderInfo(building.RenderInfo);
        }
    }

    /// <summary>
    /// Gets the system unique identifier for the building.
    /// </summary>
    /// <remarks> 
    /// Must be unique.
    /// </remarks>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the official unique identifier for the building.
    /// </summary>
    /// <remarks> 
    /// Must be unique.
    /// </remarks>
    public BuildingOfficialId OfficialId { get; }

    /// <summary>
    /// Gets the name of the building.
    /// </summary>
    /// <remarks> Must be unique.
    /// </remarks>
    public BuildingName Name { get; private set; }

    /// <summary>
    /// Gets the number of floors in the building.
    /// </summary>
    /// <remarks> Must be greater than zero.
    /// </remarks>
    public FloorCount FloorCount { get; private set; }

    /// <summary>
    /// Gets the rendering information for the building.
    /// </summary>
    /// <remarks> Rendering information must be unique for each building.
    /// </remarks>  
    public BuildingRenderInfo RenderInfo { get; private set; }

    /// <summary>
    /// Collection of learning spaces located in this building.
    /// </summary>
    public ICollection<LearningSpace> LearningSpaces { get; } = new List<LearningSpace>();

}
