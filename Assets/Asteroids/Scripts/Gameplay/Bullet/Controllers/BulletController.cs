using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.General.OnContact;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Utilitys;
using ToolsACG.Core.Utils;
using ToolsACG.Pooling.Gameplay;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Controllers
{
    [RequireComponent(typeof(ScreenEdgeTeleport))]
    [RequireComponent(typeof(PooledGameObjectController))]
    [RequireComponent(typeof(DamageOnContact))]
    [RequireComponent(typeof(PushOnContact))]
    [RequireComponent(typeof(BulletPhysicsController))]
    [RequireComponent(typeof(BulletLifeTimeController))]

    public class BulletController : MonoBehaviour
    {
        #region Fields and events

        public event Action<SO_BulletData> BulletInitialized;
        public event Action BulletReadyToBeRecycled;

        [Header("References")]
        [Inject] private readonly ScreenEdgeTeleport _screenEdgeTeleport;
        [Inject] private readonly PooledGameObjectController _pooledGameObject;
        [Inject] private readonly DamageOnContact _damageOnContact;
        [Inject] private readonly PushOnContact _pushOnContact;
        [Inject] private readonly BulletPhysicsController _bulletPhysicsController;
        [Inject] private readonly BulletLifeTimeController _bulletLifeTimeController;

        [Header("Data")]
        private SO_BulletData _bulletData;

        #endregion

        #region Initialization

        public void Initialize(SO_BulletData data)
        {
            _bulletData = data;

            _screenEdgeTeleport.SetData(_bulletData.ScreenEdgeTeleportConfiguration);
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
            _ = CleanBullet();
        }

        private void OnLifeTimeCompleted()
        {
            _ = CleanBullet();
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _ = CleanBullet();
        }

        #endregion

        #region Functionality

        private async Task CleanBullet()
        {
            BulletReadyToBeRecycled?.Invoke();
            await TimingUtils.WaitSeconds(Time.deltaTime);
            _pooledGameObject.RecycleGameObject();
        }

        #endregion
    }
}