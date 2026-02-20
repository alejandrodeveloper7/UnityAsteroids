using ACG.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Controllers
{
    [RequireComponent(typeof(BulletPhysicsController))]
    [RequireComponent(typeof(BulletController))]
    [RequireComponent(typeof(SpriteRenderer))]

    public class BulletVisualsController : MonoBehaviour
    {
        #region Fields and events

        [Header("References")]
        [Inject] private readonly BulletPhysicsController _bulletPhysicsController;
        [Inject] private readonly BulletController _bulletController;
        [Space]
        [Inject] private readonly SpriteRenderer _spriteRenderer;
        [Space]
        [Inject] private readonly IVFXManager _vFXManager;

        [Header("Cache")]
        private Sequence _bulletLifeSequence;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _bulletController.BulletInitialized += OnBulletInitialized;

            _bulletPhysicsController.BulletCollisioned += OnBulletCollisioned;
        }

        private void OnDisable()
        {
            _bulletController.BulletInitialized -= OnBulletInitialized;

            _bulletPhysicsController.BulletCollisioned -= OnBulletCollisioned;
        }

        #endregion

        #region EventCallbacks

        private void OnBulletInitialized()
        {
            ApplyAppearance();
            PlayBulletVisualSequence();
        }

        private void OnBulletCollisioned()
        {
            CreateDestructionParticles();
        }

        #endregion

        #region Appearance

        private void ApplyAppearance()
        {
            _spriteRenderer.sprite = _bulletController.BulletData.Sprite;
            _spriteRenderer.color = _bulletController.BulletData.Color;
        }

        private void PlayBulletVisualSequence()
        {
            StopBulletVisualSequence();

            transform.localScale = _bulletController.BulletData.Scale;

            _bulletLifeSequence = DOTween.Sequence();
            _bulletLifeSequence.AppendInterval(_bulletController.BulletData.FullScaleDuration);
            _bulletLifeSequence.Append(transform.DOScale(Vector3.zero, _bulletController.BulletData.DescaleDuration));
        }

        private void StopBulletVisualSequence()
        {
            transform.DOKill();
            _bulletLifeSequence?.Kill();
        }

        #endregion

        #region Particles

        private void CreateDestructionParticles()
        {
            _vFXManager.PlayParticlesVFX(_bulletController.BulletData.ParticlesOnDestruction, transform.position, Quaternion.identity, null);
        }

        #endregion
    }
}