# 📏 Standards & Quality Guidelines

> _This document defines the **standards** and **quality guidelines** to be applied across our projects.  
> Adhering to these guidelines ensures **consistency**, **maintainability**, and **high-quality outcomes**._

See the [References](#references) section for detailed linting and style guidelines.

---

## 🚀 Technologies

### Programming languages
- **C#**  
  Used as the **primary** language for building business logic, APIs, and application services.

---

### Frameworks
- **.NET 9.0**  
  Cross-platform, open-source framework for building scalable and high-performance applications.
  > Serves as the **core runtime and ecosystem** for application execution, ensuring performance and maintainability.

---

### **NuGet Packages**
- **Sonar Analyzer CSharp (v10.15.0.120848)**  
  A static code analysis tool that detects bugs, vulnerabilities, and code smells.  
  > Helps **enforce clean code principles** and **improve code quality**.

- **Sonar Analyzer CSharp Styling (v10.15.0.120848)**  
  Provides style and formatting rules aligned with Sonar guidelines.  
  > Helps to maintain a **consistent coding style** across the project.

- **FluentAssertions (v8.6.0)**  
  A library that makes unit test assertions more readable and expressive.  
  > Improves **test clarity** and **productivity**.

- **xUnit (v2.9.2)**  
  A popular open-source testing framework for .NET.  
  > Used to **write and execute automated tests**.

- **Microsoft.NET.Test.Sdk (v17.12.0)**  
  Provides test framework integration and execution support in .NET projects.  
  > Provides **seamless integration** of tests into CI/CD pipelines.

- **coverlet.collector (v6.0.2)**  
  A code coverage tool that integrates with .NET test frameworks.  
  > Measures **test coverage** and helps maintain **testing standards**.

- **Microsoft.EntityFrameworkCore.SqlServer (v9.0.9)**  
  An Object-Relational Mapper (ORM) for .NET with SQL Server support.  
  > Maps **data models** to database tables and simplifies**data access**.

---

### **Database**
- **Microsoft SQL Server**  
  A powerful relational database management system developed by Microsoft.  
  > Primary **database engine**, ensuring **data integrity, scalability, and secure storage**.

- **SQL Server Management Studio (v21)**  
  A desktop application developed by Microsoft for managing and administering **Microsoft SQL Server** databases.  
  > Allows developers and database administrators to execute SQL queries, manage database objects, configure security, monitor performance, and handle backups.
    In this project, SSMS will be used for **direct interaction with the SQL Server database**, running queries, and managing the schema and data.

- **DBeaver (v.25.2.0)**  
  A universal database management tool that supports multiple database systems including SQL Server, MySQL, PostgreSQL, Oracle, and SQLite.  
  > Provides a single interface to execute SQL queries, browse and edit data, manage database objects, and perform data import/export. 
    In this project, DBeaver will be used for **cross-database management**, easier data exploration, and testing queries across different database environments.

### **AI Agent**
- **Microsoft Copilot**  
  An AI-powered tool integrated into development environments like Visual Studio.  
  > Assists developers by providing **context-aware code suggestions**, improving **productivity** and **code quality**. Also in the context of
    the AI Agent, GitHub Copilot can be used in conjunction with the Model Context Protocol (MCP) to enhance its capabilities and connect it to 
    external data sources and tools. 

- **Microsoft extensions hosting (v 9.0.9)**  
  Hosting and startup infrastructure for applications. Is a .NET library and framework for creating and managing generic host applications.
  > It provides a way to configure and run applications with features like dependency injection, configuration management, and logging. 
    In the context of the AI Agent, Microsoft.Extensions.Hosting can be used to create a robust and scalable application that leverages AI capabilities.

- **System threading Channels (v 9.0.9)**  
  Provides a set of synchronization primitives for building producer-consumer scenarios and managing data flow between threads.
  > In the context of the AI Agent, System.Threading.Channels can be used to implement efficient and thread-safe communication between different components of the AI system,
    such as data processing, model inference, and result handling.
    
- **ModelContextProtocol (0.3.0-preview.4)**  
  An open-source protocol that enables communication between AI models and external systems.
  > In the context of the AI Agent, MCP can be used to connect the AI model with other tools and services, allowing it to access external data sources, 
    perform complex computations, and integrate with existing workflows.

- **ModelContextProtocol.ASPNetCore (0.3.0-preview.4)**  
  ASP.NET Core extensions for the C# Model Context Protocol (MCP) SDK.
  > In the context of the AI Agent, ModelContextProtocol.AspNetCore can be used to build web-based applications that leverage AI capabilities, 
    allowing users to interact with the AI model through a web interface and access its features from anywhere.

## 📝 Naming Conventions

 _Consistent naming conventions improve **readability**, **maintainability**, and **team collaboration**.  
Following these rules ensures that the codebase remains **clean** and **understandable** for all developers._

---

### Variable Naming
- Use **camelCase** for local variables and method parameters.  
  > Example: `userName`, `totalAmount`.

- Names must be **descriptive and meaningful**.  
  > Avoid vague names like `x` or `data`. Use clear names like `customerList` or `orderDate`.

- **Single-letter names** are only allowed for loop counters.  
  > Example: `i`, `j` in `for` loops.

- Boolean variables should be **prefixed** with `is`, `has`, `can`, or `should`.  
  > Example: `isActive`, `hasChildren`, `canExecute`, `shouldRetry`.

---

### Function Naming
- Use **PascalCase** for method names.  
  > Example: `CalculateTotal`, `GetUserById`.

- Method names should be **verbs** or **verb phrases** to express action.  
  > Example: ✅ `ProcessPayment` ❌ `PaymentProcessor`.

- Avoid using abbreviations unless they are **universally understood**.  
  > Example: ✅ `GetUserById` ❌ `GetUsrById`.

- Append the **`Async` suffix** for asynchronous methods.  
  > Example: `FetchDataAsync`, `SendEmailAsync`.

---

### File Naming
- Use **PascalCase** for file names.  
  > Example: `UserService.cs`, `OrderController.cs`.

- File names must **match the primary class or interface** they contain.  
  > Example: A class named `UserService` must be inside `UserService.cs`.

- Test files must follow the **`<ClassName>Test.cs`** pattern.  
  > Example: A test for `UserService` must be named `UserServiceTest.cs`.

---

## 📖 Documentation

_Clear and consistent documentation ensures that our codebase is **easy to understand**, **maintain**, and **extend**.  
Good documentation helps both current and future developers quickly grasp the **purpose**, **usage**, and **implementation details** of the code._

---

### Class and Function Documentation
- All **public classes, methods, and APIs** must include **docstrings/comments** that explain:
  - The **purpose** of the class or method.
  - The **parameters** (inputs) and their types.
  - The **return value** (if any).
  - Possible **exceptions** that might be thrown.

Example of documenting a class in C#:

```csharp
/// <summary>
/// Represents a user in the system.
/// This class handles basic user properties and authentication logic.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Checks if the user credentials are valid.
    /// </summary>
    /// <param name="username">The username input.</param>
    /// <param name="password">The password input.</param>
    /// <returns>True if the credentials match, otherwise false.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when credentials are invalid.
    /// </exception>
    public bool Authenticate(string username, string password)
    {
        // Check if username and password match stored credentials
        if (username == "adminUser" && password == "securePassword123")
            return true;

        throw new UnauthorizedAccessException("Invalid credentials provided.");
    }
}
```

---

## Solution Structure

_Maintaining a well-organized solution structure is essential for **readability**, **scalability**, and **ease of maintenance**.  
A consistent folder and layer organization helps developers quickly locate code, understand dependencies, and enforce separation of concerns._

---

### 📁 General Guidelines
- Follow a **consistent folder structure** for all projects:
  - Separate **source code**, **unit tests**, and **resources**.
  - Example folders: `src/`, `tests/`, `resources/`.
  
- Use **meaningful names** for folders and files to clearly reflect their purpose and content.  
  > Avoid vague names like `stuff` or `misc`.

- Adhere to the **standard .NET project structure** with a **layered architecture**, ensuring modularity and maintainability.

---

### Layered Architecture

- **Presentation Layer**  
  > Handles the **user interface**, **controllers**, and **request/response processing**.  
  > This layer is responsible for interacting with clients or APIs and should **not contain business logic**.

- **Application Layer**  
  > Contains the **business logic**, **application services**, and **use case orchestration**.  
  > Coordinates actions between the domain and infrastructure layers without implementing domain rules directly.

- **Domain Layer**  
  > Defines **core entities**, **value objects**, and **domain services**.  
  > Represents the **business rules** and core concepts of the system.  
  > This layer should be **independent of infrastructure and presentation details**.

- **Infrastructure Layer**  
  > Manages **data access**, **repositories**, and **integration with external services**.  
  > Encapsulates all technical implementation details, allowing other layers to remain agnostic of the infrastructure.

---

### Layer Interaction Rules
- Each layer should only interact with the **layer directly below it**, enforcing a **clear separation of concerns**.
- Avoid **cross-layer dependencies** to maintain modularity and simplify testing and maintenance.

---

## Code Quality

> _High-quality code ensures **maintainability**, **readability**, and **robustness**.  
> Following these conventions reduces technical debt, improves team collaboration, and helps deliver reliable software._

---

### Formatting
- Follow **consistent indentation**: 4 spaces per indentation level.  
- Use **spaces around operators** and after commas for readability.  
  > Example: `int total = a + b;` instead of `int total=a+b;`.
- **Limit line length** to 120 characters.  
  > If a line exceeds this limit, break it into **multiple logical lines** to improve readability.

---

### Function Length
- Keep **functions/methods short** and focused on a single task.  
- Aim for a **maximum of 20-30 lines**.  
- Small, focused methods improve **testability** and **comprehension**.

---

### File Length
- Limit **file length** to **100-150 lines**.  
- If a file exceeds this, **refactor** or **split** it into smaller, logically grouped files.  
- Organized files make the project easier to navigate and maintain.

---

### ⚠️ Error Handling
- Always use **typed exceptions** and provide **meaningful error messages**.  
- Never expose **stack traces** or **sensitive information** in user-facing errors.  
- Apply **centralized error handling** wherever appropriate to maintain consistency.

---

### Logging
- Implement logging for **critical operations**, **errors**, and **important state changes**.  
- Avoid logging **sensitive information** such as passwords or personal data.  
- Use a **consistent logging framework** across the entire project to standardize log formats and levels.

---

## Testing & TDD

Follow the convention established in the [Test-Convention](./Test-convention.md) document.

---

## Code Reviews, Version Control, and Branching Strategy

Follow the convention established in the [Git-Convention](./GITFLOW-Convention.md) document.

---

## Security

Follow the convention established in the pending Security-Convention document.

---

## Performance

> _Optimizing performance ensures the application is **fast**, **efficient**, and **scalable**.  
> This is especially important for critical paths and high-load operations._

- Optimize code for **performance**, particularly in **time-sensitive or frequently executed paths**.  
- Avoid **unnecessary computations**, redundant operations, and excessive **memory allocations**.  
- Favor **efficient algorithms** and data structures to improve overall system responsiveness.

---

## 🔄 Continuous Integration / Continuous Deployment (CI/CD)

> _A robust CI/CD pipeline ensures **code quality**, **reliability**, and **fast delivery** of features._

- Every commit should automatically trigger:
  - **Automated builds** to verify compilation and dependencies.  
  - **Linting and static code analysis** to enforce coding standards.  
  - **Automated tests** to validate functionality and catch regressions.

- Releases should be **automated**, following a **consistent, repeatable process** to reduce human errors.  
- Deployments must be **monitored post-release** for performance issues, errors, or regressions, ensuring quick detection and mitigation.

---

## References

- [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)  
  > Comprehensive guidelines for writing clean, readable, and maintainable C# code.

- [Sonar Analyzer CSharp](https://rules.sonarsource.com/csharp/)  
  > Code quality rules and best practices to enforce **robustness**, **security**, and **consistency** in C# projects.

