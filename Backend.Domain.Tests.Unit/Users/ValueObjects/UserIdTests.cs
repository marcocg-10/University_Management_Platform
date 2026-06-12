using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;


namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Users.ValueObjects;

public class UserIdTests
{

    [Theory]
    [InlineData("C24214")]
    [InlineData("C5B231")]
    [InlineData("404547117")]

    public void TryCreate_GivenValidId_ReturnsTrue(string inputId)
    {
        // Act
        bool result = UserId.TryCreate(inputId, out var outputId, out string? idError);
        // Assert
        result.Should().BeTrue(because: "input is a valid id");
        idError.Should().BeNull(because: "no error should be reported when the user id is valid");
        outputId.Value.Should().Be(inputId, because: "The user id is valid");
    }

    [Theory]
    [InlineData(" 24214")]
    [InlineData("C2")]
    [InlineData("C@231")]
    public void TryCreate_GivenInvalidUserId_ReturnsFalse(string inputId)
    {
        // ActUserId
        bool result = UserId.TryCreate(inputId, out var outputId, out string? idError);
        // Assert
        result.Should().BeFalse(because: "input is an invalid id");
        idError.Should().Be(
            $"id {inputId} is invalid: Id must be between 5 and 30 characters (letters, numbers, hyphens) and must not start or end with a hyphen",
            because: "The user id is invalid");
        outputId.Should().BeNull(because: "No user id should be created when invalid");
    }
  
    [Theory]
    [InlineData("2424")]
    [InlineData("C2")]
    [InlineData("@231")]
    public void Create_GivenInvalidId_ThrowsValidationException(string inputId)
    {
        //Act
        FluentActions.Invoking(() =>
        {
            _ = UserId.Create(inputId);
        }).Should() // Assert
            .ThrowExactly<ValidationException>(because: "the Id has an invalid format")
            .WithMessage($"*{inputId}*", because: "the exception message should include the invalid Id");
    }

    [Theory]
    [InlineData("C24214", "C24214")]
    [InlineData("457556455", "457556455")]
    
    public void EqualityOperator_GivenTwoSameUserIds_AreEqual(string inputId1, string inputId2)
    {
        // Arrange
        _ = UserId.TryCreate(inputId1, out var id1, out _);
        _ = UserId.TryCreate(inputId2, out var id2, out _);
       
        // Assert
        id1.Should().Be(id2, because: "both user ids have the same value");
    }

    [Theory]
    [InlineData("C24214", "C24254")]
    [InlineData("457556455", "457554455")]
    public void EqualityOperator_GivenTwoDifferentUserIds_AreUnequal(string inputId1, string inputId2)
    {
        // Arrange
        _ = UserId.TryCreate(inputId1, out var id1, out _);
        _ = UserId.TryCreate(inputId2, out var id2, out _);

        // Assert
        id1.Should().NotBe(id2, because: "both user ids have different values");
    }


}

