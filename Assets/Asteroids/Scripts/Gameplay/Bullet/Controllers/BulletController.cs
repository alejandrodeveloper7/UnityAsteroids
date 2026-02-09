using ACG.Core.EventBus;
using ACG.Scripts.Utilitys;
using ACG.Tools.Runtime.Pooling.Gameplay;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.General.OnContact;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Controllers
{
    [RequireComponent(typeof(BulletPhysicsController))]
    [RequireComponent(typeof(BulletLifeTimeController))]
    [RequireComponent(typeof(PooledGameObjectController))]
    [RequireComponent(typeof(ScreenEdgeTeleport))]
    [RequireComponent(typeof(DamageOnContact))]
    [RequireComponent(typeof(PushOnContact))]

    public class BulletController : MonoBehaviour
    {
        #region Fields and events

        public event Action<SO_BulletData> BulletInitialized;
        public event Action BulletReadyToBeRecycled;

        [Header("References")]
        [Inject] private readonly BulletPhysicsController _bulletPhysicsController;
        [Inject] private readonly BulletLifeTimeController _bulletLifeTimeController;
        [Space]
        [Inject] private readonly PooledGameObjectController _pooledGameObject;
        [Inject] private readonly ScreenEdgeTeleport _screenEdgeTeleport;
        [Inject] private readonly DamageOnContact _damageOnContact;
        [Inject] private readonly PushOnContact _pushOnContact;

        [Header("Data")]
        private SO_BulletData _bulletData;

        #endregion

        #region Initialization

        public void Initialize(SO_BulletData data)
        {
            _bulletData = data;

            _screenEdgeTeleport.SetData(_bulletData.ScreenEdgeTeleportData);
            _damageOnContact.SetData(_bulletData.Damage);
            _pushOnContact.SetData(_bulletData.PushForce, _bulletData.TorqueForce, (_bulletData.Speed * -transform.up).normalized);

            BulletInitialized?.Invoke(_bulletData);
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _bulletPhysicsController.BulletCollisioned += OnCollisioned;

            _bulletLifeTimeController.LifeTimeCompleted += OnLifeTimeCompleted;

            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);
        }

        private void OnDisable()
        {
            _bulletPhysicsController.BulletCollisioned -= OnCollisioned;

            _bulletLifeTimeController.LifeTimeCompleted -= OnLifeTimeCompleted;

            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);
        }

        #endregion

        #region Event callbacks

        private void OnCollisioned()
        {
            _ = Recycle();
        }

        private void OnLifeTimeCompleted()
        {
            _ = Recycle();
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _ = Recycle();
        }

        #endregion

        #region Functionality

        private async Task Recycle()
        {
            BulletReadyToBeRecycled?.Invoke();
            await Task.Yield();
            _pooledGameObject.RecycleGameObject();
        }

        #endregion
    }
}