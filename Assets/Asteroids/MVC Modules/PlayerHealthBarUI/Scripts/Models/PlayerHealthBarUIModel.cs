using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.MVC.PlayerHealthBarUI.ScriptableObjects;
using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.PlayerHealthBarUI.Models
{
    public class PlayerHealthBarUIModel : MVCModelBase
    {
        #region Fields and Properties

        [Header("Health")]
        private int _currentHealth;

        [Header("Shield")]
        private float _currentShieldSliderValue;
        public bool ShieldActive { get; private set; }

        [Header("Cache")]
        private Tween _shieldChargeTween;

        #endregion

        #region Events

        public event Action<int> HealthUpdated;
        public event Action<int> HealthRestarted;

        public event Action<float> ShieldSliderValueUpdated;
        public event Action<float> ShieldRestored;
        public event Action<float> ShieldRestarted;

        #endregion

        #region Constructors

        [Inject]
        public PlayerHealthBarUIModel(SO_PlayerHealthBarUIConfiguration configuration)
        {
            //TODO: Initialize the model with the Configuration SO and other data
        }

        #endregion

        #region Health Management

        public void RestartHealth(int newHealth)
        {
            _currentHealth = newHealth;
            HealthRestarted?.Invoke(_currentHealth);
        }

        public void SetCurrentHealth(int newHealth)
        {
            _currentHealth = newHealth;
            HealthUpdated?.Invoke(_currentHealth);
        }

        #endregion

        #region Shield Management

        public void RestartShield(float newShieldSliderValue)
        {
            _currentShieldSliderValue = newShieldSliderValue;
            ShieldActive = true;
            ShieldRestarted?.Invoke(_currentShieldSliderValue);
        }

        public void ShieldLost() 
        {
            ShieldActive = false;
        }

        public void StartShieldRecoveryProcess(float minValue, float maxValue, float duration)
        {
            float currentValue = minValue;

            _shieldChargeTween = DOTween.To(() => currentValue, x => currentValue = x, maxValue, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    _currentShieldSliderValue = currentValue;
                    ShieldSliderValueUpdated?.Invoke(_currentShieldSliderValue);
                })
                .OnComplete(() =>
                {
                    ShieldActive = true;
                    ShieldRestored?.Invoke(maxValue);
                });

        }

        public void StopShieldRecoveryProcess()
        {
            _shieldChargeTween?.Kill();
        }

        #endregion
    }
}