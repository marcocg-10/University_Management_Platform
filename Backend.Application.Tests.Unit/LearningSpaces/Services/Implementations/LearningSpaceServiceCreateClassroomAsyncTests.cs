using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

public class LearningSpaceServiceCreateClassroomAsyncTests
{
    [Fact]
    public async Task CreateClassroomAsync_WithValidInput_AddsViaRepository_AndReturnsClassroom()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);

        var (b, f, room, color, texture, w, l, h, x, y, z) = ValidArgs();

        // Act
        var created = await sut.CreateClassroomAsync(b, f, room, color, texture, w, l, h, x, y, z);

        // Assert
        created.Should().NotBeNull();
        created.Should().BeOfType<Classroom>();
        created.BuildingId.Should().Be(b, because: "Because the BuildingId was correctly set");
        created.FloorLevel.Should().Be(f, because: "Because the FloorLevel was correctly set");
        created.RoomId.Should().Be(room, because: "Because the RoomId was correctly set");
        created.Texture.Value.Should().Be(texture, because: "Because the Texture was correctly set");
        created.Color.Value.Should().Be(color, because: "Because the Color was correctly set");
        created.Dimensions.Width.Should().Be(w, because: "Because the Width was correctly set");
        created.Dimensions.Length.Should().Be(l, because: "Because the Length was correctly set");
        created.Dimensions.Height.Should().Be(h, because: "Because the Height was correctly set");
        created.Coordinates.XCoordinate.Should().Be(x, because: "Because the X Coordinate was correctly set");
        created.Coordinates.YCoordinate.Should().Be(y, because: "Because the Y Coordinate was correctly set");
        created.Coordinates.ZCoordinate.Should().Be(z, because: "Because the Z Coordinate was correctly set");

        repoMock.Verify(
            x => x.AddLearningSpaceAsync(
                    It.Is<Classroom>(c =>
                        c.RoomId == room
                        && c.BuildingId == b
                        && c.FloorLevel == f)),
            Times.Once);
    }

    [Fact]
    public async Task CreateClassroomAsync_WhenRepositoryThrowsDuplicate_PropagatesDuplicateValueInEntityException()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var (b, f, room, color, texture, w, l, h, x, y, z) = ValidArgs();

        var duplicate = new DuplicateValueInEntityException(
            entityName: "LearningSpace",
            propertyName: "UNIQUE_Room_Building",
            duplicateValue: $"{room}, {b}");

        repoMock
            .Setup(x => x.AddLearningSpaceAsync(It.IsAny<Classroom>()))
            .ThrowsAsync(duplicate);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.CreateClassroomAsync(b, f, room, color, texture, w, l, h, x, y, z))
            .Should()
            .ThrowExactlyAsync<DuplicateValueInEntityException>(because: "the roomId+buildingId is unique")
            .WithMessage("*UNIQUE_Room_Building*", because: "the exception message should include the constraint");
    }

    [Fact]
    public async Task CreateClassroomAsync_WhenRepositoryThrowsForeignKey_PropagatesForeignKeyException()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var (b, f, room, color, texture, w, l, h, x, y, z) = ValidArgs();

        var fk = new ForeignKeyException("FK_LearningSpace_Building", "Building");
        repoMock
            .Setup(x => x.AddLearningSpaceAsync(It.IsAny<Classroom>()))
            .ThrowsAsync(fk);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.CreateClassroomAsync(b, f, room, color, texture, w, l, h, x, y, z))
            .Should()
            .ThrowExactlyAsync<ForeignKeyException>(because: "the building must exist")
            .WithMessage("*FK_LearningSpace_Building*", because: "the exception message should include the constraint");
    }

    [Fact]
    public async Task CreateClassroomAsync_WhenRoomIdIsEmpty_ThrowsValidationException_AndRepositoryIsNotCalled()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var (b, f, _, color, texture, w, l, h, x, y, z) = ValidArgs();

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.CreateClassroomAsync(b, f, " ", color, texture, w, l, h, x, y, z))
            .Should()
            .ThrowExactlyAsync<ValidationException>(because: "the Room ID is invalid")
            .WithMessage("*Room ID is required*", because: "the exception message should include the error");

        repoMock.Verify(x => x.AddLearningSpaceAsync(It.IsAny<Classroom>()), Times.Never);
    }

    [Theory]
    [InlineData("not-hex")]
    [InlineData("#GGGGGG")]
    [InlineData("")]
    [InlineData(null)]
    public async Task CreateClassroomAsync_WhenColorIsInvalid_ThrowsValidationException(string? badColor)
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var (b, f, room, _, texture, w, l, h, x, y, z) = ValidArgs();

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.CreateClassroomAsync(b, f, room, badColor!, texture, w, l, h, x, y, z))
            .Should()
            .ThrowExactlyAsync<ValidationException>(because: "the color ID is invalid")
            .WithMessage("*Invalid color format*", because: "the exception message should include the error");

        repoMock.Verify(x => x.AddLearningSpaceAsync(It.IsAny<Classroom>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task CreateClassroomAsync_WhenTextureIsInvalid_ThrowsValidationException(string? badTexture)
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var (b, f, room, color, _, w, l, h, x, y, z) = ValidArgs();

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.CreateClassroomAsync(b, f, room, color, badTexture!, w, l, h, x, y, z))
            .Should()
            .ThrowExactlyAsync<ValidationException>(because: "the texture is invalid")
            .WithMessage("*Invalid texture format*", because: "the exception message should include the error");

        repoMock.Verify(x => x.AddLearningSpaceAsync(It.IsAny<Classroom>()), Times.Never);
    }

    [Theory]
    [InlineData(0f, 1f, 1f)]
    [InlineData(1f, -1f, 1f)]
    [InlineData(1f, 1f, 0f)]
    [InlineData(float.PositiveInfinity, 2f, 3f)]
    [InlineData(1f, float.PositiveInfinity, 3f)]
    [InlineData(1, 2f, float.PositiveInfinity)]
    [InlineData(float.NaN, 2f, 3f)]
    [InlineData(1f, float.NaN, 3f)]
    [InlineData(1, 2f, float.NaN)]
    public async Task CreateClassroomAsync_WhenDimensionsAreInvalid_ThrowsValidationException(float w, float l, float h)
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var (b, f, room, color, texture, _, _, _, x, y, z) = ValidArgs();

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.CreateClassroomAsync(b, f, room, color, texture, w, l, h, x, y, z))
            .Should()
            .ThrowExactlyAsync<ValidationException>(because: "the dimensions are invalid")
            .WithMessage("*Invalid dimensions provided*", because: "the exception message should include the error");

        repoMock.Verify(x => x.AddLearningSpaceAsync(It.IsAny<Classroom>()), Times.Never);
    }

    [Theory]
    [InlineData(float.NaN, 1f, 1f)]
    [InlineData(1f, float.PositiveInfinity, 1f)]
    [InlineData(1f, 1f, float.NegativeInfinity)]
    public async Task CreateClassroomAsync_WhenCoordinatesAreInvalid_ThrowsValidationException(float x, float y, float z)
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var (b, f, room, color, texture, w, l, h, _, _, _) = ValidArgs();

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.CreateClassroomAsync(b, f, room, color, texture, w, l, h, x, y, z))
            .Should()
            .ThrowExactlyAsync<ValidationException>(because: "the coordinates are invalid")
            .WithMessage("*Invalid coordinates provided*", because: "the exception message should include the error");

        repoMock.Verify(x => x.AddLearningSpaceAsync(It.IsAny<Classroom>()), Times.Never);
    }

    [Fact]
    public async Task CreateClassroomAsync_AllowsNullBuildingAndFloor()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var (_, _, room, color, texture, w, l, h, x, y, z) = ValidArgs();

        // Act
        var created = await sut.CreateClassroomAsync(null, null, room, color, texture, w, l, h, x, y, z);

        // Assert
        created.BuildingId.Should().BeNull();
        created.FloorLevel.Should().BeNull();

        repoMock.Verify(
            x => x.AddLearningSpaceAsync(
                    It.Is<Classroom>(c =>
                        c.RoomId == room)),
            Times.Once);
    }

    private static (int? buildingId, int? floor, string room, string color, string texture,
        float w, float l, float h, float x, float y, float z) ValidArgs() =>
        (1, 2, "A-101", "#33FF57", "Outdoor_Wall_T15_Ambient_occlusion.png", 5f, 6f, 3f, 10f, 20f, 30f);
}
