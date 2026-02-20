using ACG.Scripts.Managers;
using Asteroids.Core.Interfaces.Models;
using Asteroids.Gameplay.General.OnContact;
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
        
        #endregion     

        #region Monobehaviour

        private void OnEnable()
        {
            _asteroidHealthController.AsteroidDamaged += OnAsteroidDamaged;
            _asteroidHealthController.AsteroidDestroyed += OnAsteroidDestroyed;

            _damageOnContact.DamageDone += OnDamageDone;
        }

        private void OnDisable()
        {
            _asteroidHealthController.AsteroidDamaged -= OnAsteroidDamaged;
            _asteroidHealthController.AsteroidDestroyed -= OnAsteroidDestroyed;

            _damageOnContact.DamageDone -= OnDamageDone;
        }

        #endregion

        #region Event callbacks

        private void OnAsteroidDamaged(Vector3 hitPoint)
        {
            PlayDamageSound();
        }

        private void OnAsteroidDestroyed()
        {
            PlayDestructionSound();
        }

        private void OnDamageDone(DamageData data)
        {
            PlayDamageSound();
        }

        #endregion

        #region Sound

        private void PlayDamageSound()
        {
            _soundManager.Play2DSound(_asteroidController.AsteroidData.SoundsOnDamage);
        }

        private void PlayDestructionSound() 
        {
            _soundManager.Play2DSound(_asteroidController.AsteroidData.SoundsOnDestruction);
        }
        
        #endregion
    }
}