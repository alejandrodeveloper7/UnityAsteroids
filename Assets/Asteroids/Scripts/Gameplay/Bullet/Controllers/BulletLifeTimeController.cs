using ACG.Core.Utils;
using Asteroids.Core.ScriptableObjects.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Controllers
{
    [RequireComponent(typeof(BulletController))]
    [RequireComponent(typeof(BulletPhysicsController))]

    public class BulletLifeTimeController : MonoBehaviour
    {

        #region Fields and events

        public event Action LifeTimeCompleted;

        [Header("References")]
        [Inject] private readonly BulletController _bulletController;
        [Inject] private readonly BulletPhysicsController _bulletPhysicsController;

        [Header("Values")]
        private CancellationTokenSource _token;

        [Header("Data")]
        private SO_BulletData _bulletData;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _bulletController.BulletInitialized += OnBulletInitialized;
            _bulletController.BulletReadyToBeRecycled += OnBulletReadyToBeRecycled;

            _bulletPhysicsController.BulletCollisioned += OnBulletCollisioned;
        }

        private void OnDisable()
        {
            _bulletController.BulletInitialized -= OnBulletInitialized;
            _bulletController.BulletReadyToBeRecycled -= OnBulletReadyToBeRecycled;

            _bulletPhysicsController.BulletCollisioned -= OnBulletCollisioned;
        }

        #endregion

        #region EventCallbacks

        private void OnBulletInitialized(SO_BulletData data)
        {
            _bulletData = data;
            _ = StartLifeTime();
        }

        private void OnBulletReadyToBeRecycled()
        {
            CancelToken();
        }

        private void OnBulletCollisioned()
        {
            CancelToken();
        }

        #endregion

        #region Functionality

        private async Task StartLifeTime()
        {
            RestartToken();

            try
            {
                await TimingUtils.WaitSeconds(_bulletData.LifeTime, false, _token.Token);
                LifeTimeCompleted?.Invoke();
            }
            catch (OperationCanceledException)
            {

            }

            CancelToken();
        }

        #endregion

        #region Token management

        private void CancelToken()
        {
            _token?.Cancel();
            _token = null;
        }

        private void RestartToken()
        {
            _token?.Cancel();
            _token = new CancellationTokenSource();
        }

        #endregion
    }
}