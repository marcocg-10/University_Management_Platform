using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

/// <summary>
/// Represents a value object for a role name, ensuring it adheres to specific validation rules.
/// </summary>
public partial class RoleName : ValueObject, IComparable<RoleName>
{
    /// <summary>
    /// The internal valid string representing the role name.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="RoleName"/> value object.
    /// </summary>
    /// <param name="value">The string value of the role name.</param>
    private RoleName(string value)
    {
        Value = value;
    }

    public static implicit operator String(RoleName value)
    {
        return value.Value;
    }

    /// <summary>
    /// Attempts to create a valid role name value object.
    /// </summary>
    /// <param name="value">The string value to validate and create.</param>
    /// <param name="name">The created RoleName value object if successful.</param>
    /// <param name="error">An error message if the creation failed.</param>
    /// <returns>True if the value is valid, otherwise false.</returns>
    public static bool TryCreate(string value, out RoleName? name, out string? error)
    {
        name = null;
        error = null;
        if (!NameRegex().IsMatch(value))
        {
            error = $"Name {value} is invalid: Name must be over 3 characters (letters, spaces) and must not start with a whitespace";
            return false;
        }
        name = new RoleName(value);
        return true;
    }

    /// <summary>
    /// Creates an assumed valid role name value object.
    /// </summary>
    /// <param name="input">The string with the role name.</param>
    /// <returns>A valid role name value object.</returns>
    /// <exception cref="ValidationException">Thrown when the input is not valid.</exception>
    public static RoleName Create(string input)
    {
        var result = RoleName.TryCreate(input, out var name, out var error);
        if (result)
        {
            return name!;
        }
        throw new RoleInvalidDataException(error!);
    }

    /// <summary>
    /// Gets this value object's attributes necessary for testing equality.
    /// </summary>
    /// <returns>The role name string.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Provides a regular expression that validates names consisting of 3 to 30 characters,  starting with a letter and
    /// followed by letters, digits, or hyphens.
    /// </summary>
    /// <remarks>The pattern enforces the following rules: <list type="bullet"> <item><description>The name
    /// must start with an uppercase or lowercase letter.</description></item> <item><description>Subsequent characters
    /// can include letters, digits, or hyphens.</description></item> <item><description>The total length must be
    /// between 3 and 30 characters.</description></item> </list> This method is generated and returns a precompiled
    /// regular expression for optimal performance.</remarks>
    /// <returns>A <see cref="Regex"/> instance configured to match valid names based on the specified pattern.</returns>
    [GeneratedRegex(@"^[A-Za-z][A-Za-z0-9-]{2,29}$")]
    private static partial Regex NameRegex();

    public int CompareTo(RoleName? other)
    {
        if (other is null) return 1;
        return string.Compare(this.Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }
}
