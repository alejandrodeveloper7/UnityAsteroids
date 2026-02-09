using ACG.Core.Utils;
using ACG.Scripts.UIUtilitys.EventHandlers;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.MVC.PauseUI.Controllers;
using Asteroids.MVC.PauseUI.Models;
using Asteroids.MVC.PauseUI.ScriptableObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.MVC.PauseUI.Views
{
    public class PauseUIView : MVCViewBase, IPauseUIView
    {
        #region Fields     

        [Header("MVC References")]
        [Inject] private readonly IPauseUIController _controller;
        [Inject] private readonly PauseUIModel _model;
        [Inject] private readonly SO_PauseUIConfiguration _configuration;

        [Header("Gameplay References")]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private UISliderEventsHandler _musicVolumeSliderEventHandler;
        [Space]
        [SerializeField] private Slider _effectsSlider;
        [SerializeField] private UISliderEventsHandler _effectsVolumeSliderEventHandler;
        [Space]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [Space]
        [SerializeField] private Toggle _fullScreenToggle;

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
            _model.ValuesInitialized += OnValuesInitialized;
            _model.PauseStateUpdated += OnPauseStateUpdated;

            _musicVolumeSliderEventHandler.OnEndDrag += OnMusicSliderEndDrag;
            _effectsVolumeSliderEventHandler.OnEndDrag += OnEffectsSliderEndDrag;
        }

        protected override void UnRegisterListeners()
        {
            _model.ValuesInitialized -= OnValuesInitialized;
            _model.PauseStateUpdated -= OnPauseStateUpdated;

            _musicVolumeSliderEventHandler.OnEndDrag -= OnMusicSliderEndDrag;
            _effectsVolumeSliderEventHandler.OnEndDrag -= OnEffectsSliderEndDrag;
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

        private void OnValuesInitialized()
        {
            SetResolutionsDropdownOptionsAndIndex(_model.ResolutionOptions, _model.ResolutionIndex);
            SetMusicSliderValue(_model.MusicVolume);
            SetEffectsSliderValue(_model.EffectsVolume);
            SetFullScreenModeToggleState(_model.FullScreenState);
        }

        private void OnPauseStateUpdated(bool state)
        {
            if (state)
                _ = PlayEnterTransition(_configuration.DelayBeforeEnter, _configuration.TransitionDuration);
            else
                _ = PlayExitTransition(_configuration.DelayBeforeExit, _configuration.TransitionDuration);

        }

        private void OnMusicSliderEndDrag(float value)
        {
            _model.SetMusicVolume(value);
        }

        private void OnEffectsSliderEndDrag(float value)
        {
            _model.SetEffectsVolume(value);
        }

        #endregion

        #region public Methods   

        public override async Task PlayEnterTransition(float delay, float transitionDuration)
        {
            // This override prevents visual timming errors when the pause button is spammed.

            CleanTweens();
            await TimingUtils.WaitSeconds(Time.deltaTime, true);
            _ = base.PlayEnterTransition(delay, transitionDuration);
        }

        #endregion

        #region View Methods 

        private void SetMusicSliderValue(float value)
        {
            _musicSlider.value = value;
        }

        private void SetEffectsSliderValue(float value)
        {
            _effectsSlider.value = value;
        }

        private void SetFullScreenModeToggleState(bool state)
        {
            _fullScreenToggle.isOn = state;
        }

        private void SetResolutionsDropdownOptionsAndIndex(List<string> options, int selectedIndex)
        {
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = selectedIndex;
            _resolutionDropdown.RefreshShownValue();
        }

        #endregion
    }
}