using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;

/// <summary>
/// Provides predefined test data sets for use in testing user repository functionality.
/// </summary>
/// <remarks>This class contains collections of <see cref="User"/> objects representing different scenarios,  such
/// as an empty data set, a single entry, and multiple entries. These data sets are intended  to simplify the creation
/// of test cases for user repository operations.</remarks>
public class UserRepositoryTestData
{
    /// <summary>
    /// Gets an empty list of users.
    /// </summary>
    public List<User> EmptyData { get; } = [];

    /// <summary>
    /// Gets a predefined list containing a single user entry.
    /// </summary>
    public List<User> SingleEntryData { get; } = [
        new User(
            UserId.Create("9edf-8ac-8b32-bda"),
            UserName.Create("John Doe"),
            isActive:true,
            Email.Create("john.doe@universitry.com"),
            "12345678-1234-1234-1234-123456789abc")];

    /// <summary>
    /// Gets a predefined list of users containing multiple entries with unique Azure Object Identifiers.
    /// </summary>
    public List<User> MultipleEntryData { get; } = [
        new User(
            UserId.Create("9edf-8ac-8b32-bda"),
            UserName.Create("John Doe"),
            isActive:true,
            Email.Create("john.doe@universitry.com"),
            "12345678-1234-1234-1234-123456789abc"),
        new User(
            UserId.Create("9edf-8ac-8b32a"),
            UserName.Create("Jane Doe"),
            isActive:true,
            Email.Create("jane.doe@notuniversitry.com"),
            "87654321-4321-4321-4321-abcdef123456")
    ];
}

