using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;


namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Users.Entities;

/// <summary>
/// Contains unit tests for the <see cref="User"/> class, verifying the correct behavior of its constructor and the
/// initialization of its properties.
/// </summary>
/// <remarks>This class includes tests to ensure that the <see cref="User"/> constructor correctly assigns values
/// to the <see cref="User.Id"/>, <see cref="User.Name"/>, <see cref="User.IsActive"/>, <see cref="User.Email"/>, 
/// and <see cref="User.AzureObjectIdentifier"/> properties when provided with valid arguments.</remarks>
public class UserTests
{
    private readonly UserId _inputId;
    private readonly UserName _inputName;
    private readonly bool _inputIsActive;
    private readonly Email _inputEmail;
    private readonly string? _inputAzureObjectIdentifier;
    private readonly List<Role> _inputRoles;

    public UserTests()
    {
        var userId = UserId.TryCreate(
            "9ed2c75f-8ac7-4a45-8b32-bdab",
            out UserId? inputId,
            out string? idError);
        var userNameResult = UserName.TryCreate(
            "John Doe",
            out UserName? inputName,
            out string? nameError);
        bool inputIsActive = true;
        var emailResult = Email.TryCreate(
            "john.doe@universitry.com",
            out Email? inputEmail,
            out string? emailError);

        if (!emailResult || inputEmail is null)
        {
            throw new Exception(emailError);
        }
        _inputEmail = inputEmail;

        if (!userNameResult || inputName is null)
        {
            throw new Exception(nameError);
        }
        _inputName = inputName;
        if (!userId || inputId is null)
        {
            throw new Exception(idError);
        }
    }

    /// <summary>
    /// Tests that the <see cref="User"/> constructor correctly initializes the <see cref="User.Id"/> property when
    /// provided with valid arguments.
    /// </summary>
    /// <remarks>This test ensures that the <see cref="User.Id"/> property is set to the value passed to the
    /// constructor, verifying that the constructor behaves as expected for valid input.</remarks>
    [Fact]
    public void Ctor_GivenValidArguments_CorrectlyIdProperties()
    {
        // Arrange
        //Preparation, initialization, configuration


        // Act
        //Action on the CTU (component under test) / SUT (system under test)
        var user = new User(
            _inputId,
            _inputName,
            _inputIsActive,
            _inputEmail,
            _inputAzureObjectIdentifier);

        // Assert
        // Ensure expected behavior

        user.Id.Should().Be(_inputId,
            because: "ctor should correctly set the ID passed as parameter.");
    }

    /// <summary>
    /// Tests that the <see cref="User"/> constructor correctly initializes the <see cref="User.Name"/> property when
    /// provided with valid arguments.
    /// </summary>
    /// <remarks>This test verifies that the <see cref="User.Name"/> property is set to the value passed to
    /// the constructor. It ensures that the constructor behaves as expected when valid input parameters are
    /// provided.</remarks>
    [Fact]
    public void Ctor_GivenValidArguments_CorrectlyNameProperties()
    {
        // Arrange
        //Preparation, initialization, configuration

        // Act
        //Action on the CTU (component under test) / SUT (system under test)
        var user = new User(
            _inputId,
            _inputName,
            _inputIsActive,
            _inputEmail,
            _inputAzureObjectIdentifier);

        // Assert
        // Ensure expected behavior

        user.Name.Should().Be(_inputName,
            because: "ctor should correctly set the name passed as parameter");

    }

    /// <summary>
    /// Tests that the <see cref="User"/> constructor correctly initializes the <see cref="User.IsActive"/> property
    /// based on the value provided in the constructor's parameters.
    /// </summary>
    /// <remarks>This test verifies that the <see cref="User.IsActive"/> property reflects the value of the
    /// <c>isActive</c> parameter passed to the constructor, ensuring the correct behavior of the initialization
    /// logic.</remarks>
    [Fact]
    public void Ctor_GivenValidArguments_CorrectlyIsActiveProperties()
    {
        // Arrange

        // Act
        //Action on the CTU (component under test) / SUT (system under test)
        var user = new User(
            _inputId,
            _inputName,
            _inputIsActive,
            _inputEmail,
            _inputAzureObjectIdentifier);

        // Assert
        // Ensure expected behavior
        //Assert.Equal(inputId, User.Id);

        user.IsActive.Should().Be(_inputIsActive,
            because: "ctor should correctly set the active status passed as parameter");

    }

