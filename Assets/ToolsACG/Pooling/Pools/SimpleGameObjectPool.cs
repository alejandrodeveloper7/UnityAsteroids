using System.Collections.Generic;
using UnityEngine;

namespace ToolsACG.Utils.Pooling
{
    public interface IPooleableGameObject
    {
        SimpleGameObjectPool OriginPool { get; set; }
        bool ReadyToUse { get; set; }

        public void Recycle(GameObject pGameObject)
        {
            if (OriginPool == null)
            {
                Debug.Log(string.Format("OriginPool is null, gameObject destroyed - {0}", pGameObject.name));
                GameObject.Destroy(pGameObject);
            }
            else
                OriginPool.RecycleGameObject(pGameObject);
        }

        //QUICK IMPLEMENTATION

        //SimpleGameObjectPool _originPool;
        //public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
        //bool _readyToUse;
        //public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }
    }

    public class SimpleGameObjectPool : BasePool<GameObject>
    {
        #region Fields

        public string PoolName { get; }
        public string SceneName { get; }
        private GameObject ObjectPooled { get; set; }
        private readonly Transform _parent;

        #endregion

        #region Constructors

        public SimpleGameObjectPool(string poolName, string sceneName, GameObject objectCopied, Transform parent)
            : this(poolName, sceneName, objectCopied, parent, 0, 1, int.MaxValue)
        {
        }

        public SimpleGameObjectPool(string poolName, string sceneName, GameObject objectCopied, Transform parent, int initialSize)
            : this(poolName, sceneName, objectCopied, parent, initialSize, 1, int.MaxValue)
        {
        }

        public SimpleGameObjectPool(string poolName, string sceneName, GameObject objectCopied, Transform parent, int initialSize, int scalation)
            : this(poolName, sceneName, objectCopied, parent, initialSize, scalation, int.MaxValue)
        {
        }

        public SimpleGameObjectPool(string poolName, string sceneName, GameObject objectPooled, Transform parent, int initialSize, int scalation, int poolMaxSize)
        {
            PoolName = poolName;
            SceneName = sceneName;

            if (scalation <= 0)
                scalation = 1;

            if (poolMaxSize <= 0)
                poolMaxSize = 1;

            _availableInstances = new List<GameObject>();
            ObjectPooled = objectPooled;
            _parent = parent;
            Scalation = scalation;
            PoolMaxSize = poolMaxSize;

            if (initialSize > 0)
                ExpandPoolSize(initialSize);
        }

        #endregion

        #region Abstract Methods

        public override GameObject GetInstance()
        {
            GameObject instance = ObtainReadyInstance();

            if (instance == null && _availableInstances.Count < PoolMaxSize)
            {
                ExpandPoolSize(_scalation);
                instance = ObtainReadyInstance();
            }

            if (instance == null)
            {
                Debug.LogError(string.Format("- POOL - {0} pool is at max size and there are not ready instances available", ObjectPooled.name));
                return null;
            }

            instance.transform.SetParent(null);
            instance.SetActive(true);
            instance.GetComponent<IPooleableGameObject>().ReadyToUse = false;

            return instance;
        }

        protected override GameObject ObtainReadyInstance()
        {
            for (int i = 0; i < _availableInstances.Count; i++)
            {
                if (_availableInstances[i] is null)
                {
                    CleanNulls();
                    return ObtainReadyInstance();
                }

                if (_availableInstances[i].GetComponent<IPooleableGameObject>().ReadyToUse)
                    return _availableInstances[i];
            }

            return null;
        }

        protected override GameObject CreateNewInstance()
        {
            GameObject newInstance = UnityEngine.Object.Instantiate(ObjectPooled, _parent);
            IPooleableGameObject pooleableItem = newInstance.GetComponent<IPooleableGameObject>();
            if (pooleableItem is null)
            {
                Debug.LogError(string.Format("- POOL - {0} doesn't have a script implementing IPooleableGameObject", ObjectPooled.name));
                return null;
            }
            pooleableItem.OriginPool = this;

            RecycleGameObject(newInstance);

            return newInstance;
        }

        protected override void ExpandPoolSize(int pIncrement)
        {
            int maxPosibleScalation = PoolMaxSize - _availableInstances.Count;

            if (maxPosibleScalation == 0)
            {
                Debug.LogError(string.Format("- POOL - {0} pool at max size, can´t be expanded more", ObjectPooled.name));
                return;
            }

            if (pIncrement < maxPosibleScalation)
            {
                if (PoolCurrentSize == 0)
                    Debug.Log(string.Format("- POOL - {0} pool created with {1} instances", ObjectPooled.name, pIncrement));
                else
                    Debug.Log(string.Format("- POOL - {0} pool expanded in {1} instances", ObjectPooled.name, pIncrement));
            }
            else
            {
                pIncrement = maxPosibleScalation;
                if (PoolCurrentSize == 0)
                    Debug.LogWarning(string.Format("- POOL - {0} pool created with {1} units. POOL AT MAX SIZE", ObjectPooled.name, pIncrement));
                else
                    Debug.LogWarning(string.Format("- POOL - {0} pool expanded in {1} units. POOL AT MAX SIZE", ObjectPooled.name, pIncrement));
            }

            for (int i = 0; i < pIncrement; i++)
                _availableInstances.Add(CreateNewInstance());
        }

        #endregion

        #region Own Methods

        internal void RecycleGameObject(GameObject pInstance)
        {
            IPooleableGameObject pooleableItem = pInstance.GetComponent<IPooleableGameObject>();

            if (pooleableItem == null)
            {
                Debug.LogError(string.Format("- POOL - {0} is not Pooled with IPooleableItem", pInstance.name));
                return;
            }
            if (pooleableItem.OriginPool != this)
            {
                Debug.LogError(string.Format("- POOL - {0} is not an Instance of this pool", pInstance.name));
                return;
            }

            pooleableItem.ReadyToUse = true;
            pInstance.SetActive(false);
            if (_parent != null)
            {
                pInstance.transform.SetParent(_parent);
                pInstance.transform.localPosition = Vector3.zero;
                pInstance.transform.localRotation = Quaternion.identity;
                pInstance.transform.localScale = Vector3.one;
            }
        }

        private void CleanNulls()
        {
            _availableInstances.RemoveAll(i => i == null);
            Debug.LogWarning(string.Format("- POOL - null instances detected and cleaned in {0} pool", ObjectPooled.name));
        }

        public void DestroyPool()
        {
            foreach (var instance in _availableInstances)
                if (instance != null)
                    GameObject.Destroy(instance);

            _availableInstances.Clear();
            Debug.Log(string.Format("- POOL - {0} pool destroyed", ObjectPooled.name));
        }

        #endregion
    }
}
