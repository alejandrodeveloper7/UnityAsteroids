using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using Asteroids.Core.ScriptableObjects.Data;
using System;
using System.Collections;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Services;
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

        [Header("Data")]
        private SO_ShipData _shipData;

        [Header("Cache")]
        private Coroutine _currentShieldRecoveryCoroutine;

        #endregion

        #region Initialization

        private void Initialize()
        {
            _health = _shipData.HealthPoints;

            _isAlive = true;
            _shieldActive = true;
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerController.PlayerInitialized += OnPlayerInitialized;
            _playerController.PlayerReadyToRecycle += OnPlayerReadyToRecycle;
        }

        private void OnDisable()
        {
            _playerController.PlayerInitialized -= OnPlayerInitialized;
            _playerController.PlayerReadyToRecycle -= OnPlayerReadyToRecycle;
        }

        #endregion

        #region Event Callbacks

        private void OnPlayerInitialized(SO_ShipData data)
        {
            _shipData = data;
            Initialize();
        }

        private void OnPlayerReadyToRecycle()
        {
            CancelShieldRecovery();
        }

        #endregion

        #region Damage Management

        public void TakeDamage(DamageInfo data)
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
            _debugService.Log("Player", "Shield lost", Color.cyan, Color.red);
            _currentShieldRecoveryCoroutine = StartCoroutine(RecoverShieldAfterDelay(_shipData.ShieldRecoveryTime));

            PlayerShieldStateChanged?.Invoke(false);
            EventBusManager.GameplayBus.RaiseEvent(new PlayerShieldStateChanged(false));
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

        private void Die()
        {
            _isAlive = false;
            _debugService.Log("Player", "Dead", Color.cyan, Color.red);
            CancelShieldRecovery();

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