using System;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Controllers
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BulletController))]

    public class BulletPhysicsController : MonoBehaviour
    {
        #region Fields and events

        public event Action BulletCollisioned;

        [Header("References")]
        [Inject] private readonly BulletController _bulletController;
        [Space]
        [Inject] private readonly BoxCollider2D _collider;

        #endregion

        #region Monobehaviour

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TurnDetection(false);

            BulletCollisioned?.Invoke();
        }

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
            TurnDetection(true);
        }

        private void OnBulletReadyToBeRecycled()
        {
            TurnDetection(false);
        }

        #endregion

        #region Physics management

        private void TurnDetection(bool state)
        {
            _collider.enabled = state;
        }

        #endregion
    }
}