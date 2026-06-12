// Presentation/Startup/LoginInitializer.cs
using Zenject;
using UnityEngine;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Authentication;

public sealed class LoginInitializer : IInitializable
{
    private readonly IOAuth2Service _oauth;
    private readonly IAuthReady _authReady;

    public LoginInitializer(IOAuth2Service oauth, IAuthReady authReady)
    { _oauth = oauth; _authReady = authReady; }

    public async void Initialize()
    {
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var ok = await _oauth.SignInAsync();

        //_authReady.SignalReady(); // now compiles
        //Debug.Log("[LoginInitializer] Auth ready.");

        if (!ok)
        {
            Debug.LogError("[LoginInitializer] Interactive sign-in failed.");
            _authReady.SignalReady();
            return;
        }

        var token = await _oauth.GetValidAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("[LoginInitializer] No valid access token after sign-in.");
            _authReady.SignalReady();
            return;
        }

        Debug.Log("[LoginInitializer] Authentication succeeded");

        _authReady.SignalReady();
    }
}
