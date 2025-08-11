using System;
using TMPro;
using ToolsACG.Utils.PlayerPrefs;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.PauseUI
{
    [RequireComponent(typeof(PauseUIView))]
    public class PauseUIController : ModuleController
    {
        #region Private Fields

        [Header("View")]
        private PauseUIView _view;

        [Header("Model")]
        private PauseUIModel _model;
        public PauseUIModel Model { get { return _model; } }

        [Header("Data")]
        [SerializeField] SO_PauseUIConfiguration _configurationData;
        public SO_PauseUIConfiguration ModuleConfigurationData { get { return _configurationData; } }

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            CreateModel();
            RegisterActions();
            Initialize();
        }

        private void OnDestroy()
        {
            UnRegisterActions();
        }

        #endregion

        #region Initialization

        protected override void RegisterActions()
        {
            EventManager.InputBus.AddListener<PauseKeyClicked>(OnPauseKeyClicked);
            EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);

            Actions["SLD_Music"] = (Action<Slider>)((sld) => OnMusicSliderValueChaged(sld));
            Actions["SLD_Effects"] = (Action<Slider>)((sld) => OnEffectsSliderValueChaged(sld));
            Actions["DPD_Resolution"] = (Action<TMP_Dropdown>)((dpd) => OnResolutionDropdownValueChanged(dpd));
            Actions["TGL_FullScreen"] = (Action<Toggle>)((tgl) => OnFullScreenToggleValueChanged(tgl));
            Actions["BTN_LeaveGame"] = (Action<Button>)((btn) => OnLeaveGameButtonClicked(btn));
            Actions["BTN_Resume"] = (Action<Button>)((btn) => OnResumeButtonClicked(btn));

        }

        protected override void UnRegisterActions()
        {
            EventManager.InputBus.RemoveListener<PauseKeyClicked>(OnPauseKeyClicked);
            EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);

            Actions.Clear();
        }

        protected override void GetReferences()
        {
            _view = GetComponent<PauseUIView>();
        }

        protected override void Initialize()
        {
            _view.TurnGeneralContainer(false);

            InitializeAvailableResolutions();
            InitializeVolumeSliders();
            InitializeToggles();
        }

        protected override void CreateModel()
        {
            _model = new PauseUIModel();
        }

        #endregion

        #region UI Elements Actions callbacks

        private void OnMusicSliderValueChaged(Slider pSlider)
        {
            EventManager.SoundBus.RaiseEvent(new MusicVolumeUpdated(pSlider.value));
        }
        private void OnEffectsSliderValueChaged(Slider pSlider)
        {
            EventManager.SoundBus.RaiseEvent(new EffectsVolumeUpdated(pSlider.value));
        }

        private void OnResolutionDropdownValueChanged(TMP_Dropdown pDropdown)
        {
            ScreenManager.SetResolution(pDropdown.value);
            PlayerPrefsManager.SetInt(PlayerPrefsKeys.RESOLUTION_INDEX_KEY, pDropdown.value);
        }

        private void OnFullScreenToggleValueChanged(Toggle pToggle)
        {
            ScreenManager.SetFullScreenMode(pToggle.isOn);
            PlayerPrefsManager.SetInt(PlayerPrefsKeys.FULL_SCREEN_MODE_KEY, pToggle.isOn ? 1 : 0);
        }

        private void OnLeaveGameButtonClicked(Button pButton)
        {
            EventManager.UIBus.RaiseEvent(new GameLeaved());

        }
        private void OnResumeButtonClicked(Button pButton)
        {
            TurnPauseSate();
        }

        #endregion

        #region Bus callbacks

        private void OnPauseKeyClicked(PauseKeyClicked pPauseKeyClicked)
        {
            TurnPauseSate();
        }

        private void OnGameLeaved(GameLeaved pGameLeaved)
        {
            _model.InPause = false;
            _view.ExitTransion(_configurationData.DelayBeforeExit, _configurationData.FadeTransitionDuration, null);

            Time.timeScale = 1;
            EventManager.GameplayBus.RaiseEvent(new PauseStateChanged(_model.InPause));
            Debug.Log(string.Format("- PAUSE - Pause state changed to {0}", _model.InPause));
        }

        #endregion
                           
        #region Values Initialization

        private void InitializeAvailableResolutions()
        {
            int resolutionIndex = PlayerPrefsManager.GetInt(PlayerPrefsKeys.RESOLUTION_INDEX_KEY, 0);
            ScreenManager.SetResolution(resolutionIndex);
            _view.SetResolutionsOptionsAndIndex(ScreenManager.AvailableResolutionsOptions, resolutionIndex);
        }

        private void InitializeVolumeSliders()
        {
            float musicVolume = PlayerPrefsManager.GetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, 1);
            EventManager.SoundBus.RaiseEvent(new MusicVolumeUpdated(musicVolume));
            _view.SetMusicVolume(musicVolume);

            float effectsVolume = PlayerPrefsManager.GetFloat(PlayerPrefsKeys.EFFECTS_VOLUME_KEY, 1);
            EventManager.SoundBus.RaiseEvent(new EffectsVolumeUpdated(effectsVolume));
            _view.SetEffectsVolume(effectsVolume);
        }

        private void InitializeToggles()
        {
            bool fullScreen = Convert.ToBoolean(PlayerPrefsManager.GetInt(PlayerPrefsKeys.FULL_SCREEN_MODE_KEY, 0));
            ScreenManager.SetFullScreenMode(fullScreen);
            _view.SetFullScreenMode(fullScreen);
        }

        #endregion

        #region Functionality

        private void TurnPauseSate()
        {
            _model.InPause = !_model.InPause;
            _view.TurnGeneralContainer(_model.InPause);

            EventManager.GameplayBus.RaiseEvent(new PauseStateChanged(_model.InPause));
            Debug.Log(string.Format("- PAUSE - Pause state changed to {0}", _model.InPause));

            if (_model.InPause)
            {
                _view.SetViewAlpha(1);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        #endregion

        #region Methods called from View

        public void SaveMusicVolume(float pValue)
        {
            PlayerPrefsManager.SetFloat(PlayerPrefsKeys.MUSIC_VOLUME_KEY, pValue);
        }
        public void SaveEffectsVolume(float pValue)
        {
            PlayerPrefsManager.SetFloat(PlayerPrefsKeys.EFFECTS_VOLUME_KEY, pValue);
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