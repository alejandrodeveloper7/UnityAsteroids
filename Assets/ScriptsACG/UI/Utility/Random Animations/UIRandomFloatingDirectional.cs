using UnityEngine;
using DG.Tweening;

namespace ToolsACG.UI.Utilitys.Animation
{
    public class UIRandomFloatingDirectional : MonoBehaviour
    {
        #region Auxiliary enums

        private enum Direction
        {
            Horizontal,
            Vertical
        }

        #endregion

        #region Fields

        [Header("Configuration")]
        [SerializeField] private Direction _direction = Direction.Horizontal;
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
        private Vector2 _originalPosition;
        private Tween _currentTween;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
        }

        private void OnDestroy()
        {
            _currentTween?.Kill();
        }

        private void OnEnable()
        {
            _currentTween?.Kill();
            _rectTransform.anchoredPosition = _originalPosition;
            MoveToNextPoint();
        }

        private void OnDisable()
        {
            _currentTween?.Kill();
        }

        #endregion

        #region Functionality

        private void MoveToNextPoint()
        {
            Vector2 targetPosition = _originalPosition;

            // Mueve solo en un eje
            if (_direction == Direction.Horizontal)
            {
                targetPosition.x += Random.Range(-_movementRadius, _movementRadius);
            }
            else // Vertical
            {
                targetPosition.y += Random.Range(-_movementRadius, _movementRadius);
            }

            float duration = Random.Range(_minDuration, _maxDuration);

            _currentTween = _rectTransform.DOAnchorPos(targetPosition, duration)
                .SetEase(_ease)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(MoveToNextPoint);
        }

        #endregion
    }
}