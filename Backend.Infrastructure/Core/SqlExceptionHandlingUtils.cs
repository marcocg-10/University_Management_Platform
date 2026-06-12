using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

/// <summary>
/// Handles the execution of a SQL operation and provides specific exception handling for common SQL constraint
/// violations.
/// </summary>
/// <remarks>This method is designed to handle specific SQL constraint violations, such as unique constraint
/// violations and foreign key constraint violations,  and rethrows them as more descriptive exceptions.  <list
/// type="bullet"> <item> <description> If a unique constraint violation occurs, a <see
/// cref="DuplicateValueInEntityException"/> is thrown, providing details about the entity,  property, and duplicate
/// value that caused the violation. </description> </item> <item> <description> If a foreign key constraint violation
/// occurs, a <see cref="ForeignKeyException"/> is thrown, including the name of the violated  constraint and the
/// affected table. </description> </item> </list> Any other exceptions are not handled and will propagate
/// as-is.</remarks>
internal static partial class SqlExceptionHandlingUtils
{
    internal static async Task HandleSqlOperationAsync(Func<Task> sqlOperation)
    {
        try
        {
            await sqlOperation();
        }
        // Handle unique constraint violation.
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
            && sqlEx.Number == SqlExceptionError.UniqueConstraintViolation.Number)
        {
            var match = UniqueConstraintViolationRegex().Match(sqlEx.Message);
            if (!match.Success)
            {
                throw;
            }

            var constraintName = match.Groups[1].Value;
            var objectName = match.Groups[2].Value;
            var duplicateValue = match.Groups[3].Value;

            throw new DuplicateValueInEntityException(
                entityName: objectName,
                propertyName: constraintName,
                duplicateValue: duplicateValue);
        }
        // Handle foreign key constraint violation.
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
            && sqlEx.Number == SqlExceptionError.ForeignKeyViolation.Number)
        {
            var match = ForeignKeyViolationRegex().Match(sqlEx.Message);
            if (!match.Success)
            {
                throw;
            }

            var constraintName = match.Groups[2].Value;
            var tableName = match.Groups[4].Value;

            // TODO: Consider creating an exception inherited from DomainException for foreign key violations.
            throw new ForeignKeyException(
                   constraintName: constraintName,
                   tableName: tableName
                );
        }

        //Handle key constraint violation.
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
            && sqlEx.Number == SqlExceptionError.PrimaryKeyViolation.Number)
        {
            // Example message: "Cannot insert the value NULL into column 'Id', table 'ThemePark.dbo.Users'; column does not allow nulls. INSERT fails."
            var message = sqlEx.Message;
            var match = PrimaryKeyViolationRegex().Match(sqlEx.Message);
            {
                throw;
            }

            var entityName = match.Groups[1].Value;
            var key = match.Groups[2].Value;
            throw new PimaryKeyException(
                entityName: entityName,
                key: key);
        }
    }

    /// <summary>
    /// Regex pattern to match UNIQUE KEY constraint violation messages from SQL Server.
    /// </summary>
    /// <remarks>
    /// Example message: "Violation of UNIQUE KEY constraint 'UNIQUE_Name'. Cannot insert duplicate key in object 'dbo.Roles'. The duplicate key value is (Admin)."
    /// Groups: 1=ConstraintName, 2=TableName, 3=DuplicateValue
    /// </remarks>
    [GeneratedRegex("Violation of UNIQUE KEY constraint '([^']+)'\\..*?object '(?:[^.]+\\.)?([^']+)'\\..*?The duplicate key value is \\(([^)]+)\\)", RegexOptions.IgnoreCase)]
    private static partial Regex UniqueConstraintViolationRegex();

    /// <summary>
    /// Regex pattern to match FOREIGN KEY constraint violation messages from SQL Server.
    /// </summary>
    /// <remarks>
    /// Example message:
    /// Groups: 1=QuoteChar, 2=ConstraintName, 3=QuoteChar, 4=TableName
    /// The conflict occurred in database 'ThemePark', table 'Buildings.Building', column 'Id'."
    ///
    /// Capture groups:
    /// 1 = Opening quote for the constraint name
    /// 2 = ConstraintName
    /// 3 = Opening quote for the table name
    /// 4 = TableName
    /// </remarks>
    [GeneratedRegex(
        @"The\s+(?:INSERT|UPDATE|DELETE)\s+statement\s+conflicted\s+with\s+the\s+(?:FOREIGN KEY|REFERENCE)\s+constraint\s+(['""])([^'""]+)\1.*?(?:table|object)\s+(['""])(?:[^.'""]+\.)?([^'""]+)\3",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant)]
    private static partial Regex ForeignKeyViolationRegex();


    /// <summary>
    /// Regex pattern to match PRIMARY KEY constraint violation messages from SQL Server.
    /// </summary>
    /// <remarks>
    /// Example message: "There is not a 'ID'. The conflict occurred in database 'ThemePark', entity 'User'."
    /// Groups: 1=entityName, 2=Key
    /// </remarks>
    [GeneratedRegex("The (?:ENTITY|REFERENCE) statement conflicted with the (?:KEY|REFERENCE) constraint '([^']+)'\\..*?table '(?:[^.]+\\.)?([^']+)'", RegexOptions.IgnoreCase)]
    private static partial Regex PrimaryKeyViolationRegex();

}