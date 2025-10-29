using Asteroids.Core.ScriptableObjects.Data;
using ToolsACG.Core.Services;
using UnityEngine;
using System.Collections;
using Asteroids.Gameplay.Bullets.Spawners;
using Zenject;

namespace Asteroids.Gameplay.Player.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerHealthController))]

    public class PlayerCombatController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private Transform _bulletSpawnPoint;
        [Space]
        [Inject] private readonly PlayerController _playerController;
        [Inject] private readonly PlayerHealthController _playerHealthController;
        [Space]
        [Inject] private readonly BulletSpawner _bulletSpawner;
        [Space]
        [Inject] private readonly IContainerRuntimeDataService _runtimeDataService;

        [Header("States")]
        private bool _isAlive;
        private bool _shooting;
        [Space]
        private float _nextShootTime;

        [Header("Data")]
        private SO_ShipData _shipData;
        private SO_BulletData _bulletData;

        [Header("Cache")]
        private Coroutine _shootRoutine;

        #endregion

        #region Initialization

        private void Initialize()
        {
            _bulletData = _runtimeDataService.Data.SelectedBulletData;

            _bulletSpawnPoint.transform.localPosition = _shipData.BulletsSpawnPointsLocalPosition;
            _isAlive = true;
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerController.PlayerInitialized += OnPlayerInitialized;
            _playerController.PlayerReadyToRecycle += OnPlayerReadyToRecycle;
            _playerController.GamePaused += OnGamePaused;
            _playerController.ShootKeyStateChanged += OnShootKeyStateChange;

            _playerHealthController.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            _playerController.PlayerInitialized -= OnPlayerInitialized;
            _playerController.PlayerReadyToRecycle -= OnPlayerReadyToRecycle;
            _playerController.GamePaused -= OnGamePaused;
            _playerController.ShootKeyStateChanged -= OnShootKeyStateChange;

            _playerHealthController.PlayerDied -= OnPlayerDied;
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
            Cleanup();
        }

        private void OnGamePaused()
        {
            _shooting = false;
        }

        private void OnShootKeyStateChange(bool state)
        {
            _shooting = state;

            if (_shooting)
                StartShooting();
            else
                StopShooting();
        }

        private void OnPlayerDied()
        {
            Cleanup();
        }

        #endregion

        #region Shoot Management

        private void Cleanup()
        {
            StopShooting();
            _isAlive = false;
            _shooting = false;
            _nextShootTime = 0;
        }

        private void StartShooting()
        {
            _shootRoutine ??= StartCoroutine(ShootLoop());

        }
        private void StopShooting()
        {
            if (_shootRoutine != null)
            {
                StopCoroutine(_shootRoutine);
                _shootRoutine = null;
            }
        }

        private IEnumerator ShootLoop()
        {
            while (_shooting && _isAlive)
            {
                if (Time.time >= _nextShootTime)
                {
                    ShootBullet();
                    _nextShootTime = Time.time + _bulletData.BetweenBulletsTime;
                    yield return new WaitForSeconds(_bulletData.BetweenBulletsTime);
                }

                yield return null;
            }

            _shootRoutine = null;
        }

        #endregion

        #region Bullet creation

        private void ShootBullet()
        {
            _bulletSpawner.Spawn(_bulletData, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        }

        #endregion
    }
}