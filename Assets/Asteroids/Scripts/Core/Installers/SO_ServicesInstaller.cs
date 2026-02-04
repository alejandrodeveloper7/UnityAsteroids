using ACG.Scripts.Services;
using Asteroids.Core.ScriptableObjects.Configurations;
using Asteroids.Core.Services;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "ServicesInstaller", menuName = "Installers/Services")]
    public class SO_ServicesInstaller : ScriptableObjectInstaller<SO_ServicesInstaller>
    {
        #region Fields

        [Header("Configurations")]
        [SerializeField] private SO_UserNameConfiguration _userNameConfiguration;

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<IScreenService>().To<ScreenService>().AsSingle().NonLazy();
            Container.Bind<ISettingsService>().To<SettingsService>().AsSingle().NonLazy();
            Container.Bind<ICursorService>().To<CursorService>().AsSingle().NonLazy();
            Container.Bind<IDebugService>().To<DebugService>().AsSingle().NonLazy();
            Container.Bind<IPersistentDataService>().To<PlayerPrefsPersistentDataService>().AsSingle().NonLazy();
            Container.Bind<IUserNameGenerationService>().To<RandomUserNameGenerationService>().AsSingle()
                .WithArguments(_userNameConfiguration.NamePosibleCharacters, _userNameConfiguration.BaseName, _userNameConfiguration.NameRandomCharactersAmount).NonLazy();

            Container.Bind<IContainerRuntimeDataService>().To<ContainerRuntimeDataService>().AsSingle().NonLazy();
            Container.Bind<RuntimeDataContainer>().AsSingle();
        }

        #endregion
    }
}