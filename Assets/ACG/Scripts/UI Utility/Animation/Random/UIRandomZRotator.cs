using DG.Tweening;
using UnityEngine;

namespace ACG.Scripts.UIUtilitys.Animation
{
    public class UIRandomZRotator : MonoBehaviour
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _minAngle = -15f;
        [SerializeField] private float _maxAngle = 15f;
        [Space]
        [SerializeField] private float _minDuration = 0.2f;
        [SerializeField] private float _maxDuration = 0.6f;
        [Space]
        [SerializeField] private Ease _ease = Ease.InOutSine;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;

        [Header("Cache")]
        private RectTransform _rectTransform;
        private Quaternion _originalRotation;
        private Tween _currentTween;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalRotation = _rectTransform.localRotation;
        }

        private void OnDestroy()
        {
            _currentTween?.Kill();
        }

        private void OnEnable()
        {
            _currentTween?.Kill();
            _rectTransform.localRotation = _originalRotation;
            RotateToRandomAngle();
        }

        private void OnDisable()
        {
            _currentTween?.Kill();
        }

        #endregion

        #region Functionality

        private void RotateToRandomAngle()
        {
            float randomAngle = Random.Range(_minAngle, _maxAngle);
            Quaternion targetRotation = _originalRotation * Quaternion.Euler(0, 0, randomAngle);
            float duration = Random.Range(_minDuration, _maxDuration);

            _currentTween = _rectTransform.DOLocalRotateQuaternion(targetRotation, duration)
                .SetEase(_ease)
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(RotateToRandomAngle);
        }

        #endregion
    }
}