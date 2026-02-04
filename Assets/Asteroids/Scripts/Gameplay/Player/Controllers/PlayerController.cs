using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Managers;
using Asteroids.Core.ScriptableObjects.Data;
using System;
using System.Threading.Tasks;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Managers;
using ToolsACG.Core.Utilitys;
using ToolsACG.Core.Utils;
using ToolsACG.Pooling.Gameplay;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Controllers
{
    [RequireComponent(typeof(PooledGameObjectController))]
    [RequireComponent(typeof(ScreenEdgeTeleport))]
    [RequireComponent(typeof(PlayerHealthController))]

    public class PlayerController : MonoBehaviour
    {
        #region Fields and Events

        public event Action<SO_ShipData> PlayerInitialized;
        public event Action PlayerReadyToBeRecycled;

        public event Action GamePaused;
        public event Action<int> RotationKeyStateChanged;
        public event Action<bool> MoveForwardKeyStateChanged;
        public event Action<bool> ShootKeyStateChanged;

        [Header("References")]
        [Inject] private readonly PooledGameObjectController _pooledGameObject;
        [Inject] private readonly ScreenEdgeTeleport _screenEdgeTeleport;
        [Inject] private readonly PlayerHealthController _playerHealthController;
        [Space]
        [Inject] private readonly IPauseManager _pauseManager;
        [Inject] private readonly IInputManager _inputManager;

        [Header("Data")]
        private SO_ShipData _shipData;

        #endregion

        #region Initialization

        public void Initialize(SO_ShipData data)
        {
            _shipData = data;

            _screenEdgeTeleport.SetData(_shipData.ScreenEdgeTeleportConfiguration);

            PlayerInitialized?.Invoke(_shipData);
        }

        #endregion

        #region Monobehavior

        private void OnEnable()
        {
            _playerHealthController.PlayerDied += OnPlayerDied;

            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);

            _pauseManager.GamePaused += OnGamePaused;

            _inputManager.RotationtKeysStateChanged += OnRotationKeyStateChanged;
            _inputManager.MoveForwardKeyStateChanged += OnMoveForwardKeyStateChanged;
            _inputManager.ShootKeyStateChanged += OnShootKeyStateChange;
        }

        private void OnDisable()
        {
            _playerHealthController.PlayerDied -= OnPlayerDied;

            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);

            if (_pauseManager != null)
                _pauseManager.GamePaused -= OnGamePaused;

            if (_inputManager != null)
            {
                _inputManager.RotationtKeysStateChanged -= OnRotationKeyStateChanged;
                _inputManager.MoveForwardKeyStateChanged -= OnMoveForwardKeyStateChanged;
                _inputManager.ShootKeyStateChanged -= OnShootKeyStateChange;
            }
        }

        #endregion

        #region Event callbacks

        private async void OnPlayerDied()
        {
            await TimingUtils.WaitSeconds(_shipData.TimeBeforeRecicle);
            _ = CleanPlayer();
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _ = CleanPlayer();
        }

        private void OnGamePaused()
        {
            GamePaused?.Invoke();
        }

        private void OnRotationKeyStateChanged(int value)
        {
            RotationKeyStateChanged?.Invoke(value);
        }

        private void OnMoveForwardKeyStateChanged(bool state)
        {
            MoveForwardKeyStateChanged?.Invoke(state);
        }

        private void OnShootKeyStateChange(bool state)
        {
            ShootKeyStateChanged?.Invoke(state);
        }

        #endregion

        #region Functionality

        private async Task CleanPlayer()
        {
            PlayerReadyToBeRecycled?.Invoke();
            await TimingUtils.WaitSeconds(Time.deltaTime);
            _pooledGameObject.RecycleGameObject();
        }

        #endregion
    }
}