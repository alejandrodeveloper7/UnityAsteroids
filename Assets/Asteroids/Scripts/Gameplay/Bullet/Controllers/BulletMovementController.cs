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

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _bulletController.BulletInitialized += OnBulletInitialized;
            _bulletController.BulletReadyToBeRecycled += OnBulletReadyToBeRecycled;
        }

        private void OnDisable()
        {
            _bulletController.BulletInitialized -= OnBulletInitialized;
            _bulletController.BulletReadyToBeRecycled -= OnBulletReadyToBeRecycled;
        }

        #endregion

        #region EventCallbacks

        private void OnBulletInitialized()
        {
            StartMovement();
        }

        private void OnBulletReadyToBeRecycled()
        {
            StopMovement();
        }

        #endregion

        #region Movement management

        private void StartMovement()
        {
            _rigidBody.velocity = _bulletController.BulletData.Speed  * -transform.up;
        }

        private void StopMovement()
        {
            _rigidBody.velocity = Vector2.zero;
        }

        #endregion
    }
}