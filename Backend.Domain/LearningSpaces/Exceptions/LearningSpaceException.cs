using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;

/// <summary>
/// Represents an exception that occurs within the learning space domain.
/// </summary>
/// <remarks>This exception is typically thrown to indicate errors specific to the learning space domain. It
/// extends the <see cref="DomainException"/> class to provide additional context for domain-related issues.</remarks>
public class LearningSpaceException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public LearningSpaceException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceException"/> class with a specified error message and
    /// a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> 
    /// if no inner exception is specified.</param>
    public LearningSpaceException(string message, Exception innerException) : base(message, innerException) { }
}
