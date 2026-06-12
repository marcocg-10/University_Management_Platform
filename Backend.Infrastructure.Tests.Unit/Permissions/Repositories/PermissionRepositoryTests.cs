using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Permissions.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Permissions.Repositories;

public class PermissionRepositoryTests : IClassFixture<PermissionRepositoryTestData>
{
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly PermissionRepository _repository;
    private readonly PermissionRepositoryTestData _testData;    



    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionRepositoryTests"/> class.
    /// </summary>
    /// <remarks>This constructor sets up the necessary dependencies for testing the <see
    /// cref="PermissionRepository"/> class. It creates a mock instance of <see cref="AppDbContext"/> and initializes
    /// the repository with the mocked context.</remarks>
    public PermissionRepositoryTests(PermissionRepositoryTestData testData)
    {
        _dbContextMock = new Mock<AppDbContext>();
        _repository = new PermissionRepository(_dbContextMock.Object);
        _testData = testData;
    }

    /// <summary>
    /// Ensures all permissions are retrieved from the database.
    /// </summary>
    [Fact]
    public async Task AddPermissionsAsync_Should_Add_Permission_To_Database()
    {
        // Arrange
        var permissions = new List<Permission>().BuildMockDbSet();

        _dbContextMock.Setup(db => db.Permissions)
            .Returns(permissions.Object);

        var permission = new Permission(PermissionName.Create("CreateBuildings"));

        // Act
        await _repository.CreatePermissionAsync(permission);

        // Assert
        _dbContextMock.Verify(db => db.Permissions.AddAsync(permission, default), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);

    }

    /// <summary>
    /// Tests that the <see cref="PermissionRepository.GetAllPermissionsAsync"/> method returns a single data entry when
    /// the database contains exactly one permission entry.
    /// </summary>
    /// <remarks>This test verifies that the method correctly retrieves data from the mocked database context
    /// and ensures the result matches the expected single entry.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GetAllPermissionsAsync_Returns_SingleData_WhenGiven()
    {
        // Arrange
        var permissionsMock = _testData.SingleEntryData.BuildMockDbSet();
        
        _dbContextMock
            .Setup(db => db.Permissions)
            .Returns(permissionsMock.Object);
        var sut = new PermissionRepository(_dbContextMock.Object);

        // Act
        var result = await sut.GetAllPermissionsAsync();
        
        // Assert
        result.Should().BeEquivalentTo(_testData.SingleEntryData, because: "should return data from database");
    }

    /// <summary>
    /// Tests that the <see cref="PermissionRepository.GetAllPermissionsAsync"/> method  returns multiple permission
    /// entries when the database contains multiple records.
    /// </summary>
    /// <remarks>This test verifies that the method retrieves all permission entries from the database  and
    /// that the returned data matches the expected test data.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GetAllPermissionsAsync_Returns_MultipleData_WhenGiven()
    {
        // Arrange
        var permissionsMock = _testData.MultipleEntryData.BuildMockDbSet();
        
        _dbContextMock
            .Setup(db => db.Permissions)
            .Returns(permissionsMock.Object);
        var sut = new PermissionRepository(_dbContextMock.Object);
        // Act
        var result = await sut.GetAllPermissionsAsync();
        
        // Assert
        result.Should().BeEquivalentTo(_testData.MultipleEntryData, because: "should return multiple data from database");
    }

    /// <summary>
    /// Tests that the <see cref="PermissionRepository.GetAllPermissionsAsync"/> method returns an empty collection 
    /// when the database contains no permissions.
    /// </summary>
    /// <remarks>This test verifies that the method correctly handles the scenario where no data exists in the
    /// database,  ensuring that an empty collection is returned instead of null or an exception.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GetAllPermissions_WhenGivenNoData_ShouldReturnEmpty()
    {
        // Arrange
        var permissionsMock = _testData.EmptyData.BuildMockDbSet();

        _dbContextMock
            .Setup(db => db.Permissions)
            .Returns(permissionsMock.Object);
        var sut = new PermissionRepository(_dbContextMock.Object);
        // Act
        var result = await sut.GetAllPermissionsAsync();

        // Assert
        result.Should().BeEquivalentTo(_testData.EmptyData, because: "there are no permissions in database yet");

    }
}
