using System.Collections.Generic;
using System;
using ToolsACG.Networking;

namespace ToolsACG.Services.DreamloLeaderboard
{
    public class DreamloLeaderboardService: Service<DreamloLeaderboardService>
    {
        private readonly int _port = 0;

        private readonly string _setScoreURL = "{0}/add/{1}/{2}";
        public void SetScore(string pName, int pScore, Action<string> pCallback, Action<string> pCallbackError)
        {
            string endPoint = string.Format(_setScoreURL, NetworkManager.Instance.NetworkSetting.PrivateKey, pName, pScore);
            EnqueuSimpleRequest(endPoint, null, pCallback, _port, pCallbackError, HttpMethod.Get);
        }


        private readonly string _getScoresURL = "{0}/json";
        public void GetScores(Action<Response> pCallback, Action<Response> pCallbackError)
        {
            string endPoint = string.Format(_getScoresURL, NetworkManager.Instance.NetworkSetting.PublicKey);
            EnqueueJsonRequest(endPoint, null, pCallback, _port, pCallbackError, HttpMethod.Get);
        }


        private readonly string _getRangeScoresURL = "{0}/json/{1}";
        public void GetRangeScores(int pPositions, Action<Response> pCallback, Action<Response> pCallbackError)
        {
            string endPoint = string.Format(_getRangeScoresURL, NetworkManager.Instance.NetworkSetting.PublicKey, pPositions);
            EnqueueJsonRequest(endPoint, null, pCallback, _port, pCallbackError, HttpMethod.Get);
        }


        private readonly string _deleteLeaderboardURL = "{0}/clear";
        public void CleanLeaderboard(Action<string> pCallback, Action<string> pCallbackError)
        {
            string endPoint = string.Format(_deleteLeaderboardURL, NetworkManager.Instance.NetworkSetting.PrivateKey);
            EnqueuSimpleRequest(endPoint, null, pCallback, _port, pCallbackError, HttpMethod.Get);
        }



        public class Response : ResponseEmpty
        {
            public Dreamlo Dreamlo { get; set; }
        }

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
}