    /// <summary>
    /// Tests that the <see cref="User"/> constructor correctly initializes the <see cref="User.Email"/> property when
    /// provided with valid arguments.
    /// </summary>
    /// <remarks>This test ensures that the <see cref="User.Email"/> property is set to the value passed as a
    /// parameter to the constructor, verifying proper initialization behavior.</remarks>
    [Fact]
    public void Ctor_GivenValidArguments_CorrectlyEmailProperties()
    {
        // Arrange
        //Preparation, initialization, configuration

        //Action on the CTU (component under test) / SUT (system under test)
        var user = new User(
            _inputId,
            _inputName,
            _inputIsActive,
            _inputEmail,
            _inputAzureObjectIdentifier);

        // Assert
        // Ensure expected behavior

        user.Email.Should().Be(_inputEmail,
            because: "ctor should correctly set the email passed as parameter");

    }

    /// <summary>
    /// Tests that the <see cref="User"/> constructor correctly initializes the <see cref="User.AzureObjectIdentifier"/> 
    /// property when provided with valid arguments.
    /// </summary>
    /// <remarks>This test ensures that the <see cref="User.AzureObjectIdentifier"/> property is set to the value 
    /// passed as a parameter to the constructor, verifying proper initialization behavior.</remarks>
    [Fact]
    public void Ctor_GivenValidArguments_CorrectlyAzureObjectIdentifierProperties() 
    {
        // Arrange
        //Preparation, initialization, configuration

        //Action on the CTU (component under test) / SUT (system under test)
        var user = new User(
            _inputId,
            _inputName,
            _inputIsActive,
            _inputEmail,
            _inputAzureObjectIdentifier);

        // Assert
        // Ensure expected behavior

        user.AzureObjectIdentifier.Should().Be(_inputAzureObjectIdentifier,
            because: "ctor should correctly set the Azure Object Identifier passed as parameter");
    }

    /// <summary>
    /// Tests that the <see cref="User"/> constructor correctly handles null Azure Object Identifier.
    /// </summary>
    [Fact]
    public void Ctor_GivenNullAzureObjectIdentifier_CorrectlyHandlesNull()
    {
        // Arrange & Act
        var user = new User(
            _inputId,
            _inputName,
            _inputIsActive,
            _inputEmail,
            azureObjectIdentifier: null);

        // Assert
        user.AzureObjectIdentifier.Should().BeNull(
            because: "ctor should correctly handle null Azure Object Identifier");
    }

    /// <summary>
    /// Tests that the <see cref="User"/> constructor correctly uses default parameter for Azure Object Identifier.
    /// </summary>
    [Fact]
    public void Ctor_WithoutAzureObjectIdentifier_DefaultsToNull()
    {
        // Arrange & Act
        var user = new User(
            _inputId,
            _inputName,
            _inputIsActive,
            _inputEmail);

        // Assert
        user.AzureObjectIdentifier.Should().BeNull(
            because: "ctor should default Azure Object Identifier to null when not provided");
    }

    /// <summary>
    /// Tests that the <see cref="User"/> constructor correctly initializes the <see cref="User.Roles"/> property when
    /// provided with valid arguments.
    /// </summary>
    /// <remarks>This test ensures that the <see cref="User.Roles"/> property is set to the value passed as a
    /// parameter to the constructor, verifying proper initialization behavior.</remarks>
    [Fact]
    public void Ctor_GivenValidArguments_CorrectlyRolesProperties()
    {
        // Arrange
        //Preparation, initialization, configuration

        //Action on the CTU (component under test) / SUT (system under test)
        var user = new User(
            _inputId,
            _inputName,
            _inputIsActive,
            _inputEmail,
            _inputRoles);

        // Assert
        // Ensure expected behavior

        user.Roles.Should().BeEquivalentTo(_inputRoles,
            because: "ctor should correctly set the roles.");
    }
}
