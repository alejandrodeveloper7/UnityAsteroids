using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToolsACG.UI.Utilitys.Animation
{
    public class UIScaleOnHoverAndClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _hoverScale = 1.2f;
        [SerializeField] private float _hoverDuration = 0.3f;
        [Space]
        [SerializeField] private float _clickScale = 1.3f;
        [SerializeField] private float _clickDuration = 0.1f;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;

        [Header("Cache")]
        private Vector3 _originalScale;
        private Tween _currentTween;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void OnDestroy()
        {
            StopTweens();
        }

        private void OnEnable()
        {
            StopTweens();
            transform.localScale = _originalScale;
        }

        private void OnDisable()
        {
            StopTweens();
        }

        #endregion

        #region Functionality

        public void OnPointerEnter(PointerEventData eventData)
        {
            ScaleTo(_hoverScale, _hoverDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ScaleTo(1, _hoverDuration);
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            StopTweens();

            await transform.DOScale(_originalScale * _clickScale, _clickDuration * 0.5f)
                   .SetEase(Ease.OutBack)
                   .SetUpdate(_ignoreTimeScale)
                   .AsyncWaitForCompletion();

            float targetScale = eventData.pointerEnter == gameObject ? _hoverScale : 1;

            transform.DOScale(_originalScale * targetScale, _clickDuration * 0.5f)
                .SetEase(Ease.OutBack)
                .SetUpdate(_ignoreTimeScale);
        }

        private void ScaleTo(float factor, float duration)
        {
            StopTweens();

            _currentTween = transform.DOScale(_originalScale * factor, duration)
                .SetEase(Ease.OutBack)
                .SetUpdate(_ignoreTimeScale);
        }

        private void StopTweens() 
        {
            _currentTween?.Kill();
            transform.DOKill();
        }

        #endregion
    }
}