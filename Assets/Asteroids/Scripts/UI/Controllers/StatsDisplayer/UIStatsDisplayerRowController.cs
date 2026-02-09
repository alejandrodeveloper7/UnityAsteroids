using ACG.Core.Models;
using ACG.Scripts.UIControllers;
using Asteroids.Core.Interfaces.Enums;
using Asteroids.Core.Interfaces.Models;
using Asteroids.Core.ScriptableObjects.Configurations;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Controllers
{
    public class UIStatsDisplayerRowController : MonoBehaviour
    {
        #region Fields

        public StatIdType Id { get; private set; }

        [Header("Gampleplay References")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private UISliderController _sliderController;

        [Header("Data")]
        [Inject] private readonly SO_StatsDisplayerConfiguration _displayerConfiguration;
        [Space]
        private StatConfiguration _statConfiguration;

        #endregion

        #region Initialization

        public void Initialize(StatConfiguration statConfiguration)
        {
            _statConfiguration = statConfiguration;            
            Id = statConfiguration.Id;

            SetName(_statConfiguration.DisplayName);
            SetValueLimits(_statConfiguration.ValueRange);
            SetValue(_statConfiguration.ValueRange.Min,false);
        }

        #endregion

        #region Functionality
        
        public void SetName(string name)
        {
            _nameText.text = name;
        }
        
        public void SetValueLimits(FloatRange range)
        {
            _sliderController.SetLimitValues(range.Min, range.Max);
        }

        public void SetValue(float value, bool progressively)
        {
            if (_statConfiguration.IsReverseValue)
                value = _sliderController.MaxValue - value;

            if (progressively)
                SetValueProgresively(value);
            else
                SetValueInstantly(value);
        }

        private void SetValueInstantly(float value)
        {
            Color targetColor = GetColorFromValue(value);

            _sliderController.SetValueInstantly(value);
            _sliderController.SetFillColor(targetColor);
        }

        private void SetValueProgresively(float value)
        {
            float duration = _displayerConfiguration.ValueChangeTransitionDuration;
            Color targetColor = GetColorFromValue(value);

            _ = _sliderController.SetValueProgresively(value, duration);
            _ = _sliderController.LerpFillColor(targetColor, duration);
        }

        #endregion

        #region Utility

        private Color GetColorFromValue(float value)
        {
            float maxValue = _sliderController.MaxValue;

            if (value < maxValue * _displayerConfiguration.LowStatThreshold)
                return _displayerConfiguration.LowStatColor;

            if (value < maxValue * _displayerConfiguration.MidStatThreshold)
                return _displayerConfiguration.MidStatColor;

            return _displayerConfiguration.HighStatColor;
        }

        #endregion
    }
}