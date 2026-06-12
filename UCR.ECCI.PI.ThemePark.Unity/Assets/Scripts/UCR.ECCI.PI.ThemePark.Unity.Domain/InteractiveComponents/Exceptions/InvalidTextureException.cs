using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Exceptions
{

    /// <summary>
    /// Exception thrown when a provided color for an InteractiveComponent is invalid.
    /// Inherits from <see cref="ValidationException"/> to represent a validation-level error.
    /// </summary>
    public class InvalidTextureException : ValidationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTextureException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message describing the reason for the exception.</param>
        public InvalidTextureException(string message)
            : base(message)
        {
        }
    }
}