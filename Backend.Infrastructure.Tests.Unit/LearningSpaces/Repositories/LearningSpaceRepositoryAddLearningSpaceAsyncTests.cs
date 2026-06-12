using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.LearningSpaces.Repositories;

/// <summary>
/// Contains unit tests for the AddLearningSpaceAsync method.
/// </summary>
public class LearningSpaceRepositoryAddLearningSpaceAsyncTests 
    : IClassFixture<LearningSpaceRepositoryTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceRepositoryTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceRepositoryTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceRepositoryAddLearningSpaceAsyncTests(
        LearningSpaceRepositoryTestData testData)
    {
        _testData = testData;
    }

    [Fact]
    public async Task AddLearningSpaceAsync_WithValidLaboratory_AddsAndSavesChanges()
    {
        // Arrange
        var dbSetMock = new Mock<DbSet<LearningSpace>>();

        // Setup AddAsync to return a default ValueTask (returned EntityEntry is not used by repository).
        dbSetMock
            .Setup(d => d.AddAsync(It.IsAny<LearningSpace>(), It.IsAny<CancellationToken>()))
            .Returns((LearningSpace _, CancellationToken __) =>
                ValueTask.FromResult((EntityEntry<LearningSpace>)null!));

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .SetupGet(c => c.LearningSpaces)
            .Returns(dbSetMock.Object);

        // Simulate successful SaveChangesAsync.
        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        await sut.AddLearningSpaceAsync(_testData.LaboratorySingleEntryData[0]);

        // Assert
        dbSetMock.Verify(
            d => d.AddAsync(
                    It.Is<LearningSpace>(ls => ReferenceEquals(ls, _testData.LaboratorySingleEntryData[0]) && ls is Laboratory),
                    It.IsAny<CancellationToken>()),
            Times.Once,
            "repository should add the learning space to the DbSet");

        dbContextMock.Verify(
            c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "repository should call SaveChangesAsync after adding the learning space");
    }

    [Fact]
    public async Task AddLearningSpaceAsync_WithValidClassroom_AddsAndSavesChanges()
    {
        // Arrange
        var dbSetMock = new Mock<DbSet<LearningSpace>>();

        // Setup AddAsync to return a default ValueTask (returned EntityEntry is not used by repository).
        dbSetMock
            .Setup(d => d.AddAsync(It.IsAny<LearningSpace>(), It.IsAny<CancellationToken>()))
            .Returns((LearningSpace _, CancellationToken __) =>
                ValueTask.FromResult((EntityEntry<LearningSpace>)null!));

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .SetupGet(c => c.LearningSpaces)
            .Returns(dbSetMock.Object);

        // Simulate successful SaveChangesAsync.
        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        await sut.AddLearningSpaceAsync(_testData.ClassroomSingleEntryData[0]);

        // Assert
        dbSetMock.Verify(
            d => d.AddAsync(
                    It.Is<LearningSpace>(ls => ReferenceEquals(ls, _testData.ClassroomSingleEntryData[0]) && ls is Classroom),
                    It.IsAny<CancellationToken>()),
            Times.Once,
            "repository should add the learning space to the DbSet");

        dbContextMock.Verify(
            c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "repository should call SaveChangesAsync after adding the learning space");
    }

    [Fact]
    public async Task AddLearningSpaceAsync_WhenSaveThrowsDuplicate_PropagatesDuplicateValueInEntityException()
    {
        // Arrange
        var dbSetMock = new Mock<DbSet<LearningSpace>>();

        // Setup AddAsync to return a default ValueTask (returned EntityEntry is not used by repository).
        dbSetMock
            .Setup(d => d.AddAsync(It.IsAny<LearningSpace>(), It.IsAny<CancellationToken>()))
            .Returns((LearningSpace _, CancellationToken __) =>
                ValueTask.FromResult((EntityEntry<LearningSpace>)null!));

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .SetupGet(c => c.LearningSpaces)
            .Returns(dbSetMock.Object);

        // Throw DuplicateValueInEntityException when SaveChangesAsync is called.
        var expected = new DuplicateValueInEntityException(
            entityName: "LearningSpace",
            propertyName: "UNIQUE_Room_Building",
            duplicateValue: "6-6, 1");

        // Simulate exception.
        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expected);

        var sut = new LearningSpaceRepository(dbContextMock.Object);
        var classroom = _testData.ClassroomSingleEntryData[0];

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.AddLearningSpaceAsync(classroom))
            .Should()
            .ThrowExactlyAsync<DuplicateValueInEntityException>(because: "the learning space already exists")
            .WithMessage("*UNIQUE_Room_Building*", because: "the exception message should include the name of the constraint violated");
    }

    [Fact]
    public async Task AddLearningSpaceAsync_WhenSaveThrowsForeignKey_PropagatesForeignKeyException()
    {
        // Arrange
        var dbSetMock = new Mock<DbSet<LearningSpace>>();
        dbSetMock
            .Setup(d => d.AddAsync(It.IsAny<LearningSpace>(), It.IsAny<CancellationToken>()))
            .Returns((LearningSpace _, CancellationToken __) =>
                ValueTask.FromResult((EntityEntry<LearningSpace>)null!));

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .SetupGet(c => c.LearningSpaces)
            .Returns(dbSetMock.Object);

        // Throw ForeignKeyException when SaveChangesAsync is called.
        var expected = new ForeignKeyException(
            constraintName: "FK_LearningSpace_Building",
            tableName: "Building");

        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expected);

        var sut = new LearningSpaceRepository(dbContextMock.Object);
        var lab = _testData.LaboratorySingleEntryData[0];

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.AddLearningSpaceAsync(lab))
            .Should()
            .ThrowExactlyAsync<ForeignKeyException>(because: "the building reference does not exist in the database")
            .WithMessage("*FK_LearningSpace_Building*", because: "the exception message should include the name of the constraint violated");
    }

    [Fact]
    public async Task AddLearningSpaceAsync_WhenSaveThrowsForeignKey_ForClassroom_PropagatesForeignKeyException()
    {
        // Arrange
        var dbSetMock = new Mock<DbSet<LearningSpace>>();
        dbSetMock
            .Setup(d => d.AddAsync(It.IsAny<LearningSpace>(), It.IsAny<CancellationToken>()))
            .Returns((LearningSpace _, CancellationToken __) =>
                ValueTask.FromResult((EntityEntry<LearningSpace>)null!));

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .SetupGet(c => c.LearningSpaces)
            .Returns(dbSetMock.Object);

        var expected = new ForeignKeyException("FK_LearningSpace_Building", "Building");
        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expected);

        var sut = new LearningSpaceRepository(dbContextMock.Object);
        var classroom = _testData.ClassroomSingleEntryData[0];

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.AddLearningSpaceAsync(classroom))
            .Should()
            .ThrowExactlyAsync<ForeignKeyException>()
            .WithMessage("*FK_LearningSpace_Building*");
    }
}
