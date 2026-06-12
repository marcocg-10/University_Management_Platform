using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
public partial class Email : ValueObject
{
    /// <summary>
    /// The internal valid string representing an email address
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Represents a valid email address
    /// </summary>
    /// <param name="value">The string with the email address</param>
    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Attempts to create a valid email value object
    /// </summary>
    /// <param name="value">The string with the email address</param>
    /// <param name="email">The created email value object or null if not valid</param>
    /// <returns>true if the value was valid, otherwise false </returns>
    public static bool TryCreate(string value, out Email? email, out string? error)
    {
        email = null;
        error = null;

        if (!EmailRegex().IsMatch(value))
        {
            error = $"Email {value} has an invalid format.";
            return false;
        }
        email = new Email(value);
        return true;
    }

    /// <summary>
    /// Creates an assumed valid email value object
    /// </summary>
    /// <param name="input">The string with the email address</param>
    /// <returns>A valid email value object</returns>
    /// <exception cref="ValidationException">Thrown when the input is not valid</exception>
    public static Email Create(string input)
    {
        var result = Email.TryCreate(input, out var email, out var error);
        if (!result || email is null)
        {
            throw new ValidationException(error);
        }
        return email;
    }


    /// <summary>
    /// Gets this value object's attributes necessary for testing equality
    /// </summary>
    /// <returns>The email address string</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Tests the equality of two email value objects
    /// </summary>
    /// <param name="one">An optional email value object</param>
    /// <param name="two">Another optional email value object</param>
    /// <returns>True when the emails are the same, otherwise false</returns>
    public static bool operator == (Email? one, Email? two)
    {
        return ValueObject.EqualOperator(one, two);
    }

    public static implicit operator String(Email value)
    {
        return value.Value;
    }


    /// <summary>
    /// Tests the unequality of two email value objects
    /// </summary>
    /// <param name="one">An optional email value object</param>
    /// <param name="two">Another optional email value object</param>
    /// <returns>True when the email are not the same, otherwise false</returns>
    public static bool operator !=(Email? one, Email? two)
    {
        return ValueObject.NotEqualOperator(one, two);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex EmailRegex();
}

