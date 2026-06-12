using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.ValueObjects;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.ValueObjects.BuildingRenderInfo
{
    /// <summary>
    /// Value Object representing the X coordinate of a building element in the rendering system.
    /// Ensures the coordinate is positive, non-zero, and does not exceed 2,000,000.
    /// </summary>
    public partial class X : ValueObject
    {
        /// <summary>
        /// The internal decimal value representing the X coordinate.
        /// </summary>
        public decimal XValue { get; }

        /// <summary>
        /// Private constructor that assigns the validated X coordinate value.
        /// </summary>
        /// <param name="value">A valid X coordinate value.</param>
        private X(decimal value)
        {
            XValue = value;
        }

        /// <summary>
        /// Attempts to create an <see cref="X"/> instance from the given decimal value.
        /// </summary>
        /// <param name="value">The input decimal value to validate and convert.</param>
        /// <param name="XValue">The resulting <see cref="X"/> instance if valid; otherwise null.</param>
        /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
        public static bool TryCreate(decimal value, out X? XValue)
        {
            XValue = null;

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

            XValue = new X(value);
            return true;
        }

        /// <summary>
        /// Creates an <see cref="X"/> instance from the given decimal value.
        /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
        /// </summary>
        /// <param name="input">The input decimal value to validate and convert.</param>
        /// <returns>A valid <see cref="X"/> instance.</returns>
        /// <exception cref="BuildingDataException">
        /// Thrown when the input is zero, negative, or exceeds the maximum allowed X coordinate.
        /// </exception>
        public static X Create(decimal input)
        {
            var result = X.TryCreate(input, out var XValue);
            if (!result || XValue is null)
            {
                throw new BuildingDataException(string.Format("X coordinate {0} is invalid", input));
            }

            return XValue;
        }

        /// <summary>
        /// Provides the components used to compare equality between value objects.
        /// </summary>
        /// <returns>An enumerable containing the X coordinate value.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return XValue;
        }
    }
}