using DG.Tweening;
using UnityEngine;

namespace ACG.Scripts.UIUtilitys.Animation
{
    public class UIRandomFloating : MonoBehaviour
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _movementRadius = 10f;
        [Space]
        [SerializeField] private float _minDuration = 0.2f;
        [SerializeField] private float _maxDuration = 0.6f;
        [Space]
        [SerializeField] private Ease _ease = Ease.InOutSine;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;

        [Header("Cache")]
        private RectTransform _rectTransform;
        private Vector2 _originaltPosition;
        private Tween _currentTween;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originaltPosition = _rectTransform.anchoredPosition;
        }

        private void OnDestroy()
        {
            _currentTween?.Kill();
        }

        private void OnEnable()
        {
            _currentTween?.Kill();
            _rectTransform.anchoredPosition = _originaltPosition;
            MoveToRandomPoint();
        }

        private void OnDisable()
        {
            _currentTween?.Kill();
        }

        #endregion

        #region Functionality

        private void MoveToRandomPoint()
        {
            Vector2 targetPosition = _originaltPosition + Random.insideUnitCircle * _movementRadius;
            float duration = Random.Range(_minDuration, _maxDuration);

            _currentTween = _rectTransform.DOAnchorPos(targetPosition, duration)
                .SetEase(_ease)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(MoveToRandomPoint);
        }

        #endregion
    }
}