using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.Pause
{
    public interface IPauseView
    {
        void TurnGeneralContainer(bool pState);

        void SetMusicVolume(float pValue);
        float MusicVolume { get; }
       
        void SetEffectsVolume(float pValue);
        float EffectsVolume { get; }
       
        void SetResolutionsOptionsAndIndex(List<string> pOptions, int pSelectedIndex);
        int ResolutionIndex { get; }
     
        void SetFullScreenMode(bool pState);
        bool FullScreen { get; }

        Action<float> OnMusicSliderEndEdit { get; set; }
        Action<float> OnEffectsSliderEndEdit { get; set; }
    }

    public class PauseView : ModuleView, IPauseView
    {
        #region Fields        

        [SerializeField] private GameObject _generalContainer;
        [Space(20)]

        [SerializeField] private Slider _musicSlider;
        public float MusicVolume { get { return _musicSlider.value; } }
        [SerializeField] private SliderEventsHandler _musicVolumeSliderEventHandler;
        public Action<float> OnMusicSliderEndEdit { get; set; }
        [Space]
        [SerializeField] private Slider _effectsSlider;
        public float EffectsVolume { get { return _effectsSlider.value; } }
        [SerializeField] private SliderEventsHandler _effectsVolumeSliderEventHandler;
        public Action<float> OnEffectsSliderEndEdit { get; set; }
        [Space]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        public int ResolutionIndex { get { return _resolutionDropdown.value; } }
        [Space]
        [SerializeField] private Toggle _fullScreenToggle;
        public bool FullScreen { get { return _fullScreenToggle.isOn; } }

        #endregion

        #region Protected Methods     

        private void OnEnable()
        {
            _musicVolumeSliderEventHandler.OnEndDrag += MusicSliderEndEdit;
            _effectsVolumeSliderEventHandler.OnEndDrag += EffectsSliderEndEdit;
        }

        private void OnDisable()
        {
            _musicVolumeSliderEventHandler.OnEndDrag -= MusicSliderEndEdit;
            _effectsVolumeSliderEventHandler.OnEndDrag -= EffectsSliderEndEdit;
        }
        #endregion

        #region View Methods

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

        #endregion

        #region Private Methods

        private void MusicSliderEndEdit()
        {
            OnMusicSliderEndEdit?.Invoke(_musicSlider.value);
        }

        private void EffectsSliderEndEdit()
        {
            OnEffectsSliderEndEdit?.Invoke(_effectsSlider.value);
        }

        #endregion
    }
}