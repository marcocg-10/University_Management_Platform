using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;


namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

public partial class AvatarId : ValueObject
{
    public string Value { get; }

    public AvatarId(string value)
    {
        Value = value;
    }

    public static bool TryCreate(string value, out AvatarId? AvatarId, out string? error)
    {
        AvatarId = null;
        error = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            error = "AvatarId cannot be empty.";
            return false;
        }

        if (value.Length > 50)
        {
            error = "AvatarId cannot exceed 50 characters.";
            return false;
        }


        AvatarId = new AvatarId(value);

        return true;
    }

    public static AvatarId Create(string input)
    {
        var result = TryCreate(input, out var avatarId, out var error);
        if (!result || avatarId is null)
        {
            throw new ValidationException(error!);
        }
        return avatarId;
    }

    /// <summary>
    /// Gets this value object's attributes necessary for testing equality.
    /// </summary>
    /// <returns>The avatarId string.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

