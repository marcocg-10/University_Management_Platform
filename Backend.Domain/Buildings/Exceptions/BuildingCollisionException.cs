using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;

/// <summary>
/// Exception thrown when attempting to place a building that collides with an existing building.
/// </summary>
public class BuildingCollisionException : BuildingException
{
    /// <summary>
    /// Gets the X coordinate of the building that already exists.
    /// </summary>
    public BuildingRenderInfo _renderInfo { get; }


    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingCollisionException"/> class.
    /// </summary>
    /// <param name="buildingOfficialId">The official ID of the building that already exists.</param>
    /// <param name="buildingName">The name of the building that already exists.</param>
    public BuildingCollisionException(BuildingRenderInfo renderInfo)
        : base($"Cannot place building at coordinates X:'{renderInfo.XCoodinate.XValue}', Y:'{renderInfo.YCoodinate.YValue}', Z:'{renderInfo.ZCoodinate.ZValue}' because it collides with an existing building.")
    {
        _renderInfo = renderInfo;
    }
}