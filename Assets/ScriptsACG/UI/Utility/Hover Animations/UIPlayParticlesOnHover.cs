using UnityEngine;
using UnityEngine.EventSystems;

namespace ToolsACG.UI.Utilitys.Animation
{
    public class UIPlayParticlesOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        [Header("References")]
        [SerializeField] private ParticleSystem _particles;

        [Header("Configuration")]
        [SerializeField] private bool _ignoreTimeScale = true;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            if (_particles == null)
            {
                Debug.LogWarning($"ParticleSystem reference is missing on {gameObject.name}. Component disabled.");
                enabled = false;
            }

            if (_ignoreTimeScale)
            {
                ParticleSystem.MainModule main = _particles.main;
                main.useUnscaledTime = true;
            }
        }

        private void OnEnable()
        {
            _particles.Stop();
            _particles.Clear();
        }

        #endregion

        #region Interfaces

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_particles.isPlaying is false)
                _particles.Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_particles.isPlaying)
                _particles.Stop();
        }

        #endregion
    }
}