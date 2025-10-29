using System.Collections.Generic;
using System.Linq;
using ToolsACG.Pooling.Models;
using ToolsACG.Pooling.Pools;
using ToolsACG.Pooling.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ToolsACG.Pooling.Managers
{
    public class GameObjectPoolManager : PoolManagerBase
    {
        #region Fields

        [Header("References")]
        private Transform _persistentPooledGameObjectsParentTransform;
        private Transform _scenePoolsGameObjectsParentTransform;
        private readonly DiContainer _container;

        [Header("Pools")]
        private List<GameObjectPool> _persistentGameObjectsPools = new();
        private List<GameObjectPool> _sceneGameObjectsPools = new();

        [Header("Data")]
        private readonly SO_FactorySettings _factorySettings;

        #endregion

        #region Constructors

        [Inject]
        public GameObjectPoolManager(DiContainer container, SO_FactorySettings settings)
        {
            _container = container;
            _factorySettings = settings;
            Initialize();
        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            if (_factorySettings.UsePersistentPools)
            {
                CreatePersistentPoolsParent();
                CreatePersistentPools();
            }

            if (_factorySettings.UseScenePools)
            {
                CreateScenePoolsParent();
                SuscribeListeners();
            }
        }

        public override void Dispose()
        {
            if (_factorySettings.UsePersistentPools)
            {
                DestroyPersistenPools();
                DestroyPersistentPoolsParent();
            }

            if (_factorySettings.UseScenePools)
            {
                DestroyAllScenePools();
                DestroyScenePoolsParent();
                UnsuscribeListeners();
            }
        }

        private void SuscribeListeners()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        private void UnsuscribeListeners()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        #endregion

        #region Event callbacks

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            foreach (var data in _factorySettings.ScenesPoolData)
                if (data.SceneName == scene.name)
                {
                    CreateScenePools(scene.name, data.PoolsData);
                    return;
                }
        }
        private void OnSceneUnloaded(Scene scene)
        {
            DestroyScenePool(scene.name);
        }

        #endregion        

        #region Persistent Pools Management

        private void CreatePersistentPoolsParent()
        {
            GameObject newGameObject = new(_factorySettings.PersistentPoolsParentName);
            GameObject.DontDestroyOnLoad(newGameObject);
            _persistentPooledGameObjectsParentTransform = newGameObject.transform;
            _persistentPooledGameObjectsParentTransform.position = _factorySettings.PersistentPoolsParentPosition;
        }
        private void DestroyPersistentPoolsParent()
        {
            GameObject.Destroy(_persistentPooledGameObjectsParentTransform.gameObject);
        }

        private void CreatePersistentPools()
        {
            if (_factorySettings.PersistentPoolsData == null)
                return;

            foreach (PoolData data in _factorySettings.PersistentPoolsData.PoolsData)
                _persistentGameObjectsPools.Add(_container.Instantiate<GameObjectPool>(new object[]  { data.Prefab, _persistentPooledGameObjectsParentTransform, data.InitialSize, data.Escalation, data.MaxSize, data.PoolName, "" }));

            Debug.Log($"- {typeof(GameObjectPoolManager).Name} - Persistend pools created");
        }
        private void DestroyPersistenPools()
        {
            foreach (var pool in _persistentGameObjectsPools)
                pool.DestroyPool();

            _persistentGameObjectsPools = null;

            Debug.Log($"- {typeof(GameObjectPoolManager).Name} - Persistent pools destroyed");
        }

        #endregion

        #region Scene Pools Management

        private void CreateScenePoolsParent()
        {
            GameObject newGameObject = new(_factorySettings.ScenePoolsParentName);
            GameObject.DontDestroyOnLoad(newGameObject);
            _scenePoolsGameObjectsParentTransform = newGameObject.transform;
            _scenePoolsGameObjectsParentTransform.position = _factorySettings.ScenePoolsParentPosition;
        }
        private void DestroyScenePoolsParent()
        {
            GameObject.Destroy(_scenePoolsGameObjectsParentTransform.gameObject);
        }

        private void CreateScenePools(string sceneName, List<PoolData> data)
        {
            foreach (PoolData poolData in data)
                _sceneGameObjectsPools.Add(_container.Instantiate<GameObjectPool>(new object[] { poolData.Prefab, _scenePoolsGameObjectsParentTransform, poolData.InitialSize, poolData.Escalation, poolData.MaxSize, poolData.PoolName, sceneName}));

            Debug.Log($"- {typeof(GameObjectPoolManager).Name} - {sceneName} scene pools created");
        }
        private void DestroyScenePool(string sceneName)
        {
            List<GameObjectPool> poolsToRemove = _sceneGameObjectsPools.Where(p => p.SceneName == sceneName).ToList();

            foreach (var pool in poolsToRemove)
                pool.DestroyPool();

            _sceneGameObjectsPools.RemoveAll(p => p.SceneName == sceneName);

            Debug.Log($"- {typeof(GameObjectPoolManager).Name} - {sceneName} scene pools destroyed");
        }
        private void DestroyAllScenePools()
        {
            foreach (var pool in _sceneGameObjectsPools)
                pool.DestroyPool();

            _sceneGameObjectsPools = null;

            Debug.Log($"- {typeof(GameObjectPoolManager).Name} - al scene pools destroyed");
        }

        #endregion

        #region Get Instance

        public GameObject GetGameObjectInstance(string poolName)
        {
            foreach (GameObjectPool pool in _persistentGameObjectsPools)
                if (pool.PoolName == poolName)
                    return pool.GetInstance();

            foreach (GameObjectPool pool in _sceneGameObjectsPools)
                if (pool.PoolName == poolName)
                    return pool.GetInstance();

            Debug.LogError($"- {typeof(GameObjectPoolManager).Name} - Pool with the name {poolName} not found");
            return null;
        }

        #endregion
    }
}