using System;
using System.Threading.Tasks;
using ToolsACG.Utils.PlayerPrefs;
using UnityEngine;

namespace ToolsACG.Scenes.Pause
{
    [RequireComponent(typeof(PauseView))]
    public class PauseController : ModuleController
    {
        #region Private Fields

        private IPauseView _view;
        [SerializeField] private PauseModel _data;
        [Space]
        private bool _inPause;

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            Initialize();
            RegisterActions();
        }

        protected override void RegisterActions()
        {
            Actions.Add("SLD_Music", OnMusicSliderValueChaged);
            Actions.Add("SLD_Effects", OnEffectsSliderValueChaged);
            Actions.Add("DPD_Resolution", OnResolutionDropdownValueChanged);
            Actions.Add("TGL_FullScreen", OnFullScreenToggleValueChanged);
            Actions.Add("BTN_LeaveGame", OnLeaveGameButtonClicked);
            Actions.Add("BTN_Resume", OnResumeButtonClicked);

            _view.OnMusicSliderEndEdit += SaveMusicVolume;
            _view.OnEffectsSliderEndEdit += SaveEffectsVolume;
        }

        protected override void UnRegisterActions()
        {
            // TODO: Unregister listeners and dictionarie actions.      
        }

        protected override void GetReferences()
        {
            _view = GetComponent<IPauseView>();
        }

        protected override void Initialize()
        {
            _view.TurnGeneralContainer(false);

            InitializeAvailableResolutions();
            InitializeVolumeSliders();
            InitializeToggles();
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            EventManager.InputBus.AddListener<PauseKeyClicked>(OnPauseKeyClicked);
            EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
        }

        private void OnDisable()
        {
            EventManager.InputBus.RemoveListener<PauseKeyClicked>(OnPauseKeyClicked);
            EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
        }

        #endregion

        #region Bus callbacks

        private void OnPauseKeyClicked(PauseKeyClicked pPauseKeyClicked)
        {
            _inPause = !_inPause;
            EventManager.GameplayBus.RaiseEvent(new PauseStateChanged(_inPause));

            _view.TurnGeneralContainer(_inPause);

            if (_inPause)
            {
                View.SetViewAlpha(1);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }

            Debug.Log(string.Format("- PAUSE - Pause state changed to {0}", _inPause));
        }

        private void OnGameLeaved(GameLeaved pGameLeaved)
        {
            _inPause = false;
            EventManager.GameplayBus.RaiseEvent(new PauseStateChanged(_inPause));
            DoExitWithDelay(0);

            Time.timeScale = 1;
            Debug.Log(string.Format("- PAUSE - Pause state changed to {0}", _inPause));
        }

        #endregion

        #region UI Elements Callbacks

        private void OnMusicSliderValueChaged()
        {
            EventManager.UIBus.RaiseEvent(new MusicVolumeUpdated(_view.MusicVolume));
        }
        private void OnEffectsSliderValueChaged()
        {
            EventManager.UIBus.RaiseEvent(new EffectsVolumeUpdated(_view.EffectsVolume));
        }

        private void OnResolutionDropdownValueChanged()
        {
            ScreenManager.UpdateResolution(_view.ResolutionIndex);
            PlayerPrefsManager.SetInt(PlayerPrefsKeys.RESOLUTION_INDEX_KEY, _view.ResolutionIndex);
        }

        private void OnFullScreenToggleValueChanged()
        {
            ScreenManager.UpdateFullScreenMode(_view.FullScreen);
            PlayerPrefsManager.SetInt(PlayerPrefsKeys.FULL_SCREEN_MODE_KEY, _view.FullScreen ? 1 : 0);
        }

        private void OnLeaveGameButtonClicked()
        {
            EventManager.UIBus.RaiseEvent(new GameLeaved());

        }
        private void OnResumeButtonClicked()
        {
            OnPauseKeyClicked(new PauseKeyClicked());
        }

        private void SaveMusicVolume(float pValue)
        {
            PlayerPrefsManager.SetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, pValue);
        }
        private void SaveEffectsVolume(float pValue)
        {
            PlayerPrefsManager.SetFloat(PlayerPrefsKeys.EFFECTS_VOLUME_KEY, pValue);
        }

        #endregion

        #region initialization

        private void InitializeAvailableResolutions()
        {
            int resolutionIndex = PlayerPrefsManager.GetInt(PlayerPrefsKeys.RESOLUTION_INDEX_KEY, 0);
            ScreenManager.UpdateResolution(resolutionIndex);
            _view.SetResolutionsOptionsAndIndex(ScreenManager.AvailableResolutionsOptions, resolutionIndex);
        }

        private void InitializeVolumeSliders()
        {
            float musicVolume = PlayerPrefsManager.GetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, 1);
            EventManager.UIBus.RaiseEvent(new MusicVolumeUpdated(musicVolume));
            _view.SetMusicVolume(musicVolume);

            float effectsVolume = PlayerPrefsManager.GetFloat(PlayerPrefsKeys.EFFECTS_VOLUME_KEY, 1);
            EventManager.UIBus.RaiseEvent(new EffectsVolumeUpdated(effectsVolume));
            _view.SetEffectsVolume(effectsVolume);
        }

        private void InitializeToggles()
        {
            bool fullScreen = Convert.ToBoolean(PlayerPrefsManager.GetInt(PlayerPrefsKeys.FULL_SCREEN_MODE_KEY, 0));
            ScreenManager.UpdateFullScreenMode(fullScreen);
            _view.SetFullScreenMode(fullScreen);
        }

        #endregion

        #region Navigation

        private async void DoExitWithDelay(float pDelay, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            View.SetViewAlpha(1);
            View.DoFadeTransition(0, _data.FadeTransitionDuration);
            await Task.Delay((int)(_data.FadeTransitionDuration * 1000));
            _view.TurnGeneralContainer(false);
            pOnComplete?.Invoke();
        }

        #endregion
    }
}

#region IEvents

public readonly struct PauseStateChanged : IEvent
{
    public readonly bool InPause;

    public PauseStateChanged(bool inPause)
    {
        InPause = inPause;
    }
}

public readonly struct MusicVolumeUpdated : IEvent
{
    public readonly float Value;

    public MusicVolumeUpdated(float value)
    {
        Value = value;
    }
}

public readonly struct EffectsVolumeUpdated : IEvent
{
    public readonly float Value;

    public EffectsVolumeUpdated(float value)
    {
        Value = value;
    }
}
public readonly struct GameLeaved : IEvent
{

}

#endregion