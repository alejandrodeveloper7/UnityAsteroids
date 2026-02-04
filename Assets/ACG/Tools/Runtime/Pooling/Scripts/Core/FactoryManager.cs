using ACG.Tools.Runtime.Pooling.Managers;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ACG.Tools.Runtime.Pooling.Core
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
            _instance.Initialize();
            Debug.Log($"- {nameof(FactoryManager)} - persistent manager initilized");
        }

        public void Initialize()
        {
            _audioSourcePoolManager.Initialize();
            _sound3DPoolManager.Initialize();
            _gameObjectPoolManager.Initialize();
        }

        public void Dispose()
        {
            _audioSourcePoolManager.Dispose();
            _sound3DPoolManager.Dispose();
            _gameObjectPoolManager.Dispose();
        }

        #endregion

        #region Container Pools management

        public void CreateContainerPools(List<SO_PooledGameObjectData> poolsData)
        {
            _gameObjectPoolManager.CreateContainerPools(poolsData);
        }

        public void DestroyContainerPools(List<SO_PooledGameObjectData> poolsData)
        {
            _gameObjectPoolManager.DestroyContainerPools(poolsData);
        }

        #endregion

        #region Instances Management

        public AudioSource Get2DAudioSourceInstance()
        {
            return _audioSourcePoolManager.Get2DAudioSourceInstance();
        }

        public GameObject Get3DSoundInstance()
        {
            return _sound3DPoolManager.Get3DSoundInstance();
        }

        public GameObject GetGameObjectInstance(SO_PooledGameObjectData pooledGameObjectData)
        {
            return _gameObjectPoolManager.GetGameObjectInstance(pooledGameObjectData.Prefab);
        }

        #endregion
    }
}