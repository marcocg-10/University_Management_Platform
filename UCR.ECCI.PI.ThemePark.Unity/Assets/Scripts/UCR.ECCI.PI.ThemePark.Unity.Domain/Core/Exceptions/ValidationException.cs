namespace UCR.ECCI.PI.ThemePark.Unity.Domain.Core.Exceptions
{

    /// <summary>
    /// Represents errors that occur during domain validation in the theme park domain.
    /// </summary>
    public class ValidationException : DomainException 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class with a user-friendly error message.
        /// </summary>
        /// <param name="usrFriendlyMessage">The message that describes the validation error.</param>
        public ValidationException(string usrFriendlyMessage) : base(usrFriendlyMessage) {}
    }
}