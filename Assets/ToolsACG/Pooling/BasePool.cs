using UnityEngine;

namespace ToolsACG.Utils.Pooling
{
    public abstract class BasePool<T> : MonoBehaviour
    {
        protected T[] _availableInstances;

        [Header("Properties")]
        protected int _scalation;
        public int Scalation
        {
            get => _scalation;
            set => _scalation = value;
        }

        protected int _poolMaxSize;
        public int PoolMaxSize
        {
            get => _poolMaxSize;
            set => _poolMaxSize = value;
        }

        protected int _poolCurrentSize;
        public int PoolCurrentSize
        {
            get => _poolCurrentSize;
            set => _poolCurrentSize = value;
        }

        public abstract T GetInstance();
        protected abstract T ObteinReadyInstance();
        protected abstract T CreateNewInstance();
        protected abstract void ExpandPoolSize(int pScalation);
    }
}
