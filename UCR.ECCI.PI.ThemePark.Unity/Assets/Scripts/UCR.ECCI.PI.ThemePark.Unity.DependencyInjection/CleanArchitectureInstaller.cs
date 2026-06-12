using UCR.ECCI.PI.ThemePark.Unity.Application;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure;
using UCR.ECCI.PI.ThemePark.Unity.Presentation;
using UCR.ECCI.PI.ThemePark.Unity.Services;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;
using UnityEngine;
using Zenject;

namespace UCR.ECCI.PI.ThemePark.Unity.DependencyInjection
{
    public class CleanArchitectureInstaller : MonoInstaller
    {
        [SerializeField] private LoadingScreenView _loadingScreenPrefab;
        [SerializeField] private SceneTransitionRunner _runnerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<string>().WithId("SceneID").FromInstance("MAIN_SCENE");

            InfrastructureLayerInstaller.Install(Container);
            ApplicationLayerInstaller.Install(Container);
            PresentationLayerInstaller.Install(Container);
            Debug.Log($"[GlobalInstaller] installing bindings");
            // Create a singleton SceneTransitionRunner from prefab
            Container.Bind<SceneTransitionRunner>()
                .FromComponentInNewPrefab(_runnerPrefab)
                .AsSingle()
                .NonLazy();

            // Create a singleton LoadingScreenView from prefab
            Container.Bind<LoadingScreenView>()
                .FromComponentInNewPrefab(_loadingScreenPrefab)
                .AsSingle()
                .NonLazy();

            // Bind the transition service as a singleton
            Container.Bind<ISceneTransitionService>()
                .To<SceneTransitionService>()
                .AsSingle();
        }
    }
}

