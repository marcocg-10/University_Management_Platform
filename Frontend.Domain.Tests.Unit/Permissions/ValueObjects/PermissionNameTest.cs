
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.Permissions.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="PermissionName"/> class, verifying its behavior when creating and comparing
/// permission names.
/// </summary>
/// <remarks>This class includes tests for validating the creation of permission names using the <see
/// cref="PermissionName.TryCreate"/> method, as well as equality checks for instances of <see cref="PermissionName"/>.
/// The tests ensure that valid names are accepted, invalid names are rejected, and equality comparisons behave as
/// expected.</remarks>
public class PermissionNameTests
{

    /// <summary>
    /// Tests that the <see cref="PermissionName.TryCreate"/> method returns <see langword="true"/>  when provided with
    /// a valid permission name.
    /// </summary>
    /// <remarks>This test ensures that valid permission names are correctly recognized and processed  by the
    /// <see cref="PermissionName.TryCreate"/> method.</remarks>
    /// <param name="inputName">The permission name to validate and create.</param>
    [Theory]
    [InlineData("Create-Building")]
    [InlineData("Create3Buildings")]
    [InlineData("CreateBuildings")]
    [InlineData("CreateIComponents")]
    [InlineData("UpdateUsers")]
    public void TryCreate_GivenValidName_ReturnsTrue(string inputName)
    {
        // Act
        bool result = PermissionName.TryCreate(inputName, out _);
        // Assert
        result.Should().BeTrue(because: "input is a valid permission name address");
    }

    /// <summary>
    /// Tests that the <see cref="PermissionName.TryCreate"/> method returns <see langword="false"/>  when provided with
    /// an invalid permission name.
    /// </summary>
    /// <remarks>This test verifies that the <see cref="PermissionName.TryCreate"/> method correctly
    /// identifies  invalid permission names, such as those containing special characters, starting with a number, 
    /// exceeding the maximum length, or not adhering to naming conventions.</remarks>
    /// <param name="inputName">The permission name to validate.</param>
    [Theory]
    [InlineData("CreateBuilding?")]
    [InlineData("Create_Building")]
    [InlineData("1")]
    [InlineData("PermissionNumberTwentyThreeAndMoreCharacters")]
    [InlineData("ANameTooLongToBeWrittenInsideADatabase")]
    public void TryCreate_GivenInvalidName_ReturnsFalse(string inputName)
    {
        // Act
        bool result = PermissionName.TryCreate(inputName, out _);
        // Assert
        result.Should().BeFalse(because: "input is an invalid permission name address");
    }

    /// <summary>
    /// Verifies that two <see cref="PermissionName"/> instances with the same name value are considered equal.
    /// </summary>
    /// <remarks>This test ensures that the equality comparison for <see cref="PermissionName"/> is based on
    /// the name value.</remarks>
    /// <param name="inputName">The name value used to create the <see cref="PermissionName"/> instances.</param>
    [Theory]
    [InlineData("CreateBuildings")]
    [InlineData("DeleteSpaces")]
    public void Equality_GivenSameNameValues_AreEqual(string inputName)
    {
        //Act
        _ = PermissionName.TryCreate(inputName, out var name);
        _ = PermissionName.TryCreate(inputName, out var name2);
        // Assert
        name.Should().Be(name2, because: "both names have the same value");
    }

    /// <summary>
    /// Verifies that two <see cref="PermissionName"/> instances created with different name values are not considered
    /// equal.
    /// </summary>
    /// <remarks>This test ensures that the equality comparison for <see cref="PermissionName"/> distinguishes
    /// between instances created with different name values.</remarks>
    /// <param name="inputName1">The first name value used to create a <see cref="PermissionName"/> instance.</param>
    /// <param name="inputName2">The second name value used to create a <see cref="PermissionName"/> instance.</param>
    [Theory]
    [InlineData("CreateBuildings", "DeleteUsers")]
    [InlineData("ReadComponents", "ReadUsers")]
    public void Equality_GivenDifferentNameValues_AreUnequal(string inputName1, string inputName2)
    {
        //Act
        _ = PermissionName.TryCreate(inputName1, out var name);
        _ = PermissionName.TryCreate(inputName2, out var name2);
        // Assert
        name.Should().NotBe(name2, because: "both names are different");
    }
}

