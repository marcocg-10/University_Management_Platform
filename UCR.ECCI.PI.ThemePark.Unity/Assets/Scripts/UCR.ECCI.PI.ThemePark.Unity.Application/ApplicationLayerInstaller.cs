using UCR.ECCI.PI.ThemePark.Unity.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Unity.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Unity.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Unity.Application.Buildings.Services.Implementations;
using Zenject;
using UCR.ECCI.PI.ThemePark.Unity.Application.User.Services;
using UCR.ECCI.PI.ThemePark.Unity.Application.User.Services.Implementations;

namespace UCR.ECCI.PI.ThemePark.Unity.Application
{
    public class ApplicationLayerInstaller : Installer<ApplicationLayerInstaller>
    {
        public override void InstallBindings()
        {
            // Bind application services and interfaces
            Container.Bind<IBuildingService>()
                .To<BuildingService>()
                .AsTransient();
            Container.Bind<ILearningSpaceService>()
                .To<LearningSpaceService>()
                .AsTransient();
            Container.Bind<IInteractiveComponentService>()
                .To<InteractiveComponentService>()
                .AsTransient();
            Container.Bind<IUserService>()
                .To<UserService>()
                .AsTransient();

        }
    }

}