using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ToolsACG.Utils.Pooling
{
    public class AudioSourcePool : BasePool<AudioSource>
    {
        #region Fields

        private Transform _parent;

        #endregion

        #region Constructors

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

        public AudioSourcePool(int initialSize, int scalation, int poolMaxSize)
        {
            if (scalation <= 0)
                scalation = 1;

            if (poolMaxSize <= 0)
                poolMaxSize = 1;

            _availableInstances = new List<AudioSource>();

            Scalation = scalation;
            PoolMaxSize = poolMaxSize;

            CreateParent();

            if (initialSize > 0)
                ExpandPoolSize(initialSize);
        }

        #endregion

        #region Abstract Methods

        public override AudioSource GetInstance()
        {
            AudioSource instance = ObtainReadyInstance();

            if (instance == null && _availableInstances.Count < PoolMaxSize)
            {
                ExpandPoolSize(_scalation);
                instance = ObtainReadyInstance();
            }

            if (instance == null && _availableInstances.Count == PoolMaxSize)
            {
                Debug.LogError("- POOL - AudioSource pool is at max size and there are not ready instances available");
                return instance;
            }

            return instance;
        }

        protected override AudioSource ObtainReadyInstance()
        {
            foreach (AudioSource audioSource in _availableInstances)
                if (audioSource.isPlaying is false)
                    return audioSource;

            return null;
        }

        protected override AudioSource CreateNewInstance()
        {
            AudioSource newInstance = _parent.AddComponent<AudioSource>();
            return newInstance;
        }

        protected override void ExpandPoolSize(int pIncrement)
        {
            int maxPosibleScalation = PoolMaxSize - _availableInstances.Count;

            if (maxPosibleScalation == 0)
            {
                Debug.LogError("- POOL - AudioSource pool at max size, can´t be expanded more");
                return;
            }

            if (pIncrement < maxPosibleScalation)
            {
                if (PoolCurrentSize == 0)
                    Debug.Log(string.Format("- POOL - AudioSource pool created with {0} instances", pIncrement));
                else
                    Debug.Log(string.Format("- POOL - AudioSource pool expanded in {0} instances", pIncrement));
            }
            else
            {
                pIncrement = maxPosibleScalation;
                if (PoolCurrentSize == 0)
                    Debug.LogWarning(string.Format("- POOL - AudioSource pool created with {0} instances. POOL AT MAX SIZE", pIncrement));
                else
                    Debug.LogWarning(string.Format("- POOL - AudioSource pool expanded in {0} instances. POOL AT MAX SIZE", pIncrement));
            }

            for (int i = 0; i < pIncrement; i++)
                _availableInstances.Add(CreateNewInstance());
        }


        #endregion

        #region Own Methods

        private void CreateParent()
        {
            _parent = new GameObject("pooled_2D_audioSources").transform;
            _parent.AddComponent<DontDestroyOnLoad>();
        }

        #endregion
    }
}