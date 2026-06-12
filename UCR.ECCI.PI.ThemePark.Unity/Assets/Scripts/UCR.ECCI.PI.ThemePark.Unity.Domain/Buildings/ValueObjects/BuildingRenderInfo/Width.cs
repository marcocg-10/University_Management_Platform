using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.ValueObjects;

using System.Collections.Generic;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.ValueObjects.BuildingRenderInfo
{
    /// <summary>
    /// Value Object representing the width of a building element in the rendering system.
    /// Ensures the width is positive, non-zero, and does not exceed 2,000,000.
    /// </summary>
    public partial class Width : ValueObject
    {
        /// <summary>
        /// The internal decimal value representing the width.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Private constructor that assigns the validated width value.
        /// </summary>
        /// <param name="value">A valid width value.</param>
        private Width(decimal value)
        {
            Value = value;
        }

        /// <summary>
        /// Attempts to create a <see cref="Width"/> instance from the given decimal value.
        /// </summary>
        /// <param name="value">The input decimal value to validate and convert.</param>
        /// <param name="Value">The resulting <see cref="Width"/> instance if valid; otherwise null.</param>
        /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
        public static bool TryCreate(decimal value, out Width? Value)
        {
            Value = null;

            // Reject negative values
            if (value < 0)
            {
                return false;
            }

            // Reject values greater than 2,000,000
            if (value > 2000000)
            {
                return false;
            }

            // Reject zero
            if (value == 0)
            {
                return false;
            }

            Value = new Width(value);
            return true;
        }

        /// <summary>
        /// Creates a <see cref="Width"/> instance from the given decimal value.
        /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
        /// </summary>
        /// <param name="input">The input decimal value to validate and convert.</param>
        /// <returns>A valid <see cref="Width"/> instance.</returns>
        /// <exception cref="BuildingDataException">
        /// Thrown when the input is zero, negative, or exceeds the maximum allowed width.
        /// </exception>
        public static Width Create(decimal input)
        {
            var result = Width.TryCreate(input, out var Value);
            if (!result || Value is null)
            {
                throw new BuildingDataException(string.Format("Width {0} is invalid", input));
            }

            return Value;
        }

        /// <summary>
        /// Provides the components used to compare equality between value objects.
        /// </summary>
        /// <returns>An enumerable containing the width value.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}