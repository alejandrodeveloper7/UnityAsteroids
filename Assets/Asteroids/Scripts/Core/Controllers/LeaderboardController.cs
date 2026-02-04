using ACG.Core.EventBus;
using Asteroids.ApiCallers.DreamloLeaderboardApiCaller;
using Asteroids.Core.Events.GameFlow;
using Asteroids.Core.ScriptableObjects.Configurations;
using Asteroids.Core.Services;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Controllers
{
    public class LeaderboardController
    {
        #region Fields

        [Header("References")]
        private readonly IDreamloLeaderboardApiService _leaderboardService;
        private readonly IContainerRuntimeDataService _runtimeDataService;

        [Header("Data")]
        private readonly SO_LeaderboardConfiguration _leaderboardConfiguration;

        #endregion

        #region Constructors

        [Inject]
        public LeaderboardController(IDreamloLeaderboardApiService service, IContainerRuntimeDataService runtimeDataService, SO_LeaderboardConfiguration leaderboardConfiguration)
        {
            _leaderboardService = service;
            _runtimeDataService = runtimeDataService;
            _leaderboardConfiguration = leaderboardConfiguration;
        }

        #endregion

        #region Flows Management

        public async Task ProcessLeaderboardFlowFromMainMenu()
        {
            bool getScoresSuccess = await GetLeaderboardScoreRange();

            if (getScoresSuccess)
                RaiseLeaderboardDataEvent(true);
            else
                RaiseLeaderboardDataEvent(false);
        }

        public async Task ProcessLeaderboardFlowAfterRun()
        {
            bool setScoreSucess = await SetScoreToLeaderboard();

            if (setScoreSucess is false)
            {
                RaiseLeaderboardDataEvent(false);
                return;
            }

            bool getScoresSuccess = await GetLeaderboardScoreRange();

            if (getScoresSuccess is false)
            {
                RaiseLeaderboardDataEvent(false);
                return;
            }

            RaiseLeaderboardDataEvent(true);
        }

        #endregion

        #region Leaderboard Management

        private async Task<bool> SetScoreToLeaderboard()
        {
            return await _leaderboardService.SetScore(_runtimeDataService.Data.UserName, _runtimeDataService.Data.LastScore);
        }

        private async Task<bool> GetLeaderboardScoreRange()
        {
            return await _leaderboardService.GetScoreRange(_leaderboardConfiguration.PositionsAmount);
        }

        private void RaiseLeaderboardDataEvent(bool success)
        {
            if (success)
                EventBusManager.GameFlowBus.RaiseEvent(new LeaderBoardDataReady(true, _leaderboardService.Data.GetScoreRangeResponse.Dreamlo.Leaderboard));
            else
                EventBusManager.GameFlowBus.RaiseEvent(new LeaderBoardDataReady(false, null));
        }

        #endregion
    }
}