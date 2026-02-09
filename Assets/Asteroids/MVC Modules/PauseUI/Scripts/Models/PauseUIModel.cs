using ACG.Scripts.Services;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.MVC.PauseUI.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.PauseUI.Models
{
    public class PauseUIModel : MVCModelBase
    {
        #region Fields and Properties

        public bool InPause { get; private set; }

        public float MusicVolume { get; private set; }
        public float EffectsVolume { get; private set; }

        public bool FullScreenState { get; private set; }

        public int ResolutionIndex { get; private set; }

        public List<string> ResolutionOptions { get; private set; }

        [Header("Data")]
        private readonly SO_PauseUIConfiguration _configuration;

        [Header("References")]
        private readonly ISettingsService _settingsService;

        #endregion

        #region Events

        public event Action ValuesInitialized;

        public event Action<bool> PauseStateUpdated;

        public event Action<float> MusicVolumeUpdated;
        public event Action<float> EffectsVolumeUpdated;

        public event Action<bool> FullScreanStateUpdated;

        public event Action<int> ResolutionIndexUpdated;

        #endregion

        #region Constructors

        [Inject]
        public PauseUIModel(SO_PauseUIConfiguration configuration, ISettingsService settingsService)
        {
            _configuration = configuration;
            _settingsService = settingsService;
        }

        #endregion

        #region Initialization

        public void Initialize()
        {
            MusicVolume = _settingsService.GetMusicVolume();
            EffectsVolume = _settingsService.GetEffectsVolume();
            FullScreenState = _settingsService.GetFullScreen();
            ResolutionIndex = _settingsService.GetResolutionIndex();

            ResolutionOptions = _settingsService.GetAvailableResolutionOptions();

            ValuesInitialized?.Invoke();
        }

        #endregion

        #region

        public void SetPauseState(bool state)
        {
            InPause = state;
            PauseStateUpdated?.Invoke(InPause);
        }

        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            MusicVolumeUpdated?.Invoke(MusicVolume);
        }

        public void SetEffectsVolume(float value)
        {
            EffectsVolume = value;
            EffectsVolumeUpdated?.Invoke(EffectsVolume);
        }

        public void SetFullScreenState(bool state)
        {
            FullScreenState = state;
            FullScreanStateUpdated?.Invoke(FullScreenState);
        }

        public void SetResolutionIndex(int index)
        {
            ResolutionIndex = index;
            ResolutionIndexUpdated?.Invoke(ResolutionIndex);
        }

        #endregion
    }
}