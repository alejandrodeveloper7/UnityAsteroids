using DG.Tweening;
using System;
using UnityEngine;

namespace ToolsACG.Scenes.PlayerHealthBarUI
{
    public class PlayerHealthBarUIModel : ModuleModel
    {
        #region Fields

        [Header("Health")]
        private int _currentHealth; public int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;
                OnHealthUpdated(_currentHealth);
            }
        }

        [Header("Shield")]
        private float _shieldRecoveryTime;
        private float _shieldSliderMaxValue;
        private float _shieldLostSliderMinValue;
        private float _currentShieldSliderValue; public float CurrentShieldSliderValue
        {
            get { return _currentShieldSliderValue; }
            set
            {
                _currentShieldSliderValue = value;
                OnShieldSliderValueUpdated(_currentShieldSliderValue);
            }
        }

        [Header("Allocation")]
        private Tween _shieldChargeTween;

        #endregion

        #region Actions

        public event Action<int> OnHealthUpdated;
        public event Action<float> OnShieldSliderValueUpdated;

        #endregion

        #region Constructors

        public PlayerHealthBarUIModel(SO_PlayerHealthBarUIConfiguration Configuration, PlayerSettings PlayerSettings)
        {
            _shieldRecoveryTime = PlayerSettings.ShieldRecoveryTime;
            _shieldLostSliderMinValue = Configuration.ShieldLostSliderMinValue;
            _shieldSliderMaxValue = Configuration.ShielSliderMaxValue;
        }

        #endregion

        #region public Methods

        public void StartShieldRecoveryProcess()
        {
            float currentValue = _shieldLostSliderMinValue;

            _shieldChargeTween = DOTween.To(() => currentValue, x => currentValue = x, _shieldSliderMaxValue, _shieldRecoveryTime)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    CurrentShieldSliderValue = currentValue;
                })
                .OnComplete(() =>
                {
                    CurrentShieldSliderValue = _shieldSliderMaxValue;
                    EventManager.GameplayBus.RaiseEvent(new ShieldStateChanged(true));
                });
        }

        public void StopShieldRecoveryProcess()
        {
            _shieldChargeTween?.Kill();
        }

        #endregion
    }
}