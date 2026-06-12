using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.User.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Unity.Application.User.Services
{
    /// <summary>
    /// The service interface for user-related operations in the application layer.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Asynchronously retrieve AvatarId associated with the user.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation and contains the AvatarId.
        /// </returns>
        Task<AvatarId> GetAvatarIdAsync();

        /// <summary>
        /// Saves the AvatarId associated with the user.
        /// </summary>
        /// <param name="avatarId">The AvatarId to save.</param>
        Task SaveAvatarIdAsync(AvatarId avatarId);
    }
}
