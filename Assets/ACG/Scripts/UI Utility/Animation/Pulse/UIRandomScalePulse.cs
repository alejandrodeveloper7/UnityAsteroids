using DG.Tweening;
using UnityEngine;

namespace ACG.Scripts.UIUtilitys.Animation
{
    public class UIRandomScalePulse : MonoBehaviour
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private float _scaleFactor = 1.2f;
        [SerializeField] private float _returnScaleOffset = 0.05f;
        [SerializeField] private float _duration = 0.2f;
        [Space]
        [SerializeField] private float _minDelay = 3f;
        [SerializeField] private float _maxDelay = 8f;
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
            DoRandomScalePulse();
        }

        private void OnDisable()
        {
            _currentTween?.Kill();
        }

        #endregion

        #region Functionality

        private void DoRandomScalePulse()
        {
            float delay = Random.Range(_minDelay, _maxDelay);
            float randomScaleOffset = Random.Range(1f - _returnScaleOffset, 1f + _returnScaleOffset);
            float duration = Random.Range(_duration * 0.8f, _duration * 1.2f);
            Vector3 returnScale = _originalScale * randomScaleOffset;
          
            _currentTween?.Kill();

            _currentTween = DOTween.Sequence()
                .AppendInterval(delay)
                .Append(transform.DOScale(_originalScale * _scaleFactor, duration).SetEase(Ease.OutQuad))
                .Append(transform.DOScale(returnScale, duration).SetEase(Ease.InQuad))
                .SetUpdate(_ignoreTimeScale)
                .OnComplete(DoRandomScalePulse);
        }

        #endregion
    }
}