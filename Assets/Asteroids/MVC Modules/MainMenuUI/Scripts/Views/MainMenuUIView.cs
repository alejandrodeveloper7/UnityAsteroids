using ACG.Core.Utils;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.Core.Interfaces;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.MVC.MainMenuUI.Controllers;
using Asteroids.MVC.MainMenuUI.Models;
using Asteroids.MVC.MainMenuUI.ScriptableObjects;
using Asteroids.UI.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.MVC.MainMenuUI.Views
{
    public class MainMenuUIView : MVCViewBase, IMainMenuUIView
    {
        #region Fields and events    

        [Header("MVC References")]
        [Inject] private readonly IMainMenuUIController _controller;
        [Inject] private readonly MainMenuUIModel _model;
        [Inject] private readonly SO_MainMenuUIConfiguration _configuration;

        [Header("Gameplay References")]
        [SerializeField] private TMP_InputField _userNameInputField;
        [Space]
        [SerializeField] private Button _playButton;

        [Header("Selectors")]
        [SerializeField] private UISelectorController _shipSelectorController;
        [SerializeField] private UISelectorController _bulletSelectorController;

        [Header("Stat Displayers")]
        [SerializeField] private UIStatsDisplayerController _shipStatsDisplayerController;
        [SerializeField] private UIStatsDisplayerController _bulletStatsDisplayerController;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();
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
            _model.ShipsInitialized += OnShipsInitialized;
            _model.BulletsInitialized += OnBulletsInitialized;
            _model.ShipSelected += OnShipSelected;
            _model.BulletSelected += OnBulletSelected;

            _shipSelectorController.SelectedElementUpdated += OnSelectedShipUpdated;

            _bulletSelectorController.SelectedElementUpdated += OnSelectedBulletUpdated;
        }

        protected override void UnRegisterListeners()
        {
            _model.UserNameUpdated -= OnUserNameUpdated;
            _model.ShipsInitialized -= OnShipsInitialized;
            _model.BulletsInitialized -= OnBulletsInitialized;
            _model.ShipSelected -= OnShipSelected;
            _model.BulletSelected -= OnBulletSelected;

            _shipSelectorController.SelectedElementUpdated -= OnSelectedShipUpdated;

            _bulletSelectorController.SelectedElementUpdated -= OnSelectedBulletUpdated;
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
            // This line overwrites the input in case of additional validations done in the model, such as removing special characters or similar.
            _userNameInputField.text = userName;

            if (string.IsNullOrEmpty(userName))
                _playButton.interactable = false;
            else
                _playButton.interactable = true;
        }

        private void OnShipsInitialized(List<ISelectorElement> shipsCollection)
        {
            _shipSelectorController.SetData(shipsCollection);
        }

        private void OnBulletsInitialized(List<ISelectorElement> bulletsCollection)
        {
            _bulletSelectorController.SetData(bulletsCollection);
        }

        private void OnShipSelected(SO_ShipData data)
        {
            _shipStatsDisplayerController.SetStatsValues(data, true);
        }

        private void OnBulletSelected(SO_BulletData data)
        {
            _bulletStatsDisplayerController.SetStatsValues(data, true);
        }

        private void OnSelectedShipUpdated(int id)
        {
            _model.SetShipSelectedId(id);
        }

        private void OnSelectedBulletUpdated(int id)
        {
            _model.SetBulletSelectedId(id);
        }

        #endregion

        #region Public Methods

        // This override prevents the buttons from looking disabled when the menu is openned.
        public override async Task PlayExitTransition(float delay, float transitionDuration)
        {
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