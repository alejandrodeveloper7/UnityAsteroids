using Asteroids.Core.ScriptableObjects.Data;
using ToolsACG.Core.Managers;
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

        [Header("Data")]
        private SO_ShipData _shipData;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerController.PlayerInitialized += OnPlayerInitialized;

            _playerHealthController.PlayerDamaged += OnPlayerDamaged;
            _playerHealthController.PlayerShieldStateChanged += OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            _playerController.PlayerInitialized -= OnPlayerInitialized;

            _playerHealthController.PlayerDamaged -= OnPlayerDamaged;
            _playerHealthController.PlayerShieldStateChanged -= OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDied -= OnPlayerDied;
        }

        #endregion

        #region Event callbacks 

        private void OnPlayerInitialized(SO_ShipData data)
        {
            _shipData = data;
        }

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
            _soundManager.Play2DSounds(_shipData.SoundsOnShieldDown);
        }
        private void PlayShielRecoveredSound()
        {
            _soundManager.Play2DSounds(_shipData.SoundsOnShieldUp);
        }

        private void PlayDamageTakedSound()
        {
            _soundManager.Play2DSounds(_shipData.SoundsOnDamage);
        }
        
        private void PlayDiedSound()
        {
            _soundManager.Play2DSounds(_shipData.SoundsOnDestruction);
        }

        #endregion
    }
}