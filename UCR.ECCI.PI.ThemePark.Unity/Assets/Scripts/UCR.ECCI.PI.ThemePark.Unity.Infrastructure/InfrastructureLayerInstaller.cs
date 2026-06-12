using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Zenject;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;
using System;                      // for Exception, etc. (optional but common)
using System.Net.Http;             // DelegatingHandler, HttpRequestMessage, HttpResponseMessage
using System.Threading;            // CancellationToken
using System.Threading.Tasks;      // Task / async
using UnityEngine;
using UCR.ECCI.PI.ThemePark.Unity.Domain.User.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.User.Repositories;


namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure
{
    public class InfrastructureLayerInstaller : Installer<InfrastructureLayerInstaller>
    {
        public override void InstallBindings()
        {
            // OAuth2 (PKCE)
            //Container.Bind<OAuthConfig>().AsSingle();
            //Container.Bind<ITokenStore>().To<PlayerPrefsTokenStore>().AsSingle();
            //Container.Bind<IOAuth2Service>().To<OAuth2Service>().AsSingle();

            // Access token source used by Kiota
            Container.Bind<IAccessTokenSource>().To<OAuthAccessTokenSource>().AsSingle();

            // Kiota auth provider
            Container.Bind<IAccessTokenProvider>().To<KiotaAccessTokenProvider>().AsSingle();
            Container.Bind<IAuthenticationProvider>().FromMethod(ctx =>
            {
                var atp = ctx.Container.Resolve<IAccessTokenProvider>();
                // Only adds Authorization when token is non-empty
                return new BaseBearerTokenAuthenticationProvider(atp);
            }).AsSingle();

            // Bind the request adapter
            // Bind the request adapter
            Container.Bind<IRequestAdapter>()
                .FromMethod(context =>
                {
                    var authProvider = context.Container.Resolve<IAuthenticationProvider>();

                    // nested logging handler to prove Authorization header is present
                    var httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));

                    return new HttpClientRequestAdapter(authProvider, httpClient: httpClient)
                    {
                        BaseUrl = "https://localhost:7119"
                    };
                })
                .AsSingle();

            // Bind the ApiClient
            Container.Bind<ApiClient>()
                .ToSelf()
                .AsTransient();

            Container.Bind<IBuildingRepository>()
                .To<KiotaBuildingRepository>()
                .AsTransient();

            Container.Bind<ILearningSpaceRepository>()
                .To<KiotaLearningSpaceRepository>()
                .AsTransient();

            Container.Bind<IInteractiveComponentRepository>()
                .To<KiotaInteractiveComponentRepository>()
                .AsTransient();

            Container.Bind<IUserRepository>()
                .To<KiotaUserRepository>()
                .AsTransient();
        }
    }
    
    // keep this in the same file
    class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler inner) : base(inner) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                Debug.Log($"[HTTP] {request.Method} {request.RequestUri}  Authorization: {auth.Scheme} len={auth.Parameter?.Length ?? 0}");
            }
            else
            {
                Debug.LogWarning($"[HTTP] {request.Method} {request.RequestUri}  (no Authorization header)");
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }

}