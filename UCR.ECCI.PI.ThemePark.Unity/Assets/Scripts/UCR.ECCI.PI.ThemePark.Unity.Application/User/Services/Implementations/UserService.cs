using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.User.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Domain.User.ValueObjects;
using Zenject;

namespace UCR.ECCI.PI.ThemePark.Unity.Application.User.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        [Inject]
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AvatarId> GetAvatarIdAsync()
        {
            return await _userRepository.GetAvatarIdAsync();
        }

        public async Task SaveAvatarIdAsync(AvatarId avatarId)
        {
            await _userRepository.SaveAvatarIdAsync(avatarId);
        }
    }
}
