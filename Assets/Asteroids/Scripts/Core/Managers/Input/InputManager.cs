using ACG.Core.EventBus;
using ACG.Scripts.ScriptableObjects.Settings;
using ACG.Tools.Runtime.ManagersCreator.Bases;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Events.Gameplay;
using System;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Managers
{
    public class InputManager : MonobehaviourInstancesManagerBase<InputManager>, IInputManager
    {
        #region Events and Fields

        public event Action<int> RotationtKeysStateChanged;
        public event Action<bool> ShootKeyStateChanged;
        public event Action<bool> MoveForwardKeyStateChanged;

        [Header("References")]
        [Inject] private readonly IPauseManager _pauseManager;

        [Header("State")]
        private bool _inRun;
        private bool _inPause;

        [Header("Data")]
        [Inject] private readonly SO_InputSettings _inputSettings;

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

            if (_inputSettings == null)
            {
                Debug.LogWarning($"{typeof(SO_InputSettings).Name} es null, input manager disabled");
                enabled = false;
            }
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

        private void OnEnable()
        {
            _pauseManager.GamePaused += OnGamePaused;
            _pauseManager.GameResumed += OnGameResumed;

            EventBusManager.GameFlowBus.AddListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);

            EventBusManager.GameplayBus.AddListener<PlayerDied>(OnPlayerDied);
        }

        private void OnDisable()
        {
            if (_pauseManager != null)
            {
                _pauseManager.GamePaused -= OnGamePaused;
                _pauseManager.GameResumed -= OnGameResumed;
            }

            EventBusManager.GameFlowBus.RemoveListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);

            EventBusManager.GameplayBus.RemoveListener<PlayerDied>(OnPlayerDied);
        }

        private void Update()
        {
            if (_inRun is false)
                return;

            CheckPauseInput();

            if (_inPause)
                return;

            CheckRotationInput();
            CheckMovementInput();
            CheckShootInput();
        }

        #endregion

        #region Event Callbacks

        private void OnRunStarted(RunStarted runStarted)
        {
            _inRun = true;
        }

        private void OnGamePaused()
        {
            _inPause = true;
            RestartState();
        }

        private void OnGameResumed()
        {
            _inPause = false;
            RestartStatesAfterPause();
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _inRun = false;
        }

        private void OnPlayerDied(PlayerDied playerDied)
        {
            _inRun = false;
        }

        #endregion

        #region Input checks

        private void CheckRotationInput()
        {
            if (Input.GetKeyDown(_inputSettings.TurnLeftKey))
                RotationtKeysStateChanged?.Invoke(1);

            if (Input.GetKeyDown(_inputSettings.TurnRightKey))
                RotationtKeysStateChanged?.Invoke(-1);

            if (Input.GetKeyUp(_inputSettings.TurnLeftKey))
            {
                if (Input.GetKey(_inputSettings.TurnRightKey))
                    RotationtKeysStateChanged?.Invoke(-1);
                else
                    RotationtKeysStateChanged?.Invoke(0);
            }

            if (Input.GetKeyUp(_inputSettings.TurnRightKey))
            {
                if (Input.GetKey(_inputSettings.TurnLeftKey))
                    RotationtKeysStateChanged?.Invoke(1);
                else
                    RotationtKeysStateChanged?.Invoke(0);
            }
        }

        private void CheckMovementInput()
        {
            if (Input.GetKeyDown(_inputSettings.MoveForwardKey))
                MoveForwardKeyStateChanged?.Invoke(true);

            if (Input.GetKeyUp(_inputSettings.MoveForwardKey))
                MoveForwardKeyStateChanged?.Invoke(false);
        }

        private void CheckShootInput()
        {
            if (Input.GetKeyDown(_inputSettings.ShootKey))
                ShootKeyStateChanged?.Invoke(true);

            if (Input.GetKeyUp(_inputSettings.ShootKey))
                ShootKeyStateChanged?.Invoke(false);
        }

        private void CheckPauseInput()
        {
            if (Input.GetKeyDown(_inputSettings.PauseKey))
                _pauseManager.TogglePause();
        }

        #endregion

        #region Functionality

        private void RestartStatesAfterPause()
        {
            if (Input.GetKey(_inputSettings.TurnLeftKey))
                RotationtKeysStateChanged?.Invoke(1);

            if (Input.GetKey(_inputSettings.TurnRightKey))
                RotationtKeysStateChanged?.Invoke(-1);

            if (Input.GetKey(_inputSettings.MoveForwardKey))
                MoveForwardKeyStateChanged?.Invoke(true);

            if (Input.GetKey(_inputSettings.ShootKey))
                ShootKeyStateChanged?.Invoke(true);
        }

        private void RestartState()
        {
            RotationtKeysStateChanged?.Invoke(0);
            ShootKeyStateChanged?.Invoke(false);
            MoveForwardKeyStateChanged?.Invoke(false);
        }

        #endregion
    }
}
