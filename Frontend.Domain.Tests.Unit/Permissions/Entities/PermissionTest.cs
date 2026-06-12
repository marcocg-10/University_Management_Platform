
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.Permissions.Entities;

/// <summary>
/// Provides tests for the <see cref="Permission"/> class, focusing on its constructor and initialization behavior.
/// </summary>
/// <remarks>This class includes tests to verify that the <see cref="Permission"/> constructor correctly
/// initializes its properties when provided with valid arguments. It also ensures that the default permission name
/// "PermissionName" can be successfully created during the test setup.</remarks>
public class PermissionTest
{

    private readonly PermissionName _inputName;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionTest"/> class.
    /// </summary>
    /// <remarks>This constructor attempts to create a permission name using the default value
    /// "PermissionName". If the creation fails, an exception is thrown.</remarks>
    /// <exception cref="Exception">Thrown if the default permission name "PermissionName" cannot be created.</exception>
    public PermissionTest()
    {
        var result = PermissionName.TryCreate("PermissionName", out PermissionName? inputName);

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

