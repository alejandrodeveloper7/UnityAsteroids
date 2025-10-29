using Asteroids.Core.ScriptableObjects.Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Controllers
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerHealthController))]
    [RequireComponent(typeof(PlayerVisualsController))]

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

        [Header("Data")]
        private SO_ShipData _shipData;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerController.PlayerInitialized += OnPlayerInitialized;
            _playerController.PlayerReadyToRecycle += OnPlayerReadyToRecycle;

            _playerHealthController.PlayerDamaged += OnPlayerDamaged;
            _playerHealthController.PlayerShieldStateChanged += OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDied += OnPlayerDied;

            _playerVisualsController.PlayerAppearanceUpdated += OnPlayerAppearanceUpdated;
        }

        private void OnDisable()
        {
            _playerController.PlayerInitialized -= OnPlayerInitialized;
            _playerController.PlayerReadyToRecycle -= OnPlayerReadyToRecycle;

            _playerHealthController.PlayerDamaged -= OnPlayerDamaged;
            _playerHealthController.PlayerShieldStateChanged -= OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDied -= OnPlayerDied;

            _playerVisualsController.PlayerAppearanceUpdated -= OnPlayerAppearanceUpdated;
        }

        #endregion

        #region Event callbacks

        private void OnPlayerInitialized(SO_ShipData data)
        {
            _shipData = data;
            SetCooldownState(false);
        }

        private void OnPlayerReadyToRecycle()
        {
            Cleanup();
        }

        private void OnPlayerDamaged()
        {
            HandleCollision();
        }

        private void OnPlayerShieldStateChanged(bool state)
        {
            if (state is false)
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
            CancelInvoke(nameof(EndCooldown));
            InCollisionCooldown = false;
            _collider.enabled = false;
        }

        #endregion

        #region Collision

        private void HandleCollision()
        {
            SetCooldownState(true);
            Invoke(nameof(EndCooldown), _shipData.InvulnerabilityDuration);
        }

        #endregion

        #region Cooldown Management

        private void EndCooldown()
        {
            SetCooldownState(false);
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