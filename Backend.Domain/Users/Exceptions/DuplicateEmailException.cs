using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a User that already exists.
/// </summary>
public class DuplicateEmailException : UserException
{
    /// <summary>
    /// Gets the email.
    /// </summary>
    public Email UserEmail{ get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEmailException"/> class.
    /// </summary>
    /// <param name="UserEmail">The official email of the User that already exists.</param>
 
    public DuplicateEmailException(Email userEmail)
    : base($"A User already exists with the email '{userEmail?.Value}'")
    {
        UserEmail = userEmail;
    }
}