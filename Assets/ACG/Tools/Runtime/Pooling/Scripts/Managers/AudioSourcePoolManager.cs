using ACG.Tools.Runtime.Pooling.Pools;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ACG.Tools.Runtime.Pooling.Managers
{
    public class AudioSourcePoolManager : PoolManagerBase
    {
        #region Fields

        [Header("References")]
        private Transform _audioSourcePoolParentTransform;

        [Header("Pools")]
        private AudioSourcePool _audioSourcesPool;

        [Header("Data")]
        private readonly SO_FactorySettings _factorySettings;

        #endregion

        #region Constructors

        [Inject]
        public AudioSourcePoolManager(SO_FactorySettings settings)
        {
            _factorySettings = settings;
        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            if (_factorySettings.UseAudioSourcePool)
            {
                CreateAudioSourcePoolParent();
                CreateAudioSourcePool();
            }
        }

        public override void Dispose()
        {
            if (_factorySettings.UseAudioSourcePool)
            {
                DestroyAudioSourcePool();
                DestroyAudioSourcePoolParent();
            }
        }

        #endregion

        #region Get Instance

        public AudioSource Get2DAudioSourceInstance()
        {
            return _audioSourcesPool.GetInstance();
        }

        #endregion

        #region Pool Management

        private void CreateAudioSourcePoolParent()
        {
            GameObject newGameObject = new(_factorySettings.AudioSourcePoolParentName);
            GameObject.DontDestroyOnLoad(newGameObject);
            _audioSourcePoolParentTransform = newGameObject.transform;
            _audioSourcePoolParentTransform.position = _factorySettings.AudioSourcePoolParentPosition;
        }
        private void DestroyAudioSourcePoolParent()
        {
            GameObject.Destroy(_audioSourcePoolParentTransform.gameObject);
        }

        private void CreateAudioSourcePool()
        {
            _audioSourcesPool = new(_audioSourcePoolParentTransform, _factorySettings.AudiSourcesPoolInitialSize, _factorySettings.AudiSourcesPoolEscalation, _factorySettings.AudiSourcesPoolMaxSize);
            Debug.Log($"- {typeof(AudioSourcePoolManager).Name} - AudioSources pool created");
        }
        private void DestroyAudioSourcePool()
        {
            _audioSourcesPool.DestroyPool();
            _audioSourcesPool = null;
            Debug.Log($"- {typeof(AudioSourcePoolManager).Name} - AudioSources pool destroyed");
        }

        #endregion
    }
}