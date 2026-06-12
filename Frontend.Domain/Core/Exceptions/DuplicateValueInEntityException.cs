namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an attempt is made to create or update an entity with a value that
/// already exists for a unique property.
/// </summary>
/// <remarks>This exception is typically used to enforce uniqueness constraints on entity properties within a
/// domain. The exception message includes details about the entity, the property, and the duplicate value to help
/// identify the conflict.</remarks>
public class DuplicateValueInEntityException : DomainException
{
    private const string UserFriendlyMessageFormat = "{0} with the {1} '{2}' already exists.";

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateValueInEntityException"/> class with the specified entity
    /// name, property name, and duplicate value.
    /// </summary>
    /// <remarks>This exception is typically thrown when an attempt is made to add or update an entity with a
    /// value that violates a uniqueness constraint.</remarks>
    /// <param name="entityName">The name of the entity where the duplicate value was found. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="propertyName">The name of the property that contains the duplicate value. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="duplicateValue">The duplicate value that caused the exception. Cannot be <see langword="null"/>.</param>
    public DuplicateValueInEntityException(
        string entityName,
        string propertyName,
        string duplicateValue)
        : base(string.Format(
            UserFriendlyMessageFormat,
            entityName,
            propertyName,
            duplicateValue))
    {
    }
}
