using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.Services;
using System;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Services;
using ToolsACG.ManagersCreator.Bases;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Managers
{
    public class PauseManager : MonobehaviourInstancesManagerBase<PauseManager>, IPauseManager
    {
        #region Events fields and properties

        public event Action GamePaused;
        public event Action GameResumed;

        [Header("References")]
        [Inject] private readonly IDebugService _debugService;

        [Header("Configuration")]
        [SerializeField] private bool _autoPauseOnFocusLost;

        [Header("State")]
        private bool _canPause;
        public bool IsPaused { get; private set; }

        #endregion

        #region Initialization
  
        protected override void GetReferences()
        {
            base.GetReferences();
            // TODO: Get your references here
        }

        public void Setup()
        {
            // TODO: Manual method to set parameters
        }

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Manual method to initialize the manager (called in start)
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the listeners or elements that need be clean when the manager is destroyed
        }

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
        }

        protected override void Start()
        {
            base.Start();

            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                return;

            if (_autoPauseOnFocusLost && _canPause)
                Pause();
        }

        private void OnEnable()
        {
            EventBusManager.GameFlowBus.AddListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);

            EventBusManager.GameplayBus.AddListener<PlayerDied>(OnPlayerDied);
        }

        private void OnDisable()
        {
            EventBusManager.GameFlowBus.RemoveListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);

            EventBusManager.GameplayBus.RemoveListener<PlayerDied>(OnPlayerDied);
        }

        #endregion

        #region Event callbacks

        private void OnRunStarted(RunStarted runStarted)
        {
            _canPause = true;
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _canPause = false;
        }

        private void OnPlayerDied(PlayerDied playerDied)
        {
            _canPause = false;
        }

        #endregion

        #region Functionality

        public void TogglePause()
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }

        public void Pause()
        {
            if (IsPaused)
                return;

            IsPaused = true;
            TimeScaleService.Pause();
            GamePaused?.Invoke();
            _debugService.Log("Pause", "Game paused");
        }

        public void Resume()
        {
            if (IsPaused is false)
                return;

            IsPaused = false;
            TimeScaleService.Resume();
            GameResumed?.Invoke();
            _debugService.Log("Pause", "Game resumed");
        }

        #endregion
    }
}