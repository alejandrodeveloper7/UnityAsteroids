using ACG.Core.Utils;
using ACG.Scripts.Managers;
using ACG.Scripts.Models;
using Asteroids.Core.Interfaces.Models;
using Asteroids.Core.ScriptableObjects.Configurations;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.FloatingText.Spawners;
using Asteroids.Gameplay.General.OnContact;
using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Controllers
{
    [RequireComponent(typeof(AsteroidController))]
    [RequireComponent(typeof(AsteroidHealthController))]
    [RequireComponent(typeof(DamageOnContact))]
    [RequireComponent(typeof(SpriteRenderer))]

    public class AsteroidVisualsController : MonoBehaviour
    {
        #region Fields

        public event Action AsteroidAppearanceUpdated;

        [Header("References")]
        [Inject] private readonly AsteroidController _asteroidController;
        [Inject] private readonly AsteroidHealthController _asteroidHealthController;
        [Inject] private readonly DamageOnContact _damageOnContact;
        [Space]
        [Inject] private readonly SpriteRenderer _spriteRenderer;
        [Space]
        [Inject] private readonly FloatingTextSpawner _floatingTextSpawner;
        [Space]
        [Inject] private readonly IVFXManager _vFXManager;

        [Header("Data")]
        [Inject] private readonly SO_FloatingTextConfiguration _floatingTextConfiguration;
        private SO_AsteroidData _asteroidData;

        [Header("Cache")]
        private Sequence _damageFeedbackSequence;

        #endregion

        #region Initialization

        private void Initialize()
        {
            RandomizeAppearance();

            if (_asteroidData.IsInitialAsteroid)
                PlayAlphaAparition();
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _asteroidController.AsteroidInitialized += OnAsteroidInitialized;

            _asteroidHealthController.AsteroidDamaged += OnAsteroidDamaged;
            _asteroidHealthController.AsteroidDestroyed += OnAsteroidDestroyed;

            _damageOnContact.DamageDone += OnDamageDone;
        }

        private void OnDisable()
        {
            _asteroidController.AsteroidInitialized -= OnAsteroidInitialized;

            _asteroidHealthController.AsteroidDamaged -= OnAsteroidDamaged;
            _asteroidHealthController.AsteroidDestroyed -= OnAsteroidDestroyed;

            _damageOnContact.DamageDone -= OnDamageDone;
        }

        #endregion

        #region Event callbacks

        private void OnAsteroidInitialized(SO_AsteroidData data, Vector2? position, Vector2? direction)
        {
            _asteroidData = data;
            Initialize();
        }

        private void OnAsteroidDamaged(Vector3 position)
        {
            CreateDamageParticles(position);
            PlayDamageFeedbackSequence();
        }

        private void OnAsteroidDestroyed()
        {
            CreateDestructionParticles(transform.position);

            if (_floatingTextConfiguration.IsActive)
                CreateScoreFloatingText(_asteroidData.ScoreValue.ToString());
        }

        private void OnDamageDone(DamageInfo data)
        {
            CreateDamageParticles(data.HitPoint);
        }

        #endregion

        #region Appearance

        private void RandomizeAppearance()
        {
            int randomValue = UnityEngine.Random.Range(0, _asteroidData.PossibleSprites.Length);
            _spriteRenderer.sprite = _asteroidData.PossibleSprites[randomValue];
            _spriteRenderer.color = _asteroidData.Color;
            transform.localScale = _asteroidData.Scale;

            AsteroidAppearanceUpdated?.Invoke();
        }

        private void PlayDamageFeedbackSequence()
        {
            _damageFeedbackSequence?.Kill();

            _spriteRenderer.color = _asteroidData.Color;
            transform.localScale = _asteroidData.Scale;

            _damageFeedbackSequence = DOTween.Sequence();

            _damageFeedbackSequence
               .Append(transform.DOScale(_asteroidData.DamageFeedbackScale, _asteroidData.DamageFeedbackDuration).SetLoops(2, LoopType.Yoyo))
               .Join(_spriteRenderer.DOColor(_asteroidData.DamageFeedbackColor, _asteroidData.DamageFeedbackDuration).SetLoops(2, LoopType.Yoyo));
        }

        #endregion

        #region Aparition

        private void PlayAlphaAparition()
        {
            _spriteRenderer.DOKill();

            Color alphaColor = ColorUtils.GetColorWithAlpha(_asteroidData.Color, 0);
            _spriteRenderer.color = alphaColor;

            _spriteRenderer.DOFade(1, _asteroidData.AlphaAparitionDuration);
        }

        #endregion

        #region Particles

        private void CreateDamageParticles(Vector3 position)
        {
                _vFXManager.PlayParticlesVFX(_asteroidData.ParticlesOnDamage, position,Quaternion.identity, null);
        }

        private void CreateDestructionParticles(Vector3 position)
        {
                _vFXManager.PlayParticlesVFX(_asteroidData.ParticlesOnDestruction, position, Quaternion.identity, null);
        }

        #endregion

        #region Floating Text

        private void CreateScoreFloatingText(string text)
        {
            _floatingTextSpawner.Spawn(transform.position, text);
        }

        #endregion
    }
}