using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Authentication;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces;
using Zenject;
using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation
{
    public class PresentationLayerInstaller : Installer<PresentationLayerInstaller>
    {
        public override void InstallBindings()
        {
            //Container.BindInterfacesAndSelfTo<AuthReady>().AsSingle();
            //Container.BindInterfacesTo<LoginInitializer>().AsSingle();
            Container.Bind<LearningSpaceSession>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("LearningSpaceSession")
                .AsSingle()
                .NonLazy();
        }
    }
}
