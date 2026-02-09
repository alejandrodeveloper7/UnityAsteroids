using ACG.Core.Utils;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Core.Services;
using Asteroids.MVC.PlayerHealthBarUI.ScriptableObjects;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.PlayerHealthBarUI.Models
{
    public class PlayerHealthBarUIModel : MVCModelBase
    {
        #region Fields and Properties

        [Header("references")]
        private readonly IContainerRuntimeDataService _containerRuntimeDataService;

        [Header("Values")]

        private int _currentHealth;

        private float _currentShieldSliderValue;
        public bool ShieldActive { get; private set; }

        public SO_ShipData ShipData { get; private set; }

        [Header("Data")]
        private readonly SO_PlayerHealthBarUIConfiguration _configuration;

        [Header("Cache")]
        private Tween _shieldChargeTween;

        #endregion

        #region Events

        public event Action<SO_ShipData> RunInitialized;

        public event Action<int> HealthUpdated;
        public event Action<int> HealthRestarted;

        public event Action<float> ShieldSliderValueUpdated;
        public event Action ShieldLost;
        public event Action<float> ShieldRestored;
        public event Action<float> ShieldRestarted;

        #endregion

        #region Constructors

        [Inject]
        public PlayerHealthBarUIModel(SO_PlayerHealthBarUIConfiguration configuration, IContainerRuntimeDataService containerRuntimeDataService)
        {
            _configuration = configuration;
            _containerRuntimeDataService = containerRuntimeDataService;
        }

        #endregion

        #region Health Management

        private void RestartHealth()
        {
            _currentHealth = ShipData.HealthPoints;
            HealthRestarted?.Invoke(_currentHealth);
        }

        public void SetCurrentHealth(int newHealth)
        {
            _currentHealth = newHealth;
            HealthUpdated?.Invoke(_currentHealth);
        }

        #endregion

        #region Initialization

        public void InitializeRun()
        {
            UpdateShipData();

            RunInitialized?.Invoke(ShipData);

            StopShieldRecoveryProcess();
            RestartHealth();
            RestartShield();
        }

        #endregion

        #region Ship data management

        private void UpdateShipData()
        {
            ShipData = _containerRuntimeDataService.Data.SelectedShipData;
        }

        #endregion

        #region Shield Management

        public async Task StartShieldLostProcess()
        {
            if (ShieldActive is false)
                return;

            ShieldActive = false;
            ShieldLost?.Invoke();
            await TimingUtils.WaitSeconds(_configuration.ShieldSliderTransitionDuration);
            StartShieldRecoveryProcess();
        }

        private void StartShieldRecoveryProcess()
        {
            float shieldRecoveryDuration = ShipData.ShieldRecoveryTime - _configuration.ShieldSliderTransitionDuration;
            float currentValue = ShipData.ShieldSliderValueRange.Min;

            _shieldChargeTween = DOTween.To(() => currentValue, x => currentValue = x, ShipData.ShieldSliderValueRange.Max, shieldRecoveryDuration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    _currentShieldSliderValue = currentValue;
                    ShieldSliderValueUpdated?.Invoke(_currentShieldSliderValue);
                })
                .OnComplete(() =>
                {
                    ShieldActive = true;
                    ShieldRestored?.Invoke(ShipData.ShieldSliderValueRange.Max);
                });

        }

        public void StopShieldRecoveryProcess()
        {
            _shieldChargeTween?.Kill();
            _shieldChargeTween = null;
        }

        private void RestartShield()
        {
            _currentShieldSliderValue = ShipData.ShieldSliderValueRange.Max;
            ShieldActive = true;
            ShieldRestarted?.Invoke(_currentShieldSliderValue);
        }

        #endregion
    }
}