using DG.Tweening;
using UnityEngine;

namespace ToolsACG.UI.Utilitys.Animation
{
    public class UIRandomScaling : MonoBehaviour
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _minScale = 0.9f;
        [SerializeField] private float _maxScale = 1.1f;
        [Space]
        [SerializeField] private float _minDuration = 0.2f;
        [SerializeField] private float _maxDuration = 0.6f;
        [Space]
        [SerializeField] private Ease _ease = Ease.InOutSine;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;

        [Header("Cache")]
        private RectTransform _rectTransform;
        private Vector3 _originalScale;
        private Tween _currentTween;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
        }

        private void OnDestroy()
        {
            _currentTween?.Kill();
        }

        private void OnEnable()
        {
            _currentTween?.Kill();
            _rectTransform.localScale = _originalScale;
            ScaleToRandomValue();
        }

        private void OnDisable()
        {
            _currentTween?.Kill();
        }

        #endregion

        #region Functionality

        private void ScaleToRandomValue()
        {
            float randomFactor = Random.Range(_minScale, _maxScale);
            Vector3 targetScale = _originalScale * randomFactor;
            float duration = Random.Range(_minDuration, _maxDuration);

            _currentTween = _rectTransform.DOScale(targetScale, duration)
                .SetEase(_ease)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(ScaleToRandomValue);
        }

        #endregion
    }
}