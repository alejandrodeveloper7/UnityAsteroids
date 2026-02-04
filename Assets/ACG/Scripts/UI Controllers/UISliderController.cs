using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ACG.Scripts.UIControllers
{
    public class UISliderController : MonoBehaviour
    {
        #region Fields, properties and events

        public Action OnSliderFilled { get; set; }

        public float Value { get { return _slider.value; } }
        public float MinValue { get { return _slider.minValue; } }
        public float MaxValue { get { return _slider.maxValue; } }

        [Header("References")]
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _sliderFillImage;

        #endregion

        #region General Management

        public void SetLimitValues(float minValue, float maxValue)
        {
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
        }

        public void Restart()
        {
            StopSetValueProgresivelyTween();
            _slider.value = 0;
        }

        #endregion

        #region Value Management

        public void SetValueInstantly(float value)
        {
            StopSetValueProgresivelyTween();

            _slider.value = value;
            CheckSliderFilled();
        }

        public async Task SetValueProgresively(float value, float time)
        {
            StopSetValueProgresivelyTween();

            await _slider.DOValue(value, time).AsyncWaitForCompletion();
            CheckSliderFilled();
        }

        private void StopSetValueProgresivelyTween()
        {
            _slider.DOKill();
        }

        #endregion

        #region Visuals

        public void SetFillColor(Color newColor)
        {
            StopLerpFillColorTween();
            _sliderFillImage.color = newColor;
        }

        public async Task LerpFillColor(Color newColor, float duration)
        {
            StopLerpFillColorTween();
            await _sliderFillImage.DOColor(newColor, duration).AsyncWaitForCompletion();
        }

        private void StopLerpFillColorTween()
        {
            _sliderFillImage.DOKill();
        }

        #endregion

        #region Events

        private void CheckSliderFilled()
        {
            if (Value < MaxValue)
                return;

            OnSliderFilled?.Invoke();
        }

        #endregion
    }
}