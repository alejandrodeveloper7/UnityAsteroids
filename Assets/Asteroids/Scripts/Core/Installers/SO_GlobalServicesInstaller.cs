using ACG.Scripts.Services;
using Asteroids.Core.Services;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "GlobalServicesInstaller", menuName = "Installers/GlobalServices")]
    public class SO_GlobalServicesInstaller : ScriptableObjectInstaller<SO_GlobalServicesInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SettingsService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ScreenService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CursorService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SoundService>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerPrefsPersistentDataService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ContainerRuntimeDataService>().AsSingle().NonLazy();
            Container.Bind<RuntimeDataContainer>().AsSingle();

            Container.BindInterfacesAndSelfTo<DebugService>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<RandomUserNameGenerationService>().AsSingle().NonLazy();
        }

        #endregion
    }
}