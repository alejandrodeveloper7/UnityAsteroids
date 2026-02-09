using ACG.Scripts.Managers;
using Asteroids.Core.ScriptableObjects.Data;
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

        [Header("Data")]
        private SO_BulletData _bulletData;

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

        private void OnBulletInitialized(SO_BulletData data)
        {
            _bulletData = data;

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
            _spriteRenderer.sprite = _bulletData.Sprite;
            _spriteRenderer.color = _bulletData.Color;
        }

        private void PlayBulletVisualSequence()
        {
            StopBulletVisualSequence();

            transform.localScale = _bulletData.Scale;

            _bulletLifeSequence = DOTween.Sequence();
            _bulletLifeSequence.AppendInterval(_bulletData.FullScaleDuration);
            _bulletLifeSequence.Append(transform.DOScale(Vector3.zero, _bulletData.DescaleDuration));
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
            _vFXManager.PlayParticlesVFX(_bulletData.ParticlesOnDestruction, transform.position, Quaternion.identity, null);
        }

        #endregion
    }
}