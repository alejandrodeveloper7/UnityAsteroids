using System.Collections.Generic;
using UnityEngine;

namespace ToolsACG.Pooling.Pools
{
    public abstract class BasePool<T>
    {
        #region Fields and Properties

        [Header("References")]
        protected List<T> _availableInstances;
        protected Transform _parent;

        //[Header("Properties")]
        public int Scalation { get; set; }
        public int PoolMaxSize { get; set; }

        public int PoolCurrentSize
        {
            get => _availableInstances.Count;
        }

        #endregion

        #region Abstract Methods

        public abstract T GetInstance();
        protected abstract T GetReadyInstance();
        protected abstract T CreateNewInstance();
        protected abstract void ExpandPoolSize(int scalation);
        public abstract void DestroyPool();

        #endregion
    }
}