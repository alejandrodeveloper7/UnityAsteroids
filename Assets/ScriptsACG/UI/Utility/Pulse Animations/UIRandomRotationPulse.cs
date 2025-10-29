using DG.Tweening;
using UnityEngine;

namespace ToolsACG.UI.Utilitys.Animation
{
    public class UIRandomRotationPulse : MonoBehaviour
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _angle = 15f;
        [SerializeField] private float _returnOffsetAngle = 3f;
        [SerializeField] private float _duration = 0.2f;
        [Space]
        [SerializeField] private float _minDelay = 3f;
        [SerializeField] private float _maxDelay = 8f;
        [Space]
        [SerializeField] private Ease _ease = Ease.InOutSine;
        [Space]
        [SerializeField] private bool _ignoreTimeScale = true;

        [Header("Cache")]
        private Quaternion _originalRotation;
        private Tween _currentTween;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _originalRotation = transform.localRotation;
        }

        private void OnDestroy()
        {
            _currentTween?.Kill();
        }

        private void OnEnable()
        {
            _currentTween?.Kill();
            transform.localRotation = _originalRotation;
            DoRandomRotationPulse();
        }

        private void OnDisable()
        {
            _currentTween?.Kill();
        }

        #endregion

        #region Functionality

        private void DoRandomRotationPulse()
        {
            float delay = Random.Range(_minDelay, _maxDelay);
            float direction = Random.value < 0.5f ? -1f : 1f;
            Quaternion targetRotation = _originalRotation * Quaternion.Euler(0, 0, _angle * direction);
            Quaternion returnRotation = _originalRotation * Quaternion.Euler(0, 0, Random.Range(-_returnOffsetAngle, _returnOffsetAngle));

            _currentTween?.Kill();

            _currentTween = DOTween.Sequence()
                .SetUpdate(_ignoreTimeScale)
                .AppendInterval(delay)
                .Append(transform.DOLocalRotateQuaternion(targetRotation, _duration).SetEase(_ease))
                .Append(transform.DOLocalRotateQuaternion(returnRotation, _duration).SetEase(_ease))
                .OnComplete(DoRandomRotationPulse);
        }

        #endregion
    }
}