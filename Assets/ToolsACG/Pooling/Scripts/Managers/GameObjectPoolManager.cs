using System.Collections.Generic;
using System.Linq;
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
        private Transform _containersPoolsGameObjectsParentTransform;
        [Space]
        private readonly DiContainer _container;

        [Header("Pools")]
        private List<GameObjectPool> _persistentGameObjectsPools = new();
        private List<GameObjectPool> _sceneGameObjectsPools = new();
        private List<GameObjectPool> _containerObjectsPools = new();

        [Header("Data")]
        private readonly SO_FactorySettings _factorySettings;

        #endregion

        #region Constructors

        [Inject]
        public GameObjectPoolManager(DiContainer container, SO_FactorySettings settings)
        {
            _container = container;
            _factorySettings = settings;
        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            CreateContainersPoolsParent();
         
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

            DestroyAllContainerPools();
            DestroyContainersPoolsParent();
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
                    CreateScenePools(data);
                    return;
                }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            DestroyScenePools(scene.name);
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
            foreach (SO_PooledGameObjectData data in _factorySettings.PersistentPoolsData)
                _persistentGameObjectsPools.Add(_container.Instantiate<GameObjectPool>(new object[] { data, _persistentPooledGameObjectsParentTransform, "" }));

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

        private void CreateScenePools(SO_ScenePoolData sceneData)
        {
            foreach (SO_PooledGameObjectData gameObjectData in sceneData.GameObjectesData)
                _sceneGameObjectsPools.Add(_container.Instantiate<GameObjectPool>(new object[] { gameObjectData, _scenePoolsGameObjectsParentTransform, sceneData.SceneName }));

            Debug.Log($"- {typeof(GameObjectPoolManager).Name} - {sceneData.SceneName} scene pools created");
        }

        private void DestroyScenePools(string sceneName)
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

        #region Container pools Management

        private void CreateContainersPoolsParent()
        {
            GameObject newGameObject = new(_factorySettings.ContainerPoolsParentName);
            GameObject.DontDestroyOnLoad(newGameObject);
            _containersPoolsGameObjectsParentTransform = newGameObject.transform;
            _containersPoolsGameObjectsParentTransform.position = _factorySettings.ContainerPoolsParentPosition;
        }

        private void DestroyContainersPoolsParent()
        {
            GameObject.Destroy(_containersPoolsGameObjectsParentTransform.gameObject);
        }

        public void CreateContainerPools(List<SO_PooledGameObjectData> poolsData)
        {
            foreach (SO_PooledGameObjectData data in poolsData)
                _containerObjectsPools.Add(_container.Instantiate<GameObjectPool>(new object[] { data, _containersPoolsGameObjectsParentTransform, "" }));
        }

        public void DestroyContainerPools(List<SO_PooledGameObjectData> poolsData)
        {
            for (int i = _containerObjectsPools.Count - 1; i >= 0; i--)
            {
                GameObjectPool pool = _containerObjectsPools[i];

                foreach (var data in poolsData)
                    if (pool.ObjectPooled == data.Prefab)
                    {
                        pool.DestroyPool();
                        _containerObjectsPools.RemoveAt(i);
                        break;
                    }
            }
        }

        public void DestroyAllContainerPools()
        {
            for (int i = _containerObjectsPools.Count - 1; i >= 0; i--)
            {
                GameObjectPool pool = _containerObjectsPools[i];
                pool.DestroyPool();
                _containerObjectsPools.RemoveAt(i);
            }
        }

        #endregion

        #region Get Instance

        public GameObject GetGameObjectInstance(GameObject prefab)
        {
            foreach (GameObjectPool pool in _persistentGameObjectsPools)
                if (pool.ObjectPooled == prefab)
                    return pool.GetInstance();

            foreach (GameObjectPool pool in _containerObjectsPools)
                if (pool.ObjectPooled == prefab)
                    return pool.GetInstance();

            foreach (GameObjectPool pool in _sceneGameObjectsPools)
                if (pool.ObjectPooled == prefab)
                    return pool.GetInstance();

            Debug.LogError($"- {typeof(GameObjectPoolManager).Name} - Pool of the Gameobject with the name {prefab.name} not found");
            return null;
        }

        #endregion
    }
}