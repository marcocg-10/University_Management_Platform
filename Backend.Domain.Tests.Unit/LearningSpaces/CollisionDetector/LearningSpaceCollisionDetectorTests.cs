using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.CollisionDetector.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.LearningSpaces.CollisionDetector;

/// <summary>
/// Tests for the <see cref="ILearningSpaceCollisionDetector"/> implementation.
/// </summary>
public class LearningSpaceCollisionDetectorTests
{
    private readonly ILearningSpaceCollisionDetector _detector;

    private readonly LearningSpaceColor _color;
    private readonly LearningSpaceTexture _texture;
    private readonly LearningSpaceDimensions _defaultDimensions;
    private readonly LearningSpaceCoordinates _defaultCoordinates;

    public LearningSpaceCollisionDetectorTests()
    {
        _detector = new LearningSpaceCollisionDetector();

        var colorOk = LearningSpaceColor.TryCreate("#FFFFFF", out var color);
        var textureOk = LearningSpaceTexture.TryCreate("WallTex.png", out var texture);
        var dimsOk = LearningSpaceDimensions.TryCreate(2f, 2f, 2f, out var dims);
        var coordsOk = LearningSpaceCoordinates.TryCreate(0f, 0f, 0f, out var coords);

        if (!colorOk || color is null) throw new Exception("Failed to construct color VO for tests.");
        if (!textureOk || texture is null) throw new Exception("Failed to construct texture VO for tests.");
        if (!dimsOk || dims is null) throw new Exception("Failed to construct dimensions VO for tests.");
        if (!coordsOk || coords is null) throw new Exception("Failed to construct coordinates VO for tests.");

        _color = color;
        _texture = texture;
        _defaultDimensions = dims;
        _defaultCoordinates = coords;
    }

    [Fact]
    public void NonOverlapping_LearningSpaces_ShouldNotCollide()
    {
        // Arrange
        var ls1 = new LearningSpace(
            1,
            1,
            1,
            "R-101",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(0f, 0f, 0f)
        );

        var ls2 = new LearningSpace(
            2,
            1,
            1,
            "R-102",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(5f, 5f, 5f)
        );

        // Act
        bool hasCollision = _detector.HasCollision(ls1, ls2);

        // Assert
        hasCollision.Should().BeFalse(because: "the spaces are far apart and their AABBs do not overlap");
    }

    [Fact]
    public void LearningSpace_ShouldNotCollide_WithItSelf()
    {
        // Arrange
        var ls = new LearningSpace(
            3,
            1,
            1,
            "R-201",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(0f, 0f, 0f)
        );

        // Act
        bool hasCollision = _detector.HasCollision(ls, ls);

        // Assert
        hasCollision.Should().BeFalse(because: "an entity must not collide with itself");
    }

    [Fact]
    public void Overlapping_LearningSpaces_ShouldCollide()
    {
        // Arrange
        var ls1 = new LearningSpace(
            4,
            1,
            1,
            "R-301",
            _color,
            _texture,
            LearningSpaceDimensions.Create(2f, 2f, 2f),
            LearningSpaceCoordinates.Create(1f, 1f, 1f)
        );

        var ls2 = new LearningSpace(
            5,
            1,
            1,
            "R-302",
            _color,
            _texture,
            LearningSpaceDimensions.Create(2f, 2f, 2f),
            LearningSpaceCoordinates.Create(1.5f, 1f, 1f)
        );

        // Act
        bool hasCollision = _detector.HasCollision(ls1, ls2);

        // Assert
        hasCollision.Should().BeTrue(because: "their AABBs overlap on at least one axis combination");
    }

    [Fact]
    public void IdenticalCoords_LearningSpaces_ShouldCollide()
    {
        // Arrange
        var ls1 = new LearningSpace(
            6,
            1,
            1,
            "R-401",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(2f, 2f, 2f)
        );

        var ls2 = new LearningSpace(
            7,
            1,
            1,
            "R-402",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(2f, 2f, 2f)
        );

        // Act
        bool hasCollision = _detector.HasCollision(ls1, ls2);

        // Assert
        hasCollision.Should().BeTrue(because: "same centers and equal extents should produce overlap");
    }

    [Fact]
    public void NonOverlapping_LearningSpaces_InCollection_ShouldNotCollide()
    {
        // Arrange
        var candidate = new LearningSpace(
            8,
            1,
            1,
            "R-501",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(0f, 0f, 0f)
        );

        var other1 = new LearningSpace(
            9,
            1,
            1,
            "R-502",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(5f, 5f, 5f)
        );

        var other2 = new LearningSpace(
            10,
            1,
            1,
            "R-503",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(6f, 6f, 6f)
        );

        var list = new List<LearningSpace> { other1, other2 };

        // Act
        bool result = _detector.DetectCollision(candidate, list);

        // Assert
        result.Should().BeFalse(because: "no element in the collection overlaps the candidate");
    }

    [Fact]
    public void Overlapping_LearningSpaces_InCollection_ShouldCollide()
    {
        // Arrange
        var candidate = new LearningSpace(
            11,
            1,
            1,
            "R-601",
            _color,
            _texture,
            LearningSpaceDimensions.Create(2f, 2f, 2f),
            LearningSpaceCoordinates.Create(1f, 1f, 1f)
        );

        var overlapping = new LearningSpace(
            12,
            1,
            1,
            "R-602",
            _color,
            _texture,
            LearningSpaceDimensions.Create(2f, 2f, 2f),
            LearningSpaceCoordinates.Create(1.5f, 1f, 1f)
        );

        var nonOverlapping = new LearningSpace(
            13,
            1,
            1,
            "R-603",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(10f, 10f, 10f)
        );

        var list = new List<LearningSpace> { overlapping, nonOverlapping };

        // Act
        bool result = _detector.DetectCollision(candidate, list);

        // Assert
        result.Should().BeTrue(because: "at least one element in the collection overlaps the candidate");
    }

    [Fact]
    public void CandidateIsNull_ShouldThrowException()
    {
        // Arrange
        var other = new LearningSpace(
            14,
            1,
            1,
            "R-701",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(1f, 1f, 1f)
        );

        var list = new List<LearningSpace> { other };

        // Act
        Action act = () => _detector.DetectCollision(null!, list);

        // Assert
        act.Should().Throw<LearningSpaceNotFoundException>();
    }

    [Fact]
    public void CollectionIsNull_ShouldThrowException()
    {
        // Arrange
        var candidate = new LearningSpace(
            15,
            1,
            1,
            "R-801",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(1f, 1f, 1f)
        );

        // Act
        Action act = () => _detector.DetectCollision(candidate, null!);

        // Assert
        act.Should().Throw<LearningSpaceNotFoundException>();
    }

    [Fact]
    public void EmptyCollection_ShouldNotCollide()
    {
        // Arrange
        var candidate = new LearningSpace(
            16,
            1,
            1,
            "R-901",
            _color,
            _texture,
            LearningSpaceDimensions.Create(1f, 1f, 1f),
            LearningSpaceCoordinates.Create(1f, 1f, 1f)
        );

        var empty = new List<LearningSpace>();

        // Act
        bool result = _detector.DetectCollision(candidate, empty);

        // Assert
        result.Should().BeFalse(because: "there are no learning spaces to collide with");
    }
}
