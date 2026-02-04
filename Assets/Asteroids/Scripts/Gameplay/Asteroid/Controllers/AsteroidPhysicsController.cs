using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Controllers
{
    [RequireComponent(typeof(AsteroidController))]
    [RequireComponent(typeof(AsteroidVisualsController))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PolygonCollider2D))]

    public class AsteroidPhysicsController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [Inject] private readonly AsteroidVisualsController _asteroidVisualsController;
        [Inject] private readonly AsteroidController _asteroidController;
        [Space]
        [Inject] private readonly PolygonCollider2D _collider;
        [Inject] private readonly SpriteRenderer _spriteRenderer;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _asteroidVisualsController.AsteroidAppearanceUpdated += OnAsteroidAppearanceUpdated;

            _asteroidController.AsteroidReadyToBeRecycled += OnAsteroidReadyToBeRecycled;
        }

        private void OnDisable()
        {
            _asteroidVisualsController.AsteroidAppearanceUpdated -= OnAsteroidAppearanceUpdated;

            _asteroidController.AsteroidReadyToBeRecycled -= OnAsteroidReadyToBeRecycled;
        }

        #endregion

        #region Event callbacks

        private void OnAsteroidAppearanceUpdated()
        {
            RebuildColliderShape();
            TurnDetection(true);
        }

        private void OnAsteroidReadyToBeRecycled()
        {
            TurnDetection(false);
        }

        #endregion

        #region Functionality

        private void TurnDetection(bool state)
        {
            _collider.enabled = state;
        }

        private void RebuildColliderShape()
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