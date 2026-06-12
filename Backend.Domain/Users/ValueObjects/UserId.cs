using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;


namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

public partial class UserId : ValueObject, IComparable<UserId>
{
    public UserId(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static bool TryCreate(string value, out UserId? id, out string? error)
    {
        id = null;
        error = null;

        if (!IdRegex().IsMatch(value))
        {
            error = $"id {value} is invalid: Id must be between 5 and 30 characters (letters, numbers, hyphens) and must not start or end with a hyphen";
            return false;
        }
        id = new UserId(value);
        return true;
    }

    public static UserId Create(string input)
    {
        var result = UserId.TryCreate(input, out var id, out var error);
        if (!result || id is null)
        {
            throw new ValidationException(error!);
        }
        return id;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Tests the equality of two user id value objects
    /// </summary>
    /// <param name="one">An optional user id value object</param>
    /// <param name="two">An optional user id value object</param>
    /// <returns>True when the user ids are the same, otherwise false</returns>
    public static bool operator ==(UserId? one, UserId? two)
    {
        return ValueObject.EqualOperator(one, two);
    }

    public int CompareTo(UserId? other)
    {
        if (other is null) return 1;
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    public static implicit operator String(UserId value)
    {
        return value.Value;
    }

    /// <summary>
    /// Tests the inequality of two user id value objects
    /// </summary>
    /// <param name="one">An optional user id value object</param>
    /// <param name="two">An optional user id value object</param>
    /// <returns>True when the user ids are not the same, otherwise false</returns>
    public static bool operator !=(UserId? one, UserId? two)
    {
        return ValueObject.NotEqualOperator(one, two);
    }

    [GeneratedRegex(@"^(?!-)[a-zA-Z0-9-]{5,30}(?<!-)$")]
    private static partial Regex IdRegex();
}

