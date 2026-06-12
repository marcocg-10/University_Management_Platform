namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a foreign key constraint violation occurs during a database operation.
/// </summary>
/// <remarks>
/// This exception is typically thrown when an operation (such as delete or update) would violate a foreign key constraint,
/// for example, when attempting to delete a record that is referenced by other records. The exception message includes
/// details about the constraint and the related table to help identify the source of the violation.
/// </remarks>
public class ForeignKeyException : DomainException
{
    //constraint and table name
    private const string ForeignKeyViolationMessageFormat = "The operation could not be completed because '{0}' is linked to existing '{1}' record.";

    /// <summary>
    /// Initializes a new instance of the <see cref="ForeignKeyException"/> class with the specified entity
    /// name, action, and related entity.
    /// </summary>
    /// <param name="constraintName">The constraint violation</param>
    /// <param name="tableName">The table from the constraint</param>
    public ForeignKeyException(
        string constraintName,
        string tableName)
        : base(string.Format(
            ForeignKeyViolationMessageFormat,
            constraintName,
            tableName))
    {
    }
}
