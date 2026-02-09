using ACG.Core.EventBus;
using ACG.Scripts.Services;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Managers;
using Asteroids.MVC.PauseUI.Models;
using Asteroids.MVC.PauseUI.ScriptableObjects;
using Asteroids.MVC.PauseUI.Views;
using System;
using TMPro;
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

        [Header("View")]
        [Inject] private readonly IPauseUIView _view;

        [Header("Model")]
        [Inject] private readonly PauseUIModel _model;

        [Header("Data")]
        [Inject] private readonly SO_PauseUIConfiguration _configuration;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();
        }

        protected override void Initialize()
        {
            _model.Initialize();
        }

        protected override void RegisterActions()
        {
            _pauseManager.GamePaused += OnGamePaused;
            _pauseManager.GameResumed += OnGameResumed;

            _model.MusicVolumeUpdated += OnMusicVolumeUpdated;
            _model.EffectsVolumeUpdated += OnEffectsVolumeUpdated;

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

            _model.MusicVolumeUpdated -= OnMusicVolumeUpdated;
            _model.EffectsVolumeUpdated -= OnEffectsVolumeUpdated;

            Actions.Clear();
        }

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
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
            _model.SetPauseState(true);
        }
        private void OnGameResumed()
        {
            _model.SetPauseState(false);
        }

        private void OnMusicVolumeUpdated(float value)
        {
            _settingsService.SaveMusicVolume(value);
        }
        private void OnEffectsVolumeUpdated(float value)
        {
            _settingsService.SaveEffectsVolume(value);
        }

        #endregion

        #region UI Elements Actions callbacks

        // To avoid updating the model continuously during slider drag, 
        // the slider controls the temporary UI value, 
        // and the model is updated only once the drag ends, 
        // which also triggers saving the value in settings.

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
            _model.SetResolutionIndex(dropdown.value);
            _settingsService.SetResolution(_model.ResolutionIndex);
        }

        private void OnFullScreenToggleValueChanged(Toggle toggle)
        {
            _model.SetFullScreenState(toggle.isOn);
            _settingsService.SetFullScreen(_model.FullScreenState);
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
    }
}