using Asteroids.Core.ScriptableObjects.Configurations;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Backgrounds.Controllers
{
    public class BackgroundController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [Inject] private readonly BackgroundVisualsController _backgroundVisualsController;

        [Header("Data")]
        [Inject] private readonly SO_BackgroundConfiguration _backgroundConfiguration;

        #endregion

        #region Initialization

        private void Initialize() 
        {
            _backgroundVisualsController.SetAlphaValue(_backgroundConfiguration.InitialAlphaValue);
            _backgroundVisualsController.DoFadeTransition(_backgroundConfiguration.FinalAlphaValue, _backgroundConfiguration.AlphaTransitionDuration);
        }

        #endregion

        #region Monobehaviour

        private void Start()
        {
            Initialize();
            PlayMovementLoop();
        }

        #endregion

        #region Movement

        private void PlayMovementLoop()
        {
            DOTween.Sequence()
         .Append(transform.DOLocalMove(_backgroundConfiguration.FinalPosition, _backgroundConfiguration.MovementDuration).SetEase(_backgroundConfiguration.MovementEase))
         .Append(transform.DOLocalMove(_backgroundConfiguration.InitialPosition, _backgroundConfiguration.MovementDuration).SetEase(_backgroundConfiguration.MovementEase))
         .SetLoops(-1, LoopType.Yoyo);
        }

        #endregion
    }
}