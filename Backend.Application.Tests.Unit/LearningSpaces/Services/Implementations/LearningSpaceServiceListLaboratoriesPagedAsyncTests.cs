using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

public class LearningSpaceServiceListLaboratoriesPagedAsyncTests
{
    private readonly Mock<ILearningSpaceRepository> _repositoryMock;
    private readonly LearningSpaceService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public LearningSpaceServiceListLaboratoriesPagedAsyncTests()
    {
        _repositoryMock = new Mock<ILearningSpaceRepository>();
        _service = new LearningSpaceService(_repositoryMock.Object);
    }

    private static Laboratory CreateLaboratory(int id)
    {
        return new Laboratory(
            id,
            1,
            1,
            $"L-{id:D3}",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("default.png"),
            LearningSpaceDimensions.Create(10, 10, 3),
            LearningSpaceCoordinates.Create(id, id, 0));
    }

    /// <summary>
    /// Ensures that the service returns the correct page of laboratories and total count.
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_Correct_Page_And_TotalCount()
    {
        // Arrange
        var pageNumber = 2;
        var pageSize = 10;
        var expectedLaboratories = Enumerable.Range(11, 10)
            .Select(CreateLaboratory)
            .ToList();
        var expectedTotalCount = 25;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedLaboratories, expectedTotalCount));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize);

        // Assert
        laboratories.Should().NotBeNull();
        laboratories.Should().HaveCount(10);
        laboratories.Should().BeEquivalentTo(expectedLaboratories);
        totalCount.Should().Be(expectedTotalCount);

        _repositoryMock.Verify(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, null), Times.Once);
    }

    /// <summary>
    /// Ensures that the service returns the first page of laboratories correctly.
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_First_Page_Correctly()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var expectedLaboratories = Enumerable.Range(1, 10)
            .Select(CreateLaboratory)
            .ToList();
        var expectedTotalCount = 15;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedLaboratories, expectedTotalCount));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize);

        // Assert
        laboratories.Should().HaveCount(10);
        laboratories.First().Id.Should().Be(1);
        laboratories.Last().Id.Should().Be(10);
        totalCount.Should().Be(15);
    }

    /// <summary>
    /// Ensures that the service returns the last page of laboratories with remaining items.
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_Last_Page_With_Remaining_Items()
    {
        // Arrange
        var pageNumber = 3;
        var pageSize = 10;
        var expectedLaboratories = Enumerable.Range(21, 5)
            .Select(CreateLaboratory)
            .ToList();
        var expectedTotalCount = 25;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedLaboratories, expectedTotalCount));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize);

        // Assert
        laboratories.Should().HaveCount(5);
        laboratories.First().Id.Should().Be(21);
        laboratories.Last().Id.Should().Be(25);
        totalCount.Should().Be(25);
    }

    /// <summary>
    /// Ensures that the service returns an empty list when requesting a page beyond the total count.
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Return_Empty_List_When_Page_Exceeds_Total()
    {
        // Arrange
        var pageNumber = 10;
        var pageSize = 10;
        var expectedLaboratories = new List<Laboratory>();
        var expectedTotalCount = 15;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedLaboratories, expectedTotalCount));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize);

        // Assert
        laboratories.Should().BeEmpty();
        totalCount.Should().Be(15);

        _repositoryMock.Verify(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, null), Times.Once);
    }

    /// <summary>
    /// Ensures that the service correctly handles different page sizes.
    /// </summary>
    [Theory]
    [InlineData(5, 5)]
    [InlineData(10, 10)]
    [InlineData(20, 20)]
    [InlineData(50, 25)] // Only 25 total items
    public async Task ListLaboratoriesPagedAsync_Should_Handle_Different_Page_Sizes(int pageSize, int expectedCount)
    {
        // Arrange
        var pageNumber = 1;
        var totalLaboratories = 25;
        var expectedLaboratories = Enumerable.Range(1, Math.Min(pageSize, totalLaboratories))
            .Select(CreateLaboratory)
            .ToList();

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, null))
            .ReturnsAsync((expectedLaboratories, totalLaboratories));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize);

        // Assert
        laboratories.Should().HaveCount(expectedCount);
        totalCount.Should().Be(totalLaboratories);
    }

    /// <summary>
    /// Ensures that the service propagates ValidationException for invalid page number.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListLaboratoriesPagedAsync_Should_Throw_Exception_For_Invalid_PageNumber(int invalidPageNumber)
    {
        // Arrange
        var pageSize = 10;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(invalidPageNumber, pageSize, null))
            .ThrowsAsync(new ValidationException("Page number must be greater than 0."));

        // Act
        Func<Task> act = () => _service.ListLaboratoriesPagedAsync(invalidPageNumber, pageSize);

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
    public async Task ListLaboratoriesPagedAsync_Should_Throw_Exception_For_Invalid_PageSize(int invalidPageSize)
    {
        // Arrange
        var pageNumber = 1;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, invalidPageSize, null))
            .ThrowsAsync(new ValidationException("Page size must be greater than 0."));

        // Act
        Func<Task> act = () => _service.ListLaboratoriesPagedAsync(pageNumber, invalidPageSize);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Page size must be greater than 0*");
    }

    /// <summary>
    /// Ensures that the service filters laboratories by keyword.
    /// </summary>
    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Filter_By_Keyword()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var keyword = "L-00";
        var expectedLaboratories = Enumerable.Range(1, 10)
            .Select(CreateLaboratory)
            .ToList();
        var expectedTotalCount = 10;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword))
            .ReturnsAsync((expectedLaboratories, expectedTotalCount));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword);

        // Assert
        laboratories.Should().HaveCount(10);
        laboratories.First().Id.Should().Be(1);
        laboratories.Last().Id.Should().Be(10);
        totalCount.Should().Be(10);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ListLaboratoriesPagedAsync_Given_No_Filter_Should_Filter_All_Laboratories(string? keyword)
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var expectedLaboratories = Enumerable.Range(1, 10)
            .Select(CreateLaboratory)
            .ToList();
        var expectedTotalCount = 15;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword))
            .ReturnsAsync((expectedLaboratories, expectedTotalCount));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword);

        // Assert
        laboratories.Should().HaveCount(10);
        laboratories.First().Id.Should().Be(1);
        laboratories.Last().Id.Should().Be(10);
        totalCount.Should().Be(15);
    }

    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Filter_One_Laboratory_By_Keyword()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var keyword = "L-001";
        var expectedLaboratories = new List<Laboratory> { CreateLaboratory(1) };
        var expectedTotalCount = 1;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword))
            .ReturnsAsync((expectedLaboratories, expectedTotalCount));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword);

        // Assert
        laboratories.Should().HaveCount(1);
        totalCount.Should().Be(1);
    }

    [Fact]
    public async Task ListLaboratoriesPagedAsync_Should_Filter_None_Laboratories_By_Keyword()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var keyword = "L-00A";
        var expectedLaboratories = new List<Laboratory>();
        var expectedTotalCount = 0;

        _repositoryMock
            .Setup(r => r.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword))
            .ReturnsAsync((expectedLaboratories, expectedTotalCount));

        // Act
        var (laboratories, totalCount) = await _service.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword);

        // Assert
        laboratories.Should().BeEmpty();
        totalCount.Should().Be(0);
    }
}
