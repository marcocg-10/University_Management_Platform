using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.ValueObjects;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.ValueObjects.BuildingRenderInfo
{

    /// <summary>
    /// Value Object representing the Y coordinate of a building element in the rendering system.
    /// Ensures the coordinate is positive, non-zero, and does not exceed 2,000,000.
    /// </summary>
    public partial class Y : ValueObject
    {
        /// <summary>
        /// The internal decimal value representing the Y coordinate.
        /// </summary>
        public decimal YValue { get; }

        /// <summary>
        /// Private constructor that assigns the validated Y coordinate value.
        /// </summary>
        /// <param name="value">A valid Y coordinate value.</param>
        private Y(decimal value)
        {
            YValue = value;
        }

        /// <summary>
        /// Attempts to create a <see cref="Y"/> instance from the given decimal value.
        /// </summary>
        /// <param name="value">The input decimal value to validate and convert.</param>
        /// <param name="YValue">The resulting <see cref="Y"/> instance if valid; otherwise null.</param>
        /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
        public static bool TryCreate(decimal value, out Y? YValue)
        {
            YValue = null;

            // Reject values greater than 2,000,000
            if (value > 2000000)
            {
                return false;
            }

            // Reject values less than 2,000,000
            if (value < -2000000)
            {
                return false;
            }

            YValue = new Y(value);
            return true;
        }

        /// <summary>
        /// Creates a <see cref="Y"/> instance from the given decimal value.
        /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
        /// </summary>
        /// <param name="input">The input decimal value to validate and convert.</param>
        /// <returns>A valid <see cref="Y"/> instance.</returns>
        /// <exception cref="BuildingDataException">
        /// Thrown when the input is zero, negative, or exceeds the maximum allowed Y coordinate.
        /// </exception>
        public static Y Create(decimal input)
        {
            var result = Y.TryCreate(input, out var YValue);
            if (!result || YValue is null)
            {
                throw new BuildingDataException(string.Format("Y Coordinate {0} is invalid", input));
            }

            return YValue;
        }

        /// <summary>
        /// Provides the components used to compare equality between value objects.
        /// </summary>
        /// <returns>An enumerable containing the Y coordinate value.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return YValue;
        }
    }
}