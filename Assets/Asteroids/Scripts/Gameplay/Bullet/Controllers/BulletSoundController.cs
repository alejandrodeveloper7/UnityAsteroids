using ACG.Scripts.Managers;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Controllers
{
    [RequireComponent(typeof(BulletController))]

    public class BulletSoundController : MonoBehaviour
    {
        #region Fields and events

        [Header("References")]
        [Inject] private readonly BulletController _bulletController;
        [Space]
        [Inject] private readonly ISoundManager _soundManager;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _bulletController.BulletInitialized += OnBulletInitialized;
        }

        private void OnDisable()
        {
            _bulletController.BulletInitialized -= OnBulletInitialized;
        }

        #endregion

        #region EventCallbacks

        private void OnBulletInitialized()
        {

            PlayShootedSound();
        }

        #endregion

        #region Sound

        private void PlayShootedSound()
        {
            _soundManager.Play2DSound(_bulletController.BulletData.SoundsOnShoot);
        }

        #endregion
    }
}