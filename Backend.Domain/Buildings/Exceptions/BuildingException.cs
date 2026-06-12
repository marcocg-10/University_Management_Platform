using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;

public class BuildingException : DomainException
{   
    public BuildingException(string message) : base(message) { }
    public BuildingException(string message, Exception innerException) : base(message, innerException) { }
}
