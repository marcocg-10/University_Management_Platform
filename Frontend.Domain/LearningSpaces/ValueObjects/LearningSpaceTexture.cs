using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.ValueObjects;

public class LearningSpaceTexture : ValueObject
{

    /// <summary>
    /// The internal string value of the textures file name.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Private constructor that assigns the validated texture value.
    /// </summary>
    /// <param name="value">A valid texture string.</param>
    private LearningSpaceTexture(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Attempts to create a <see cref="Texture"/> instance from the given string.
    /// </summary>
    /// <param name="value">The input string to validate and convert.</param>
    /// <param name="Value">The resulting <see cref="Texture"/> instance if valid; otherwise null.</param>
    /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
    public static bool TryCreate(string value, out LearningSpaceTexture? Value)
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

        Value = new LearningSpaceTexture(value);
        return true;
    }

    /// <summary>
    /// Creates a <see cref="Texture"/> instance from the given string.
    /// Throws a <see cref="ValidationException"/> if the input is invalid.
    /// </summary>
    /// <param name="input">The input string to validate and convert.</param>
    /// <returns>A valid <see cref="Texture"/> instance.</returns>
    /// <exception cref="ValidationException">
    /// Thrown when the input is null(for now), or exceeds 50 characters.
    /// </exception>
    public static LearningSpaceTexture Create(string input)
    {
        var result = LearningSpaceTexture.TryCreate(input, out var Value);
        if (!result || Value is null)
        {
            throw new ValidationException(string.Format("Texture {0} is invalid, it must be less than or " +
                "equal to 50 characters.", input));
        }

        return Value;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
