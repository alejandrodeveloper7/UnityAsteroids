using System.Collections.Generic;

namespace Asteroids.ApiCallers.DreamloLeaderboardApiCaller
{
    public class Dreamlo
    {
        public Leaderboard Leaderboard { get; set; }
    }

    public class Leaderboard
    {
        public List<LeaderboardEntry> Entry { get; set; }
    }

    public class LeaderboardEntry
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Seconds { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
    }
}
