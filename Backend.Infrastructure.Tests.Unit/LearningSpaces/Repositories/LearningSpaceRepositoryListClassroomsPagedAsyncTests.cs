
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.LearningSpaces.Repositories;

public class LearningSpaceRepositoryListClassroomsPagedAsyncTests : IClassFixture<LearningSpaceRepositoryTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceRepositoryTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceRepositoryTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceRepositoryListClassroomsPagedAsyncTests(LearningSpaceRepositoryTestData testData)
    {
        _testData = testData;
    }

    private static Classroom CreateClassroom(int id)
    {
        var classroom = new Classroom(
            1,
            1,
            $"C-{id:D3}",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("default.png"),
            LearningSpaceDimensions.Create(10, 10, 3),
            LearningSpaceCoordinates.Create(id, id, 0));

        // Manually set the ID for testing purposes, as it's database-generated
        typeof(LearningSpace)
            .GetProperty(nameof(LearningSpace.Id))!
            .SetValue(classroom, id);

        return classroom;
    }

    /// <summary>
    /// Ensures that paginated classrooms are retrieved correctly with valid page number and size
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_Correct_Page()
    {
        // Arrange
        var classrooms = Enumerable.Range(1, 25)
            .Select(CreateClassroom)
            .ToList();

        var learningSpacesDbSet = classrooms.Cast<LearningSpace>().ToList().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(2, 10);

        // Assert
        totalCount.Should().Be(25);
        pagedClassrooms.Should().HaveCount(10);
        pagedClassrooms.First().Id.Should().Be(11);
        pagedClassrooms.Last().Id.Should().Be(20);
    }

    /// <summary>
    /// Ensures that the first page returns the correct classrooms.
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_First_Page_Correctly()
    {
        // Arrange
        var classrooms = Enumerable.Range(1, 15)
            .Select(CreateClassroom)
            .ToList();
        var learningSpacesDbSet = classrooms.Cast<LearningSpace>().ToList().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(1, 10);

        // Assert
        totalCount.Should().Be(15);
        pagedClassrooms.Should().HaveCount(10);
        pagedClassrooms.First().Id.Should().Be(1);
        pagedClassrooms.Last().Id.Should().Be(10);
    }

    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_Last_Page_With_Remaining_Items()
    {
        // Arrange
        var classrooms = Enumerable.Range(1, 25)
            .Select(CreateClassroom)
            .ToList();

        var learningSpacesDbSet = classrooms.Cast<LearningSpace>().ToList().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(3, 10);

        // Assert
        totalCount.Should().Be(25);
        pagedClassrooms.Should().HaveCount(5);
        pagedClassrooms.First().Id.Should().Be(21);
        pagedClassrooms.Last().Id.Should().Be(25);
    }

    /// <summary>
    /// Ensures empty list is returned when page number exceeds available pages.
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_Empty_When_Page_Exceeds_Total()
    {
        // Arrange
        var classrooms = Enumerable.Range(1, 10)
            .Select(CreateClassroom)
            .ToList();

        var learningSpacesDbSet = classrooms.Cast<LearningSpace>().ToList().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(5, 10);

        // Assert
        totalCount.Should().Be(10);
        pagedClassrooms.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures ArgumentOutOfRangeException is thrown for invalid page number.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListClassroomsPagedAsync_Should_Throw_For_Invalid_PageNumber(int invalidPageNumber)
    {
        // Arrange
        var dbContextMock = new Mock<AppDbContext>();
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        Func<Task> act = () => repository.ListClassroomsPagedAsync(invalidPageNumber, 10);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Page number must be greater than zero*");
    }

    /// <summary>
    /// Ensures ArgumentOutOfRangeException is thrown for invalid page size.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListClassroomsPagedAsync_Should_Throw_For_Invalid_PageSize(int invalidPageSize)
    {
        // Arrange
        var dbContextMock = new Mock<AppDbContext>();
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        Func<Task> act = () => repository.ListClassroomsPagedAsync(1, invalidPageSize);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Page size must be greater than zero*");
    }

    /// <summary>
    /// Ensures correct total count is returned even when classrooms list is empty.
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_Zero_TotalCount_When_No_Classrooms()
    {
        // Arrange
        var learningSpacesDbSet = new List<LearningSpace>().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(1, 10);

        // Assert
        totalCount.Should().Be(0);
        pagedClassrooms.Should().BeEmpty();
    }

    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Filter_By_Keyword()
    {
        // Arrange
        var Classrooms = new List<Classroom>
        {
            CreateClassroom(1), // C-001
            CreateClassroom(2), // C-002
            CreateClassroom(3), // C-003
            CreateClassroom(4), // C-004
            CreateClassroom(5)  // C-005
        };

        var learningSpacesDbSet = Classrooms.Cast<LearningSpace>().ToList().BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);

        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(1, 10, "C-00");

        // Assert
        totalCount.Should().Be(5, because: "Total count of Classrooms found should be the same count as entered.");
        pagedClassrooms.Should().HaveCount(5, because: "All Classrooms match the keyword.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ListClassroomsPagedAsync_Given_No_Filter_Should_Filter_All_Classrooms(string? keyword)
    {
        // Arrange
        var Classrooms = new List<Classroom>
        {
            CreateClassroom(1), // C-001
            CreateClassroom(2), // C-002
            CreateClassroom(3), // C-003
            CreateClassroom(4), // C-004
            CreateClassroom(5)  // C-005
        };

        var learningSpacesDbSet = Classrooms.Cast<LearningSpace>().ToList().BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);

        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(1, 10, keyword);

        // Assert
        totalCount.Should().Be(5, because: "Total count of Classrooms found should be the same count as entered.");
        pagedClassrooms.Should().HaveCount(5, because: "All classrooms match the keyword.");
    }

    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Filter_One_Classroom_By_Keyword()
    {
        // Arrange
        var Classrooms = new List<Classroom>
        {
            CreateClassroom(1), // C-001
            CreateClassroom(2), // C-002
            CreateClassroom(3), // C-003
            CreateClassroom(4), // C-004
            CreateClassroom(5)  // C-005
        };

        var learningSpacesDbSet = Classrooms.Cast<LearningSpace>().ToList().BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);

        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(1, 10, "C-001");

        // Assert
        totalCount.Should().Be(1, because: "Total count of Classrooms found should be the same count as entered.");
        pagedClassrooms.Should().HaveCount(1, because: "Only 1 Classroom matches the keyword.");
    }

    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Filter_None_Classrooms_By_Keyword()
    {
        // Arrange
        var Classrooms = new List<Classroom>
        {
            CreateClassroom(1), // C-001
            CreateClassroom(2), // C-002
            CreateClassroom(3), // C-003
            CreateClassroom(4), // C-004
            CreateClassroom(5)  // C-005
        };

        var learningSpacesDbSet = Classrooms.Cast<LearningSpace>().ToList().BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);

        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedClassrooms, totalCount) = await repository.ListClassroomsPagedAsync(1, 10, "L-006");

        // Assert
        totalCount.Should().Be(0, because: "Total count of Classrooms found should be the same count as entered.");
        pagedClassrooms.Should().BeEmpty(because: "There are none Classrooms matching the keyword");
    }
}
