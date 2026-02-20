using ACG.Core.EventBus;
using ACG.Scripts.Utilitys;
using ACG.Tools.Runtime.Pooling.Gameplay;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.Interfaces;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.General.OnContact;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Controllers
{
    [RequireComponent(typeof(AsteroidMovementController))]
    [RequireComponent(typeof(AsteroidHealthController))]
    [RequireComponent(typeof(DamageOnContact))]
    [RequireComponent(typeof(PooledGameObjectController))]
    [RequireComponent(typeof(ScreenEdgeTeleport))]

    public class AsteroidController : MonoBehaviour
    {
        #region Fields and events

        public event Action<Vector2?, Vector2?> AsteroidInitialized;
        public event Action AsteroidReadyToBeRecycled;

        [Header("References")]
        [Inject] private readonly AsteroidMovementController _asteroidMovementcontroller;
        [Inject] private readonly AsteroidHealthController _asteroidHealthController;
        [Inject] private readonly DamageOnContact _damageOnContact;
        [Inject] private readonly PooledGameObjectController _pooledGameObject;
        [Inject] private readonly ScreenEdgeTeleport _screenEdgeTeleport;

        [Header("Data")]
        private SO_AsteroidData _asteroidData;
        public IAsteroidData AsteroidData => _asteroidData;

        #endregion

        #region Initialization

        public void Initialize(SO_AsteroidData data, Vector2? position = null, Vector2? direction = null)
        {
            _asteroidData = data;

            _screenEdgeTeleport.SetData(_asteroidData.ScreenEdgeTeleportData);
            _damageOnContact.SetData(_asteroidData.Damage);

            AsteroidInitialized?.Invoke(position, direction);
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _asteroidHealthController.AsteroidDestroyed += OnAsteroidDestroyed;
        }

        private void OnDisable()
        {
            _asteroidHealthController.AsteroidDestroyed -= OnAsteroidDestroyed;
        }

        #endregion

        #region Event callbacks

        private void OnAsteroidDestroyed()
        {
            EventBusManager.GameplayBus.RaiseEvent(new AsteroidDestroyed(this, _asteroidData, transform.position, _asteroidMovementcontroller.Direction));
            _ = Recycle();
        }

        #endregion

        #region Functionality

        public async Task Recycle()
        {
            AsteroidReadyToBeRecycled?.Invoke();
            await Task.Yield();
            _pooledGameObject.RecycleGameObject();
        }

        #endregion
    }
}