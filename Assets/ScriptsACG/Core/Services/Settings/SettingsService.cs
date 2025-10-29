using System;
using ToolsACG.Core.Keys;
using ToolsACG.Core.Managers;
using ToolsACG.ServicesCreator.Bases;
using UnityEngine;
using Zenject;

namespace ToolsACG.Core.Services
{
    public class SettingsService : InstancesServiceBase, ISettingsService
    {
        #region Fields

        [Header("References")]
        private readonly IScreenService _screenService;
        private readonly ISoundManager _soundManager;
        private readonly IPersistentDataService _persistentDataService;

        #endregion

        #region Constructors

        [Inject]
        public SettingsService(IScreenService screenService, ISoundManager soundManager, IPersistentDataService persistentDataService)
        {
            _screenService = screenService;
            _soundManager = soundManager;
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
            ApplyStoredMusicVolume();
            ApplyStoredEffectsVolume();
            ApplyStoredFullScreen();
            ApplyStoredResolution();
        }

        #endregion

        #region FullScreen

        public void ApplyStoredFullScreen()
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

        #region Volumes

        public void ApplyStoredMusicVolume()
        {
            _soundManager.SetMusicVolume(GetMusicVolume());
        }
        public void SetMusicVolume(float value)
        {
            _soundManager.SetMusicVolume(value);
        }
        public void SaveMusicVolume(float value)
        {
            _persistentDataService.SetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, value);
        }
        public float GetMusicVolume()
        {
            return _persistentDataService.GetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, 1);
        }


        public void ApplyStoredEffectsVolume()
        {
            _soundManager.SetEffectsVolume(GetEffectsVolume());
        }
        public void SetEffectsVolume(float value)
        {
            _soundManager.SetEffectsVolume(value);
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

        #region Resolution

        public void ApplyStoredResolution()
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

        #endregion
    }
}