using ACG.Core.EventBus;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.Core.Events.GameFlow;
using Asteroids.MVC.LeaderboardUI.Models;
using Asteroids.MVC.LeaderboardUI.ScriptableObjects;
using Asteroids.MVC.LeaderboardUI.Views;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.MVC.LeaderboardUI.Controllers
{
    public class LeaderboardUIController : MVCControllerBase, ILeaderboardUIController
    {
        #region Private Fields
       
        [Header("View")]
        [Inject] private readonly ILeaderboardUIView _view;

        [Header("Model")]
        [Inject] private readonly LeaderboardUIModel _model;

        [Header("Data")]
        [Inject] readonly SO_LeaderboardUIConfiguration _configuration;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();
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

        private async void OnGoToLeaderboardRequested(GoToLeaderboardRequested goToLeaderboardRequested) 
        {
            _view.RestartView();
            await ViewBase.PlayEnterTransition(_configuration.DelayBeforeEnter, _configuration.TransitionDuration);
        }

        private async void OnRunEnded(RunEnded runEnded)
        {
            _view.RestartView();
            await ViewBase.PlayEnterTransition(_configuration.DelayBeforeEnter, _configuration.TransitionDuration);
        }

        private void OnLeaderBoardDataReady(LeaderBoardDataReady leaderBoardDataReady)
        {
            if (leaderBoardDataReady.Success)
            {
                _model.UpdateUsername();
                _model.SetLeaderboardData(leaderBoardDataReady.LeaderboardData);
            }
            else 
            {
                _view.DisplayLeaderboardError();
            }
        }

        #endregion

        #region UI Elements Actions callbacks

        private void OnBackToMenuButtonPressed(Button button)
        {
            EventBusManager.GameFlowBus.RaiseEvent(new GoToMainMenuRequested());
            _ = ViewBase.PlayExitTransition(_configuration.DelayBeforeExit, _configuration.TransitionDuration);
        }

        #endregion
    }
}