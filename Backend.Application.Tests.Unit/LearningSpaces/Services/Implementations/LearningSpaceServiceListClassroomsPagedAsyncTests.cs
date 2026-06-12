using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

public class LearningSpaceServiceListClassroomsPagedAsyncTests
{
    private readonly Mock<ILearningSpaceRepository> _repositoryMock;
    private readonly LearningSpaceService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public LearningSpaceServiceListClassroomsPagedAsyncTests()
    {
        _repositoryMock = new Mock<ILearningSpaceRepository>();
        _service = new LearningSpaceService(_repositoryMock.Object);
    }

    private static Classroom CreateClassroom(int id)
    {
        return new Classroom(
            id,
            1,
            1,
            $"C-{id:D3}",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("default.png"),
            LearningSpaceDimensions.Create(10, 10, 3),
            LearningSpaceCoordinates.Create(id, id, 0));
    }

    /// <summary>
    /// Ensures that the service returns the correct page of classrooms and total count.
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_Correct_Page_And_TotalCount()
    {
        // Arrange
        var pageNumber = 2;
        var pageSize = 10;
        var expectedClassrooms = Enumerable.Range(11, 10)
            .Select(CreateClassroom)
            .ToList();
        var expectedTotalCount = 25;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedClassrooms, expectedTotalCount));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

        // Assert
        classrooms.Should().NotBeNull();
        classrooms.Should().HaveCount(10);
        classrooms.Should().BeEquivalentTo(expectedClassrooms);
        totalCount.Should().Be(expectedTotalCount);

        _repositoryMock.Verify(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null), Times.Once);
    }

    /// <summary>
    /// Ensures that the service returns the first page of classrooms correctly.
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_First_Page_Correctly()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var expectedClassrooms = Enumerable.Range(1, 10)
            .Select(CreateClassroom)
            .ToList();
        var expectedTotalCount = 15;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedClassrooms, expectedTotalCount));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

        // Assert
        classrooms.Should().HaveCount(10);
        classrooms.First().Id.Should().Be(1);
        classrooms.Last().Id.Should().Be(10);
        totalCount.Should().Be(15);
    }

    /// <summary>
    /// Ensures that the service returns the last page of classrooms with remaining items.
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_Last_Page_With_Remaining_Items()
    {
        // Arrange
        var pageNumber = 3;
        var pageSize = 10;
        var expectedClassrooms = Enumerable.Range(21, 5)
            .Select(CreateClassroom)
            .ToList();
        var expectedTotalCount = 25;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedClassrooms, expectedTotalCount));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

        // Assert
        classrooms.Should().HaveCount(5);
        classrooms.First().Id.Should().Be(21);
        classrooms.Last().Id.Should().Be(25);
        totalCount.Should().Be(25);
    }

    /// <summary>
    /// Ensures that the service returns an empty list when requesting a page beyond the total count.
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Return_Empty_List_When_Page_Exceeds_Total()
    {
        // Arrange
        var pageNumber = 10;
        var pageSize = 10;
        var expectedClassrooms = new List<Classroom>();
        var expectedTotalCount = 15;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedClassrooms, expectedTotalCount));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

        // Assert
        classrooms.Should().BeEmpty();
        totalCount.Should().Be(15);

        _repositoryMock.Verify(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null), Times.Once);
    }

    /// <summary>
    /// Ensures that the service correctly handles different page sizes.
    /// </summary>
    [Theory]
    [InlineData(5, 5)]
    [InlineData(10, 10)]
    [InlineData(20, 20)]
    [InlineData(50, 25)] // Only 25 total items
    public async Task ListClassroomsPagedAsync_Should_Handle_Different_Page_Sizes(int pageSize, int expectedCount)
    {
        // Arrange
        var pageNumber = 1;
        var totalClassrooms = 25;
        var expectedClassrooms = Enumerable.Range(1, Math.Min(pageSize, totalClassrooms))
            .Select(CreateClassroom)
            .ToList();

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedClassrooms, totalClassrooms));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

        // Assert
        classrooms.Should().HaveCount(expectedCount);
        totalCount.Should().Be(totalClassrooms);
    }

    /// <summary>
    /// Ensures that the service propagates ValidationException for invalid page number.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListClassroomsPagedAsync_Should_Throw_Exception_For_Invalid_PageNumber(int invalidPageNumber)
    {
        // Arrange
        var pageSize = 10;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(invalidPageNumber, pageSize, null))
            .ThrowsAsync(new ValidationException("Page number must be greater than 0."));

        // Act
        Func<Task> act = () => _service.ListClassroomsPagedAsync(invalidPageNumber, pageSize);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Page number must be greater than 0*");
    }

    /// <summary>
    /// Ensures that the service propagates ValidationException for invalid page size.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListClassroomsPagedAsync_Should_Throw_Exception_For_Invalid_PageSize(int invalidPageSize)
    {
        // Arrange
        var pageNumber = 1;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, invalidPageSize, null))
            .ThrowsAsync(new ValidationException("Page size must be greater than 0."));

        // Act
        Func<Task> act = () => _service.ListClassroomsPagedAsync(pageNumber, invalidPageSize);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Page size must be greater than 0*");
    }

    /// <summary>
    /// Ensures that the service filters classrooms by keyword.
    /// </summary>
    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Filter_By_Keyword()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var keyword = "C-00";
        var expectedClassrooms = Enumerable.Range(1, 10)
            .Select(CreateClassroom)
            .ToList();
        var expectedTotalCount = 10;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, keyword))
            .ReturnsAsync((expectedClassrooms, expectedTotalCount));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize, keyword);

        // Assert
        classrooms.Should().HaveCount(10);
        classrooms.First().Id.Should().Be(1);
        classrooms.Last().Id.Should().Be(10);
        totalCount.Should().Be(10);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ListClassroomsPagedAsync_Given_No_Filter_Should_Filter_All_Classrooms(string? keyword)
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var expectedClassrooms = Enumerable.Range(1, 10)
            .Select(CreateClassroom)
            .ToList();
        var expectedTotalCount = 15;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, keyword))
            .ReturnsAsync((expectedClassrooms, expectedTotalCount));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize, keyword);

        // Assert
        classrooms.Should().HaveCount(10);
        classrooms.First().Id.Should().Be(1);
        classrooms.Last().Id.Should().Be(10);
        totalCount.Should().Be(15);
    }

    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Filter_One_Classroom_By_Keyword()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var keyword = "C-001";
        var expectedClassrooms = new List<Classroom> { CreateClassroom(1) };
        var expectedTotalCount = 1;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, keyword))
            .ReturnsAsync((expectedClassrooms, expectedTotalCount));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize, keyword);

        // Assert
        classrooms.Should().HaveCount(1);
        totalCount.Should().Be(1);
    }

    [Fact]
    public async Task ListClassroomsPagedAsync_Should_Filter_None_Classrooms_By_Keyword()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var keyword = "C-00A";
        var expectedClassrooms = new List<Classroom>();
        var expectedTotalCount = 0;

        _repositoryMock
            .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, keyword))
            .ReturnsAsync((expectedClassrooms, expectedTotalCount));

        // Act
        var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize, keyword);

        // Assert
        classrooms.Should().BeEmpty();
        totalCount.Should().Be(0);
    }
}