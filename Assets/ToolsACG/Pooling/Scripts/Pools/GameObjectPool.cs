using ToolsACG.Pooling.Interfaces;
using ToolsACG.Pooling.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ToolsACG.Pooling.Pools
{
    public class GameObjectPool : BasePool<GameObject>
    {
        #region Fields

        [Header("Fields")]
        private readonly DiContainer _container;
        public string SceneName { get; }
        public GameObject ObjectPooled { get; }

        #endregion

        #region Constructors

        [Inject]
        public GameObjectPool(DiContainer container, SO_PooledGameObjectData data, Transform parent, string sceneName = "")
        {
            _container = container;
            SceneName = sceneName;

            if (data.Scalation <= 0)
                Scalation = 1;
            else
                Scalation = data.Scalation;

            if (data.MaxSize <= 0)
                PoolMaxSize = 1;
            else
                PoolMaxSize = data.MaxSize;

            ObjectPooled = data.Prefab;
            _parent = parent;

            _availableInstances = new();

            if (data.InitialSize > 0)
                ExpandPoolSize(data.InitialSize);
        }

        #endregion

        #region Abstract Methods

        public override GameObject GetInstance()
        {
            GameObject instance = GetReadyInstance();

            if (instance == null && _availableInstances.Count < PoolMaxSize)
            {
                ExpandPoolSize(Scalation);
                instance = GetReadyInstance();
            }

            if (instance == null)
            {
                Debug.LogError($"- {typeof(GameObjectPool).Name} - {ObjectPooled.name} pool is at max size and there are not ready instances available");
                return null;
            }

            instance.transform.SetParent(null);
            instance.SetActive(true);
            instance.GetComponent<IPooleableGameObject>().ReadyToUse = false;

            return instance;
        }

        protected override GameObject GetReadyInstance()
        {
            for (int i = 0; i < _availableInstances.Count; i++)
            {
                if (_availableInstances[i] == null)
                {
                    CleanNulls();
                    return GetReadyInstance();
                }

                if (_availableInstances[i].GetComponent<IPooleableGameObject>().ReadyToUse)
                    return _availableInstances[i];
            }

            return null;
        }

        protected override GameObject CreateNewInstance()
        {
            GameObject newInstance = _container.InstantiatePrefab(ObjectPooled, _parent);
            IPooleableGameObject pooleableItem = newInstance.GetComponent<IPooleableGameObject>();
            if (pooleableItem is null)
            {
                Debug.LogError($"- {typeof(GameObjectPool).Name} - {ObjectPooled.name} doesn't have a script implementing IPooleableGameObject");
                return null;
            }
            pooleableItem.OriginPool = this;
            RecycleGameObject(newInstance);

            return newInstance;
        }

        protected override void ExpandPoolSize(int increment)
        {
            int maxPosibleScalation = PoolMaxSize - _availableInstances.Count;

            if (maxPosibleScalation == 0)
            {
                Debug.LogError($"- {typeof(GameObjectPool).Name} - {ObjectPooled.name} pool at max size, can´t be expanded more");
                return;
            }

            if (increment < maxPosibleScalation)
            {
                if (PoolCurrentSize == 0)
                    Debug.Log($"- {typeof(GameObjectPool).Name} - {ObjectPooled.name} pool created with {increment} instances");
                else
                    Debug.Log($"- {typeof(GameObjectPool).Name} - {ObjectPooled.name} pool expanded in {increment} instances");
            }
            else
            {
                increment = maxPosibleScalation;

                if (PoolCurrentSize == 0)
                    Debug.LogWarning($"- {typeof(GameObjectPool).Name} - {ObjectPooled.name} pool created with {increment} instances. POOL AT MAX SIZE");
                else
                    Debug.LogWarning($"- {typeof(GameObjectPool).Name} - {ObjectPooled.name} pool expanded in {increment} instances. POOL AT MAX SIZE");
            }

            for (int i = 0; i < increment; i++)
                _availableInstances.Add(CreateNewInstance());
        }

        public override void DestroyPool()
        {
            foreach (GameObject instance in _availableInstances)
                if (instance != null)
                    GameObject.Destroy(instance);

            _availableInstances.Clear();
        }

        #endregion

        #region Own Methods

        internal void RecycleGameObject(GameObject instance)
        {
            instance.TryGetComponent(out IPooleableGameObject pooleableItem);

            if (pooleableItem == null)
            {
                Debug.LogError($"- {typeof(GameObjectPool).Name} - {instance.name} is not Pooled with IPooleableItem");
                return;
            }

            if (pooleableItem.OriginPool != this)
            {
                Debug.LogError($"- {typeof(GameObjectPool).Name} - {instance.name} is not an Instance of this pool");
                return;
            }

            pooleableItem.ReadyToUse = true;
            instance.SetActive(false);

            if (_parent != null)
                instance.transform.SetParent(_parent, false);
        }

        private void CleanNulls()
        {
            _availableInstances.RemoveAll(i => i == null);
            Debug.LogWarning($"- {typeof(GameObjectPool).Name} - null instances detected and cleaned in {ObjectPooled.name} pool");
        }

        #endregion
    }
}
