using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.ValueObjects;

/// <summary>
/// Represents the 3-D coordinates (X, Y, Z) of a learning space on a building floor.
/// The origin (0,0) is at the bottom-left corner of the floor plan,
/// using the X and Z axes (as in Unity's coordinate system).
/// </summary>
public partial class LearningSpaceCoordinates : ValueObject
{
    /// <summary>
    /// X-axis position in meters.
    /// </summary>
    public float XCoordinate { get; }

    /// <summary>
    /// Y-axis position in meters.
    /// </summary>
    public float YCoordinate { get; }

    /// <summary>
    /// Z-axis position in meters.
    /// </summary>
    public float ZCoordinate { get; }

    private LearningSpaceCoordinates(float xCoordinate, float yCoordinate, float zCoordinate)
    {
        XCoordinate = xCoordinate;
        YCoordinate = yCoordinate;
        ZCoordinate = zCoordinate;
    }

    /// <summary>
    /// Safely creates the value object validating its invariants.
    /// Returns true if the coordinates are valid, otherwise false.
    /// </summary>
    public static bool TryCreate(float xCoordinate, float yCoordinate, float zCoordinate, out LearningSpaceCoordinates? result)
    {
        result = null;

        // Validate X is not null.
        if (float.IsNaN(xCoordinate))
            return false;

        // Validate Y is not null.
        if (float.IsNaN(yCoordinate))
            return false;

        // Validate Z is not null.
        if (float.IsNaN(zCoordinate))
            return false;

        // Validate X is not infinite.
        if (float.IsInfinity(xCoordinate))
            return false;

        // Validate Y is not infinite.
        if (float.IsInfinity(yCoordinate))
            return false;

        // Validate Z is not infinite.
        if (float.IsInfinity(zCoordinate))
            return false;

        result = new LearningSpaceCoordinates(xCoordinate, yCoordinate, zCoordinate);
        return true;
    }

    /// <summary>
    /// Creates a LearningSpaceCoordinates instance and validates parameters.
    /// </summary>
    /// <param name="xCoordinate">X axis coordinate.</param>
    /// <param name="yCoordinate">Y axis coordinate.</param>
    /// <param name="zCoordinate">Z axis coordinate.</param>
    /// <returns>LearningSpaceCoordinates instance</returns>
    /// <exception cref="ValidationException">Throws exception if learning space
    /// coordinates are negative.</exception>
    public static LearningSpaceCoordinates Create(float xCoordinate, float yCoordinate, float zCoordinate)
    {
        // Create LearningSpaceCoordinates. Throw exception if coordinates are invalid.
        var result = LearningSpaceCoordinates.TryCreate(xCoordinate, yCoordinate, zCoordinate, out var learningSpaceCoordinates);
        if (!result || learningSpaceCoordinates is null)
        {
            throw new ValidationException(string.Format(
                "Learning Space Coordinates {0}, {1}, {2} are invalid", xCoordinate, yCoordinate, zCoordinate));
        }

        return learningSpaceCoordinates;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return XCoordinate;
        yield return YCoordinate;
        yield return ZCoordinate;
    }
}
