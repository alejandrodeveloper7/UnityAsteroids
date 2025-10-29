using ToolsACG.Pooling.Managers;
using UnityEngine;
using Zenject;

namespace ToolsACG.Pooling.Core
{
    public class FactoryManager
    {
        #region Fields

        [Header("Singleton")]
        private static FactoryManager _instance;
        public static FactoryManager Instance { get { return _instance; } }

        [Header("References")]
        private readonly AudioSourcePoolManager _audioSourcePoolManager;
        private readonly Sound3DPoolManager _sound3DPoolManager;
        private readonly GameObjectPoolManager _gameObjectPoolManager;

        #endregion

        #region Constructors

        [Inject]
        public FactoryManager(AudioSourcePoolManager audioSourcePoolManager, Sound3DPoolManager sound3DPoolManager, GameObjectPoolManager gameObjectPoolManager)
        {
            _audioSourcePoolManager = audioSourcePoolManager;
            _sound3DPoolManager = sound3DPoolManager;
            _gameObjectPoolManager = gameObjectPoolManager;
        }

        #endregion

        #region Initialization

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInit()
        {
            _instance = ProjectContext.Instance.Container.Resolve<FactoryManager>();
            Debug.Log($"- {nameof(FactoryManager)} - persistent manager initilized");
        }

        public void Dispose()
        {
            _audioSourcePoolManager.Dispose();
            _sound3DPoolManager.Dispose();
            _gameObjectPoolManager.Dispose();
        }

        #endregion

        #region Instances Management

        public AudioSource Get2DAudioSourceInstance()
           => _audioSourcePoolManager.Get2DAudioSourceInstance();

        public GameObject Get3DSoundInstance()
            => _sound3DPoolManager.Get3DSoundInstance();

        public GameObject GetGameObjectInstance(string poolName)
            => _gameObjectPoolManager.GetGameObjectInstance(poolName);

        #endregion
    }
}