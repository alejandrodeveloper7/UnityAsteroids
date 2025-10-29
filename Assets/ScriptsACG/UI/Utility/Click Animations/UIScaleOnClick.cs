using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToolsACG.UI.Utilitys.Animation
{
    public class UIScaleOnClick : MonoBehaviour, IPointerClickHandler
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _scaleFactor = 1.2f;
        [SerializeField] private float _duration = 0.2f;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;

        [Header("Cache")]
        private Vector3 _originalScale;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        private void OnEnable()
        {
            transform.DOKill();
            transform.localScale = _originalScale;
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        #endregion

        #region Functionality

        public async void OnPointerClick(PointerEventData eventData)
        {
            transform.DOKill();

            await transform.DOScale(_originalScale * _scaleFactor, _duration * 0.5f)
                .SetEase(Ease.OutBack)
                .SetUpdate(_ignoreTimeScale)
                .AsyncWaitForCompletion();

            await transform.DOScale(_originalScale, _duration * 0.5f)
                .SetEase(Ease.InBack)
                .SetUpdate(_ignoreTimeScale)
                .AsyncWaitForCompletion();
        }

        #endregion
    }
}