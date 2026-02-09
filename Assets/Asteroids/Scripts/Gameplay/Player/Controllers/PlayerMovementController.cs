using Asteroids.Core.ScriptableObjects.Data;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerHealthController))]
    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerMovementController : MonoBehaviour
    {
        #region Fields

        public event Action<bool> ForwardMovementStateChanged;

        [Header("References")]
        [Inject] private readonly PlayerController _playerController;
        [Inject] private readonly PlayerHealthController _playerHealthController;
        [Space]
        [Inject] private readonly Rigidbody2D _rigidBody;

        [Header("States")]
        private bool _isAlive;
        private bool _isMovingForward;
        private int _rotationValue;

        [Header("Cache")]
        private Coroutine _moveForwardCoroutine;
        private Coroutine _rotationCoroutine;

        [Header("Data")]
        private SO_ShipData _shipData;

        #endregion

        #region Initilization

        private void Initialize()
        {
            _isAlive = true;
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerController.PlayerInitialized += OnPlayerInitialized;
            _playerController.PlayerReadyToBeRecycled += OnPlayerReadyToBeRecycled;
            _playerController.GamePaused += OnGamePaused;
            _playerController.RotationKeyStateChanged += OnRotationKeyStateChanged;
            _playerController.MoveForwardKeyStateChanged += OnMoveForwardKeyStateChanged;

            _playerHealthController.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            _playerController.PlayerInitialized -= OnPlayerInitialized;
            _playerController.PlayerReadyToBeRecycled -= OnPlayerReadyToBeRecycled;
            _playerController.GamePaused -= OnGamePaused;
            _playerController.RotationKeyStateChanged -= OnRotationKeyStateChanged;
            _playerController.MoveForwardKeyStateChanged -= OnMoveForwardKeyStateChanged;

            _playerHealthController.PlayerDied -= OnPlayerDied;
        }

        #endregion

        #region Event Callbacks

        private void OnPlayerInitialized(SO_ShipData data)
        {
            _shipData = data;
            Initialize();
        }

        private void OnPlayerReadyToBeRecycled()
        {
            Cleanup();
            StopPlayer();
        }

        private void OnGamePaused()
        {
            ResetMovementState();
        }

        private void OnRotationKeyStateChanged(int value)
        {
            _rotationValue = value;

            if (_rotationValue != 0 && _rotationCoroutine == null)
                StartRotation();
            else if (_rotationValue == 0 && _rotationCoroutine != null)
                StopRotation();
        }

        private void OnMoveForwardKeyStateChanged(bool state)
        {
            _isMovingForward = state;
            ForwardMovementStateChanged?.Invoke(_isMovingForward);

            if (_isMovingForward)
                StartForwardMovement();
            else
                StopForwardMovement();
        }

        private void OnPlayerDied()
        {
            Cleanup();
            StopPlayer();
        }

        #endregion

        #region Forward Movement

        private void StartForwardMovement()
        {
            _moveForwardCoroutine ??= StartCoroutine(MoveForwardCoroutine());
        }

        private void StopForwardMovement()
        {
            if (_moveForwardCoroutine == null)
                return;

            StopCoroutine(_moveForwardCoroutine);
            _moveForwardCoroutine = null;
        }

        private IEnumerator MoveForwardCoroutine()
        {
            while (_isMovingForward && _isAlive)
            {
                _rigidBody.AddForce(_shipData.MovementSpeed * Time.deltaTime * -transform.up, ForceMode2D.Impulse);
                yield return null;
            }

            _moveForwardCoroutine = null;
        }

        private void StopPlayer()
        {
            _rigidBody.velocity = Vector3.zero;
        }

        #endregion

        #region Rotation

        private void StartRotation()
        {
            StopRotation();
            _rotationCoroutine = StartCoroutine(RotateCoroutine());
        }

        private void StopRotation()
        {
            if (_rotationCoroutine == null)
                return;

            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }

        private IEnumerator RotateCoroutine()
        {
            while (_rotationValue != 0 && _isAlive)
            {
                transform.Rotate(0, 0, _rotationValue * _shipData.RotationSpeed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }

            _rotationCoroutine = null;
        }

        #endregion

        #region Cleanup 

        private void Cleanup()
        {
            _isAlive = false;
            ResetMovementState();
        }

        private void ResetMovementState()
        {
            _rotationValue = 0;
            _isMovingForward = false;
        }

        #endregion
    }
}