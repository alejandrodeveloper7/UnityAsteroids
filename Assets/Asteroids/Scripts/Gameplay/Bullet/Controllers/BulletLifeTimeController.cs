using ACG.Core.Utils;
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
        private CancellationTokenSource _cancellationToken;

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

        private void OnBulletInitialized()
        {
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
                await TimingUtils.WaitSeconds(_bulletController.BulletData.LifeTime, false, _cancellationToken.Token);
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
            _cancellationToken?.Cancel();
            _cancellationToken = null;
        }

        private void RestartToken()
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        #endregion
    }
}