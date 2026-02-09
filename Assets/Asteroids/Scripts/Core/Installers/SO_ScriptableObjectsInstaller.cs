using ACG.Scripts.ScriptableObjects.Collections;
using ACG.Scripts.ScriptableObjects.Configurations;
using ACG.Scripts.ScriptableObjects.Settings;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using Asteroids.Core.ScriptableObjects.Collections;
using Asteroids.Core.ScriptableObjects.Configurations;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "ScriptableObjectsInstaller", menuName = "Installers/ScriptableObjects")]
    public class SO_ScriptableObjectsInstaller : ScriptableObjectInstaller<SO_ScriptableObjectsInstaller>
    {
        #region Fields

        [Header("Collections")]
        [SerializeField] private SO_ShipsCollection _shipCollection;
        [SerializeField] private SO_BulletsCollection _bulletCollection;
        [SerializeField] private SO_AsteroidsCollection _asteroidCollection;
        [SerializeField] private SO_MusicCollection _musicCollection;

        // These configuration ScriptableObjects are unique, so they can be injected.
        // If you need multiple configurations, you cant inject them like this.
        // SO_StatsConfiguration is not injected because there are 2 different ones.

        [Header("Configurations")]
        [SerializeField] private SO_StageConfiguration _stageConfiguration;
        [SerializeField] private SO_LeaderboardConfiguration _leaderboardConfiguration;
        [SerializeField] private SO_ScenesConfiguration _scenesConfiguration;
        [SerializeField] private SO_FloatingTextConfiguration _floatingTextConfiguration;
        [SerializeField] private SO_BackgroundConfiguration _backgroundConfiguration;
        [SerializeField] private SO_StatsDisplayerConfiguration _statsDisplayConfiguration;
        [SerializeField] private SO_LeaderboardRowConfiguration _leaderboardRowConfiguration;
        [SerializeField] private SO_SelectorConfiguration _selectorConfiguration;
        [SerializeField] private SO_RandomUserNameGenerationConfiguration _randomUserNameConfiguration;

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            //SO Settings
            Container.Bind<SO_FactorySettings>().FromInstance(SO_FactorySettings.Instance).AsSingle();
            Container.Bind<SO_CursorSettings>().FromInstance(SO_CursorSettings.Instance).AsSingle();
            Container.Bind<SO_DebugSettings>().FromInstance(SO_DebugSettings.Instance).AsSingle();
            Container.Bind<SO_InputSettings>().FromInstance(SO_InputSettings.Instance).AsSingle();
            Container.Bind<SO_SoundSettings>().FromInstance(SO_SoundSettings.Instance).AsSingle();
            Container.Bind<SO_ScreenSettings>().FromInstance(SO_ScreenSettings.Instance).AsSingle();
            Container.Bind<SO_NetworkSettings>().FromInstance(SO_NetworkSettings.Instance).AsSingle();
            Container.Bind<SO_PauseSettings>().FromInstance(SO_PauseSettings.Instance).AsSingle();

            //SO Collections
            Container.Bind<SO_ShipsCollection>().FromInstance(_shipCollection).AsSingle();
            Container.Bind<SO_BulletsCollection>().FromInstance(_bulletCollection).AsSingle();
            Container.Bind<SO_AsteroidsCollection>().FromInstance(_asteroidCollection).AsSingle();
            Container.Bind<SO_MusicCollection>().FromInstance(_musicCollection).AsSingle();

            //SO Configurations
            Container.Bind<SO_StageConfiguration>().FromInstance(_stageConfiguration).AsSingle();
            Container.Bind<SO_LeaderboardConfiguration>().FromInstance(_leaderboardConfiguration).AsSingle();
            Container.Bind<SO_ScenesConfiguration>().FromInstance(_scenesConfiguration).AsSingle();
            Container.Bind<SO_FloatingTextConfiguration>().FromInstance(_floatingTextConfiguration).AsSingle();
            Container.Bind<SO_BackgroundConfiguration>().FromInstance(_backgroundConfiguration).AsSingle();
            Container.Bind<SO_StatsDisplayerConfiguration>().FromInstance(_statsDisplayConfiguration).AsSingle();
            Container.Bind<SO_LeaderboardRowConfiguration>().FromInstance(_leaderboardRowConfiguration).AsSingle();
            Container.Bind<SO_SelectorConfiguration>().FromInstance(_selectorConfiguration).AsSingle();
            Container.Bind<SO_RandomUserNameGenerationConfiguration>().FromInstance(_randomUserNameConfiguration);
        }

        #endregion
    }
}
