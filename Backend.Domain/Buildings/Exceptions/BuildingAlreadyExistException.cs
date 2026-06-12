using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a building that already exists.
/// </summary>
public class DuplicateBuildingException : BuildingException
{
    /// <summary>
    /// Gets the official identifier of the building that already exists.
    /// </summary>
    public BuildingOfficialId BuildingOfficialId { get; }

    /// <summary>
    /// Gets the name of the building that already exists.
    /// </summary>
    public BuildingName BuildingName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateBuildingException"/> class.
    /// </summary>
    /// <param name="buildingOfficialId">The official ID of the building that already exists.</param>
    /// <param name="buildingName">The name of the building that already exists.</param>
    public DuplicateBuildingException(BuildingOfficialId buildingOfficialId, BuildingName buildingName)
    : base($"A building already exists with either the official ID '{buildingOfficialId?.Value}' " +
        $"or the name '{buildingName?.Value}'. Please verify both.")
    {
        BuildingOfficialId = buildingOfficialId;
        BuildingName = buildingName;
    }
}