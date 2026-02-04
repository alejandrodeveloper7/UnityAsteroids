using Asteroids.MVC.PauseUI.Controllers;
using Asteroids.MVC.PauseUI.Models;
using Asteroids.MVC.PauseUI.ScriptableObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using ToolsACG.Core.Utils;
using ToolsACG.MVCModulesCreator.Bases;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.MVC.PauseUI.Views
{
    public class PauseUIView : MVCViewBase, IPauseUIView
    {
        #region Fields     

        [Header("MVC References")]
        [Inject] private readonly IPauseUIController _controller;
        [Inject] private readonly PauseUIModel _model;
        [Inject] private readonly SO_PauseUIConfiguration _configuration;

        [Header("References")]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;
        [Space]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [Space]
        [SerializeField] private Toggle _fullScreenToggle;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();

            // Not used thanks to Zenject injection
            //_controller = GetComponent<IMainMenuUIController>();
            //_model = _controller.Model;
            //_configuration = _controller.ModuleConfigurationData;
        }

        protected override void Initialize()
        {
            base.Initialize();

            SetGeneralContainerActive(_configuration.ViewInitialState);
            SetGeneralContainerScale(_configuration.ViewInitialScale);
            SetAlpha(_configuration.ViewInitialAlpha);
        }

        protected override void RegisterListeners()
        {
            // TODO: Add listeners for the events
        }

        protected override void UnRegisterListeners()
        {
            // TODO: Remove listeners for the events
        }

        #endregion

        #region Monobehaviour        

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            RegisterListeners();
        }

        protected override void Start()
        {
            base.Start();

            Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnRegisterListeners();
        }

        #endregion

        #region Event Callbacks

        // TODO: Define here callbacks for the events

        #endregion

        #region public Methods   

        public void SetMusicSliderValue(float value)
        {
            _musicSlider.value = value;
        }
        public void SetEffectsSliderValue(float value)
        {
            _effectsSlider.value = value;
        }
        public void SetFullScreenModeToggleState(bool state)
        {
            _fullScreenToggle.isOn = state;
        }

        public void SetResolutionsDropdownOptionsAndIndex(List<string> options, int selectedIndex)
        {
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = selectedIndex;
            _resolutionDropdown.RefreshShownValue();
        }

        public override async Task PlayEnterTransition(float delay, float transitionDuration)
        {
            // This override prevents visual timming errors when the pause button is spammed.

            CleanTweens();
            await TimingUtils.WaitSeconds(Time.deltaTime,true);
            _=base.PlayEnterTransition(delay, transitionDuration);
        }

        #endregion

        #region View Methods 

        // TODO: Define here methods called from other view methods     

        #endregion
    }
}