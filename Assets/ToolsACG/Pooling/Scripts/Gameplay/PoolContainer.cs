using System.Collections.Generic;
using ToolsACG.Pooling.Models;
using ToolsACG.Pooling.Pools;
using ToolsACG.Pooling.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ToolsACG.Pooling.Gameplay
{
    public class PoolContainer : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        private Transform _pooledGameObjectsParentTransform;
        [Inject] private readonly DiContainer _container;

        [Header("Pools")]
        private readonly List<GameObjectPool> _gameObjectsPools = new();

        [Header("Data")]
        [SerializeField] private SO_PoolContainerData _gameObjectPoolsData;

        #endregion

        #region Initialization

        private void Initialize()
        {
            if (_gameObjectPoolsData == null)
            {
                Debug.LogError($"- {nameof(PoolContainer)} - {nameof(SO_PoolContainerData)} in {gameObject.name} is null, script disabled");
                enabled = false;
                return;
            }

            CreatePoolsParent();
            CreateGameObjectPools();
        }

        private void Dispose()
        {
            DestroyGameObjectPools();
            DestroyPoolsParent();
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        #region Parent Management

        private void CreatePoolsParent()
        {
            GameObject newGameObject = new(_gameObjectPoolsData.ParentName);
            _pooledGameObjectsParentTransform = newGameObject.transform;
            _pooledGameObjectsParentTransform.SetParent(transform, false);
            _pooledGameObjectsParentTransform.localPosition = Vector3.zero;
        }
        private void DestroyPoolsParent()
        {
            if (_pooledGameObjectsParentTransform != null)
                Destroy(_pooledGameObjectsParentTransform.gameObject);
        }

        #endregion

        #region Pools Management

        private void CreateGameObjectPools()
        {
            foreach (PoolData data in _gameObjectPoolsData.PoolsData)
                CreateGameObjectPool(data);

            Debug.Log($"- {nameof(PoolContainer)} - {gameObject.name} gameObject pools created");
        }
        private void CreateGameObjectPool(PoolData configuration)
        {
            GameObjectPool newPool = _container.Instantiate<GameObjectPool>(new object[] { configuration.Prefab, _pooledGameObjectsParentTransform, configuration.InitialSize, configuration.Escalation, configuration.MaxSize, configuration.PoolName, "" });
            _gameObjectsPools.Add(newPool);
        }

        private void DestroyGameObjectPools()
        {
            foreach (GameObjectPool pool in _gameObjectsPools)
                pool.DestroyPool();

            _gameObjectsPools.Clear();

            Debug.Log($"- {nameof(PoolContainer)} - {gameObject.name} gameObject pools destroyed");
        }

        #endregion

        #region Instances Management

        public GameObject GetGameObjectInstance(string poolName)
        {
            foreach (GameObjectPool pool in _gameObjectsPools)
                if (pool.PoolName == poolName)
                    return pool.GetInstance();

            Debug.LogError($"- {nameof(PoolContainer)} - Pool with the name {poolName} not found");
            return null;
        }

        #endregion
    }
}