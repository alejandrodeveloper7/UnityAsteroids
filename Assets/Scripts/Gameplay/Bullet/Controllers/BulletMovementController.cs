using Asteroids.Core.ScriptableObjects.Data;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BulletController))]

    public class BulletMovementController : MonoBehaviour
    {
        #region Fields and events

        [Header("References")]
        [Inject] private readonly BulletController _bulletController;
        [Space]
        [Inject] private readonly Rigidbody2D _rigidBody;

        [Header("Data")]
        private SO_BulletData _bulletData;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _bulletController.BulletInitialized += OnBulletInitialized;
            _bulletController.BulletReadyToRecycle += OnBulletReadyToRecycle;
        }

        private void OnDisable()
        {
            _bulletController.BulletInitialized -= OnBulletInitialized;
            _bulletController.BulletReadyToRecycle -= OnBulletReadyToRecycle;
        }

        #endregion

        #region EventCallbacks

        private void OnBulletInitialized(SO_BulletData data)
        {
            _bulletData = data;
            StartMovement();
        }

        private void OnBulletReadyToRecycle()
        {
            StopMovement();
        }

        #endregion

        #region Movement management

        private void StartMovement()
        {
            _rigidBody.velocity = _bulletData.Speed  * -transform.up;
        }

        private void StopMovement()
        {
            _rigidBody.velocity = Vector2.zero;
        }

        #endregion
    }
}