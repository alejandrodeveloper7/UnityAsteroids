using ToolsACG.Pooling.Interfaces;
using ToolsACG.Pooling.Pools;
using UnityEngine;

namespace ToolsACG.Pooling.Gameplay
{
    public class PooledParticleSystem : MonoBehaviour, IPooleableGameObject
    {
        #region Fields

        //[Header("IPooleableGameObject")]
        public GameObjectPool OriginPool { get; set; }
        public bool ReadyToUse { get; set; }

        [Header("References")]
        private ParticleSystem _particleSystem;

        #endregion

        #region Initialization

        private void GetReferences()
        {
            _particleSystem = GetComponent<ParticleSystem>();
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
            float duration = _particleSystem.main.duration;

            _particleSystem.Play();
            Invoke(nameof(RecycleGameObject), duration);
        }

        private void RecycleGameObject()
        {
            GetComponent<IPooleableGameObject>().Recycle(gameObject);
        }

        #endregion
    }
}
