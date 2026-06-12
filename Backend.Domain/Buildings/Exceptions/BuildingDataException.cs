namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;

/// <summary>
/// Exception thrown when a database operation fails for a building.
/// </summary>
public class BuildingDataException : BuildingException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingDataException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public BuildingDataException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingDataException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public BuildingDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}