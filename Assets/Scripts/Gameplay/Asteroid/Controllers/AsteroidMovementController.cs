using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using Asteroids.Core.ScriptableObjects.Data;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(AsteroidController))]

    public class AsteroidMovementController : MonoBehaviour , IPushable
    {
        #region Fields

        public Vector2 Direction { get { return _rigidBody.velocity.normalized; } }

        [Header("References")]
        [Inject] private readonly AsteroidController _asteroidController;
        [Space]
        [Inject] private readonly Rigidbody2D _rigidBody;

        [Header("Data")]
        private SO_AsteroidData _asteroidData;

        #endregion

        #region Initialization

        private void Initialize(Vector2? position, Vector2? direction)
        {
            SetPosition(position);
            RandomizeInitialTorque();
            StartMovement(direction);
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _asteroidController.AsteroidInitialized += OnAsteroidInitialized;
            _asteroidController.AsteroidReadyToRecycle += OnAsteroidReadyToRecycle;
        }

        private void OnDisable()
        {
            _asteroidController.AsteroidInitialized -= OnAsteroidInitialized;
            _asteroidController.AsteroidReadyToRecycle -= OnAsteroidReadyToRecycle;
        }

        #endregion

        #region Event callbacks

        private void OnAsteroidInitialized(SO_AsteroidData data, Vector2? position, Vector2? direction)
        {
            _asteroidData = data;
            Initialize(position, direction);
        }

        private void OnAsteroidReadyToRecycle()
        {
            StopMovement();
        }

        #endregion

        #region Position

        private void SetPosition(Vector2? position)
        {
            if (position.HasValue)
                transform.position = position.Value;
            else
                transform.position = GetRandomSpawnPosition();
        }

        private Vector2 GetRandomSpawnPosition()
        {
            float minLimitX = 0f - _asteroidData.ScreenEdgeTeleportConfiguration.EdgeRepositionOffsetX;
            float maxLimitX = 1f + _asteroidData.ScreenEdgeTeleportConfiguration.EdgeRepositionOffsetX;
            float minLimitY = 0f - _asteroidData.ScreenEdgeTeleportConfiguration.EdgeRepositionOffsetY;
            float maxLimitY = 1f + _asteroidData.ScreenEdgeTeleportConfiguration.EdgeRepositionOffsetY;

            float RandomValueX = Random.Range(minLimitX, maxLimitX);
            float RandomValueY = Random.Range(minLimitY, maxLimitY);

            bool spawnOnXEdge = Random.value > 0.5f;

            if (spawnOnXEdge)
                return Camera.main.ViewportToWorldPoint(new Vector2(maxLimitX, RandomValueY));
            else
                return Camera.main.ViewportToWorldPoint(new Vector2(RandomValueX, maxLimitY));
        }

        #endregion

        #region Movement

        private void StartMovement(Vector2? direction) 
        {
            Vector2 newDirection;

            if (direction.HasValue)
                newDirection = direction.Value;
            else
                newDirection = Random.insideUnitCircle.normalized;          

            _rigidBody.velocity = _asteroidData.Speed * newDirection;
        }

        private void StopMovement()
        {
            _rigidBody.velocity = Vector2.zero;
        }
        
        public void Push(PushInfo data)
        {
            _rigidBody.AddForce(data.HitDirection.normalized * data.PushForce, ForceMode2D.Impulse);

            Vector2 hitOffset = data.HitPoint - _rigidBody.worldCenterOfMass;
            float torque = data.TorqueForce * Cross2D(hitOffset, data.HitDirection.normalized);

            _rigidBody.AddTorque(torque, ForceMode2D.Impulse);
        }

        private float Cross2D(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        #endregion

        #region Rotation

        private void RandomizeInitialTorque() 
        {
            _rigidBody.AddTorque(Random.Range(-_asteroidData.PosibleTorque, _asteroidData.PosibleTorque));
        }

        #endregion
    }
}