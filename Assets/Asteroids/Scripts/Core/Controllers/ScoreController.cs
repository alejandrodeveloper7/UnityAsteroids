using ACG.Core.EventBus;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.Services;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Controllers
{
    public class ScoreController
    {
        #region Fields

        [Header("References")]
        private readonly IContainerRuntimeDataService _runtimeDataService;

        [Header("Values")]
        private int _score;

        [Header("States")]
        private bool _playing;

        #endregion

        #region Constructors

        [Inject]
        public ScoreController(IContainerRuntimeDataService runtimeDataService)
        {
            _runtimeDataService = runtimeDataService;

            CreateListeners();
        }

        #endregion

        #region Initialization

        public void Disponse()
        {
            RemoveListeners();
        }

        public void CreateListeners()
        {
            EventBusManager.GameFlowBus.AddListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.AddListener<RunExitRequested>(OnRunExitRequested);

            EventBusManager.GameplayBus.AddListener<AsteroidDestroyed>(OnAsteroidDestroyed);
            EventBusManager.GameplayBus.AddListener<PlayerDied>(OnPlayerDied);
        }
        public void RemoveListeners()
        {
            EventBusManager.GameFlowBus.RemoveListener<RunStarted>(OnRunStarted);
            EventBusManager.GameFlowBus.RemoveListener<RunExitRequested>(OnRunExitRequested);

            EventBusManager.GameplayBus.RemoveListener<AsteroidDestroyed>(OnAsteroidDestroyed);
            EventBusManager.GameplayBus.RemoveListener<PlayerDied>(OnPlayerDied);
        }

        #endregion 

        #region Event Callbacks

        private void OnRunStarted(RunStarted runStarted)
        {
            _playing = true;
        }

        private void OnRunExitRequested(RunExitRequested runExitRequested)
        {
            _playing = false;
        }

        private void OnAsteroidDestroyed(AsteroidDestroyed asteroidDestroyed)
        {
            if (_playing is false)
                return;

            _score += asteroidDestroyed.AsteroidData.ScoreValue;
            EventBusManager.GameplayBus.RaiseEvent(new ScoreUpdated(_score));
        }

        private void OnPlayerDied(PlayerDied playerDied)
        {
            _playing = false;
        }

        #endregion

        #region Public methods

        public void RestartScore()
        {
            _score = 0;
            EventBusManager.GameplayBus.RaiseEvent(new ScoreUpdated(_score));
        }

        public void SaveLasScore()
        {
            _runtimeDataService.Data.LastScore = _score;
        }

        #endregion
    }
}