using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.Roles.ValueObjects;

public class RoleNameTests
{
    [Fact]
    public void TryCreate_ValidName_ReturnsTrueAndCreatesRoleName()
    {
        // Arrange
        var validName = "Administrator";
        // Act
        var result = RoleName.TryCreate(validName, out var roleName, out var error);
        // Assert
        result.Should().BeTrue();
        roleName.Should().NotBeNull();
        roleName!.Value.Should().Be(validName);
        error.Should().BeNull();
    }
    [Fact]
    public void TryCreate_EmptyName_ReturnsFalseAndErrorMessage()
    {
        // Arrange
        var emptyName = "";
        // Act
        var result = RoleName.TryCreate(emptyName, out var roleName, out var error);
        // Assert
        result.Should().BeFalse();
        roleName.Should().BeNull();
        error.Should().Be($"Name {emptyName} is invalid: Name must be over 3 characters (letters, spaces) and must not start with a whitespace");
    }
    [Fact]
    public void TryCreate_WhitespaceName_ReturnsFalseAndErrorMessage()
    {
        // Arrange
        var whitespaceName = "   ";
        // Act
        var result = RoleName.TryCreate(whitespaceName, out var roleName, out var error);
        // Assert
        result.Should().BeFalse();
        roleName.Should().BeNull();
        error.Should().Be($"Name {whitespaceName} is invalid: Name must be over 3 characters (letters, spaces) and must not start with a whitespace");
    }
    [Fact]
    public void TryCreate_NameExceedingMaxLength_ReturnsFalseAndErrorMessage()
    {
        // Arrange
        var longName = new string('a', 31); // 31 characters
        // Act
        var result = RoleName.TryCreate(longName, out var roleName, out var error);
        // Assert
        result.Should().BeFalse();
        roleName.Should().BeNull();
        error.Should().Be($"Name {longName} is invalid: Name must be over 3 characters (letters, spaces) and must not start with a whitespace");
    }
    [Fact]
    public void Create_ValidName_ReturnsRoleName()
    {
        // Arrange
        var validName = "User";
        // Act
        var roleName = RoleName.Create(validName);
        // Assert
        roleName.Should().NotBeNull();
        roleName.Value.Should().Be(validName);
    }
    [Fact]
    public void Create_InvalidName_ThrowsValidationException()
    {
        // Arrange
        var invalidName = ""; // Empty name
        // Act & Assert

        //Act
        FluentActions.Invoking(() =>
            {
            _ = RoleName.Create(invalidName);
            }).Should() // Assert
            .ThrowExactly<RoleInvalidDataException>(because: $"Name {invalidName} is invalid: Name must be over 3 characters (letters, spaces) and must not start with a whitespace")
            .WithMessage($"*{invalidName}*", because: "the exception message should include the invalid name");
    }
}

