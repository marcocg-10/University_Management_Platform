// File: Infrastructure/OAuth/ITokenStore.cs
using System.Threading.Tasks;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    public interface ITokenStore
    {
        Task SaveAsync(OAuthTokenResponse t);
        Task<OAuthTokenResponse?> LoadAsync();
        Task ClearAsync();
    }
}
