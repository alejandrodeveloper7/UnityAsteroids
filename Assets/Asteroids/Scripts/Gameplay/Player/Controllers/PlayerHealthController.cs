using ACG.Core.EventBus;
using ACG.Scripts.Services;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerPhysicsController))]

    public class PlayerHealthController : MonoBehaviour, IDamageable
    {
        #region Fields and events

        public event Action<bool> PlayerShieldStateChanged;
        public event Action PlayerDamaged;
        public event Action PlayerDied;

        [Header("References")]
        [Inject] private readonly PlayerController _playerController;
        [Inject] private readonly PlayerPhysicsController _playerPhysicsController;
        [Space]
        [Inject] private readonly IDebugService _debugService;

        [Header("Stats")]
        private int _health;

        [Header("States")]
        private bool _shieldActive;
        private bool _isAlive;

        [Header("Cache")]
        private Coroutine _currentShieldRecoveryCoroutine;

        #endregion

        #region Initialization

        private void Initialize()
        {
            _health = _playerController.ShipData.HealthPoints;

            _isAlive = true;
            _shieldActive = true;
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerController.PlayerInitialized += OnPlayerInitialized;
            _playerController.PlayerReadyToBeRecycled += OnPlayerReadyToBeRecycled;
        }

        private void OnDisable()
        {
            _playerController.PlayerInitialized -= OnPlayerInitialized;
            _playerController.PlayerReadyToBeRecycled -= OnPlayerReadyToBeRecycled;
        }

        #endregion

        #region Event Callbacks

        private void OnPlayerInitialized()
        {
            Initialize();
        }

        private void OnPlayerReadyToBeRecycled()
        {
            CancelShieldRecovery();
        }

        #endregion

        #region Damage Management

        public void TakeDamage(DamageData data)
        {
            if (_playerPhysicsController.InCollisionCooldown)
                return;

            if (_shieldActive)
                LoseShield();
            else
                LoseHealth(data.Amount);
        }

        private void LoseShield()
        {
            _shieldActive = false;
            _currentShieldRecoveryCoroutine = StartCoroutine(RecoverShieldAfterDelay(_playerController.ShipData.ShieldRecoveryTime));

            PlayerShieldStateChanged?.Invoke(false);
            EventBusManager.GameplayBus.RaiseEvent(new PlayerShieldStateChanged(false));
            
            _debugService.Log("Player", "Shield lost", Color.cyan, Color.red);
        }

        private void LoseHealth(int amount)
        {
            _health -= amount;
            
            _debugService.Log("Player", $"{amount} Damage taked", Color.cyan, Color.red);

            PlayerDamaged?.Invoke();
            EventBusManager.GameplayBus.RaiseEvent(new PlayerDamaged(_health));

            if (_isAlive && _health <= 0)
                Die();
        }

        public void Die()
        {
            _isAlive = false;
            CancelShieldRecovery();

            _debugService.Log("Player", "Dead", Color.cyan, Color.red);

            PlayerDied?.Invoke();
            EventBusManager.GameplayBus.RaiseEvent(new PlayerDied());
        }

        #endregion

        #region Shield

        private IEnumerator RecoverShieldAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _shieldActive = true;

            _debugService.Log("Player", "Shield recovered", Color.cyan);

            PlayerShieldStateChanged?.Invoke(true);
            EventBusManager.GameplayBus.RaiseEvent(new PlayerShieldStateChanged(true));
        }

        private void CancelShieldRecovery()
        {
            if (_currentShieldRecoveryCoroutine != null)
                StopCoroutine(_currentShieldRecoveryCoroutine);
        }

        #endregion
    }
}