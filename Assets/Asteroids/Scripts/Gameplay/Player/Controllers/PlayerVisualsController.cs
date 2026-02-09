using ACG.Core.Utils;
using ACG.Scripts.Managers;
using Asteroids.Core.ScriptableObjects.Data;
using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerHealthController))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(SpriteRenderer))]

    public class PlayerVisualsController : MonoBehaviour
    {
        #region Fields

        public event Action PlayerAppearanceUpdated;

        [Header("References")]
        [Inject] private readonly PlayerController _playerController;
        [Inject] private readonly PlayerHealthController _playerHealthController;
        [Inject] private readonly PlayerMovementController _playerMovementController;
        [Space]
        [Inject] private readonly SpriteRenderer _shipSpriteRenderer;
        [Space]
        [SerializeField] private SpriteRenderer _shieldFX;
        [SerializeField] private SpriteRenderer _propulsionFireFX;
        [SerializeField] private ParticleSystem _propulsionParticlesFX;
        [Space]
        [Inject] private readonly ICameraFXManager _cameraFXManager;
        [Inject] private readonly IVFXManager _vFXManager;

        [Header("Data")]
        private SO_ShipData _shipData;

        [Header("Cache")]
        private Sequence _blinkSequence;
        private Sequence _invulnerabilityBlinkSequence;

        #endregion

        #region Initialization

        private void Initialize()
        {
            StopInvulnerabilityBlinkSequence();
            RestartShield();
            ConfigurePropulsionFire();
            ApplyShipAppearance();
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _playerController.PlayerInitialized += OnPlayerInitialized;
            _playerController.PlayerReadyToBeRecycled += OnPlayerReadyToBeRecycled;

            _playerMovementController.ForwardMovementStateChanged += OnForwardMovementStateChanged;

            _playerHealthController.PlayerShieldStateChanged += OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDamaged += OnPlayerDamaged;
            _playerHealthController.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            _playerController.PlayerInitialized -= OnPlayerInitialized;
            _playerController.PlayerReadyToBeRecycled -= OnPlayerReadyToBeRecycled;

            _playerMovementController.ForwardMovementStateChanged -= OnForwardMovementStateChanged;

            _playerHealthController.PlayerShieldStateChanged -= OnPlayerShieldStateChanged;
            _playerHealthController.PlayerDamaged -= OnPlayerDamaged;
            _playerHealthController.PlayerDied -= OnPlayerDied;
        }

        #endregion

        #region Event Callbacks

        private void OnPlayerInitialized(SO_ShipData data)
        {
            _shipData = data;
            Initialize();
        }

        private void OnPlayerReadyToBeRecycled()
        {
            StopInvulnerabilityBlinkSequence();
        }

        private void OnForwardMovementStateChanged(bool state)
        {
            UpdatePropulsionFire(state);
        }

        private void OnPlayerShieldStateChanged(bool state)
        {
            if (state)
                ActivateShield();
            else
            {
                DeactivateShield();
                PlayInvulnerabilityBlinkSequence();
                _cameraFXManager.PlayCameraShake(_shipData.ShieldLostCameraShakeData);
            }
        }

        private void OnPlayerDamaged()
        {
            PlayInvulnerabilityBlinkSequence();
            _cameraFXManager.PlayCameraShake(_shipData.DamageTakedCameraShakedata);
        }

        private void OnPlayerDied()
        {
            CreateDeadParticles();
            StopInvulnerabilityBlinkSequence();
            _cameraFXManager.PlayCameraShake(_shipData.DeadCameraShakeData);
        }

        #endregion

        #region Appearance

        private void ApplyShipAppearance()
        {
            _shipSpriteRenderer.sprite = _shipData.Sprite;
            _shipSpriteRenderer.color = _shipData.Color;

            PlayerAppearanceUpdated?.Invoke();
        }

        #endregion

        #region Shield Management

        private void RestartShield()
        {
            _shieldFX.enabled = true;
            _shieldFX.color = _shipData.ShieldColor;
            PlayShieldBlinkSequenceLoop();
        }

        private async void ActivateShield()
        {
            _shieldFX.enabled = true;
            _blinkSequence?.Kill();
            await _shieldFX.DOFade(_shipData.ShieldBlinkAlphaRange.Max, _shipData.ShieldFadeInDuration).AsyncWaitForCompletion();
            PlayShieldBlinkSequenceLoop();
        }

        private async void DeactivateShield()
        {
            _blinkSequence?.Kill();
            await _shieldFX.DOFade(0f, _shipData.ShieldFadeOutDuration).AsyncWaitForCompletion();
            _shieldFX.enabled = false;
        }

        private void PlayShieldBlinkSequenceLoop()
        {
            _blinkSequence = DOTween.Sequence()
            .Append(_shieldFX.DOFade(_shipData.ShieldBlinkAlphaRange.Min, _shipData.ShieldBlinkDuration).SetEase(Ease.InOutSine))
            .Append(_shieldFX.DOFade(_shipData.ShieldBlinkAlphaRange.Max, _shipData.ShieldBlinkDuration).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Yoyo);
        }

        #endregion

        #region Invulnerability

        private void PlayInvulnerabilityBlinkSequence()
        {
            StopInvulnerabilityBlinkSequence();

            _invulnerabilityBlinkSequence = DOTween.Sequence()
                .Append(_shipSpriteRenderer.DOFade(0f, _shipData.InvulnerabilityBlinkDuration).SetEase(Ease.Linear))
                .Append(_shipSpriteRenderer.DOFade(1f, _shipData.InvulnerabilityBlinkDuration).SetEase(Ease.Linear))
                .SetLoops(_shipData.TotalInvulnerabilityBlinks, LoopType.Yoyo);
        }

        private void StopInvulnerabilityBlinkSequence()
        {
            _invulnerabilityBlinkSequence?.Kill();
        }

        #endregion

        #region Particles

        private void CreateDeadParticles()
        {
            _vFXManager.PlayParticlesVFX(_shipData.ParticlesOnDead, transform.position, Quaternion.identity, null);
        }

        #endregion

        #region Propulsion Fire

        private void ConfigurePropulsionFire()
        {
            _propulsionFireFX.transform.localPosition = _shipData.PropulsionFireLocalPosition;
            _propulsionFireFX.color = ColorUtils.GetColorWithAlpha(_propulsionFireFX.color, 0);
        }

        private void UpdatePropulsionFire(bool state)
        {
            _propulsionFireFX.DOKill();
            float targetAlpha = state ? 1f : 0f;
            _propulsionFireFX.DOFade(targetAlpha, _shipData.PropulsionFireTransitionDuration);

            if (state)
                _propulsionParticlesFX.Play();
            else
                _propulsionParticlesFX.Stop();
        }

        #endregion
    }
}