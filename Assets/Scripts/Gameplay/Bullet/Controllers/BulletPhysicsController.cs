using Asteroids.Core.ScriptableObjects.Data;
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
            TurnDetection(true);
        }

        private void OnBulletReadyToRecycle()
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