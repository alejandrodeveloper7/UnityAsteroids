using System.Collections.Generic;
using ACG.Tools.Runtime.Pooling.Core;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using UnityEngine;

namespace ACG.Tools.Runtime.Pooling.Gameplay
{
    public class PoolContainer : MonoBehaviour
    {
        #region Fields              

        [Header("Configuration")]
        [SerializeField] private List<SO_PooledGameObjectData> _pooledGameObjects;

        #endregion

        #region Initialization

        private void Initialize()
        {
            if (_pooledGameObjects.Count is 0)
            {
                Debug.LogError($"- {nameof(PoolContainer)} - Pooled Gameobjects Data in {gameObject.name} is empty, script disabled");
                enabled = false;
                return;
            }

            CreateGameObjectPools();
        }

        private void Dispose()
        {
            DestroyGameObjectPools();
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

        #region Pools Management

        private void CreateGameObjectPools()
        {
            FactoryManager.Instance.CreateContainerPools(_pooledGameObjects);
        }

        private void DestroyGameObjectPools()
        {
            FactoryManager.Instance.DestroyContainerPools(_pooledGameObjects);
        }

        #endregion
    }
}