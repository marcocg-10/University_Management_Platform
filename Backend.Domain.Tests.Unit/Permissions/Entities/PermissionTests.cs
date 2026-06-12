using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Permissions.Entities;

/// <summary>
/// Contains unit tests for the <see cref="Permission"/> class, verifying its behavior and correctness.
/// </summary>
/// <remarks>This class includes tests to ensure that the <see cref="Permission"/> class behaves as expected when
/// instantiated and used. The tests validate the proper initialization of properties and other expected
/// behaviors.</remarks>
public class PermissionTest
{

    private readonly PermissionName _inputName;

    public PermissionTest()
    {
        var result = PermissionName.TryCreate("PermissionName", out PermissionName? inputName, out _);

        if (!result || inputName is null)
        {
            throw new Exception("Failed to construct input permission name");
        }
        _inputName = inputName;
    }

    /// <summary>
    /// Tests that the <see cref="Permission"/> constructor correctly initializes the <see cref="Permission.Name"/>
    /// property when provided with a valid name.
    /// </summary>
    /// <remarks>This test verifies that the <see cref="Permission.Name"/> property is set to the value passed
    /// to the constructor.</remarks>
    [Fact]
    public void Ctor_GivenValidArguments_CorrectlyNameProperties()
    {
        // Arrange
        //Preparation, initialization, configuration

        // Act
        var permission = new Permission(_inputName);

        // Assert
        // Ensure expected behavior
        permission.Name.Should().Be(_inputName,
            because: "ctor should correctly set the name passed as parameter.");
    }

}
