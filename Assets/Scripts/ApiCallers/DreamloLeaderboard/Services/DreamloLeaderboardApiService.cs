using System.Threading.Tasks;
using ToolsACG.ApiCallersCreator.Models;
using ToolsACG.ApiCallersCreator.Enums;
using ToolsACG.ApiCallersCreator.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Asteroids.ApiCallers.DreamloLeaderboardApiCaller
{
    public class DreamloLeaderboardApiService : IDreamloLeaderboardApiService
    {
        #region Fields

        [Header("References")]
        private readonly IDreamloLeaderboardApiCaller _apiCaller;
        public DreamloLeaderboardApiContainer Data { get; }

        [Header("Data")]
        private readonly SO_NetworkSettings _networkSettings;

        #endregion

        #region Constructors

        [Inject]
        public DreamloLeaderboardApiService(IDreamloLeaderboardApiCaller apiCaller, DreamloLeaderboardApiContainer container, SO_NetworkSettings networkSettings)
        {
            _apiCaller = apiCaller;
            Data = container;
            _networkSettings = networkSettings;
        }

        #endregion

        #region Functionality

        public async Task<bool> SetScore(string name, int score, RequestMode mode = RequestMode.Direct)
        {
            SO_NetworkConfiguration networkConfiguration = _networkSettings.GetNetworkConfiguration(NetworkConfigurationType.Default);
            return await SetScore(name, score, networkConfiguration, mode);
        }
        public async Task<bool> SetScore(string name, int score, SO_NetworkConfiguration networkConfiguration, RequestMode mode = RequestMode.Direct)
        {
            string response = await _apiCaller.SetScore(name, score, networkConfiguration, new RequestEmpty(), mode);

            if (response == null || string.IsNullOrEmpty(response))
                return false;

            Data.SetScoreResponse = response;
            return true;
        }


        public async Task<bool> GetScoreRange(int range, RequestMode mode = RequestMode.Direct)
        {
            SO_NetworkConfiguration networkConfiguration = _networkSettings.GetNetworkConfiguration(NetworkConfigurationType.Default);
            return await GetScoreRange(range, networkConfiguration, mode);
        }
        public async Task<bool> GetScoreRange(int range, SO_NetworkConfiguration networkConfiguration, RequestMode mode = RequestMode.Direct)
        {
            Response response = await _apiCaller.GetScoreRange(range, networkConfiguration, new RequestEmpty(), mode);

            if (response == null)
                return false;

            Data.GetScoreRangeResponse = response;
            return true;
        }

        #endregion
    }
}
