using ACG.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerHealthController))]

    public class PlayerSoundController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [Inject] private readonly PlayerController _playerController;
        [Inject] private readonly PlayerHealthController _playerHealthController;
        [Space]
        [Inject] private readonly ISoundManager _soundManager;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerHealthController.PlayerDamaged += OnPlayerDamaged;
            _playerHealthController.PlayerShieldStateChanged += OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            _playerHealthController.PlayerDamaged -= OnPlayerDamaged;
            _playerHealthController.PlayerShieldStateChanged -= OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDied -= OnPlayerDied;
        }

        #endregion

        #region Event callbacks 

        private void OnPlayerDamaged()
        {
            PlayDamageTakedSound();
        }

        private void OnPlayerShieldStateChanged(bool state)
        {
            if (state)
                PlayShielRecoveredSound();
            else
                PlayShielLostSound();
        }

        private void OnPlayerDied()
        {
            PlayDiedSound();
        }

        #endregion

        #region Sound

        private void PlayShielLostSound()
        {
            _soundManager.Play2DSound(_playerController.ShipData.SoundsOnShieldDown);
        }

        private void PlayShielRecoveredSound()
        {
            _soundManager.Play2DSound(_playerController.ShipData.SoundsOnShieldUp);
        }

        private void PlayDamageTakedSound()
        {
            _soundManager.Play2DSound(_playerController.ShipData.SoundsOnDamage);
        }
        
        private void PlayDiedSound()
        {
            _soundManager.Play2DSound(_playerController.ShipData.SoundsOnDestruction);
        }

        #endregion
    }
}