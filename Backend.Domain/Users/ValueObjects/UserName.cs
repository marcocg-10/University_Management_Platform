using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;


namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

public partial class UserName : ValueObject, IComparable<UserName>
{
    public UserName(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static bool TryCreate(string value, out UserName? name, out string? error)
    {
        name = null;
        error = null;

        if (!NameRegex().IsMatch(value))
        {
            error = $"Name {value} is invalid: Name must be over 3 characters (letters, spaces) and must not start with a whitespace";
            return false;
        }
        name = new UserName(value);
        return true;
    }

    public static UserName Create(string input)
    {
        var result = UserName.TryCreate(input, out var name, out var error);
        if (!result || name is null)
        {
            throw new ValidationException(error!);
        }
        return name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Tests the equality of two permission name value objects
    /// </summary>
    /// <param name="one">An optional permission name value object</param>
    /// <param name="two">Another optional permission name value object</param>
    /// <returns>True when the emails are the same, otherwise false</returns>
    public static bool operator == (UserName? one, UserName? two)
    {
        return ValueObject.EqualOperator(one, two);
    }

    /// <summary>
    /// Tests the unequality of two permission name value objects
    /// </summary>
    /// <param name="one">An optional permission name value object</param>
    /// <param name="two">Another optional permission name value object</param>
    /// <returns>True when the permission name are not the same, otherwise false</returns>
    public static bool operator !=(UserName? one, UserName? two)
    {
        return ValueObject.NotEqualOperator(one, two);
    }

    [GeneratedRegex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ][ A-Za-zÁÉÍÓÚáéíóúÑñ]{3,50}$")]
    private static partial Regex NameRegex();

    public int CompareTo(UserName? other)
    {
        if (other is null) return 1;
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    public static implicit operator string(UserName name)
    {
        return name.Value;
    }

}
