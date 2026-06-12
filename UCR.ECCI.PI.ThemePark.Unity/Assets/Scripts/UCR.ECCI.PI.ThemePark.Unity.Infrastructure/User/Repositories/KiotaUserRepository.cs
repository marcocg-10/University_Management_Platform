using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.User.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Domain.User.ValueObjects;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota.Users.Avatar;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.User.Repositories
{
    /// <summary>
    /// Repository implementation for managing User data.
    /// </summary>
    /// <remarks>
    /// Uses Kiota-generated API client to perform operations.
    /// </remarks>
    internal class KiotaUserRepository : IUserRepository
    {
        /// <summary>
        /// Represents the API client used to interact with external services.
        /// </summary>
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="apiClient">The API client used to interact with the remote service.</param>
        public KiotaUserRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AvatarId> GetAvatarIdAsync()
        {
            return new AvatarId("692fa303bcfe438b1885557b");
        }

        public async Task SaveAvatarIdAsync(AvatarId avatarId)
        {

            var avatarRequestBuilder = new AvatarIdDto
            {
                AvatarId = avatarId.Value
            };

            // Endpoint to call the API and save the AvatarId.
            try
            {
                var response = await _apiClient.Users.Avatar.PutAsync(avatarRequestBuilder);
            }
            catch (ErrorPersistAvatarIdResponse ex)
            {
                // Handle exceptions as needed
                throw new System.Exception("Error saving AvatarId", ex);
            }
            catch (ExceptionResult ex)
            {
                // Handle exceptions as needed
                throw new System.Exception("General error saving AvatarId", ex);
            }
            catch (System.Exception ex)
            {
                // Handle exceptions as needed
                throw new System.Exception("Unexpected error saving AvatarId", ex);
            }


        }
    }

}