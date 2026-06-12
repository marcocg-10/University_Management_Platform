using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;

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
            error = $"Email should have at least one '@' and a domain.";
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
        var result = Email.TryCreate(input, out var email, out string? error);
        if (!result || email is null)
        {
            throw new UserDataException(error!);
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
    public static bool operator ==(Email? one, Email? two)
    {
        return ValueObject.EqualOperator(one, two);
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
