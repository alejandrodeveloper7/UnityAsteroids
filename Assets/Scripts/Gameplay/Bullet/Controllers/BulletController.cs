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
    [RequireComponent(typeof(PooledGameObject))]
    [RequireComponent(typeof(DamageOnContact))]
    [RequireComponent(typeof(PushOnContact))]
    [RequireComponent(typeof(BulletPhysicsController))]

    public class BulletController : MonoBehaviour
    {
        #region Fields and events

        public event Action<SO_BulletData> BulletInitialized;
        public event Action BulletReadyToRecycle;

        [Header("References")]
        [Inject] private readonly ScreenEdgeTeleport _screenEdgeTeleport;
        [Inject] private readonly PooledGameObject _pooledGameObject;
        [Inject] private readonly DamageOnContact _damageOnContact;
        [Inject] private readonly PushOnContact _pushOnContact;
        [Inject] private readonly BulletPhysicsController _bulletPhysicsController;

        [Header("Data")]
        private SO_BulletData _bulletData;

        [Header("Cache")]
        private Sequence _bulletLifeSequence;

        #endregion

        #region Initialization

        public void Initialize(SO_BulletData data)
        {
            _bulletData = data;

            _screenEdgeTeleport.SetData(_bulletData.ScreenEdgeTeleportConfiguration);
            _damageOnContact.SetData(_bulletData.Damage);
            _pushOnContact.SetData(_bulletData.PushForce,_bulletData.TorqueForce, (_bulletData.Speed * -transform.up).normalized);

            BulletInitialized?.Invoke(_bulletData);

            PlayBulletLifeSequence();
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _bulletPhysicsController.BulletCollisioned += OnCollisioned;

            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);
        }

        private void OnDisable()
        {
            _bulletPhysicsController.BulletCollisioned -= OnCollisioned;

            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);
        }

        #endregion

        #region Event callbacks

        private void OnCollisioned()
        {
            StopBulletLifeSequence();
            _ = CleanBullet();
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            StopBulletLifeSequence();
            _ = CleanBullet();
        }

        #endregion

        #region Functionality

        private void PlayBulletLifeSequence()
        {
            StopBulletLifeSequence();

            transform.localScale = _bulletData.Scale;

            _bulletLifeSequence = DOTween.Sequence();
            _bulletLifeSequence.AppendInterval(_bulletData.LifeDuration);
            _bulletLifeSequence.Append(transform.DOScale(Vector3.zero, _bulletData.DescaleDuration));
            _bulletLifeSequence.AppendCallback(() => _=CleanBullet());
        }

        private void StopBulletLifeSequence()
        {
            transform.DOKill();
            _bulletLifeSequence?.Kill();
        }

        private async Task CleanBullet()
        {
            BulletReadyToRecycle?.Invoke();
            await TimingUtils.WaitSeconds(Time.deltaTime);
            _pooledGameObject.RecycleGameObject();
        }

        #endregion
    }
}