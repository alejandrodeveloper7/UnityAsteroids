using ACG.Scripts.Services;
using Asteroids.Core.Services;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "ServicesInstaller", menuName = "Installers/Services")]
    public class SO_ServicesInstaller : ScriptableObjectInstaller<SO_ServicesInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<ISettingsService>().To<SettingsService>().AsSingle().NonLazy();
            Container.Bind<IScreenService>().To<ScreenService>().AsSingle().NonLazy();
            Container.Bind<ICursorService>().To<CursorService>().AsSingle().NonLazy();
            Container.Bind<ISoundService>().To<SoundService>().AsSingle().NonLazy();

            Container.Bind<IPersistentDataService>().To<PlayerPrefsPersistentDataService>().AsSingle().NonLazy();
            Container.Bind<IContainerRuntimeDataService>().To<ContainerRuntimeDataService>().AsSingle().NonLazy();
            Container.Bind<RuntimeDataContainer>().AsSingle();

            Container.Bind<IDebugService>().To<DebugService>().AsSingle().NonLazy();

            Container.Bind<IUserNameGenerationService>().To<RandomUserNameGenerationService>().AsSingle().NonLazy();
        }

        #endregion
    }
}