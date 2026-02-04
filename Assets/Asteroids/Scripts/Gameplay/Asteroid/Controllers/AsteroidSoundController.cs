using Asteroids.Core.Interfaces.Models;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.General.OnContact;
using ToolsACG.Core.Managers;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Controllers
{
    [RequireComponent(typeof(AsteroidController))]
    [RequireComponent(typeof(AsteroidHealthController))]
    [RequireComponent(typeof(DamageOnContact))]

    public class AsteroidSoundController : MonoBehaviour
    {
        #region Fields     

        [Header("References")]
        [Inject] private readonly AsteroidController _asteroidController;
        [Inject] private readonly AsteroidHealthController _asteroidHealthController;
        [Inject] private readonly DamageOnContact _damageOnContact;
        [Space]
        [Inject] private readonly ISoundManager _soundManager;

        [Header("Data")]
        private SO_AsteroidData _asteroidData;
        
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
        }

        private void OnAsteroidDamaged(Vector3 hitPoint)
        {
            PlayDamageSound();
        }

        private void OnAsteroidDestroyed()
        {
            PlayDestructionSound();
        }

        private void OnDamageDone(DamageInfo data)
        {
            PlayDamageSound();
        }

        #endregion

        #region Sound

        private void PlayDamageSound()
        {
            _soundManager.Play2DSounds(_asteroidData.SoundsOnDamage);
        }

        private void PlayDestructionSound() 
        {
            _soundManager.Play2DSounds(_asteroidData.SoundsOnDestruction);
        }
        
        #endregion
    }
}