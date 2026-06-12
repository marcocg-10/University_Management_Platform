using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;

/// <summary>
/// Represents a value object for a permission name, ensuring it adheres to specific validation rules.
/// </summary>
/// <remarks>A <see cref="PermissionName"/> encapsulates a valid permission name string, enforcing constraints on
/// its format and length. Instances of this class are immutable and can only be created through the provided factory
/// methods (<see cref="TryCreate"/> and <see cref="Create"/>), which ensure the validity of the input.</remarks>
public partial class PermissionName : ValueObject
{
    /// <summary>
    /// The internal valid string representing a permission name address
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Represents a valid permission name address
    /// </summary>
    /// <param name="value">The string with the permission name address</param>
    private PermissionName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Attempts to create a valid permission name value object
    /// </summary>
    /// <param name="value">The string with the permission name</param>
    /// <param name="name">The created permission name value object or null if not valid</param>
    /// <returns>true if the value was valid, otherwise false </returns>
    public static bool TryCreate(string value, out PermissionName? name)
    {
        name = null;

        if (!NameRegex().IsMatch(value))
        {
            return false;
        }
        name = new PermissionName(value);
        return true;
    }

    /// <summary>
    /// Creates an assumed valid permission name value object
    /// </summary>
    /// <param name="input">The string with the permission name address</param>
    /// <returns>A valid permission name value object</returns>
    /// <exception cref="ValidationException">Thrown when the input is not valid</exception>
    public static PermissionName Create(string input)
    {
        var result = PermissionName.TryCreate(input, out var name);
        if (!result || name is null)
        {
            throw new ValidationException(
                string.Format("Name {0} is invalid: should be from 3 to 30 characters (letters, numbers, -) and must start with a letter", input));
        }
        return name;
    }

    /// <summary>
    /// Gets this value object's attributes necessary for testing equality
    /// </summary>
    /// <returns>The permission name string</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Tests the equality of two permission name value objects
    /// </summary>
    /// <param name="one">An optional permission name value object</param>
    /// <param name="two">Another optional permission name value object</param>
    /// <returns>True when the permission names are the same, otherwise false</returns>
    public static bool operator ==(PermissionName? one, PermissionName? two)
    {
        return ValueObject.EqualOperator(one, two);
    }

    /// <summary>
    /// Tests the unequality of two permission name value objects
    /// </summary>
    /// <param name="one">An optional permission name value object</param>
    /// <param name="two">Another optional permission name value object</param>
    /// <returns>True when the permission name are not the same, otherwise false</returns>
    public static bool operator !=(PermissionName? one, PermissionName? two)
    {
        return ValueObject.NotEqualOperator(one, two);
    }

    /// <summary>
    /// Regex used to validate permission name format
    /// </summary>
    /// <remarks>
    /// Permission name must start with a letter and be between 3 and 30 alphanumeric characters
    /// </remarks> 
    [GeneratedRegex(@"^[A-Za-z][A-Za-z0-9-]{2,29}$")]
    private static partial Regex NameRegex();
}

