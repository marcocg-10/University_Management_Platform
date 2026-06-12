using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.ValueObjects;

/// <summary>
/// Represents the dimensions of a learning space, including width, length, and height.
/// </summary>
/// <remarks>This class is a value object that encapsulates the dimensions of a learning space.</remarks>
public partial class LearningSpaceDimensions : ValueObject
{

    /// <summary>
    /// Gets the width of the object, measured in meters.
    /// </summary>
    public float Width { get; }

    /// <summary>
    /// Gets the length of the object, measured in meters.
    /// </summary>
    public float Length { get; }

    /// <summary>
    /// Gets the height of the object, measured in meters.
    /// </summary>
    public float Height { get; }

    /// <summary>
    /// Represents the dimensions of a learning space, including width, length, and height.
    /// </summary>
    /// <remarks>This constructor initializes the dimensions of the learning space. Ensure that all parameters
    /// are positive values to represent valid physical dimensions.</remarks>
    /// <param name="width">The width of the learning space, in meters. Must be a positive value.</param>
    /// <param name="length">The length of the learning space, in meters. Must be a positive value.</param>
    /// <param name="height">The height of the learning space, in meters. Must be a positive value.</param>
    private LearningSpaceDimensions(float width, float length, float height)
    {
        Width = width;
        Length = length;
        Height = height;
    }

    /// <summary>
    /// Attempts to create a new instance of <see cref="LearningSpaceDimensions"/> with the specified dimensions.
    /// </summary>
    /// <remarks>This method validates that all dimensions are positive, finite numbers. If any dimension is
    /// less than or equal to zero, or if it is <see cref="float.NaN"/> or <see cref="float.PositiveInfinity"/>/<see
    /// cref="float.NegativeInfinity"/>, the method returns <see langword="false"/> and <paramref name="result"/> is set
    /// to <see langword="null"/>.</remarks>
    /// <param name="width">The width of the learning space. Must be a positive, finite number.</param>
    /// <param name="length">The length of the learning space. Must be a positive, finite number.</param>
    /// <param name="height">The height of the learning space. Must be a positive, finite number.</param>
    /// <param name="result">When this method returns, contains the created <see cref="LearningSpaceDimensions"/> instance if the operation
    /// succeeds; otherwise, <see langword="null"/>. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the dimensions are valid and the <see cref="LearningSpaceDimensions"/> instance is
    /// successfully created; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreate(float width, float length, float height, out LearningSpaceDimensions? result)
    {
        result = null;

        // Validate positive width
        if (width <= 0)
            return false;

        // Validate positive length
        if (length <= 0)
            return false;

        // Validate positive height
        if (height <= 0)
            return false;

        // Validate width is not NaN
        if (float.IsNaN(width))
            return false;

        // Validate length is not NaN
        if (float.IsNaN(length))
            return false;

        // Validate height is not NaN
        if (float.IsNaN(height))
            return false;

        // Validate width is not Infinity
        if (float.IsInfinity(width))
            return false;

        // Validate length is not Infinity
        if (float.IsInfinity(length))
            return false;

        // Validate height is not Infinity
        if (float.IsInfinity(height))
            return false;

        result = new LearningSpaceDimensions(width, length, height);
        return true;
    }

    /// <summary>
    /// Creates a LearningSpaceDimensions instance and validates parameters.
    /// </summary>
    /// <param name="width">Width of the learning space in meters.</param>
    /// <param name="length">Length of the learning space in meters.</param>
    /// <param name="height">Height of the learning space in meters.</param>
    /// <returns>LearningSpaceDimensions instance</returns>
    /// <exception cref="ValidationException">Throws exception if learning space
    /// dimensions are negative, infinite, or a non-float format.</exception>
    public static LearningSpaceDimensions Create(float width, float length, float height)
    {
        // Create LearningSpaceDimensions. Throw exception if dimensions are invalid.
        var result = LearningSpaceDimensions.TryCreate(width, length, height, out var learningSpaceDimensions);
        if (!result || learningSpaceDimensions is null)
        {
            throw new ValidationException(string.Format(
                "Learning Space Dimensions {0}, {1}, {2} are invalid", width, length, height));
        }

        return learningSpaceDimensions;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Width;
        yield return Length;
        yield return Height;
    }
}
