
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.CollisionDetector.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.InteractiveComponents.CollisionDetector;

/// <summary>
/// Tests for the <see cref="IInteractiveComponentCollisionDetector"/> interface.
/// </summary>
public class InteractiveComponentCollisionDetectorTests
{
    private readonly IInteractiveComponentCollisionDetector _detector;

    public InteractiveComponentCollisionDetectorTests()
    {
        _detector = new InteractiveComponentCollisionDetector();
    }

    /// <summary>
    /// Tests that two non-overlapping interactive components do not collide.
    /// </summary>
    [Fact]
    public void NonOverlapping_InteractiveComponents_ShouldNotCollide()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(2, 2, 2),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        // Act
        bool hasCollision = _detector.HasCollision(board1, projector1);
        // Assert
        hasCollision.Should().BeFalse(because:"Components do not collide");
    }

    /// <summary>
    /// Tests that an interactive component does not collide with itself.
    /// </summary>
    [Fact]
    public void InteractiveComponent_ShouldNotCollide_WithItSelf()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        // Act
        bool hasCollision = _detector.HasCollision(board1, board1);
        // Assert
        hasCollision.Should().BeFalse(because: "Is the same component, it does not collide with itself");
    }

    /// <summary>
    /// Tests that two overlapping interactive components collide.
    /// </summary>
    [Fact]
    public void Overlapping_InteractiveComponents_ShouldCollide()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(1, 1, 1),
            new Dimensions(2, 1, 0.5),
            new Rotations(0, 0, 0),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(2, 1, 1),
            new Dimensions(1, 1, 0.5),
            new Rotations(0, 0, 0),
            1);
        // Act
        bool hasCollision = _detector.HasCollision(board1, projector1);
        // Assert
        hasCollision.Should().BeTrue(because: "Components with overlaping parts should collide");
    }

    /// <summary>
    /// Tests that two interactive components with identical coordinates collide.
    /// </summary>
    [Fact]
    public void IdenticalCoords_InteractiveComponents_ShouldCollide()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(2, 2, 2),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(2, 2, 2),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        // Act
        bool hasCollision = _detector.HasCollision(board1, projector1);
        // Assert
        hasCollision.Should().BeTrue(because: "Components with identical coordinates should collide");
    }

    /// <summary>
    /// Tests that non-overlapping interactive components in a room do not collide.
    /// </summary>
    [Fact]
    public void NonOverlapping_InteractiveComponents_InARoom_ShouldNotCollide()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(5, 5, 5),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        var board2 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(4, 4, 4),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);

        var components = new List<InteractiveComponent> { projector1, board2 };

        // Act
        bool result = _detector.DetectCollision(board1, components);

        // Assert
        result.Should().BeFalse("The components are far apart and should not collide");
    }

    /// <summary>
    /// Tests that overlapping interactive components in a room collide.
    /// </summary>
    [Fact]
    public void Overlapping_InteractiveComponents_InARoom_ShouldCollide()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(1, 1, 1),
            new Dimensions(2, 2, 2),
            new Rotations(0, 0, 0),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(2, 1, 1),
            new Dimensions(2, 2, 2),
            new Rotations(0, 0, 0),
            1);
        var board2 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(4, 4, 4),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);

        var components = new List<InteractiveComponent> { projector1, board2 };

        // Act
        bool result = _detector.DetectCollision(board1, components);

        // Assert
        result.Should().BeTrue("The components that overlap should collide");
    }

    /// <summary>
    /// Tests that passing a null component throws an exception.
    /// </summary>
    [Fact]
    public void ComponentIsNull_Should_ThrowException()
    {
        // Arrange
         var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(2, 1, 1),
            new Dimensions(2, 2, 2),
            new Rotations(0, 0, 0),
            1);
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(4, 4, 4),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);

        var components = new List<InteractiveComponent> { projector1, board1 };

        // Act
        Action act = () => _detector.DetectCollision(null, components);

        // Assert
        act.Should().Throw<InteractiveComponentNotFoundException>()
            .WithMessage("Interactive component could not be found.");
    }

    /// <summary>
    /// Tests that passing a null component list throws an exception.
    /// </summary>
    [Fact]
    public void ComponentListIsNull_Should_ThrowException()
    {
        // Arrange
        var projector1 = new Projector(
           new Color("#FFF"),
           "Smooth",
           100,
           new PlateId("222222"),
           new Resolution(1920, 1080),
           new Coordinates(2, 1, 1),
           new Dimensions(2, 2, 2),
           new Rotations(0, 0, 0),
           1);

        // Act
        Action act = () => _detector.DetectCollision(projector1, null);

        // Assert
        act.Should().Throw<InteractiveComponentNotFoundException>()
            .WithMessage("Interactive component could not be found.");
    }

    /// <summary>
    /// Tests that an empty component list does not result in a collision.
    /// </summary>
    [Fact]
    public void ComponentListIsEmpty_InteractiveComponent_Should_NotCollide()
    {
        // Arrange
        var projector1 = new Projector(
           new Color("#FFF"),
           "Smooth",
           100,
           new PlateId("222222"),
           new Resolution(1920, 1080),
           new Coordinates(2, 1, 1),
           new Dimensions(2, 2, 2),
           new Rotations(0, 0, 0),
           1);

        var components = new List<InteractiveComponent> { };

        // Act
        bool result = _detector.DetectCollision(projector1, components);

        // Assert
        result.Should().BeFalse("No components to collide with");
    }


    /// <summary>
    /// Tests that overlapping interactive components in different rooms do not collide.
    /// </summary>
    [Fact]
    public void Overlapping_InteractiveComponents_NotInSameRoom_ShouldNotCollide()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(1, 1, 1),
            new Dimensions(2, 2, 2),
            new Rotations(0, 0, 0),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(4, 4, 4),
            new Dimensions(2, 2, 2),
            new Rotations(0, 0, 0),
            1);
        var board2 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("333333"),
            new Coordinates(1, 1, 1),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            2);

        var components = new List<InteractiveComponent> { projector1 };

        // Act
        bool result = _detector.DetectCollision(board1, components);

        // Assert
        result.Should().BeFalse("The components that 'overlap' are not in the same room they should not collide");
    }

    /// <summary>
    /// Tests that two rotated OBBs that don't overlap should not collide.
    /// </summary>
    [Fact]
    public void RotatedOBBs_NotOverlapping_ShouldNotCollide()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(1.536, 1.329, 0.78),
            new Dimensions(1, 2, 2),
            new Rotations(22.5, 30, 45),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(1.97, -3.43, 4.86),
            new Dimensions(2, 1, 3),
            new Rotations(30, 45, 22.5),
            1);
        // Act
        bool hasCollision = _detector.HasCollision(board1, projector1);
        // Assert
        hasCollision.Should().BeFalse(because: "Components do not collide");
    }

    /// <summary>
    /// Tests that two rotated OBBs that overlap should collide.
    /// </summary>
    [Fact]
    public void RotatedOBBs_Overlapping_ShouldCollide()
    {
        // Arrange - Two boxes rotated 45 degrees around Y-axis, positioned to overlap
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(0, 0, 0),
            new Dimensions(2, 2, 2),
            new Rotations(0, 45, 0),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(1, 0, 0),
            new Dimensions(2, 2, 2),
            new Rotations(0, 45, 0),
            1);

        // Act
        bool hasCollision = _detector.HasCollision(board1, projector1);

        // Assert
        hasCollision.Should().BeTrue("Rotated boxes should collide when overlapping");
    }

    /// <summary>
    /// Tests that one rotated and one non-rotated box can correctly detect collision.
    /// </summary>
    [Fact]
    public void RotatedAndNonRotated_ShouldCorrectlyDetectCollision()
    {
        // Arrange - One rotated, one axis-aligned
        var rotatedBox = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(0, 0, 0),
            new Dimensions(2, 2, 2),
            new Rotations(0, 45, 0),
            1);
        var axisAlignedBox = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(1.5, 0, 0),
            new Dimensions(2, 2, 2),
            new Rotations(0, 0, 0),
            1);

        // Act
        bool hasCollision = _detector.HasCollision(rotatedBox, axisAlignedBox);

        // Assert
        hasCollision.Should().BeTrue("Mixed rotation states should correctly detect collision");
    }

    /// <summary>
    /// Tests complex rotation around multiple axes.
    /// </summary>
    [Fact]
    public void MultiAxisRotatedOBBs_ShouldCorrectlyDetectCollision()
    {
        // Arrange - Boxes rotated around multiple axes
        var box1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(0, 0, 0),
            new Dimensions(2, 1, 3),
            new Rotations(30, 45, 22.5),
            1);
        var box2 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("222222"),
            new Resolution(1920, 1080),
            new Coordinates(1, 0.5, 1),
            new Dimensions(1, 2, 2),
            new Rotations(22.5, 30, 45),
            1);

        // Act
        bool hasCollision = _detector.HasCollision(box1, box2);

        // Assert
        hasCollision.Should().BeTrue("Complex multi-axis rotations should correctly detect collision");
    }
}