using UnityEngine;
using Zenject;

using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.Authentication
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private OAuthConfig _oauthConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_oauthConfig)
                .AsSingle();
            
            Container.Bind<ITokenStore>()
                .To<PlayerPrefsTokenStore>()
                .AsSingle()
                .NonLazy();
            
            // Global auth state
            Container.Bind<IAuthReady>().To<AuthReady>().AsSingle().NonLazy();

            Container.Bind<IOAuth2Service>()
                .To<OAuth2Service>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<LoginInitializer>()
                .AsSingle()
                .NonLazy();
        }
    }
}
