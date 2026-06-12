using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;

public class UserException : DomainException
{
    public UserException(string message) : base(message) { }
    public UserException(string message, Exception innerException) : base(message, innerException) { }
}
