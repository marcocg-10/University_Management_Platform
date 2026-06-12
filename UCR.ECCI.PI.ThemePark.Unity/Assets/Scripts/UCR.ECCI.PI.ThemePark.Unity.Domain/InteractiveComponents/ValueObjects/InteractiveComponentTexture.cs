using System.Collections.Generic;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Exceptions;
using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.ValueObjects
{
    public class InteractiveComponentsTexture : ValueObject
    {

        /// <summary>
        /// The internal string value of the textures file name.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Private constructor that assigns the validated texture value.
        /// </summary>
        /// <param name="value">A valid texture string.</param>
        public InteractiveComponentsTexture(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Attempts to create a <see cref="Texture"/> instance from the given string.
        /// </summary>
        /// <param name="value">The input string to validate and convert.</param>
        /// <param name="Value">The resulting <see cref="Texture"/> instance if valid; otherwise null.</param>
        /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
        public static bool TryCreate(string value, out InteractiveComponentsTexture? Value)
        {
            Value = null;

            //TODO: refactor return value when default value is defined
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (value.Length > 50)
            {
                return false;
            }

            Value = new InteractiveComponentsTexture(value);
            return true;
        }

        /// <summary>
        /// Creates a <see cref="Texture"/> instance from the given string.
        /// Throws a <see cref="InteractiveComponentsDataException"/> if the input is invalid.
        /// </summary>
        /// <param name="input">The input string to validate and convert.</param>
        /// <returns>A valid <see cref="Texture"/> instance.</returns>
        /// <exception cref="InteractiveComponentsDataException">
        /// Thrown when the input is null(for now), or exceeds 50 characters.
        /// </exception>
        public static InteractiveComponentsTexture Create(string input)
        {
            var result = InteractiveComponentsTexture.TryCreate(input, out var Value);
            if (!result || Value is null)
            {
                throw new InvalidTextureException(string.Format("Texture {0} is invalid, it must be less than or " +
                    "equal to 50 characters.", input));
            }

            return Value;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}