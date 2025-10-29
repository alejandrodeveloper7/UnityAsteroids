using Asteroids.Core.Events.GameFlow;
using Asteroids.MVC.LeaderboardUI.Models;
using Asteroids.MVC.LeaderboardUI.ScriptableObjects;
using Asteroids.MVC.LeaderboardUI.Views;
using System;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Services;
using ToolsACG.MVCModulesCreator.Bases;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.MVC.LeaderboardUI.Controllers
{
    public class LeaderboardUIController : MVCControllerBase, ILeaderboardUIController
    {
        #region Private Fields

        [Header("References")]
        [Inject] private readonly IContainerRuntimeDataService _runtimeDataService;
       
        [Header("View")]
        [Inject] private readonly ILeaderboardUIView _view;

        [Header("Model")]
        [Inject] private readonly LeaderboardUIModel _model;

        [Header("Data")]
        [Inject] readonly SO_LeaderboardUIConfiguration _configurationData;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();

            // Not used thanks to Zenject injection
            //_view = GetComponent<ILeaderboardUIView>();
        }

        protected override void Initialize()
        {
            // TODO: Initialize controller
        }

        protected override void RegisterActions()
        {
            EventBusManager.GameFlowBus.AddListener<GoToLeaderboardRequested>(OnGoToLeaderboardRequested);
            EventBusManager.GameFlowBus.AddListener<RunEnded>(OnRunEnded);
            EventBusManager.GameFlowBus.AddListener<LeaderBoardDataReady>(OnLeaderBoardDataReady);

            Actions["BTN_BackToMenu"] = (Action<Button>)(btn => OnBackToMenuButtonPressed(btn));
        }

        protected override void UnRegisterActions()
        {
            EventBusManager.GameFlowBus.RemoveListener<GoToLeaderboardRequested>(OnGoToLeaderboardRequested);
            EventBusManager.GameFlowBus.RemoveListener<RunEnded>(OnRunEnded);
            EventBusManager.GameFlowBus.RemoveListener<LeaderBoardDataReady>(OnLeaderBoardDataReady);

            Actions.Clear();
        }

        protected override void CreateModel()
        {
            // Not used thanks to Zenject injection
            //_model = new LeaderboardUIModel(_configurationData);
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

        private async void OnGoToLeaderboardRequested(GoToLeaderboardRequested goToLeaderboardRequested) 
        {
            _view.RestartView();
            await ViewBase.PlayEnterTransition(_configurationData.DelayBeforeEnter, _configurationData.TransitionDuration);
        }

        private async void OnRunEnded(RunEnded runEnded)
        {
            _view.RestartView();
            await ViewBase.PlayEnterTransition(_configurationData.DelayBeforeEnter, _configurationData.TransitionDuration);
        }

        private void OnLeaderBoardDataReady(LeaderBoardDataReady leaderBoardDataReady)
        {
            if (leaderBoardDataReady.Success)
                _view.UpdateLeaderboardRowsData(leaderBoardDataReady.LeaderboardData.Entry, _runtimeDataService.Data.UserName);
            else
                _view.DisplayLeaderboardError();
        }

        #endregion

        #region UI Elements Actions callbacks

        private void OnBackToMenuButtonPressed(Button button)
        {
            EventBusManager.GameFlowBus.RaiseEvent(new GoToMainMenuRequested());
            _ = ViewBase.PlayExitTransition(_configurationData.DelayBeforeExit, _configurationData.TransitionDuration);
        }

        #endregion

    }
}