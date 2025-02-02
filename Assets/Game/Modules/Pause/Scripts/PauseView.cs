using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.Pause
{
    public interface IPauseView
    {
        void TurnGeneralContainer(bool pState);

        void SetViewAlpha(float pValue);
        void ViewFadeTransition(float pDestinyValue, float pDuration);

        float MusicVolume { get; }
        float EffectsVolume { get; }
        int ResolutionIndex { get; }
        bool FullScreen { get; }

        void SetResolutionsOptionsAndIndex(List<string> pOptions, int pSelectedIndex);
    }

    public class PauseView : ModuleView, IPauseView
    {
        #region Fields        

        [SerializeField] private GameObject _generalContainer;
        [Space(20)]

        [SerializeField] private Slider _musicSlider;
        public float MusicVolume { get { return _musicSlider.value; } }
        [Space]
        [SerializeField] private Slider _effectsSlider;
        public float EffectsVolume { get { return _effectsSlider.value; } }
        [Space]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        public int ResolutionIndex { get { return _resolutionDropdown.value; } }
        [Space]
        [SerializeField] private Toggle _fullScreenToggle;
        public bool FullScreen { get { return _fullScreenToggle.isOn; } }

        #endregion

        #region Protected Methods     

        protected override void Awake()
        {
            base.Awake();
        }

        #endregion

        #region View Methods

        public void TurnGeneralContainer(bool pState)
        {
            _generalContainer.SetActive(pState);
        }

        public void SetViewAlpha(float pValue)
        {
            CanvasGroup.alpha = pValue;
        }

        public void ViewFadeTransition(float pDestinyValue, float pDuration)
        {
            CanvasGroup.DOKill();
            CanvasGroup.DOFade(pDestinyValue, pDuration).SetEase(Ease.OutQuad);
        }

        public void SetResolutionsOptionsAndIndex(List<string> pOptions, int pSelectedIndex) 
        {
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(pOptions);
            _resolutionDropdown.value=pSelectedIndex;
            _resolutionDropdown.RefreshShownValue();
        }

        #endregion

        #region Private Methods
        // TODO: define here methods called from view methods
        #endregion
    }
}