using ACG.Core.Keys;
using ACG.Scripts.Managers;
using ACG.Tools.Runtime.ServicesCreator.Bases;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ACG.Scripts.Services
{
    public class SettingsService : InstancesServiceBase, ISettingsService
    {
        #region Fields

        [Header("References")]
        private readonly IScreenService _screenService;
        private readonly ISoundService _soundService;

        private readonly IPersistentDataService _persistentDataService;

        #endregion

        #region Constructors

        [Inject]
        public SettingsService(IScreenService screenService, ISoundService soundService, IPersistentDataService persistentDataService)
        {
            _screenService = screenService;
            _soundService = soundService;
            _persistentDataService = persistentDataService;

            Initialize();
        }

        #endregion

        #region Initialization     

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method called in the constructor to initialize the Service
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the elements that need be clean when the Service is destroyed
        }

        #endregion

        #region Management

        public void ApplyAllStoredSettings()
        {
            SetStoredMusicVolume();
            SetStoredEffectsVolume();
            SetStoredFullScreen();
            SetStoredResolution();
        }

        #endregion

        #region FullScreen

        public void SetStoredFullScreen()
        {
            _screenService.SetFullScreenMode(GetFullScreen());
        }

        public void SetFullScreen(bool isFullScreen)
        {
            _screenService.SetFullScreenMode(isFullScreen);
            _persistentDataService.SetInt(PlayerPrefsKeys.FULL_SCREEN_MODE_KEY, isFullScreen ? 1 : 0);
        }

        public bool GetFullScreen()
        {
            return Convert.ToBoolean(_persistentDataService.GetInt(PlayerPrefsKeys.FULL_SCREEN_MODE_KEY, 0));
        }

        #endregion

        #region Music Volume

        public void SetStoredMusicVolume()
        {
            _soundService.SetMusicVolume(GetMusicVolume());
        }

        public void SetMusicVolume(float value)
        {
            _soundService.SetMusicVolume(value);
        }

        public void SaveMusicVolume(float value)
        {
            _persistentDataService.SetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, value);
        }

        public float GetMusicVolume()
        {
            return _persistentDataService.GetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, 1);
        }

        #endregion

        #region Music mute

        public void SetStoredMusicMute()
        {
            _soundService.SetMuteMusicState(GetMusicMuted());
        }

        public void SetMusicMute(bool state)
        {
            _soundService.SetMuteMusicState(state);
            _persistentDataService.SetInt(PlayerPrefsKeys.MUSIC_MUTED_KEY, state ? 1 : 0);
        }

        public bool GetMusicMuted()
        {
            return Convert.ToBoolean(_persistentDataService.GetInt(PlayerPrefsKeys.MUSIC_MUTED_KEY, 0));
        }

        #endregion

        #region Effects Volume

        public void SetStoredEffectsVolume()
        {
            _soundService.SetEffectsVolume(GetEffectsVolume());
        }

        public void SetEffectsVolume(float value)
        {
            _soundService.SetEffectsVolume(value);
        }

        public void SaveEffectsVolume(float value)
        {
            _persistentDataService.SetFloat(PlayerPrefsKeys.EFFECTS_VOLUME_KEY, value);
        }

        public float GetEffectsVolume()
        {
            return _persistentDataService.GetFloat(PlayerPrefsKeys.EFFECTS_VOLUME_KEY, 1);
        }

        #endregion

        #region Effects mute

        public void SetStoredEffectsMute()
        {
            _soundService.SetMuteEffectsState(GetEffectsMuted());
        }

        public void SetEffectsMute(bool state)
        {
            _soundService.SetMuteEffectsState(state);
            _persistentDataService.SetInt(PlayerPrefsKeys.EFFECTS_MUTED_KEY, state ? 1 : 0);
        }

        public bool GetEffectsMuted()
        {
            return Convert.ToBoolean(_persistentDataService.GetInt(PlayerPrefsKeys.EFFECTS_MUTED_KEY, 0));
        }

        #endregion

        #region Resolution

        public void SetStoredResolution()
        {
            _screenService.SetResolution(GetResolutionIndex());
        }

        public void SetResolution(int index)
        {
            _screenService.SetResolution(index);
            _persistentDataService.SetInt(PlayerPrefsKeys.RESOLUTION_INDEX_KEY, index);
        }

        public int GetResolutionIndex()
        {
            return _persistentDataService.GetInt(PlayerPrefsKeys.RESOLUTION_INDEX_KEY, 0);
        }

        public List<string> GetAvailableResolutionOptions()
        {
            return _screenService.AvailableResolutionsOptions;
        }

        #endregion
    }
}