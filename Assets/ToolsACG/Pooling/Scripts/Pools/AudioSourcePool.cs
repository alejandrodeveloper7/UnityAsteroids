using Unity.VisualScripting;
using UnityEngine;

namespace ToolsACG.Pooling.Pools
{
    public class AudioSourcePool : BasePool<AudioSource>
    {
        #region Constructors

        public AudioSourcePool(Transform parent, int initialSize, int scalation=1, int poolMaxSize=int.MaxValue)
        {
            if (scalation <= 0)
                scalation = 1;

            if (poolMaxSize <= 0)
                poolMaxSize = 1;

            Scalation = scalation;
            PoolMaxSize = poolMaxSize;

            _parent=parent; ;

            _availableInstances = new ();

            if (initialSize > 0)
                ExpandPoolSize(initialSize);
        }

        #endregion

        #region Abstract Methods

        public override AudioSource GetInstance()
        {
            AudioSource instance = GetReadyInstance();

            if (instance == null && _availableInstances.Count < PoolMaxSize)
            {
                ExpandPoolSize(Scalation);
                instance = GetReadyInstance();
            }

            if (instance == null && _availableInstances.Count == PoolMaxSize)
            {
                Debug.LogError($"- {typeof(AudioSourcePool).Name} - AudioSource pool is at max size and there are not ready instances available");
                return instance;
            }

            return instance;
        }

        protected override AudioSource GetReadyInstance()
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

        protected override void ExpandPoolSize(int increment)
        {
            int maxPosibleScalation = PoolMaxSize - _availableInstances.Count;

            if (maxPosibleScalation == 0)
            {
                Debug.LogError($"- {typeof(AudioSourcePool).Name} - AudioSource pool at max size, can´t be expanded more");
                return;
            }

            if (increment < maxPosibleScalation)
            {
                if (PoolCurrentSize == 0)
                    Debug.Log($"- {typeof(AudioSourcePool).Name} - AudioSource pool created with {increment} instances");
                else
                    Debug.Log($"- {typeof(AudioSourcePool).Name} - AudioSource pool expanded in {increment} instances");
            }
            else
            {
                increment = maxPosibleScalation;

                if (PoolCurrentSize == 0)
                    Debug.LogWarning($"- {typeof(AudioSourcePool).Name} - AudioSource pool created with {increment} instances. POOL AT MAX SIZE");
                else
                    Debug.LogWarning($"- {typeof(AudioSourcePool).Name} - AudioSource pool expanded in {increment} instances. POOL AT MAX SIZE");
            }

            for (int i = 0; i < increment; i++)
                _availableInstances.Add(CreateNewInstance());
        }

        public override void DestroyPool()
        {
            GameObject.Destroy(_parent);
            _availableInstances.Clear();
        }

        #endregion
    }
}