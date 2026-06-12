using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;

/// <summary>
/// Represents the number of floors in a building.
/// </summary>
/// <remarks>
/// Value must be between 1 and 10, inclusive.
/// </remarks>
public partial class FloorCount : ValueObject
{
    /// <summary>
    /// The number of floors in the building.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FloorCount"/> class.
    /// </summary>
    /// <param name="value"></param>
    private FloorCount(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Tries to create a new instance of the <see cref="FloorCount"/> class.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="floorCount"></param>
    /// <returns></returns>
    public static bool TryCreate(int value, out FloorCount? floorCount)
    {
        floorCount = null;
        if (value < 1 || value > 10)
        {
            return false;
        }
        floorCount = new FloorCount(value);
        return true;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="FloorCount"/> class.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="BuildingDataException"></exception>
    public static FloorCount Create(int input)
    {
        var result = FloorCount.TryCreate(input, out var floorCount);
        if (!result || floorCount is null)
        {
            throw new BuildingDataException(string.Format("Floor count {0} is invalid, must be less than or equal to 10", input));
        }
        return floorCount;
    }

    /// <summary>
    /// When implemented in a derived class, returns the components of the value object that
    /// are used to determine equality.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
