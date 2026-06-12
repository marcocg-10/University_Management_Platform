using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Users.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("user@gmail.com")]
    [InlineData("user.name@test-mail.cr")]
    public void TryCreate_GivenValidEmail_ReturnsTrue(string inputEmail)
    {
        // Act
        bool result = Email.TryCreate(inputEmail, out var outputEmail, out string? emailError);
        // Assert
        result.Should().BeTrue(because: "input is a valid email address");
        emailError.Should().BeNull(because: "no error should be reported when the email is valid");
        outputEmail.Value.Should().Be(inputEmail, because: "The email is valid");
    }

    [Theory]
    [InlineData("user")]
    [InlineData("user@")]
    [InlineData("user@email")]
    [InlineData("@email.com")]
    [InlineData("@")]
    [InlineData(".@.")]
    public void TryCreate_GivenInvalidEmail_ReturnsFalse(string inputEmail)
    {
        // Act
        bool result = Email.TryCreate(inputEmail, out var outputEmail, out string? emailError);
        // Assert
        result.Should().BeFalse(because: "input is an invalid email address");
        emailError.Should().Be(
            $"Email {inputEmail} has an invalid format.",
            because: "The email is invalid");
        outputEmail.Should().BeNull(because: "No email should be created when invalid");
    }

    [Theory]
    [InlineData("user")]
    [InlineData("user@")]
    [InlineData("user@email")]
    [InlineData("@email.com")]
    [InlineData("@")]
    [InlineData(".@.")]
    public void Create_GivenInvalidEmail_ThrowsValidationException(string inputEmail)
    {
        //Act
        FluentActions.Invoking(() =>
            {
            _ = Email.Create(inputEmail);
            }).Should() // Assert
            .ThrowExactly<ValidationException>(because: $"Email {inputEmail} has an invalid format.")
            .WithMessage($"*{inputEmail}*", because: "the exception message should include the invalid email");
    }

    [Theory]
    [InlineData("user@gmail.com")]
    [InlineData("user.name@test-mail.cr")]
    public void Equality_GivenSameEmailValues_AreEqual(string inputEmail)
    {
        //Act
        _ = Email.TryCreate(inputEmail, out var email, out _);
        _ = Email.TryCreate(inputEmail, out var email2, out _);
        // Assert
        email.Should().Be(email2, because: "both emails have the same value");
    }

    [Theory]
    [InlineData("user@gmail.com", "john.a@hotmail.com")]
    [InlineData("user.name@test-mail.cr", "jane.a@ucr.ac.cr")]
    public void Equality_GivenDifferentEmailValues_AreUnequal(string inputEmail1, string inputEmail2)
    {
        //Act
        _ = Email.TryCreate(inputEmail1, out var email, out _);
        _ = Email.TryCreate(inputEmail2, out var email2, out _);
        // Assert
        email.Should().NotBe(email2, because: "both emails are different");
    }
}
