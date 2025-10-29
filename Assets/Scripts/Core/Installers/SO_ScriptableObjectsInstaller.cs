using Asteroids.Core.ScriptableObjects.Collections;
using Asteroids.Core.ScriptableObjects.Configurations;
using ToolsACG.ApiCallersCreator.ScriptableObjects;
using ToolsACG.Core.ScriptableObjects.Collections;
using ToolsACG.Core.ScriptableObjects.Settings;
using ToolsACG.Pooling.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "ScriptableObjectsInstaller", menuName = "Installers/ScriptableObjectsInstaller")]
    public class SO_ScriptableObjectsInstaller : ScriptableObjectInstaller<SO_ScriptableObjectsInstaller>
    {
        #region Fields

        [Header("Collections")]
        [SerializeField] private SO_ShipsCollection _shipCollection;
        [SerializeField] private SO_BulletsCollection _bulletCollection;
        [SerializeField] private SO_AsteroidsCollection _asteroidCollection;
        [SerializeField] private SO_MusicCollection _musicCollection;

        // These configuration ScriptableObjects are unique, so they can be injected.
        // If you need multiple configurations, it's better to assign the references manually in the editor.
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

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            //SO Settings
            Container.Bind<SO_FactorySettings>().FromInstance(SO_FactorySettings.Instance).AsSingle();
            Container.Bind<SO_CursorSettings>().FromInstance(SO_CursorSettings.Instance).AsSingle();
            Container.Bind<SO_DebugSettings>().FromInstance(SO_DebugSettings.Instance).AsSingle();
            Container.Bind<SO_InputSettings>().FromInstance(SO_InputSettings.Instance).AsSingle();
            Container.Bind<SO_ScreenSettings>().FromInstance(SO_ScreenSettings.Instance).AsSingle();
            Container.Bind<SO_NetworkSettings>().FromInstance(SO_NetworkSettings.Instance).AsSingle();

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
        }

        #endregion
    }
}
