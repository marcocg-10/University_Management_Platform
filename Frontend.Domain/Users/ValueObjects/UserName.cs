using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;

public partial class UserName : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserName"/> class with the specified value.
    /// </summary>
    /// <param name="value">The string value representing the user's name. Cannot be null or empty.</param>
    public UserName(string value)
    {
        Value = value;
    }
    public string Value { get; }

    /// <summary>
    /// Attempts to create a <see cref="UserName"/> instance from the specified string value.
    /// </summary>
    /// <remarks>The input <paramref name="value"/> must be at least 4 characters long, consist of letters and
    /// spaces, and must not start with a whitespace character. If these conditions are not met, the method returns <see
    /// langword="false"/> and provides an error message in the <paramref name="error"/> parameter.</remarks>
    /// <param name="value">The string value to validate and use for creating the <see cref="UserName"/> instance.</param>
    /// <param name="name">When this method returns, contains the created <see cref="UserName"/> instance if the operation succeeds;
    /// otherwise, <see langword="null"/>.</param>
    /// <param name="error">When this method returns, contains an error message describing why the operation failed, if the operation does
    /// not succeed; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="UserName"/> instance was successfully created; otherwise, <see
    /// langword="false"/>.</returns>
    public static bool TryCreate(string value, out UserName? name, out string? error)
    {
        name = null;
        error = null;

        if (!NameRegex().IsMatch(value))
        {
            error = $"Name must be over 3 characters (letters, spaces) and must not start with a whitespace";
            return false;
        }
        name = new UserName(value);
        return true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserName"/> class from the specified input string.
    /// </summary>
    /// <param name="input">The input string to validate and convert into a <see cref="UserName"/> instance.</param>
    /// <returns>A valid <see cref="UserName"/> instance created from the input string.</returns>
    /// <exception cref="ValidationException">Thrown if the input string is invalid or cannot be converted into a <see cref="UserName"/> instance.</exception>
    public static UserName Create(string input)
    {
        var result = UserName.TryCreate(input, out var name, out var error);
        if (!result || name is null)
        {
            throw new UserDataException(error!);
        }
        return name;
    }

    /// <summary>
    /// Provides the components that are used to determine equality for this instance.
    /// </summary>
    /// <remarks>This method is typically used in value object implementations to define equality based on the
    /// values of specific properties or fields. Override this method to specify which components uniquely identify the
    /// object.</remarks>
    /// <returns>An <see cref="IEnumerable{T}"/> containing the components that contribute to the equality comparison of this
    /// instance.</returns>
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
    public static bool operator ==(UserName? one, UserName? two)
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

    [GeneratedRegex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ][ A-Za-zÁÉÍÓÚáéíóúÑñ]{2,50}$")]
    private static partial Regex NameRegex();
}
