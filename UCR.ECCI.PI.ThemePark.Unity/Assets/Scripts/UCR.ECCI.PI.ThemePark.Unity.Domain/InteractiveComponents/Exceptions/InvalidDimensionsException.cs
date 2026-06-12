using UCR.ECCI.PI.ThemePark.Unity.Domain.Core.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Exceptions
{

    /// <summary>
    /// Exception thrown when the provided dimensions for an InteractiveComponent are invalid.
    /// Inherits from <see cref="ValidationException"/> to represent a validation error.
    /// </summary>
    public class InvalidDimensionsException : ValidationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDimensionsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message describing the reason for the exception.</param>
        public InvalidDimensionsException(string message)
            : base(message)
        {
        }
    }
}