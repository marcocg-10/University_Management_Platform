namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

internal class SqlExceptionError
{
    
    /// <summary>
    /// Represents a SQL error indicating that a column name is invalid.
    /// </summary>
    /// <remarks>This error corresponds to SQL Server error code 207, which is typically raised when a query
    /// references a column that does not exist in the specified table or view.</remarks>
    internal static readonly SqlExceptionError InvalidColumnName = new(207);

    /// <summary>
    /// Represents a SQL exception error for a unique constraint violation.
    /// </summary>
    /// <remarks>This error corresponds to SQL Server error code 2627, which occurs when an operation 
    /// attempts to insert or update a row that violates a unique constraint or primary key constraint.</remarks>
    internal static readonly SqlExceptionError UniqueConstraintViolation = new(2627);

    /// <summary>
    /// Represents a SQL error indicating a foreign key constraint violation.
    /// </summary>
    /// <remarks>
    /// This error typically occurs when an operation attempts to delete or update a record that is
    /// referenced by a foreign key in another table, or when an insert operation violates a foreign key
    /// constraint. For more information, see 
    /// <see href="https://stackoverflow.com/questions/8057617/sql-exception-error-547-foreign-key-constraint-violations-while-attempting-a" />.
    /// </remarks>
    internal static readonly SqlExceptionError ForeignKeyViolation = new(547);

    /// <summary>
    /// Represents a SQL error indicating duplicate key (row insertion attempt) in unique index.
    /// </summary>
    /// <remarks>This error occurs when inserting a duplicate value in a column with a unique index.</remarks>
    internal static readonly SqlExceptionError DuplicateKeyInUniqueIndex = new(2601);

    /// <summary>
    /// Represents a SQL error indicating failed connection to server.
    /// </summary>
    /// <remarks>This error occurs when the application cannot establish a connection to the SQL Server instance.</remarks>
    internal static readonly SqlExceptionError CannotOpenUserDefaultDatabase = new(4064);

    /// <summary>
    /// Represents a SQL error indicating the database is not accessible.
    /// </summary>
    /// <remarks>This error occurs when the specified database cannot be accessed or does not exist.</remarks>
    internal static readonly SqlExceptionError DatabaseNotAccessible = new(4060);

    /// <summary>
    /// Represents a SQL error indicating conversion failed.
    /// </summary>
    /// <remarks>This error occurs when data type conversion fails (e.g., converting string to int).</remarks>
    internal static readonly SqlExceptionError ConversionFailed = new(245);

    /// <summary>
    /// Represents a SQL error indicating null value cannot be inserted.
    /// </summary>
    /// <remarks>This error occurs when attempting to insert NULL into a NOT NULL column.</remarks>
    internal static readonly SqlExceptionError NullValueNotAllowed = new(515);

    /// <summary>
    /// Represents a SQL error indicating primary key violation.
    /// </summary>
    /// <remarks> This error occurs when the entity or attribute doesn't have its primary key</remarks>
    internal static readonly SqlExceptionError PrimaryKeyViolation = new(224);

    public int Number { get; }

    private SqlExceptionError(int number)
    {
        Number = number;
    }
}
