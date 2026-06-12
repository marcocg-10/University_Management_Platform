using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a User that already exists.
/// </summary>
public class DuplicateIdException : UserException
{
    /// <summary>
    /// Gets the id
    /// </summary>
    public UserId UserId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateIdException"/> class.
    /// </summary>
    /// <param name="UserIdl">The official id of the User that already exists.</param>
    public DuplicateIdException(UserId id)
    : base($"User with id '{id?.Value}' already exists.")
    {
        UserId = id;
    }
}