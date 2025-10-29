using System.Threading.Tasks;
using ToolsACG.ApiCallersCreator.Enums;
using ToolsACG.ApiCallersCreator.Models;
using ToolsACG.ApiCallersCreator.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace ToolsACG.ApiCallers.PokemonApiCaller
{
    public class PokemonApiService : IPokemonApiService
    {
        #region Fields

        [Header("References")]
        private readonly IPokemonApiCaller _exampleApiCaller;
        public PokemonApiContainer Data { get; }

        [Header("Data")]
        private readonly SO_NetworkSettings _networkSettings;

        #endregion

        #region Constructors

        [Inject]
        public PokemonApiService(IPokemonApiCaller apiCaller, PokemonApiContainer container, SO_NetworkSettings networkSettings)
        {
            _exampleApiCaller = apiCaller;
            Data=container;
            _networkSettings = networkSettings;
        }

        #endregion

        #region Functionality

        public async Task<bool> UpdatePikachuData(RequestMode mode = RequestMode.Direct)
        {
            SO_NetworkConfiguration networkConfiguration = _networkSettings.GetNetworkConfiguration(NetworkConfigurationType.Default);
            return await UpdatePikachuData(networkConfiguration, mode);
        }
        public async Task<bool> UpdatePikachuData(SO_NetworkConfiguration networkConfiguration, RequestMode mode = RequestMode.Direct)
        {
            Pokemon response = await _exampleApiCaller.GetPikachuData(networkConfiguration, new RequestEmpty(), mode);

            if (response == null)
                return false;

            Data.PikachuData = response;
            return true;
        }


        public async Task<bool> UpdateCharmanderData(RequestMode mode = RequestMode.Direct)
        {
            SO_NetworkConfiguration networkConfiguration = _networkSettings.GetNetworkConfiguration(NetworkConfigurationType.Default);
            return await UpdateCharmanderData(networkConfiguration, mode);
        }
        public async Task<bool> UpdateCharmanderData(SO_NetworkConfiguration networkConfiguration, RequestMode mode = RequestMode.Direct)
        {
            Pokemon response = await _exampleApiCaller.GetCharmanderData(networkConfiguration, new RequestEmpty(), mode);

            if (response == null)
                return false;

            Data.CharmanderData = response;
            return true;
        }

        #endregion
    }
}
