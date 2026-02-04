using ACG.Tools.Runtime.Pooling.Interfaces;
using ACG.Tools.Runtime.Pooling.Pools;
using UnityEngine;

namespace ACG.Tools.Runtime.Pooling.Gameplay
{
    public class Pooled3DSoundController : MonoBehaviour, IPooledDetonable
    {
        #region Fields

        //[Header("IPooleableGameObject")]
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

        public void Detonate()
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
