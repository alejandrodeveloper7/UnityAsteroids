using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Managers;
using Asteroids.MVC.PauseUI.Models;
using Asteroids.MVC.PauseUI.ScriptableObjects;
using Asteroids.MVC.PauseUI.Views;
using System;
using TMPro;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Services;
using ToolsACG.MVCModulesCreator.Bases;
using ToolsACG.UI.Utilitys.EventHandlers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.MVC.PauseUI.Controllers
{
    public class PauseUIController : MVCControllerBase, IPauseUIController
    {
        #region Private Fields

        [Header("References")]
        [Inject] private readonly IPauseManager _pauseManager;
        [Inject] private readonly ISettingsService _settingsService;
        [Inject] private readonly IScreenService _screenService;
        [Space]
        [SerializeField] private UISliderEventsHandler _musicVolumeSliderEventHandler;
        [SerializeField] private UISliderEventsHandler _effectsVolumeSliderEventHandler;
      
        [Header("View")]
        [Inject] private readonly IPauseUIView _view;

        [Header("Model")]
        [Inject] private readonly PauseUIModel _model;

        [Header("Data")]
        [Inject] private readonly SO_PauseUIConfiguration _configurationData;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();

            // Not used thanks to Zenject injection
            //_view = GetComponent<IPauseUIView>();
        }

        protected override void Initialize()
        {
            InitializePauseOptions();
        }

        protected override void RegisterActions()
        {
            _pauseManager.GamePaused += OnGamePaused;
            _pauseManager.GameResumed += OnGameResumed;

            _musicVolumeSliderEventHandler.OnEndDrag += OnMusicSliderEndDrag;
            _effectsVolumeSliderEventHandler.OnEndDrag += OnEffectsSliderEndDrag;

            Actions["SLD_Music"] = (Action<Slider>)((sld) => OnMusicSliderValueChaged(sld));
            Actions["SLD_Effects"] = (Action<Slider>)((sld) => OnEffectsSliderValueChaged(sld));
            Actions["DPD_Resolution"] = (Action<TMP_Dropdown>)((dpd) => OnResolutionDropdownValueChanged(dpd));
            Actions["TGL_FullScreen"] = (Action<Toggle>)((tgl) => OnFullScreenToggleValueChanged(tgl));
            Actions["BTN_LeaveGame"] = (Action<Button>)((btn) => OnLeaveGameButtonPressed(btn));
            Actions["BTN_Resume"] = (Action<Button>)((btn) => OnResumeButtonPressed(btn));
        }

        protected override void UnRegisterActions()
        {
            if (_pauseManager != null)
            {
                _pauseManager.GamePaused -= OnGamePaused;
                _pauseManager.GameResumed -= OnGameResumed;
            }

            _musicVolumeSliderEventHandler.OnEndDrag -= OnMusicSliderEndDrag;
            _effectsVolumeSliderEventHandler.OnEndDrag -= OnEffectsSliderEndDrag;

            Actions.Clear();
        }

        protected override void CreateModel()
        {
            // Not used thanks to Zenject injection
            //_model = new PauseUIModel(_configurationData);
        }

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            CreateModel();
            RegisterActions();
        }

        protected override void Start()
        {
            base.Start();

            Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnRegisterActions();
        }

        #endregion

        #region Event callbacks

        private void OnGamePaused()
        {
            _model.InPause = true;
            _ = ViewBase.PlayEnterTransition(_configurationData.DelayBeforeEnter, _configurationData.TransitionDuration);
        }
        private void OnGameResumed()
        {
            _model.InPause = false;
            _ = ViewBase.PlayExitTransition(_configurationData.DelayBeforeExit, _configurationData.TransitionDuration);
        }

        private void OnMusicSliderEndDrag(float value)
        {
            _settingsService.SaveMusicVolume(value);
        }

        private void OnEffectsSliderEndDrag(float value)
        {
            _settingsService.SaveEffectsVolume(value);
        }

        #endregion

        #region UI Elements Actions callbacks

        private void OnMusicSliderValueChaged(Slider slider)
        {
            _settingsService.SetMusicVolume(slider.value);
        }

        private void OnEffectsSliderValueChaged(Slider slider)
        {
            _settingsService.SetEffectsVolume(slider.value);
        }

        private void OnResolutionDropdownValueChanged(TMP_Dropdown dropdown)
        {
            _settingsService.SetResolution(dropdown.value);
        }

        private void OnFullScreenToggleValueChanged(Toggle toggle)
        {
            _settingsService.SetFullScreen(toggle.isOn);
        }

        private void OnLeaveGameButtonPressed(Button button)
        {
            _pauseManager.Resume();
            EventBusManager.GameFlowBus.RaiseEvent(new RunExitRequested());
        }

        private void OnResumeButtonPressed(Button button)
        {
            _pauseManager.Resume();
        }

        #endregion

        #region Values Initialization

        private void InitializePauseOptions()
        {
            _view.SetResolutionsDropdownOptionsAndIndex(_screenService.AvailableResolutionsOptions, _settingsService.GetResolutionIndex());
            _view.SetMusicSliderValue(_settingsService.GetMusicVolume());
            _view.SetEffectsSliderValue(_settingsService.GetEffectsVolume());
            _view.SetFullScreenModeToggleState(_settingsService.GetFullScreen());
        }

        #endregion
    }
}