using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Services;
using Zenject;
using Asteroids.MVC.MainMenuUI.Views;
using Asteroids.MVC.MainMenuUI.Models;
using Asteroids.MVC.MainMenuUI.ScriptableObjects;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using ACG.Core.EventBus;
using Asteroids.Core.ScriptableObjects.Data;

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

        [Header("View")]
        [Inject] private readonly IMainMenuUIView _view;

        [Header("Model")]
        [Inject] private readonly MainMenuUIModel _model;

        [Header("Data")]
        [Inject] private readonly SO_MainMenuUIConfiguration _configuration;


        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();
        }

        protected override void Initialize()
        {
            _model.Initialize();
        }

        protected override void RegisterActions()
        {
            EventBusManager.GameFlowBus.AddListener<GameInitialized>(OnGameInitialized);
            EventBusManager.GameFlowBus.AddListener<GoToMainMenuRequested>(OnGoToMainMenuRequested);
            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);

            _model.UserNameUpdated += OnUserNameUpdated;
            _model.ShipSelected += OnShipSelected;
            _model.BulletSelected += OnBulletSelected;

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

            _model.UserNameUpdated -= OnUserNameUpdated;
            _model.ShipSelected -= OnShipSelected;
            _model.BulletSelected -= OnBulletSelected;

            Actions.Clear();
        }

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
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
            _ = ViewBase.PlayEnterTransition(_configuration.DelayBeforeEnter, _configuration.TransitionDuration);
        }

        private void OnGoToMainMenuRequested(GoToMainMenuRequested goToMainMenuRequested)
        {
            _ = ViewBase.PlayEnterTransition(_configuration.DelayBeforeEnter, _configuration.TransitionDuration);
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _ = ViewBase.PlayEnterTransition(_configuration.DelayBeforeEnter, _configuration.TransitionDuration);
        }

        private void OnUserNameUpdated(string newName) 
        {
            _runtimeDataService.Data.UserName = newName;
        }

        private void OnShipSelected(SO_ShipData data) 
        {
            _runtimeDataService.Data.SelectedShipData = data;
        }

        private void OnBulletSelected(SO_BulletData data) 
        {
            _runtimeDataService.Data.SelectedBulletData = data;
        }

        #endregion

        #region UI Elements Actions callbacks

        private void OnLeaderboardButtonPressed(Button button)
        {
            _ = ViewBase.PlayExitTransition(_configuration.DelayBeforeExit, _configuration.TransitionDuration);
            EventBusManager.GameFlowBus.RaiseEvent(new GoToLeaderboardRequested());
        }

        private void OnPlayButtonPressed(Button button)
        {
            _ = ViewBase.PlayExitTransition(_configuration.DelayBeforeExit, _configuration.TransitionDuration);
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
    }
}