# 📑 Testing Convention

## ⚒️ Tools

- **xUnit**: This tool will be the standard framework for the different tests across the development of the project.

## 📝 Tests Format

The format for the different test methods will be the following:

```plaintext
[Test type] public void Method_Scenario_ExpectedResult()
{
    // Arrange
    <Vars>
    // Act
    <Call methods>
    // Assert
    <Verify returned values>
}

```

**✍️Unit test example:**

```plaintext
[Fact]
public void Ctor_GivenValidArguments_CorrectlySetsIDProperties()
{
    //Arrange (Define variables/parameters)
    string inputId = "123456789";
    string inputUniversityId = "U20230001";
    string inputName = "John Doe";
    bool inputIsActive = true;
    string inputEmail = "john.doe@university.com";

    //Act (Call methods, use the parameters)
    var student = new Student(
        inputId,
        inputUniversityId,
        inputName,
        inputIsActive,
        inputEmail);

    //Assert  (Verify the returned values)
    //Assert.Equal(inputId, student.Id);

    student.Id.Should().Be(inputId, 
        because: "ctor should correctly set the ID passed as parameter");
}
```

**✔️TDD example**

```plaintext
[Theory]
[InlineData("test")]
[InlineData("test@")]
[InlineData("test@email")]
[InlineData("@email.com")]
[InlineData("@")]
[InlineData(".@.")]
public void TryCreate_GivenInvalidEmail_ReturnsFalse(string inputEmail)
{
    // Arrange

    // Act
    bool result = Email.TryCreate(inputEmail, out _);

    // Assert
    result.Should().BeFalse(because: "input is an invalid email address");
}
```

## 🧠Considerations

- Each developer should test their own integration before a pull request.

- Use TDD whenever applicable.

- Prefer `[Fact]` for single, specific test without parameters.

- Prefer `[Theory]` when testing the same logic with multiple input combinations.

- Avoid testing trivial logic.

- Use the specified format presented above.