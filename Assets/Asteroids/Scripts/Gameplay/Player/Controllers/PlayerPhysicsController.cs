using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerVisualsController))]
    [RequireComponent(typeof(PlayerHealthController))]
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]

    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [Inject] private readonly PlayerController _playerController;
        [Inject] private readonly PlayerVisualsController _playerVisualsController;
        [Inject] private readonly PlayerHealthController _playerHealthController;
        [Space]
        [Inject] private readonly PolygonCollider2D _collider;
        [Inject] private readonly SpriteRenderer _spriteRenderer;

        //[Header("States")]
        public bool InCollisionCooldown { get; private set; }

        [Header("Cache")]
        private Coroutine _cooldownCoroutine;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerController.PlayerInitialized += OnPlayerInitialized;
            _playerController.PlayerReadyToBeRecycled += OnPlayerReadyToBeRecycled;

            _playerHealthController.PlayerDamaged += OnPlayerDamaged;
            _playerHealthController.PlayerShieldStateChanged += OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDied += OnPlayerDied;

            _playerVisualsController.PlayerAppearanceUpdated += OnPlayerAppearanceUpdated;
        }

        private void OnDisable()
        {
            _playerController.PlayerInitialized -= OnPlayerInitialized;
            _playerController.PlayerReadyToBeRecycled -= OnPlayerReadyToBeRecycled;

            _playerHealthController.PlayerDamaged -= OnPlayerDamaged;
            _playerHealthController.PlayerShieldStateChanged -= OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDied -= OnPlayerDied;

            _playerVisualsController.PlayerAppearanceUpdated -= OnPlayerAppearanceUpdated;
        }

        #endregion

        #region Event callbacks

        private void OnPlayerInitialized()
        {
            SetCooldownState(false);
        }

        private void OnPlayerReadyToBeRecycled()
        {
            Cleanup();
        }

        private void OnPlayerDamaged()
        {
            HandleCollision();
        }

        private void OnPlayerShieldStateChanged(bool state)
        {
            if (state)
                return;

            HandleCollision();
        }

        private void OnPlayerDied()
        {
            Cleanup();
        }

        private void OnPlayerAppearanceUpdated()
        {
            ReBuildColliderShape();
        }

        #endregion

        #region General Management

        private void Cleanup()
        {
            StopCooldownCoroutine();
            SetCooldownState(false);
            _collider.enabled = false;
        }

        #endregion

        #region Collision

        private void HandleCollision()
        {
            StartCooldownCoroutine();
        }

        #endregion

        #region Cooldown Management

        private void StartCooldownCoroutine()
        {
            StopCooldownCoroutine();
            _cooldownCoroutine = StartCoroutine(CooldownCoroutine());
        }

        private void StopCooldownCoroutine()
        {
            if (_cooldownCoroutine == null)
                return;

            StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
        }

        private IEnumerator CooldownCoroutine()
        {
            SetCooldownState(true);
            yield return new WaitForSeconds(_playerController.ShipData.InvulnerabilityDuration);
            SetCooldownState(false);
            _cooldownCoroutine = null;
        }

        private void SetCooldownState(bool state)
        {
            InCollisionCooldown = state;
            _collider.enabled = !state;
        }

        #endregion

        #region Functionality

        private void ReBuildColliderShape()
        {
            if (_spriteRenderer.sprite == null)
                return;

            Sprite sprite = _spriteRenderer.sprite;
            _collider.pathCount = sprite.GetPhysicsShapeCount();

            for (int i = 0; i < _collider.pathCount; i++)
            {
                List<Vector2> path = new();
                sprite.GetPhysicsShape(i, path);
                _collider.SetPath(i, path.ToArray());
            }
        }

        #endregion
    }
}