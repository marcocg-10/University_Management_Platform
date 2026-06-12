using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.Users.ValueObjects;

public class UserNameTests
{
    [Theory]
    [InlineData("Arianna Leitón Ñ")]
    [InlineData("Andrew Leiva")]
    public void TryCreate_GivenValidName_ReturnsTrue(string inputName)
    {
        // Act
        bool result = UserName.TryCreate(inputName, out var outputName, out string? nameError);
        // Assert
        result.Should().BeTrue(because: "input is a valid name");
        nameError.Should().BeNull(because: "no error should be reported when the user name is valid");
        outputName.Value.Should().Be(inputName, because: "The user name is valid");
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("a21")]

    public void TryCreate_GivenInvalidUserName_ReturnsFalse(string inputName)
    {
        // ActUserName
        bool result = UserName.TryCreate(inputName, out var outputName, out string? nameError);
        // Assert
        result.Should().BeFalse(because: "input is an invalid name");
        nameError.Should().Be(
            $"Name must be over 3 characters (letters, spaces) and must not start with a whitespace");
        outputName.Should().BeNull(because: "No user name should be created when invalid");
    }

    [Theory]
    [InlineData("us")]
    [InlineData("us24")]
    [InlineData("ul")]

    public void Create_GivenInvalidName_ThrowsUserDataException(string inputName)
    {
        //Act
        FluentActions.Invoking(() =>
            {
            _ = UserName.Create(inputName);
            }).Should() // Assert
            .ThrowExactly<UserDataException>(because: "the Name has an invalid format")
            .WithMessage("Name must be over 3 characters (letters, spaces) and must not start with a whitespace");
    }
}