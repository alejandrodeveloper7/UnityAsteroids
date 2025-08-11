using DG.Tweening;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.PlayerHealthBarUI
{
    public class PlayerHealthBarUIView : ModuleView
    {
        #region Fields     

        [Header("MVC References")]
        private PlayerHealthBarUIController _controller;
        private PlayerHealthBarUIModel _model;
        private SO_PlayerHealthBarUIConfiguration _configuration;

        [SerializeField] private GameObject _generalContainer;

        [Header("Healt Points")]
        [SerializeField] private GameObject _healthPointPrefab;
        [Space]
        [SerializeField] private Transform _healthPointsContainer;
        private List<Image> _currentHealtPoints = new List<Image>();
        [Space]
        private Sprite _healthPointSprite;
        private Sprite _emptyHealthPointSprite;

        [Header("Shield Bar")]
        [SerializeField] private Slider _shieldSlider;
        [SerializeField] private Image _shieldSliderImage;
        [Space]
        private Sprite _shieldBarSprite;
        private Sprite _fullShieldBarSprite;


        #endregion

        #region Monobehaviour        

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            RegisterListeners();
        }

        private void OnDestroy()
        {
            UnRegisterListeners();
        }

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            _controller = GetComponent<PlayerHealthBarUIController>();
            _model = _controller.Model;
            _configuration = _controller.ModuleConfigurationData;

            _healthPointSprite = _configuration.HealthPointSprite;
            _emptyHealthPointSprite = _configuration.emptyHealtPointSprite;

            _shieldBarSprite = _configuration.ShieldBarSprite;
            _fullShieldBarSprite = _configuration.FullShieldBarSprite;

            _shieldSlider.maxValue = _configuration.ShielSliderMaxValue;
        }

        protected override void RegisterListeners()
        {
            _model.OnHealthUpdated += OnHealthUpdated;
            _model.OnShieldSliderValueUpdated += OnShieldSliderValueUpdated;
        }

        protected override void UnRegisterListeners()
        {
            _model.OnHealthUpdated -= OnHealthUpdated;
            _model.OnShieldSliderValueUpdated -= OnShieldSliderValueUpdated;
        }

        #endregion

        #region Model Callbacks     

        private void OnHealthUpdated(int pValue)
        {
            SetCurrentHealth(pValue);
        }

        private void OnShieldSliderValueUpdated(float pValue)
        {
            SetShieldSliderValue(pValue);
        }

        #endregion

        #region View Methods   

        public void TurnGeneralContainer(bool pState)
        {
            _generalContainer.SetActive(pState);
        }

        public void SetMaxPosibleHealthValue(int pAmount)
        {
            foreach (var item in _currentHealtPoints)
                Destroy(item.gameObject);

            _currentHealtPoints.Clear();

            for (int i = 0; i < pAmount; i++)
            {
                Image newHeathPoint = Instantiate(_healthPointPrefab, _healthPointsContainer).GetComponent<Image>();
                newHeathPoint.sprite = _healthPointSprite;
                _currentHealtPoints.Add(newHeathPoint);
            }
        }

        public void SetCurrentHealth(int pAmount)
        {
            for (int i = 0; i < _currentHealtPoints.Count; i++)
            {
                if (i < pAmount)
                    _currentHealtPoints[i].sprite = _healthPointSprite;
                else
                    _currentHealtPoints[i].sprite = _emptyHealthPointSprite;
            }
        }

        public void SetShieldSliderValue(float pValue)
        {
            _shieldSlider.value = pValue;
            UpdateSliderSprite();
        }

        public void DoShielSliderTransition(float pValue, float pDuration)
        {
            if (pValue < _shieldSlider.maxValue)
                _shieldSliderImage.sprite = _shieldBarSprite;

            _shieldSlider.DOValue(pValue, pDuration)
                .OnComplete(() =>
                {
                    UpdateSliderSprite();
                });
        }


        private void UpdateSliderSprite()
        {
            if (_shieldSlider.value == _shieldSlider.maxValue)
                _shieldSliderImage.sprite = _fullShieldBarSprite;
            else
                _shieldSliderImage.sprite = _shieldBarSprite;
        }

        public async void EnterTransition(float pDelay, float pTransitionDuration, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            SetViewAlpha(0);
            TurnGeneralContainer(true);
            DoFadeTransition(1, pTransitionDuration);
            await Task.Delay((int)(pTransitionDuration * 1000));
            pOnComplete?.Invoke();
        }

        public async void ExitTransiion(float pDelay, float pTransitionDuration, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            SetViewAlpha(1);
            DoFadeTransition(0, pTransitionDuration);
            await Task.Delay((int)(pTransitionDuration * 1000));
            TurnGeneralContainer(false);
            pOnComplete?.Invoke();
        }

        #endregion

    }
}