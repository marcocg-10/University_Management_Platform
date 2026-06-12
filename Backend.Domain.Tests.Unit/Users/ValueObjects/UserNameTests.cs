using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Users.ValueObjects;

public class UserNameTests
{
    [Theory]
    [InlineData("Arianna Leitón Ñan")]
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
            $"Name {inputName} is invalid: Name must be over 3 characters (letters, spaces) and must not start with a whitespace",
            because: "The user name is invalid");
        outputName.Should().BeNull(because: "No user name should be created when invalid");
    }

    [Theory]
    [InlineData("us")]
    [InlineData("us24")]
    [InlineData("ul")]
  
    public void Create_GivenInvalidName_ThrowsValidationException(string inputName)
    {
        //Act
        FluentActions.Invoking(() =>
        {
            _ = UserName.Create(inputName);
        }).Should() // Assert
            .ThrowExactly<ValidationException>(because: "the Name has an invalid format")
            .WithMessage($"*{inputName}*", because: "the exception message should include the invalid Name");
    }
}