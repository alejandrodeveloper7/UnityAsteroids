using System.Collections.Generic;
using System.Threading.Tasks;
using ToolsACG.Networking;

namespace ToolsACG.ApiCaller.DreamloLeaderboardApiCaller
{
    public class DreamloLeaderboardApiCaller : ApiCaller<DreamloLeaderboardApiCaller>
    {
        #region Stored Data

        public string SetScoreResponse;
        public Response RangeScoreResponse;

        #endregion

        #region Api calls

        private readonly int _port = 0;

        private readonly string _setScoreURL = "{0}/add/{1}/{2}";
        public async Task<bool> SetScore(string pName, int pScore)
        {
            string endPoint = string.Format(_setScoreURL, NetworkManager.Instance.NetworkSetting.PrivateKey, pName, pScore);
            string response = await SendSimpleRequest(endPoint, null, _port, HttpMethod.Get);

            if (response.Contains("OK") is false)
            {
                SetScoreResponse = "";
                return false;
            }

            SetScoreResponse = response;
            return true;
        }

        private readonly string _getRangeScoresURL = "{0}/json/{1}";
        public async Task<bool> GetRangeScores(int pPositions)
        {
            string endPoint = string.Format(_getRangeScoresURL, NetworkManager.Instance.NetworkSetting.PublicKey, pPositions);
            Response response = await EnqueueJsonRequest<Response>(endPoint, null, _port, HttpMethod.Get);

            if (response == null) 
            {
                RangeScoreResponse = null;
                return false;
            }

            RangeScoreResponse = response;
            return true;
        }

        #endregion

        #region Models

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

        #endregion
    }
}
