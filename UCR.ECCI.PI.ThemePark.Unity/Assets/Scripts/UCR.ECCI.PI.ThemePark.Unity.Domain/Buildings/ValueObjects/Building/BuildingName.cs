using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.ValueObjects;
using System.Collections.Generic;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.ValueObjects
{
    /// <summary>
    /// Value Object representing the name of a building within the theme park domain.
    /// Ensures the name is non-empty and does not exceed 200 characters.
    /// </summary>
    public partial class BuildingName : ValueObject
    {
        /// <summary>
        /// The internal string value of the building name.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Private constructor that assigns the validated name value.
        /// </summary>
        /// <param name="value">A valid building name string.</param>
        private BuildingName(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Attempts to create a <see cref="BuildingName"/> instance from the given string.
        /// </summary>
        /// <param name="value">The input string to validate and convert.</param>
        /// <param name="buildingName">The resulting <see cref="BuildingName"/> instance if valid; otherwise null.</param>
        /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
        public static bool TryCreate(string value, out BuildingName? buildingName)
        {
            buildingName = null;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Length > 200)
            {
                return false;
            }

            buildingName = new BuildingName(value);
            return true;
        }

        /// <summary>
        /// Creates a <see cref="BuildingName"/> instance from the given string.
        /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
        /// </summary>
        /// <param name="input">The input string to validate and convert.</param>
        /// <returns>A valid <see cref="BuildingName"/> instance.</returns>
        /// <exception cref="BuildingDataException">Thrown when the input is null, empty, or exceeds 200 characters.</exception>
        public static BuildingName Create(string input)
        {
            var result = BuildingName.TryCreate(input, out var buildingName);
            if (!result || buildingName is null)
            {
                throw new BuildingDataException(string.Format("Name {0} is invalid", input));
            }

            return buildingName;
        }

        /// <summary>
        /// Provides the components used to compare equality between value objects.
        /// </summary>
        /// <returns>An enumerable containing the value of the building name.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}