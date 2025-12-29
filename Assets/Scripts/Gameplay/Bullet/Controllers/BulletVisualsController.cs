using Asteroids.Core.ScriptableObjects.Data;
using ToolsACG.Core.Managers;
using ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs;
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

        [Header("Data")]
        private SO_BulletData _bulletData;

        #endregion

        #region Initialization

        private void Initialize()
        {
            ApplyAppearance();
        }

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
            Initialize();
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

        #endregion

        #region Particles

        private void CreateDestructionParticles()
        {
            foreach (ParticleConfiguration item in _bulletData.ParticlesOnDestruction)
                _vFXManager.PlayParticlesVFX(item.PrefabData, transform.position, Quaternion.identity, null, item.ParticleConfig);
        }

        #endregion
    }
}