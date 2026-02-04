using ACG.Core.EventBus;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Events.Gameplay;
using Asteroids.MVC.ScoreUI.Models;
using Asteroids.MVC.ScoreUI.ScriptableObjects;
using Asteroids.MVC.ScoreUI.Views;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.ScoreUI.Controllers
{
    public class ScoreUIController : MVCControllerBase, IScoreUIController
    {
        #region Private Fields

        [Header("View")]
        [Inject] private readonly IScoreUIView _view;

        [Header("Model")]
        [Inject] private readonly ScoreUIModel _model;

        [Header("Data")]
        [Inject] private readonly SO_ScoreUIConfiguration _configurationData;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();

            // Not used thanks to Zenject injection
            //_view = GetComponent<IScoreUIView>();
        }

        protected override void Initialize()
        {
            // TODO: Initialize controller
        }

        protected override void RegisterActions()
        {
            EventBusManager.GameFlowBus.AddListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);
            EventBusManager.GameFlowBus.AddListener<GoToMainMenuRequested>(OnGoToMainMenuRequested);

            EventBusManager.GameplayBus.AddListener<ScoreUpdated>(OnScoreUpdated);
        }

        protected override void UnRegisterActions()
        {
            EventBusManager.GameFlowBus.RemoveListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);
            EventBusManager.GameFlowBus.RemoveListener<GoToMainMenuRequested>(OnGoToMainMenuRequested);

            EventBusManager.GameplayBus.RemoveListener<ScoreUpdated>(OnScoreUpdated);

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

        private void OnRunStarted(RunStarted runStarted)
        {
            _ = ViewBase.PlayEnterTransition(_configurationData.DelayBeforeEnter, _configurationData.TransitionDuration);
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _ = ViewBase.PlayExitTransition(_configurationData.DelayBeforeExit, _configurationData.TransitionDuration);
        }

        private void OnGoToMainMenuRequested(GoToMainMenuRequested goToMainMenuRequested)
        {
            _ = ViewBase.PlayExitTransition(_configurationData.DelayBeforeExit, _configurationData.TransitionDuration);
        }

        private void OnScoreUpdated(ScoreUpdated scoreUpdated)
        {
            if (scoreUpdated.Score is 0)
                _model.RestartScore();
            else
                _model.SetScore(scoreUpdated.Score);
        }

        #endregion

        #region UI Elements Actions callbacks

        // TODO: Declare here the callbacks of the UI interactios registered in the Actions Dictionary

        #endregion
    }
}