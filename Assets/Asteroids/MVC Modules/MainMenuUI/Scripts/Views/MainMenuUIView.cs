using ACG.Core.Utils;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.MVC.MainMenuUI.Controllers;
using Asteroids.MVC.MainMenuUI.Models;
using Asteroids.MVC.MainMenuUI.ScriptableObjects;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.MVC.MainMenuUI.Views
{
    public class MainMenuUIView : MVCViewBase, IMainMenuUIView
    {
        #region Fields     

        [Header("MVC References")]
        [Inject] private readonly IMainMenuUIController _controller;
        [Inject] private readonly MainMenuUIModel _model;
        [Inject] private readonly SO_MainMenuUIConfiguration _configuration;

        [Header("References")]
        [SerializeField] private TMP_InputField _userNameInputField;
        [Space]
        [SerializeField] private Button _playButton;

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
            _model.UserNameUpdated += OnUserNameUpdated;
        }

        protected override void UnRegisterListeners()
        {
            _model.UserNameUpdated -= OnUserNameUpdated;
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

        private void OnUserNameUpdated(string userName)
        {
            // This line overwrites the input in case of additional validations, such as removing special characters or similar done in the controller.
            _userNameInputField.text = userName;

            if (string.IsNullOrEmpty(userName))
                _playButton.interactable = false;
            else
                _playButton.interactable = true;
        }

        #endregion

        #region Public Methods

        public override async Task PlayExitTransition(float delay, float transitionDuration)
        {
            // This override prevents the buttons from looking disabled when the menu opens.

            await TimingUtils.WaitSeconds(delay, true);
            //SetCanvasGroupDetection(false);
            _ = PlayScaleTransition(0, transitionDuration);
            await PlayAlphaTransition(0, transitionDuration);
            SetGeneralContainerActive(false);
        }

        #endregion

        #region View Methods   

        // TODO: Define here methods called from other view methods     

        #endregion
    }
}