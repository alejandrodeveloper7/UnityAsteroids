using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToolsACG.UI.Utilitys.Animation
{
    public class UIRandomFloatingOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

        [Header("States")]
        private bool _isHovering;
        
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
        }
        
        private void OnDisable()
        {
            _currentTween?.Kill();
        }

        #endregion

        #region Pointer Events

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
            MoveToRandomPoint();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovering = false;
            _currentTween?.Kill();
            _currentTween = _rectTransform.DOAnchorPos(_originalPosition, 0.3f).SetEase(_ease);
        }

        #endregion

        #region Functionality

        private void MoveToRandomPoint()
        {
            if (!_isHovering)
                return;

            Vector2 targetPosition = _originalPosition + Random.insideUnitCircle * _movementRadius;
            float duration = Random.Range(_minDuration, _maxDuration);

            _currentTween = _rectTransform.DOAnchorPos(targetPosition, duration)
                .SetEase(_ease)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(MoveToRandomPoint);
        }

        #endregion
    }
}