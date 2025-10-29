using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToolsACG.UI.Utilitys.Animation
{
    public class UIScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _scaleFactor = 1.2f;
        [SerializeField] private float _duration = 0.2f;
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
            _currentTween?.Kill();
        }

        private void OnEnable()
        {
            _currentTween?.Kill();
            transform.localScale = _originalScale;
        }

        private void OnDisable()
        {
            _currentTween?.Kill();
        }

        #endregion

        #region Interfaces

        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentTween?.Kill();
            _currentTween = transform.DOScale(_originalScale * _scaleFactor, _duration)
                              .SetEase(Ease.OutBack)
                              .SetUpdate(_ignoreTimeScale);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _currentTween?.Kill();
            _currentTween = transform.DOScale(_originalScale, _duration)
                              .SetEase(Ease.OutBack)
                              .SetUpdate(_ignoreTimeScale);
        }

        #endregion
    }
}
