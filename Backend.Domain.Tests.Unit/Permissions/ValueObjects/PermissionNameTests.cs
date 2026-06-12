using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Permissions.ValueObjects;

public class PermissionNameTests
{
    [Theory]
    [InlineData("Create-Building")]
    [InlineData("Create3Buildings")]
    [InlineData("CreateBuildings")]
    [InlineData("CreateIComponents")]
    [InlineData("UpdateUsers")]
    public void TryCreate_GivenValidName_ReturnsTrue(string inputName)
    {
        // Act
        bool result = PermissionName.TryCreate(inputName, out _, out _);
        // Assert
        result.Should().BeTrue(because: "input is a valid permission name address");
    }

    [Theory]
    [InlineData("CreateBuilding?")]
    [InlineData("Create_Building")]
    [InlineData("1")]
    [InlineData("PermissionNumberTwentyThreeAndMoreCharacters")]
    [InlineData("ANameTooLongToBeWrittenInsideADatabase")]
    public void TryCreate_GivenInvalidName_ReturnsFalse(string inputName)
    {
        // Act
        bool result = PermissionName.TryCreate(inputName, out _, out _);
        // Assert
        result.Should().BeFalse(because: "input is an invalid permission name address");
    }


    [Theory]
    [InlineData("CreateBuildings")]
    [InlineData("DeleteSpaces")]
    public void Equality_GivenSameNameValues_AreEqual(string inputName)
    {
        //Act
        _ = PermissionName.TryCreate(inputName, out var name, out _);
        _ = PermissionName.TryCreate(inputName, out var name2, out _);
        // Assert
        name.Should().Be(name2, because: "both names have the same value");
    }

    [Theory]
    [InlineData("CreateBuildings", "DeleteUsers")]
    [InlineData("ReadComponents", "ReadUsers")]
    public void Equality_GivenDifferentNameValues_AreUnequal(string inputName1, string inputName2)
    {
        //Act
        _ = PermissionName.TryCreate(inputName1, out var name, out _);
        _ = PermissionName.TryCreate(inputName2, out var name2, out _);
        // Assert
        name.Should().NotBe(name2, because: "both names are different");
    }
}

//TODO: validate out parameter
