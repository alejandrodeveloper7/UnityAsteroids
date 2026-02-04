using ACG.Core.EventBus;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Core.Services;
using Asteroids.MVC.PlayerHealthBarUI.Models;
using Asteroids.MVC.PlayerHealthBarUI.ScriptableObjects;
using Asteroids.MVC.PlayerHealthBarUI.Views;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.PlayerHealthBarUI.Controllers
{
    public class PlayerHealthBarUIController : MVCControllerBase, IPlayerHealthBarUIController
    {
        #region Private Fields

        [Header("References")]
        [Inject] private readonly IContainerRuntimeDataService _runtimeDataService;

        [Header("Model")]
        [Inject] private readonly IPlayerHealthBarUIView _view;

        [Header("Model")]
        [Inject] private readonly PlayerHealthBarUIModel _model;

        [Header("Data")]
        [Inject] private readonly SO_PlayerHealthBarUIConfiguration _configurationData;
        [Space]
        private SO_ShipData _shipData;

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
            EventBusManager.GameFlowBus.AddListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);

            EventBusManager.GameplayBus.AddListener<PlayerShieldStateChanged>(OnPlayerShieldStateChanged);
            EventBusManager.GameplayBus.AddListener<PlayerDamaged>(OnPlayerDamaged);
            EventBusManager.GameplayBus.AddListener<PlayerDied>(OnPlayerDied);
        }

        protected override void UnRegisterActions()
        {
            EventBusManager.GameFlowBus.RemoveListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);

            EventBusManager.GameplayBus.RemoveListener<PlayerShieldStateChanged>(OnPlayerShieldStateChanged);
            EventBusManager.GameplayBus.RemoveListener<PlayerDamaged>(OnPlayerDamaged);
            EventBusManager.GameplayBus.RemoveListener<PlayerDied>(OnPlayerDied);

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
            _shipData = _runtimeDataService.Data.SelectedShipData;
            InitializeRun();
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _model.StopShieldRecoveryProcess();
            _ = ViewBase.PlayExitTransition(_configurationData.DelayBeforeExit, _configurationData.TransitionDuration);
        }

        private void OnPlayerShieldStateChanged(PlayerShieldStateChanged playerShieldStateChanged)
        {
            if (playerShieldStateChanged.Active)
                return;

            _ = LoseShield();
        }

        private void OnPlayerDamaged(PlayerDamaged playerDamaged)
        {
            _model.SetCurrentHealth(playerDamaged.Health);
        }

        private void OnPlayerDied(PlayerDied playerDied)
        {
            _model.StopShieldRecoveryProcess();
            _ = ViewBase.PlayExitTransition(_configurationData.DelayBeforeExit, _configurationData.TransitionDuration);
        }

        #endregion

        #region UI Elements Actions callbacks

        // TODO: Declare here the callbacks of the UI interactios registered in the Actions Dictionary

        #endregion

        #region Functionality

        private void InitializeRun()
        {
            _view.SetShieldSliderColors(_shipData.ShieldSliderRecoveringColor, _shipData.ShieldSliderFullColor, _shipData.ShieldShineColor);

            _view.SetMaxPosibleHealthValue(_shipData.HealthPoints);
            _view.SetMaxPosibleShieldSliderValue(_shipData.ShielSliderMaxValue);

            _model.RestartHealth(_shipData.HealthPoints);
            _model.RestartShield(_shipData.ShielSliderMaxValue);

            _ = ViewBase.PlayEnterTransition(_configurationData.DelayBeforeEnter, _configurationData.TransitionDuration);
        }

        private async Task LoseShield()
        {
            _model.ShieldLost();
            await _view.PlayShieldLostSliderTransition(_shipData.ShieldSliderMinValue);
            float shieldRecoveryDuration = _shipData.ShieldRecoveryTime - _configurationData.ShieldSliderTransitionDuration;
            _model.StartShieldRecoveryProcess(_shipData.ShieldSliderMinValue, _shipData.ShielSliderMaxValue, shieldRecoveryDuration);
        }

        #endregion
    }
}