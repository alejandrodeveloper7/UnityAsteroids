using ACG.Tools.Runtime.ApiCallersCreator.Core;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.Models;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;
using System.Threading.Tasks;
using UnityEngine;

namespace Asteroids.ApiCallers.DreamloLeaderboardApiCaller
{
    public class DreamloLeaderboardApiCaller : ApiCallerBase<DreamloLeaderboardApiCaller> , IDreamloLeaderboardApiCaller
    {         
        #region Fields

        [Header("Values")]
        private readonly int _localHostPort = 0;

        #endregion

        #region Calls

        private readonly string _setScoreURL = "{0}/add/{1}/{2}";
        public async Task<string> SetScore(string name, int score, SO_NetworkConfiguration configuration, RequestEmpty request, RequestMode mode = RequestMode.Direct)
        {
            string endPoint = string.Format(_setScoreURL, configuration.PrivateKey, name, score);
            string url = BuildUrl(configuration, endPoint, _localHostPort);

            switch (mode)
            {
                case RequestMode.Direct:
                    return await SendSimpleRequest(configuration, url, request, HttpMethod.Get);

                case RequestMode.Queued:
                    return await EnqueuSimpleRequest(configuration, url, request, HttpMethod.Get);

                case RequestMode.GlobalQueue:
                    return await ApiCallerGlobalQueue.Instance.AddSimpleRequest(configuration, url, request, HttpMethod.Get);

                default:
                    Debug.LogError($"Request mode {mode} not controlled");
                    return null;
            }
        }


        private readonly string _getRangeScoresURL = "{0}/json/{1}";
        public async Task<Response> GetScoreRange(int range, SO_NetworkConfiguration configuration, RequestEmpty request, RequestMode mode = RequestMode.Direct)
        {
            string endPoint = string.Format(_getRangeScoresURL, configuration.PublicKey, range);
            string url = BuildUrl(configuration, endPoint, _localHostPort);

            switch (mode)
            {
                case RequestMode.Direct:
                    return await SendJsonRequest<Response>(configuration, url, request, HttpMethod.Get);

                case RequestMode.Queued:
                    return await EnqueueJsonRequest<Response>(configuration, url, request, HttpMethod.Get);

                case RequestMode.GlobalQueue:
                    return await ApiCallerGlobalQueue.Instance.AddJsonRequest<Response>(configuration, url, request, HttpMethod.Get);

                default:
                    Debug.LogError($"Request mode {mode} not controlled");
                    return null;
            }
        }

        #endregion
    }
}
