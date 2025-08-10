using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToolsACG.Utils.Pooling
{
    public class FactoryManager
    {
        #region Fields

        private static FactoryManager _instance; public static FactoryManager Instance { get { return _instance; } }

        [Header("References")]
        private Transform _pooledGameObjectsParentTransform;

        [Header("Pools")]
        private List<SimpleGameObjectPool> _gameObjectsPools = new List<SimpleGameObjectPool>();
        private AudioSourcePool _2DAudioSourcesPool;

        [Header("Data")]
        private List<SO_ScenePoolData> _scenePoolsData = new List<SO_ScenePoolData>();
        private SO_FactorySettings _factorySettings;

        #endregion

        #region Initialization

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            _instance = new FactoryManager();

            _instance.GetReferences();
            _instance.LoadScenePoolsData();

            _instance.CreateListeners();

            _instance.Create2DAudioSourcePool();
            _instance.CreatePoolGameObjectsParent();
            _instance.CreatePersistentPools();
        }

        private void GetReferences()
        {
            _factorySettings = Resources.Load<SO_FactorySettings>(ScriptableObjectKeys.FACTORY_SETTINGS_KEY);
        }
        private void LoadScenePoolsData()
        {
            SO_ScenePoolData[] scenePoolsData = Resources.LoadAll<SO_ScenePoolData>("ScenesPoolData");
            foreach (SO_ScenePoolData item in scenePoolsData)
                _scenePoolsData.Add(item);
        }
        private void CreateListeners()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        #endregion

        #region Event callbacks

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            foreach (var data in _scenePoolsData)
                if (data.SceneName == scene.name)
                {
                    CreateScenePools(scene.name, data.PoolsData);
                    return;
                }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            DestroyScenePools(scene.name);
        }

        #endregion

        #region 2D AudioSource Pools Management

        private void Create2DAudioSourcePool()
        {
            _2DAudioSourcesPool = new AudioSourcePool(_factorySettings.AudiSourcesPoolInitialSize, _factorySettings.AudiSourcesPoolEscalation, _factorySettings.AudiSourcesPoolMaxSize);
            Debug.Log("- POOL - 2DAudioSources pool created");
        }

        #endregion

        #region GameObject Pools Management

        private void CreatePoolGameObjectsParent()
        {
            GameObject newGameObject = new GameObject(_factorySettings.ParentName);
            newGameObject.AddComponent<DontDestroyOnLoad>();
            _pooledGameObjectsParentTransform = newGameObject.transform;
            _pooledGameObjectsParentTransform.position = _factorySettings.ParentPosition;
        }

        private void CreatePersistentPools()
        {
            foreach (PoolData data in _factorySettings.PersistentPoolData.PoolsData)
                CreateGameObjectPool("", data);

            Debug.Log("- POOL - Persistend pools created");
        }

        private void CreateScenePools(string pSceneName, List<PoolData> pData)
        {
            foreach (PoolData data in pData)
                CreateGameObjectPool(pSceneName, data);

            Debug.Log(string.Format("- POOL - {0} pools created", pSceneName));
        }

        private void DestroyScenePools(string pSceneName)
        {
            List<SimpleGameObjectPool> poolsToRemove = _gameObjectsPools.Where(p => p.SceneName == pSceneName).ToList();

            foreach (var pool in poolsToRemove)
                pool.DestroyPool();

            _gameObjectsPools.RemoveAll(p => p.SceneName == pSceneName);
         
            Debug.Log(string.Format("- POOL - {0} pools destroyed", pSceneName));
        }
             
        private void CreateGameObjectPool(string pSceneName, PoolData pConfiguration)
        {
            SimpleGameObjectPool newPool = new SimpleGameObjectPool(pConfiguration.PoolName, pSceneName, pConfiguration.Prefab, _pooledGameObjectsParentTransform, pConfiguration.InitialSize, pConfiguration.Escalation, pConfiguration.MaxSize);
            _gameObjectsPools.Add(newPool);
        }

        #endregion


        #region Instances Management

        public AudioSource Get2DAudioSource()
        {
            return _2DAudioSourcesPool.GetInstance();
        }
        public GameObject GetGameObjectInstance(string pPoolName)
        {
            foreach (SimpleGameObjectPool pool in _gameObjectsPools)
                if (pool.PoolName == pPoolName)
                    return pool.GetInstance();

            Debug.LogError(string.Format("- POOL - Pool with the name {0} not found", pPoolName));
            return null;
        }

        #endregion
    }
}









