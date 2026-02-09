using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.MVC.PlayerHealthBarUI.Controllers;
using Asteroids.MVC.PlayerHealthBarUI.Models;
using Asteroids.MVC.PlayerHealthBarUI.ScriptableObjects;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.MVC.PlayerHealthBarUI.Views
{
    public class PlayerHealthBarUIView : MVCViewBase, IPlayerHealthBarUIView
    {
        #region Fields     

        [Header("MVC References")]
        [Inject] private readonly IPlayerHealthBarUIController _controller;
        [Inject] private readonly PlayerHealthBarUIModel _model;
        [Inject] private readonly SO_PlayerHealthBarUIConfiguration _configuration;

        [Header("General")]
        [SerializeField] private Transform _panel;

        [Header("Healt Points")]
        [SerializeField] private Transform _healthPointsContainer;
        private readonly List<Image> _currentHealthPoints = new();

        [Header("Shield")]
        [SerializeField] private Slider _shieldSlider;
        [SerializeField] private Image _shieldSliderImage;
        [Space]
        [SerializeField] private Image _shieldShine;

        [Header("Values")]
        private Color _shieldSliderRecoveringColor;
        private Color _shieldSliderFullColor;
        [Space]
        private Color _shielShineColor;

        [Header("Cache")]
        private Sequence _blinkShineSequence;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();
        }

        protected override void Initialize()
        {
            base.Initialize();

            SetGeneralContainerActive(_configuration.ViewInitialState);
            SetGeneralContainerScale(_configuration.ViewInitialScale);
            SetAlpha(_configuration.ViewInitialAlpha);
        }

        protected override void RegisterListeners()
        {
            _model.RunInitialized += OnRunInitialized;

            _model.HealthRestarted += OnHealthRestarted;
            _model.HealthUpdated += OnHealthUpdated;

            _model.ShieldLost += OnShieldLost;
            _model.ShieldRestarted += OnShieldRestarted;
            _model.ShieldSliderValueUpdated += OnShieldSliderValueUpdated;
            _model.ShieldRestored += OnShieldRestored;
        }

        protected override void UnRegisterListeners()
        {
            _model.RunInitialized -= OnRunInitialized;

            _model.HealthRestarted -= OnHealthRestarted;
            _model.HealthUpdated -= OnHealthUpdated;

            _model.ShieldLost -= OnShieldLost;
            _model.ShieldRestarted -= OnShieldRestarted;
            _model.ShieldSliderValueUpdated -= OnShieldSliderValueUpdated;
            _model.ShieldRestored -= OnShieldRestored;
        }

        #endregion

        #region Monobehaviour           

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            RegisterListeners();
        }

        protected override void Start()
        {
            base.Start();

            Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnRegisterListeners();
        }

        #endregion

        #region Event Callbacks     

        private void OnRunInitialized(SO_ShipData data)
        {
            SetShieldSliderColors(data.ShieldSliderRecoveringColor, data.ShieldSliderFullColor, data.ShieldShineColor);

            SetMaxPosibleHealthValue(data.HealthPoints);
            SetMaxPosibleShieldSliderValue(data.ShieldSliderValueRange.Max);

            _ = PlayEnterTransition(_configuration.DelayBeforeEnter, _configuration.TransitionDuration);
        }

        private void OnHealthRestarted(int value)
        {
            SetCurrentHealth(value);
        }
        private void OnHealthUpdated(int value)
        {
            SetCurrentHealth(value);
            PlayFeedbackTween();
        }

        private void OnShieldLost() 
        {
            PlayShieldLostSliderTransition();
        }
        private void OnShieldRestarted(float value)
        {
            SetShieldSliderValue(value);
            UpdateShieldSliderColor(false);
            RestartShieldShine();
        }
        private void OnShieldSliderValueUpdated(float value)
        {
            SetShieldSliderValue(value);
        }
        private void OnShieldRestored(float value)
        {
            SetShieldSliderValue(value);
            UpdateShieldSliderColor(true);
            ActivateShieldShine();
        }

        #endregion

        #region public Methods

        // TODO: Define here methods called from the controller     

        #endregion

        #region Private methods

        private void PlayFeedbackTween()
        {
            _panel.DOKill();
            _panel.localScale = Vector3.one;

            _panel.DOScale(_configuration.PanelFeedbackScale, _configuration.PanelFeedbackDuration * 0.5f)
                .SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
        }

        #endregion


        #region Health 

        private void SetMaxPosibleHealthValue(int amount)
        {
            foreach (var item in _currentHealthPoints)
                Destroy(item.gameObject);

            _currentHealthPoints.Clear();

            for (int i = 0; i < amount; i++)
            {
                Image newHeathPoint = Instantiate(_configuration.HealthPointPrefab, _healthPointsContainer).GetComponent<Image>();
                newHeathPoint.sprite = _configuration.HealthPointSprite;
                _currentHealthPoints.Add(newHeathPoint);
            }
        }

        private void SetCurrentHealth(int amount)
        {
            for (int i = 0; i < _currentHealthPoints.Count; i++)
            {
                if (i < amount)
                    _currentHealthPoints[i].sprite = _configuration.HealthPointSprite;
                else
                    _currentHealthPoints[i].sprite = _configuration.EmptyHealtPointSprite;
            }
        }

        #endregion

        #region Shield

        public void SetShieldSliderColors(Color recoveringColor, Color FullColor, Color ShineColor)
        {
            _shieldSliderRecoveringColor = recoveringColor;
            _shieldSliderFullColor = FullColor;
            _shielShineColor = ShineColor;
        }

        public void SetMaxPosibleShieldSliderValue(float value)
        {
            _shieldSlider.maxValue = value;
        }
      
        public void SetShieldSliderValue(float value)
        {
            _shieldSlider.value = value;
        }

        public void PlayShieldLostSliderTransition()
        {
            PlayFeedbackTween();
            UpdateShieldSliderColor(true);
            DeactivateShieldShine();
            _ = _shieldSlider.DOValue(_model.ShipData.ShieldSliderValueRange.Min, _configuration.ShieldSliderTransitionDuration).AsyncWaitForCompletion();
        }

        private void UpdateShieldSliderColor(bool progressively)
        {
            _shieldSliderImage.DOKill();
            Color targetColor = _model.ShieldActive ? _shieldSliderFullColor : _shieldSliderRecoveringColor;

            if (progressively)
                _shieldSliderImage.DOColor(targetColor, _configuration.ShieldSliderTransitionDuration);
            else
                _shieldSliderImage.color = targetColor;
        }

        private void RestartShieldShine()
        {
            _shieldShine.enabled = true;
            _shieldShine.color = _shielShineColor;
            PlayShieldShineBlinkSequenceLoop();
        }
     
        private async void ActivateShieldShine()
        {
            _blinkShineSequence?.Kill();

            _shieldShine.enabled = true;
            await _shieldShine.DOFade(_configuration.ShieldShineBlinkAlphaRange.Max, _configuration.ShieldShineFadeInDuration).AsyncWaitForCompletion();
            PlayShieldShineBlinkSequenceLoop();
        }
     
        private async void DeactivateShieldShine()
        {
            _blinkShineSequence?.Kill();

            await _shieldShine.DOFade(0f, _configuration.ShieldShineFadeOutDuration).AsyncWaitForCompletion();
            _shieldShine.enabled = false;
        }
    
        private void PlayShieldShineBlinkSequenceLoop()
        {
            _blinkShineSequence?.Kill();

            _blinkShineSequence = DOTween.Sequence()
            .Append(_shieldShine.DOFade(_configuration.ShieldShineBlinkAlphaRange.Min, _configuration.ShieldShineBlinkDuration).SetEase(Ease.InOutSine))
            .Append(_shieldShine.DOFade(_configuration.ShieldShineBlinkAlphaRange.Max, _configuration.ShieldShineBlinkDuration).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Yoyo);
        }

        #endregion
    }
}