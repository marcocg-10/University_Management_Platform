using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.Exceptions;
using System;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Exceptions
{
    public class BuildingException : DomainException
    {
        public BuildingException(string message) : base(message) { }
        public BuildingException(string message, Exception innerException) : base(message, innerException) { }
    }
}