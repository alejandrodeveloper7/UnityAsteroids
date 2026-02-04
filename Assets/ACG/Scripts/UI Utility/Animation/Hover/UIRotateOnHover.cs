using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ACG.Scripts.UIUtilitys.Animation
{
    public class UIRotateOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _targetRotationZ = 15f;
        [SerializeField] private float _duration = 0.2f;
        [Space]
        [SerializeField] private Ease _ease = Ease.OutQuad;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;

        [Header("Cache")]
        private Vector3 _originalRotation;
        private Tween _currentTween;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _originalRotation = transform.localEulerAngles;
        }

        private void OnDestroy()
        {
            _currentTween?.Kill();
        }

        private void OnEnable()
        {
            _currentTween?.Kill();
            transform.localEulerAngles = _originalRotation;
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
            Vector3 targetRotation = _originalRotation + new Vector3(0, 0, _targetRotationZ);
            _currentTween = transform.DOLocalRotate(targetRotation, _duration).SetEase(_ease)
                              .SetUpdate(_ignoreTimeScale);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _currentTween?.Kill();
            _currentTween = transform.DOLocalRotate(_originalRotation, _duration).SetEase(_ease)
                              .SetUpdate(_ignoreTimeScale);

        }

        #endregion
    }
}