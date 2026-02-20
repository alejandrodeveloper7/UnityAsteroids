using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using System;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Controllers
{
    [RequireComponent(typeof(AsteroidController))]

    public class AsteroidHealthController : MonoBehaviour, IDamageable
    {
        #region Fields and events

        public event Action<Vector3> AsteroidDamaged;
        public event Action AsteroidDestroyed;

        [Header("References")]
        [Inject] private readonly AsteroidController _asteroidController;

        [Header("Values")]
        private int _health;

        [Header("States")]
        private bool _alive;

        #endregion

        #region Initialization

        private void Initialize()
        {
            _alive = true;
            _health = _asteroidController.AsteroidData.MaxHP;
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _asteroidController.AsteroidInitialized += OnAsteroidInitialized;
        }

        private void OnDisable()
        {
            _asteroidController.AsteroidInitialized -= OnAsteroidInitialized;
        }

        #endregion

        #region Event callbacks

        private void OnAsteroidInitialized(Vector2? position, Vector2? direction)
        {
            Initialize();
        }

        #endregion

        #region Management

        public void TakeDamage(DamageData data)
        {
            if (_alive is false)
                return;

            _health -= data.Amount;

            AsteroidDamaged?.Invoke(data.HitPoint);

            if (_health <= 0)
                Die();
        }

        public void Die()
        {
            _alive = false;
            _health = 0;

            AsteroidDestroyed?.Invoke();
        }

        #endregion
    }
}