using ToolsACG.Pooling.Pools;
using ToolsACG.Pooling.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ToolsACG.Pooling.Managers
{
    public class Sound3DPoolManager : PoolManagerBase
    {
        #region Fields

        [Header("References")]
        private Transform _pooled3DSoundsParentTransform;
        private readonly DiContainer _container;

        [Header("Pools")]
        private GameObjectPool _3DSoundPool;

        [Header("Data")]
        private readonly SO_FactorySettings _factorySettings;

        #endregion

        #region Constructors

        [Inject]
        public Sound3DPoolManager(DiContainer container, SO_FactorySettings settings)
        {
            _container = container;
            _factorySettings = settings;
        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            if (_factorySettings.Use3DSoundPool)
            {
                Create3DSoundPoolParent();
                Create3DSoundPool();
            }
        }

        public override void Dispose()
        {
            if (_factorySettings.Use3DSoundPool)
            {
                Destroy3DSoundPool();
                Destroy3DSoundPoolParent();
            }
        }

        #endregion

        #region Get Instance

        public GameObject Get3DSoundInstance()
        {
            return _3DSoundPool.GetInstance();
        }

        #endregion

        #region 3DSound Pool Management

        private void Create3DSoundPoolParent()
        {
            GameObject newGameObject = new(_factorySettings.Sound3DPoolParentName);
            GameObject.DontDestroyOnLoad(newGameObject);
            _pooled3DSoundsParentTransform = newGameObject.transform;
            _pooled3DSoundsParentTransform.position = _factorySettings.Sound3DPoolParentPosition;
        }
        private void Destroy3DSoundPoolParent()
        {
            GameObject.Destroy(_pooled3DSoundsParentTransform.gameObject);
        }

        private void Create3DSoundPool()
        {
            _3DSoundPool = _container.Instantiate<GameObjectPool>(new object[] { _factorySettings.Sound3DPooledGameObjectData, _pooled3DSoundsParentTransform,"" });
            Debug.Log($"- {typeof(Sound3DPoolManager).Name} - 3DSound pool created");
        }
        private void Destroy3DSoundPool()
        {
            _3DSoundPool.DestroyPool();
            _3DSoundPool = null;
            Debug.Log($"- {typeof(Sound3DPoolManager).Name} - 3DSound pool destroyed");
        }

        #endregion
    }
}