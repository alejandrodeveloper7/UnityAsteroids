using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.ScriptableObjects.Configurations;
using Asteroids.Gameplay.Player.Spawners;
using System.Threading.Tasks;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Services;
using ToolsACG.Core.Utils;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Controllers
{
    public class GameFlowController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [Inject] private readonly AsteroidsController _asteroidsController;
        [Inject] private readonly LeaderboardController _leaderboardController;
        [Inject] private readonly ScoreController _scoreController;
        [Space]
        [Inject] private readonly PlayerSpawner _playerSpawner;
        [Space]
        [Inject] private readonly ISettingsService _settingsService;

        [Header("Data")]
        [Inject] private readonly SO_StageConfiguration _stageConfiguration;
        [Inject] private readonly SO_ScenesConfiguration _scenesConfiguration;

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
            await AdditiveScenesService.LoadAdditiveScenes(_scenesConfiguration.DesktopSceneDependencies);
            _asteroidsController.CreateDecorationAsteroids();
            EventBusManager.GameFlowBus.RaiseEvent(new GameInitialized());
        }

        private void StartRun()
        {
            _asteroidsController.CleanCurrentAsteroids();
            _asteroidsController.RestartRound();
            _scoreController.RestartScore();
            _playerSpawner.SpawnSelectedPlayer();
            _ = _asteroidsController.CreateInitialAsteroids();
            EventBusManager.GameFlowBus.RaiseEvent(new RunStarted());
        }

        private async Task EndRun()
        {
            await TimingUtils.WaitSeconds(_stageConfiguration.DelayAfterPlayerDead);
            EventBusManager.GameFlowBus.RaiseEvent(new RunEnded());
            _scoreController.SaveLasScore();
            _ = _leaderboardController.ProcessLeaderboardFlowAfterRun();
        }

        #endregion
    }
}