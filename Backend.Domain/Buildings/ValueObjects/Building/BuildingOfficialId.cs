using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;

/// <summary>
/// Value Object representing the official identifier of a building.
/// Ensures the ID is non-empty, non-numeric, and does not exceed 30 characters.
/// </summary>
public partial class BuildingOfficialId : ValueObject
{
    /// <summary>
    /// The internal string value of the building's official ID.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Private constructor that assigns the validated official ID value.
    /// </summary>
    /// <param name="value">A valid official ID string.</param>
    private BuildingOfficialId(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Attempts to create a <see cref="BuildingOfficialId"/> instance from the given string.
    /// </summary>
    /// <param name="value">The input string to validate and convert.</param>
    /// <param name="buildingOfficialId">The resulting <see cref="BuildingOfficialId"/> instance if valid; otherwise null.</param>
    /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
    public static bool TryCreate(string value, out BuildingOfficialId? buildingOfficialId)
    {
        buildingOfficialId = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        // Reject purely numeric values
        if (IdRegex().IsMatch(value))
        {
            return false;
        }

        if (value.Length > 30)
        {
            return false;
        }

        buildingOfficialId = new BuildingOfficialId(value);
        return true;
    }

    /// <summary>
    /// Creates a <see cref="BuildingOfficialId"/> instance from the given string.
    /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
    /// </summary>
    /// <param name="input">The input string to validate and convert.</param>
    /// <returns>A valid <see cref="BuildingOfficialId"/> instance.</returns>
    /// <exception cref="BuildingDataException">Thrown when the input is null, numeric, or exceeds 30 characters.</exception>
    public static BuildingOfficialId Create(string input)
    {
        var result = BuildingOfficialId.TryCreate(input, out var buildingOfficialId);
        if (!result || buildingOfficialId is null)
        {
            throw new BuildingDataException(string.Format("Official ID {0} is invalid, verify that it does not contain " +
                "only numbers and is not longer than 30 characters", input));
        }

        return buildingOfficialId;
    }

    /// <summary>
    /// Provides the components used to compare equality between value objects.
    /// </summary>
    /// <returns>An enumerable containing the official ID value.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Regular expression used to validate building Id string.
    /// Accepts formats B001 and C001.
    /// </summary>
    /// <returns>A compiled regex for Id validation.</returns>
    [GeneratedRegex(@"^\d+$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex IdRegex();
}
