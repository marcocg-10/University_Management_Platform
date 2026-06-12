using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;

public class UserException : DomainException
{
    public UserException(string message) : base(message) { }
    public UserException(string message, Exception innerException) : base(message, innerException) { }
}
