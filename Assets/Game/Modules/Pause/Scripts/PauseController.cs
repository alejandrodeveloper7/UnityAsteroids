using System;
using System.Collections.Generic;
using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

namespace ToolsACG.Scenes.Pause
{
    [RequireComponent(typeof(PauseView))]
    public class PauseController : ModuleController
    {
        #region Private Fields

        private IPauseView _view;
        private PauseModel _data;

        private bool _inPause;

        #endregion

        #region Properties

        public PauseModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<IPauseView>();
            Initialize();
            base.Awake();
            _data = new PauseModel();
        }

        protected override void RegisterActions()
        {
            Actions.Add("SLD_Music", OnMusicSliderValueChaged);
            Actions.Add("SLD_Effects", OnEffectsSliderValueChaged);
            Actions.Add("DPD_Resolution", OnResolutionDropdownValueChanged);
            Actions.Add("TGL_FullScreen", OnFullScreenToggleValueChanged);
            Actions.Add("BTN_LeaveGame", OnLeaveGameButtonClicked);
            Actions.Add("BTN_Resume", OnResumeButtonClicked);
        }

        protected override void SetData()
        {
            // TODO: initialize model with services data (if it's not initialized externally using Data property).
            // TODO: call view methods to display data.
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.GetGameplayBus().AddListener<PauseKeyClicked>(OnPauseKeyClicked);
        }

        private void OnDisable()
        {
            EventManager.GetGameplayBus().RemoveListener<PauseKeyClicked>(OnPauseKeyClicked);
        }

        #endregion

        #region Bus callbacks

        private void OnPauseKeyClicked(PauseKeyClicked pPauseKeyClicked)
        {
            _inPause = !_inPause;
            EventManager.GetGameplayBus().RaiseEvent(new PauseStateChanged() { InPause = _inPause });
            _view.TurnGeneralContainer(_inPause);

            if (_inPause)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        #endregion

        #region UI Elements Callbacks

        private void OnMusicSliderValueChaged()
        {
            EventManager.GetUiBus().RaiseEvent(new MusicVolumeUpdated() { Value = _view.MusicVolume });
            PlayerPrefsManager.SetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, _view.MusicVolume);
        }

        private void OnEffectsSliderValueChaged()
        {
            EventManager.GetUiBus().RaiseEvent(new EffectsVolumeUpdated() { Value = _view.EffectsVolume });
            PlayerPrefsManager.SetFloat(PlayerPrefsKeys.EFFECTS_VOLUME_KEY, _view.EffectsVolume);
        }

        private void OnResolutionDropdownValueChanged()
        {
            ScreenController.UpdateResolution(_view.ResolutionIndex);
            EventManager.GetUiBus().RaiseEvent(new ResolutionUpdated() { Index = _view.ResolutionIndex });
            PlayerPrefsManager.SetInt(PlayerPrefsKeys.RESOLUTION_INDEX_KEY, _view.ResolutionIndex);
        }

        private void OnFullScreenToggleValueChanged()
        {
            ScreenController.UpdateFullScreenMode(_view.FullScreen);
            EventManager.GetUiBus().RaiseEvent(new FullScreenModeUpdated() { Active = _view.FullScreen });
            PlayerPrefsManager.SetInt(PlayerPrefsKeys.FULL_SCREEN_MODE_KEY, _view.FullScreen ? 1 : 0);
        }

        private void OnLeaveGameButtonClicked()
        {
            EventManager.GetUiBus().RaiseEvent(new GameLeaved());

        }
        private void OnResumeButtonClicked()
        {
            OnPauseKeyClicked(null);
        }

        #endregion

        private void Initialize()
        {
            _view.TurnGeneralContainer(false);
            InitializeAvailableResolutions();
            InitializeVolumeSliders();
            InitializeToggles();
        }

        private void InitializeAvailableResolutions()
        {
            List<Vector2Int> availableResolutions = Screen.resolutions.Select(res => new Vector2Int(res.width, res.height)).Distinct().ToList();

            List<string> options = new List<string>();

            foreach (Vector2Int resolution in availableResolutions)
                options.Add(string.Format("{0}x{1}", resolution.x, resolution.y));

            int resolutionIndex = PlayerPrefsManager.GetInt(PlayerPrefsKeys.RESOLUTION_INDEX_KEY, 0);
            ScreenController.UpdateResolution(resolutionIndex);
            _view.SetResolutionsOptionsAndIndex(options, resolutionIndex);
        }

        private void InitializeVolumeSliders()
        {
            float musicVolume = PlayerPrefsManager.GetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, 1);
            EventManager.GetUiBus().RaiseEvent(new MusicVolumeUpdated() { Value = musicVolume });
            _view.SetMusicVolume(musicVolume);
          
            float effectsVolume = PlayerPrefsManager.GetFloat(PlayerPrefsKeys.EFFECTS_VOLUME_KEY, 1);
            EventManager.GetUiBus().RaiseEvent(new EffectsVolumeUpdated() { Value = effectsVolume });
            _view.SetEffectsVolume(effectsVolume);
        }

        private void InitializeToggles()
        {
            bool fullScreen = Convert.ToBoolean(PlayerPrefsManager.GetInt(PlayerPrefsKeys.FULL_SCREEN_MODE_KEY, 0));
            ScreenController.UpdateFullScreenMode(fullScreen);
            _view.SetFullScreenMode(fullScreen);
        }
    }
}
public class PauseStateChanged : IEvent
{
    public bool InPause { get; set; }
}

public class MusicVolumeUpdated : IEvent
{
    public float Value { get; set; }
}

public class EffectsVolumeUpdated : IEvent
{
    public float Value { get; set; }
}

public class ResolutionUpdated : IEvent
{
    public int Index { get; set; }
}
public class FullScreenModeUpdated : IEvent
{
    public bool Active { get; set; }
}
public class GameLeaved : IEvent
{
}