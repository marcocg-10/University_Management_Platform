namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

/// <summary>
/// Represents a non interactive component, located inside of a learning space.
/// </summary>
/// <remarks>
/// All non interactive components must inherit from this class.
/// </remarks>
internal class NonInteractiveComponent
{
    /// <summary>
    /// Represents the identifier of the NonInteractiveComponent with the learning space.
    /// </summary>
    public string NonInteractiveComponentID { get; }

    /// <summary>
    /// Represents the X-axis coordinate of the NonInteractiveComponent's location within the
    /// learning space.
    /// </summary>
    /// <remarks>
    /// The value of the X-axis coordinate must be nonnegative.
    /// </remarks>
    public float XAxis { get; }

    /// <summary>
    /// Represents the Y-axis coordinate of the NonInteractiveComponent's location within the
    /// learning space.
    /// </summary>
    /// <remarks>
    /// The value of the Y-axis coordinate must be nonnegative.
    /// </remarks>
    public float YAxis { get; }

    /// <summary>
    /// Represents the Z-axis coordinate of the NonInteractiveComponent's location within the
    /// learning space.
    /// </summary>
    /// <remarks>
    /// The value of the Z-axis coordinate must be nonnegative.
    /// </remarks>
    public float ZAxis { get; }

    /// <summary>
    /// Represents the orientation of the NonInteractiveComponent's location within the learning
    /// space.
    /// </summary>
    /// <remarks>
    /// The value of the orientation must fall in the range [0, 359].
    /// </remarks>
    public float Orientation { get; }

    /// <summary>
    /// Creates an instance of a NonInteractiveComponent with basic properties.
    /// </summary>
    /// <param name="nonInteractiveComponentID">Identifier of the NonInteractiveComponent.</param>
    /// <param name="xAxis">X-axis coordinate of the NonInteractiveComponent's location.</param>
    /// <param name="yAxis">Y-axis coordinate of the NonInteractiveComponent's location.</param>
    /// <param name="zAxis">Z-axis coordinate of the NonInteractiveComponent's location.</param>
    /// <param name="orientation">Orientation of the NonInteractiveComponent's location.</param>
    public NonInteractiveComponent(
        string nonInteractiveComponentID,
        float xAxis,
        float yAxis,
        float zAxis,
        float orientation)
    {
        NonInteractiveComponentID = nonInteractiveComponentID;
        XAxis = xAxis;
        YAxis = yAxis;
        ZAxis = zAxis;
        Orientation = orientation;
    }
}