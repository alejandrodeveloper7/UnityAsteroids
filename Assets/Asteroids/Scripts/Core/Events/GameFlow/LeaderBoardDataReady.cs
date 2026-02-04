using Asteroids.ApiCallers.DreamloLeaderboardApiCaller;
using ToolsACG.Core.EventBus;

namespace Asteroids.Core.Events.GameFlow
{
    public sealed class LeaderBoardDataReady : IEvent
    {
        public bool Success { get; }
        public Leaderboard LeaderboardData { get; }

        public LeaderBoardDataReady(bool success, Leaderboard leaderboardData)
        {
            Success = success;
            LeaderboardData = leaderboardData;
        }
    }
}
