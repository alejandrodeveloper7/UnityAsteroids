using ToolsACG.Pooling.Interfaces;
using ToolsACG.Pooling.Pools;
using UnityEngine;

namespace ToolsACG.Pooling.Gameplay
{
    public class Pooled3DSound : MonoBehaviour, IPooleableGameObject
    {
        #region Fields

        public GameObjectPool OriginPool { get; set; }
        public bool ReadyToUse { get; set; }

        [Header("References")]
        private AudioSource _audioSource;

        #endregion

        #region Initialization

        private void GetReferences()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            GetReferences();
        }

        #endregion

        #region Functionality

        public void Play()
        {
            _audioSource.Play();
            Invoke(nameof(RecycleGameObject), _audioSource.clip.length);
        }

        private void RecycleGameObject()
        {
            GetComponent<IPooleableGameObject>().Recycle(gameObject);
        }

        #endregion
    }
}
