using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.General.OnContact;
using System;
using System.Threading.Tasks;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Utilitys;
using ToolsACG.Core.Utils;
using ToolsACG.Pooling.Gameplay;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Controllers
{
    [RequireComponent(typeof(AsteroidHealthController))]
    [RequireComponent(typeof(PooledGameObject))]
    [RequireComponent(typeof(ScreenEdgeTeleport))]
    [RequireComponent(typeof(DamageOnContact))]
    [RequireComponent(typeof(AsteroidMovementController))]

    public class AsteroidController : MonoBehaviour
    {
        #region Fields and events

        public event Action<SO_AsteroidData, Vector2?, Vector2?> AsteroidInitialized;
        public event Action AsteroidReadyToRecycle;

        [Header("References")]
        [Inject] private readonly AsteroidMovementController _asteroidMovementcontroller;
        [Inject] private readonly AsteroidHealthController _asteroidHealthController;
        [Inject] private readonly DamageOnContact _damageOnContact;
        [Inject] private readonly PooledGameObject _pooledGameObject;
        [Inject] private readonly ScreenEdgeTeleport _screenEdgeTeleport;

        [Header("Data")]
        private SO_AsteroidData _asteroidData;

        #endregion

        #region Initialization

        public void Initialize(SO_AsteroidData data, Vector2? position = null, Vector2? direction = null)
        {
            _asteroidData = data;

            _screenEdgeTeleport.SetData(_asteroidData.ScreenEdgeTeleportConfiguration);
            _damageOnContact.SetData(_asteroidData.Damage);

            AsteroidInitialized?.Invoke(_asteroidData, position, direction);
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
            _ = CleanAsteroid();
        }

        #endregion

        #region Functionality

        public async Task CleanAsteroid()
        {
            AsteroidReadyToRecycle?.Invoke();
            await TimingUtils.WaitSeconds(Time.deltaTime);
            _pooledGameObject.RecycleGameObject();
        }

        #endregion
    }
}