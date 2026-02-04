using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.Handlers;
using Asteroids.Core.ScriptableObjects.Configurations;
using System.Threading.Tasks;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Managers;
using ToolsACG.Core.Services;
using ToolsACG.Core.Utils;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Controllers
{
    public class GameFlowController : MonoBehaviour
    {
        #region Fields

        [Header("Controllers")]
        [Inject] private readonly ScenesController _scenesController;
        [Inject] private readonly PlayersController _playersController;
        [Inject] private readonly AsteroidsController _asteroidsController;
        [Inject] private readonly ScoreController _scoreController;
        [Inject] private readonly LeaderboardController _leaderboardController;

        [Header("Managers")]
        [Inject] private readonly ICursorManager _cursorManager;

        [Header("Services")]
        [Inject] private readonly ISettingsService _settingsService;

        [Header("Handlers")]
        [Inject] private readonly GameFlowEventsHandler _gameFlowEventsHandler;

        [Header("Data")]
        [Inject] private readonly SO_StageConfiguration _stageConfiguration;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            _ = StartGame();
        }

        private void OnEnable()
        {
            EventBusManager.GameFlowBus.AddListener<StartRunRequested>(OnStartRunRequested);
            EventBusManager.GameFlowBus.AddListener<GoToLeaderboardRequested>(OnGoToLeaderboardRequested);

            EventBusManager.GameplayBus.AddListener<PlayerDied>(OnPlayerDied);
        }

        private void OnDisable()
        {
            EventBusManager.GameFlowBus.RemoveListener<StartRunRequested>(OnStartRunRequested);
            EventBusManager.GameFlowBus.RemoveListener<GoToLeaderboardRequested>(OnGoToLeaderboardRequested);

            EventBusManager.GameplayBus.RemoveListener<PlayerDied>(OnPlayerDied);
        }

        #endregion

        #region Event callbacks

        private void OnStartRunRequested(StartRunRequested startRunRequested)
        {
            StartRun();
        }

        private void OnGoToLeaderboardRequested(GoToLeaderboardRequested goToLeaderboardRequested)
        {
            _ = _leaderboardController.ProcessLeaderboardFlowFromMainMenu();
        }

        private void OnPlayerDied(PlayerDied playerDied)
        {
            _ = EndRun();
        }

        #endregion

        #region Game Flow

        private async Task StartGame()
        {
            _settingsService.ApplyAllStoredSettings();
            await _scenesController.LoadGameplayAdditiveScenes();
            _cursorManager.SetUICursor();
            _asteroidsController.CreateDecorationAsteroids();
            _gameFlowEventsHandler.GameInitialized();
        }

        private void StartRun()
        {
            _cursorManager.SetGameplayCursor();
            _asteroidsController.CleanCurrentAsteroids();
            _asteroidsController.RestartRound();
            _scoreController.RestartScore();
            _playersController.CreatePlayer();
            _ = _asteroidsController.CreateInitialAsteroids();
            _gameFlowEventsHandler.RunStarted();
        }

        private async Task EndRun()
        {
            await TimingUtils.WaitSeconds(_stageConfiguration.DelayAfterPlayerDead);
            _gameFlowEventsHandler.RunEnded();
            _cursorManager.SetUICursor();
            _scoreController.SaveLasScore();
            _ = _leaderboardController.ProcessLeaderboardFlowAfterRun();
        }

        #endregion
    }
}