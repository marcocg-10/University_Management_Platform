using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.LearningSpaces.Repositories;

public class LearningSpaceRepositoryListLaboratoriesPagedAsyncTests : IClassFixture<LearningSpaceRepositoryTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceRepositoryTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceRepositoryTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceRepositoryListLaboratoriesPagedAsyncTests(LearningSpaceRepositoryTestData testData)
    {
        _testData = testData;
    }

    private static Laboratory CreateLaboratory(int id)
    {
        var lab = new Laboratory(
            1,
            1,
            $"L-{id:D3}",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("default.png"),
            LearningSpaceDimensions.Create(10, 10, 3),
            LearningSpaceCoordinates.Create(id, id, 0));

        // Manually set the ID for testing purposes, as it's database-generated
        typeof(LearningSpace)
            .GetProperty(nameof(LearningSpace.Id))!
            .SetValue(lab, id);

        return lab;
    }

    /// <summary>
    /// Ensures that paginated laboratories are retrieved correctly with valid page number and size
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_Correct_Page()
    {
        // Arrange
        var laboratories = Enumerable.Range(1, 25)
            .Select(CreateLaboratory)
            .ToList();

        var learningSpacesDbSet = laboratories.Cast<LearningSpace>().ToList().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(2, 10);

        // Assert
        totalCount.Should().Be(25, because: "25 laboratories were created to test.");
        pagedLaboratories.Should().HaveCount(10, because: "Second page should have 10 laboratories.");
        pagedLaboratories.First().Id.Should().Be(11, because: "The first laboratory's ID in second page should be 11.");
        pagedLaboratories.Last().Id.Should().Be(20, because: "The last laboratory's ID in second page should be 20.");
    }

    /// <summary>
    /// Ensures that the first page returns the correct laboratories.
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_First_Page_Correctly()
    {
        // Arrange
        var laboratories = Enumerable.Range(1, 15)
            .Select(CreateLaboratory)
            .ToList();
        var learningSpacesDbSet = laboratories.Cast<LearningSpace>().ToList().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(1, 10);

        // Assert
        totalCount.Should().Be(15, because: "25 laboratories were created to test.");
        pagedLaboratories.Should().HaveCount(10, because: "There are 10 laboratories available in the first page");
        pagedLaboratories.First().Id.Should().Be(1, because: "The first laboratory's ID in first page should be 1.");
        pagedLaboratories.Last().Id.Should().Be(10, because: "The last laboratory's ID in first page should be 10.");
    }

    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_Last_Page_With_Remaining_Items()
    {
        // Arrange
        var laboratories = Enumerable.Range(1, 25)
            .Select(CreateLaboratory)
            .ToList();

        var learningSpacesDbSet = laboratories.Cast<LearningSpace>().ToList().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(3, 10);

        // Assert
        totalCount.Should().Be(25, because: "25 laboratories were created to test.");
        pagedLaboratories.Should().HaveCount(5, because: "There are only 5 laboratories available in the last page");
        pagedLaboratories.First().Id.Should().Be(21, because: "The first laboratory's ID in last page should be 21.");
        pagedLaboratories.Last().Id.Should().Be(25, because: "The last laboratory's ID in last page should be 25.");
    }

    /// <summary>
    /// Ensures empty list is returned when page number exceeds available pages.
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_Empty_When_Page_Exceeds_Total()
    {
        // Arrange
        var laboratories = Enumerable.Range(1, 10)
            .Select(CreateLaboratory)
            .ToList();

        var learningSpacesDbSet = laboratories.Cast<LearningSpace>().ToList().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(5, 10);

        // Assert
        totalCount.Should().Be(10, because: "10 laboratories were created to test.");
        pagedLaboratories.Should().BeEmpty(because: "Count of pages requested exceeds available pages.");
    }

    /// <summary>
    /// Ensures ArgumentOutOfRangeException is thrown for invalid page number.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListLaboratoriesPagedAsync_Should_Throw_For_Invalid_PageNumber(int invalidPageNumber)
    {
        // Arrange
        var dbContextMock = new Mock<AppDbContext>();
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        Func<Task> act = () => repository.ListLaboratoriesPagedAsync(invalidPageNumber, 10);

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
    public async Task ListLaboratoriesPagedAsync_Should_Throw_For_Invalid_PageSize(int invalidPageSize)
    {
        // Arrange
        var dbContextMock = new Mock<AppDbContext>();
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        Func<Task> act = () => repository.ListLaboratoriesPagedAsync(1, invalidPageSize);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Page size must be greater than zero*");
    }

    /// <summary>
    /// Ensures correct total count is returned even when laboratories list is empty.
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_Zero_TotalCount_When_No_Laboratories()
    {
        // Arrange
        var learningSpacesDbSet = new List<LearningSpace>().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);
        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(1, 10);

        // Assert
        totalCount.Should().Be(0, because: "There are no laboratories to page.");
        pagedLaboratories.Should().BeEmpty(because: "There are no laboratories to page.");
    }

    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Filter_By_Keyword()
    {
        // Arrange
        var laboratories = new List<Laboratory>
        {
            CreateLaboratory(1), // L-001
            CreateLaboratory(2), // L-002
            CreateLaboratory(3), // L-003
            CreateLaboratory(4), // L-004
            CreateLaboratory(5)  // L-005
        };

        var learningSpacesDbSet = laboratories.Cast<LearningSpace>().ToList().BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);

        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(1, 10, "L-00");

        // Assert
        totalCount.Should().Be(5, because: "Total count of laboratories found should be the same count as entered.");
        pagedLaboratories.Should().HaveCount(5, because: "All laboratories match the keyword.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ListLaboratoriesPagedAsync_Given_No_Filter_Should_Filter_All_Laboratories(string? keyword)
    {
        // Arrange
        var laboratories = new List<Laboratory>
        {
            CreateLaboratory(1), // L-001
            CreateLaboratory(2), // L-002
            CreateLaboratory(3), // L-003
            CreateLaboratory(4), // L-004
            CreateLaboratory(5)  // L-005
        };

        var learningSpacesDbSet = laboratories.Cast<LearningSpace>().ToList().BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);

        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(1, 10, keyword);

        // Assert
        totalCount.Should().Be(5, because: "Total count of laboratories found should be the same count as entered.");
        pagedLaboratories.Should().HaveCount(5, because: "All labortories match the keyword.");
    }

    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Filter_One_Laboratory_By_Keyword()
    {
        // Arrange
        var laboratories = new List<Laboratory>
        {
            CreateLaboratory(1), // L-001
            CreateLaboratory(2), // L-002
            CreateLaboratory(3), // L-003
            CreateLaboratory(4), // L-004
            CreateLaboratory(5)  // L-005
        };

        var learningSpacesDbSet = laboratories.Cast<LearningSpace>().ToList().BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);

        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(1, 10, "L-001");

        // Assert
        totalCount.Should().Be(1, because: "Total count of laboratories found should be the same count as entered.");
        pagedLaboratories.Should().HaveCount(1, because: "Only 1 laboratory matches the keyword.");
    }

    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Filter_None_Laboratories_By_Keyword()
    {
        // Arrange
        var laboratories = new List<Laboratory>
        {
            CreateLaboratory(1), // L-001
            CreateLaboratory(2), // L-002
            CreateLaboratory(3), // L-003
            CreateLaboratory(4), // L-004
            CreateLaboratory(5)  // L-005
        };

        var learningSpacesDbSet = laboratories.Cast<LearningSpace>().ToList().BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock.Setup(db => db.LearningSpaces).Returns(learningSpacesDbSet.Object);

        var repository = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var (pagedLaboratories, totalCount) = await repository.ListLaboratoriesPagedAsync(1, 10, "L-006");

        // Assert
        totalCount.Should().Be(0, because: "Total count of laboratories found should be the same count as entered.");
        pagedLaboratories.Should().BeEmpty(because: "There are none laboratories matching the keyword");
    }
}
