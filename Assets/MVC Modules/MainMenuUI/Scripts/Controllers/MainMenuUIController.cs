using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Core.ScriptableObjects.Collections;
using Asteroids.UI.Controllers;
using ToolsACG.MVCModulesCreator.Bases;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Services;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Services;
using Zenject;
using Asteroids.Core.Interfaces;
using System.Collections.Generic;
using Asteroids.MVC.MainMenuUI.Views;
using Asteroids.MVC.MainMenuUI.Models;
using Asteroids.MVC.MainMenuUI.ScriptableObjects;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Asteroids.MVC.MainMenuUI.Controllers
{
    public class MainMenuUIController : MVCControllerBase, IMainMenuUIController
    {
        #region Private Fields

        [Header("References")]
        [Inject] private readonly IContainerRuntimeDataService _runtimeDataService;
        [Inject] private readonly IUserNameGenerationService _userNameGenerationService;
        [Space]
        [SerializeField] private UISelectorController _shipSelectorController;
        [SerializeField] private UIStatsDisplayerController _shipStatsDisplayerController;
        [Space]
        [SerializeField] private UISelectorController _bulletSelectorController;
        [SerializeField] private UIStatsDisplayerController _bulletStatsDisplayerController;
       
        [Header("View")]
        [Inject] private readonly IMainMenuUIView _view;

        [Header("Model")]
        [Inject] private readonly MainMenuUIModel _model;

        [Header("Data")]
        [Inject] private readonly SO_MainMenuUIConfiguration _configurationData;
        [Space]
        [Inject] private readonly SO_BulletsCollection _bulletsCollection;
        [Inject] private readonly SO_ShipsCollection _shipsCollection;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();

            // Not used thanks to Zenject injection
            //_view = GetComponent<IMainMenuUIView>();
        }

        protected override void Initialize()
        {
            InitializeUserName();
            InitializeShipSelector();
            InitializeBulletSelector();
        }

        protected override void RegisterActions()
        {
            EventBusManager.GameFlowBus.AddListener<GameInitialized>(OnGameInitialized);
            EventBusManager.GameFlowBus.AddListener<GoToMainMenuRequested>(OnGoToMainMenuRequested);
            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);

            _shipSelectorController.SelectedElementUpdated += OnSelectedShipUpdated;
            _bulletSelectorController.SelectedElementUpdated += OnSelectedBulletUpdated;

            Actions["BTN_Leaderboard"] = (Action<Button>)((btn) => OnLeaderboardButtonPressed(btn));
            Actions["BTN_Play"] = (Action<Button>)((btn) => OnPlayButtonPressed(btn));
            Actions["BTN_ExitGame"] = (Action<Button>)((btn) => OnExitGameButtonPressed(btn));
            Actions["INF_Username"] = (Action<TMP_InputField>)((inf) => OnUserNameInputFieldValueChanged(inf));
        }

        protected override void UnRegisterActions()
        {
            EventBusManager.GameFlowBus.RemoveListener<GameInitialized>(OnGameInitialized);
            EventBusManager.GameFlowBus.RemoveListener<GoToMainMenuRequested>(OnGoToMainMenuRequested);
            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);

            _shipSelectorController.SelectedElementUpdated -= OnSelectedShipUpdated;
            _bulletSelectorController.SelectedElementUpdated -= OnSelectedBulletUpdated;

            Actions.Clear();
        }

        protected override void CreateModel()
        {
            // Not used thanks to Zenject injection
            //_model = new MainMenuUIModel(_configurationData);
        }

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            CreateModel();
            RegisterActions();
        }

        protected override void Start()
        {
            base.Start();

            Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnRegisterActions();
        }

        #endregion

        #region Event callbacks

        private void OnGameInitialized(GameInitialized gameInitialized)
        {
            _ = ViewBase.PlayEnterTransition(_configurationData.DelayBeforeEnter, _configurationData.TransitionDuration);
        }

        private void OnGoToMainMenuRequested(GoToMainMenuRequested goToMainMenuRequested)
        {
            _ = ViewBase.PlayEnterTransition(_configurationData.DelayBeforeEnter, _configurationData.TransitionDuration);
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _ = ViewBase.PlayEnterTransition(_configurationData.DelayBeforeEnter, _configurationData.TransitionDuration);
        }

        private void OnSelectedShipUpdated(int id)
        {
            SO_ShipData shipData = _shipsCollection.GetElementById(_shipSelectorController.SelectedId);
            _shipStatsDisplayerController.SetStatsValues(shipData, true);
        }

        private void OnSelectedBulletUpdated(int id)
        {
            SO_BulletData bulletData = _bulletsCollection.GetElementById(_bulletSelectorController.SelectedId);
            _bulletStatsDisplayerController.SetStatsValues(bulletData, true);
        }

        #endregion

        #region UI Elements Actions callbacks

        private void OnLeaderboardButtonPressed(Button button)
        {
            _ = ViewBase.PlayExitTransition(_configurationData.DelayBeforeExit, _configurationData.TransitionDuration);
            EventBusManager.GameFlowBus.RaiseEvent(new GoToLeaderboardRequested());
        }

        private void OnPlayButtonPressed(Button button)
        {
            SaveSelectedValues();
            _ = ViewBase.PlayExitTransition(_configurationData.DelayBeforeExit, _configurationData.TransitionDuration);
            EventBusManager.GameFlowBus.RaiseEvent(new StartRunRequested());
        }

        private void OnExitGameButtonPressed(Button button)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnUserNameInputFieldValueChanged(TMP_InputField field)
        {
            _model.SetUserName(field.text);
        }

        #endregion

        #region Initializations

        private void InitializeUserName()
        {
            _model.SetUserName(_userNameGenerationService.GetUsername());

            // By saving this value during initialization, we can immediately display the player's score on the leaderboard if they are using the system username.
            _runtimeDataService.Data.UserName = _model.UserName;
        }

        private void InitializeShipSelector()
        {
            List<ISelectorElement> CastedCollection = _shipsCollection.Elements.Cast<ISelectorElement>().ToList();
            _shipSelectorController.SetData(CastedCollection);
        }

        private void InitializeBulletSelector()
        {
            List<ISelectorElement> CastedCollection = _bulletsCollection.Elements.Cast<ISelectorElement>().ToList();
            _bulletSelectorController.SetData(CastedCollection);
        }

        #endregion

        #region Data Managements

        private void SaveSelectedValues() 
        {
            _runtimeDataService.Data.SelectedBulletData = _bulletsCollection.GetElementById(_bulletSelectorController.SelectedId);
            _runtimeDataService.Data.SelectedShipData = _shipsCollection.GetElementById(_shipSelectorController.SelectedId);
            _runtimeDataService.Data.UserName = _model.UserName;
        }

        #endregion
    }
}