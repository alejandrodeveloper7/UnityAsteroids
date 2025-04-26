using System;
using UnityEngine;

namespace ToolsACG.Utils.Pooling
{
    public interface IPooleableGameObject
    {
        SimpleGameObjectPool OriginPool
        {
            get;
            set;
        }

        bool ReadyToUse
        {
            get;
            set;
        }

        //QUICK IMPLEMENTATION

        //SimpleGameObjectPool _originPool;
        //public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
        //bool _readyToUse;
        //public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }
    }

    public class SimpleGameObjectPool:BasePool<GameObject>
    {
        #region Fields

        [Header("Fields")]
        private readonly GameObject _objectPooled;
        private readonly Transform _parent;

        #endregion

        #region Constuctors

        public SimpleGameObjectPool(GameObject objectCopied, Transform parent)
            : this(objectCopied, parent, 0, 1, int.MaxValue)
        {
        }

        public SimpleGameObjectPool(GameObject objectCopied, Transform parent, int initialSize)
            : this(objectCopied, parent, initialSize, 1, int.MaxValue)
        {
        }

        public SimpleGameObjectPool(GameObject objectCopied, Transform parent, int initialSize, int scalation)
            : this(objectCopied, parent, initialSize, scalation, int.MaxValue)
        {
        }

        public SimpleGameObjectPool(GameObject objectPooled, Transform parent, int initialSize, int scalation, int poolMaxSize)
        {
            if (scalation <= 0)
                scalation = 1;

            if (poolMaxSize <= 0)
                poolMaxSize = 1;

            _availableInstances = new GameObject[0];
            _objectPooled = objectPooled;
            _parent = parent;
            Scalation = scalation;
            PoolMaxSize = poolMaxSize;
            PoolCurrentSize = 0;

            if (initialSize > 0)
                ExpandPoolSize(initialSize);
        }

        #endregion

        #region Abstract Methods

        public override GameObject GetInstance()
        {
            GameObject instance = ObteinReadyInstance();

            if (instance == null && _availableInstances.Length < PoolMaxSize)
            {
                ExpandPoolSize(_scalation);
                instance = ObteinReadyInstance();
            }

            if (instance == null)
            {
                Debug.LogError(string.Format("- POOL - {0}'s pool is at max size and there are not ready instances available", _objectPooled.name));
                return instance;
            }

            instance.transform.SetParent(null);
            instance.SetActive(true);
            instance.GetComponent<IPooleableGameObject>().ReadyToUse = false;

            return instance;
        }

        protected override GameObject ObteinReadyInstance()
        {
            for (int i = 0; i < _availableInstances.Length; i++)
                if (_availableInstances[i].GetComponent<IPooleableGameObject>().ReadyToUse)
                    return _availableInstances[i];

            return null;
        }

        protected override GameObject CreateNewInstance()
        {
            GameObject newInstance = UnityEngine.Object.Instantiate(_objectPooled, _parent);
            IPooleableGameObject pooleableItem = newInstance.GetComponent<IPooleableGameObject>();
            if (pooleableItem is null)
            {
                Debug.LogError(string.Format("- POOL - {0} dont have a script with IPooleableITem", _objectPooled.name));
                return null;
            }
            pooleableItem.OriginPool = this;

            RecycleGameObject(newInstance);

            return newInstance;
        }

        protected override void ExpandPoolSize(int pIncrement)
        {
            int maxPosibleScalation = PoolMaxSize - _availableInstances.Length;

            if (maxPosibleScalation == 0)
            {
                Debug.LogError(string.Format("- POOL - {0}'s pool at max size, can´t be expanded more", _objectPooled.name));
                return;
            }

            if (pIncrement < maxPosibleScalation)
            {
                Debug.Log(string.Format("- POOL - {0}'s pool expanded in {1} units", _objectPooled.name, pIncrement));
            }
            else
            {
                pIncrement = maxPosibleScalation;
                Debug.Log(string.Format("- POOL - {0}'s pool expanded in {1} units. POOL AT MAX SIZE", _objectPooled.name, pIncrement));
            }

            int firstPosition = _availableInstances.Length;
            Array.Resize(ref _availableInstances, firstPosition + pIncrement);

            for (int i = firstPosition; i < _availableInstances.Length; i++)
                _availableInstances[i] = CreateNewInstance();

            PoolCurrentSize = _availableInstances.Length;
        }

        #endregion

        #region Own Methods

        public void RecycleGameObject(GameObject pInstance)
        {
            IPooleableGameObject poleableItem = pInstance.GetComponent<IPooleableGameObject>();

            if (poleableItem == null)
            {
                Debug.LogError(string.Format("- POOL - {0} is not Pooled with IPooleableItem", pInstance.name));
                return;
            }
            if (poleableItem.OriginPool != this)
            {
                Debug.LogError(string.Format("- POOL - {0} is not an Instance of this pool", pInstance.name));
                return;
            }

            poleableItem.ReadyToUse = true;
            pInstance.SetActive(false);
            if (_parent != null)
            {
                pInstance.transform.SetParent(_parent);
                pInstance.transform.localPosition = Vector3.zero;
                pInstance.transform.localRotation = Quaternion.identity;
            }
        }

        #endregion
    }
}
