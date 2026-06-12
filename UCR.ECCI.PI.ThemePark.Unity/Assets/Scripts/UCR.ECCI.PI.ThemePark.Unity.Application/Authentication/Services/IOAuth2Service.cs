using System;
using System.Threading;
using System.Threading.Tasks;

namespace UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services
{
    public interface IOAuth2Service
    {
        Task<bool> SignInAsync(CancellationToken ct = default);          // launches browser, returns true if tokens stored
        Task<string?> GetValidAccessTokenAsync(CancellationToken ct = default); // refreshes if needed, returns access token or null
        Task SignOutAsync();
    }
}