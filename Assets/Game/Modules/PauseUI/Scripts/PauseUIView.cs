using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.PauseUI
{
    public class PauseUIView : ModuleView
    {
        #region Fields     

        [Header("MVC References")]
        private PauseUIController _controller;
        private PauseUIModel _model;
        private SO_PauseUIConfiguration _configuration;

        [Header("References")]
        [SerializeField] private GameObject _generalContainer;
        [Space(20)]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private SliderEventsHandler _musicVolumeSliderEventHandler;
        [Space]
        [SerializeField] private Slider _effectsSlider;
        [SerializeField] private SliderEventsHandler _effectsVolumeSliderEventHandler;
        [Space]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [Space]
        [SerializeField] private Toggle _fullScreenToggle;
    
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
            _controller = GetComponent<PauseUIController>();
            _model = _controller.Model;
            _configuration = _controller.ModuleConfigurationData;
        }

        protected override void RegisterListeners()
        {
            _musicVolumeSliderEventHandler.OnEndDrag += MusicSliderEndEdit;
            _effectsVolumeSliderEventHandler.OnEndDrag += EffectsSliderEndEdit;
        }

        protected override void UnRegisterListeners()
        {
            _musicVolumeSliderEventHandler.OnEndDrag -= MusicSliderEndEdit;
            _effectsVolumeSliderEventHandler.OnEndDrag -= EffectsSliderEndEdit;
        }

        #endregion

        #region Model Callbacks

        // TODO: Define here callbacks of the events from the model

        #endregion

        #region View Methods 

        private void MusicSliderEndEdit()
        {
            _controller.SaveMusicVolume(_musicSlider.value);
        }

        private void EffectsSliderEndEdit()
        {
            _controller.SaveEffectsVolume(_effectsSlider.value);
        }

        #endregion

        #region public Methods   

        public void TurnGeneralContainer(bool pState)
        {
            _generalContainer.SetActive(pState);
        }

        public void SetMusicVolume(float pValue)
        {
            _musicSlider.value = pValue;
        }
        public void SetEffectsVolume(float pValue)
        {
            _effectsSlider.value = pValue;
        }
        public void SetFullScreenMode(bool pState)
        {
            _fullScreenToggle.isOn = pState;
        }

        public void SetResolutionsOptionsAndIndex(List<string> pOptions, int pSelectedIndex)
        {
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(pOptions);
            _resolutionDropdown.value = pSelectedIndex;
            _resolutionDropdown.RefreshShownValue();
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

        public async void ExitTransion(float pDelay, float pTransitionDuration, Action pOnComplete = null)
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