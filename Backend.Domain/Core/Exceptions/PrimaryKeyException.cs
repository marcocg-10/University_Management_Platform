
namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
/// <summary>
/// Represents an exception that is thrown when a key constraint violation occurs during a database operation.
/// </summary>

public class PimaryKeyException : DomainException
{

    private const string PrimaryKeyViolationMessageFormat =
        "Some informations is missing, the '{0}' doesn't have its required identifier '{1}'";
    /// <summary>
    /// Initializes a new instance of the <see cref="PimaryKeyException"/> class.
    /// </summary>
    /// <param name="entityName">The name of the entity with the missing identifier.</param>
    /// <param name="key">The name of the missing identifier property.</param>
    
    public PimaryKeyException(
        string entityName,
        string key)
        : base(string.Format(
            PrimaryKeyViolationMessageFormat,
            entityName,
            key))
    {
    }
}
