using System.Collections.Generic;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.ValueObjects
{

    /// <summary>
    /// Represents a validated color value object in hexadecimal format.
    /// Ensures color values follow the correct format (e.g., '#FFFFFF' or '#FFF').
    /// </summary>
    public partial class Color : ValueObject
    {
        /// <summary>
        /// Gets the validated color value in uppercase hexadecimal format.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class
        /// with the specified color value.
        /// </summary>
        /// <param name="value">
        /// The color value in hexadecimal format (e.g., '#FFFFFF').
        /// </param>
        /// <exception cref="InvalidColorException">
        /// Thrown if the color is null, empty, whitespace, or not in the correct format.
        /// </exception>
        public Color(string value)
        {
            ValidateColor(value);

            // Ensure the color is stored in uppercase for consistency
            Value = value.ToUpperInvariant();
        }

        /// <summary>
        /// Returns the components of the value object for equality comparisons.
        /// </summary>
        /// <returns>An enumerable of equality components.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        /// <summary>
        /// Validates the given color value.
        /// </summary>
        /// <param name="value">The color value to validate.</param>
        /// <exception cref="InvalidColorException">
        /// Thrown if the color is null, empty, whitespace, or does not match
        /// the expected hexadecimal format.
        /// </exception>
        private static void ValidateColor(string value)
        {
            if (value is null)
                throw new InvalidColorException("Color cannot be null.");

            value = value.Trim();

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidColorException("Color cannot be empty or whitespace.");
        }
    }
}