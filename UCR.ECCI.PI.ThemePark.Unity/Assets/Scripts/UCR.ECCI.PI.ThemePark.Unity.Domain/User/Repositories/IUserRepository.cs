using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.User.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.User.Repositories
{
    /// <summary>
    /// Defines the contract for user data operations in the domain layer.
    /// </summary>
    /// <remarks>
    /// This interface provides methods to access user entities from the data source.
    /// </remarks>
    public interface IUserRepository
    {
        /// <summary>
        /// Asynchronously retrieve AvatarId associated with the user.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation and contains the AvatarId.
        /// </returns>
        Task<AvatarId> GetAvatarIdAsync();

        /// <summary>
        /// Asynchronously saves the AvatarId associated with the user.
        /// </summary>
        /// <param name="avatarId">The AvatarId to save.</param>
        Task SaveAvatarIdAsync(AvatarId avatarId);
    }
}