using System;
using Unity.VisualScripting;
using UnityEngine;

namespace ToolsACG.Utils.Pooling
{
    public class AudioSourcePool
    {
        private AudioSource[] _availableInstances;
        private Transform _parent;

        [Header("Properties")]
        private int _scalation;
        public int Scalation
        {
            get => _scalation;
            set => _scalation = value;
        }

        private int _poolMaxSize;
        public int PoolMaxSize
        {
            get => _poolMaxSize;
            set => _poolMaxSize = value;
        }

        private int _poolCurrentSize;
        public int PoolCurrentSize
        {
            get => _poolCurrentSize;
            private set => _poolCurrentSize = value;
        }

        public AudioSourcePool()
              : this(1)
        {
        }

        public AudioSourcePool(int initialSize)
            : this(initialSize, 1, int.MaxValue)
        {
        }

        public AudioSourcePool(int initialSize, int scalation)
            : this(initialSize, scalation, int.MaxValue)
        {
        }

        public AudioSourcePool( int initialSize, int scalation, int poolMaxSize)
        {
            if (scalation <= 0)
                scalation = 1;

            if (poolMaxSize <= 0)
                poolMaxSize = 1;

            _availableInstances = new AudioSource[0];

            Scalation = scalation;
            PoolMaxSize = poolMaxSize;
            PoolCurrentSize = 0;

            CreateParent();

            if (initialSize > 0)
                ExpandPoolSize(initialSize);
        }


        #region public Methods

        public AudioSource GetInstance()
        {
            AudioSource instance = ObteinReadyInstance();

            if (instance == null && _availableInstances.Length < PoolMaxSize)
            {
                ExpandPoolSize(_scalation);
                instance = ObteinReadyInstance();
            }

            if (instance == null)
            {
                Debug.LogError("AudioSource pool is at max size and there are not ready instances available");
                return instance;
            }

            return instance;
        }

        #endregion

        #region Internal Logic

        private void CreateParent() 
        {
            _parent = new GameObject("Pooled2DAudioSources").transform;
        }

        private AudioSource ObteinReadyInstance()
        {
            foreach (AudioSource audioSource in _availableInstances)
                if (audioSource.isPlaying is false)
                    return audioSource;

            return null;
        }

        internal AudioSource CreateNewInstance()
        {
            AudioSource newInstance = _parent.AddComponent<AudioSource>();
            return newInstance;
        }

        internal void ExpandPoolSize(int pIncrement)
        {
            int maxPosibleScalation = PoolMaxSize - _availableInstances.Length;

            if (maxPosibleScalation == 0)
            {
                Debug.LogError("- POOL - AudioSource pool at max size, can´t be expanded more");
                return;
            }

            if (pIncrement < maxPosibleScalation)
            {
                Debug.Log(string.Format("- POOL - AudioSource pool expanded in {0} units", pIncrement));
            }
            else
            {
                pIncrement = maxPosibleScalation;
                Debug.Log(string.Format("- POOL - AudioSource pool expanded in {0} units. POOL AT MAX SIZE", pIncrement));
            }

            int firstPosition = _availableInstances.Length;
            Array.Resize(ref _availableInstances, firstPosition + pIncrement);

            for (int i = firstPosition; i < _availableInstances.Length; i++)
                _availableInstances[i] = CreateNewInstance();

            PoolCurrentSize = _availableInstances.Length;
        }

        #endregion

    }
}