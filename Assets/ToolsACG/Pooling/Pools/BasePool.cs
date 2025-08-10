using System.Collections.Generic;
using UnityEngine;

namespace ToolsACG.Utils.Pooling
{
    public abstract class BasePool<T>
    {
        #region Fields and Properties

        [Header("References")]
        protected List<T> _availableInstances;

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

        public int PoolCurrentSize
        {
            get => _availableInstances.Count;
        }

        #endregion

        #region Abstract Methods
        
        public abstract T GetInstance();
        protected abstract T ObtainReadyInstance();
        protected abstract T CreateNewInstance();
        protected abstract void ExpandPoolSize(int pScalation);
        
        #endregion
    }
}