using System.Threading.Tasks;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.Models;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;
using UnityEngine;

namespace ACG.Tools.Runtime.ApiCallers.Pokemon
{
    public class PokemonApiService : IPokemonApiService
    {
        #region Fields

        [Header("Singleton")]
        private static PokemonApiService _instance;
        public static PokemonApiService Instance => _instance ??= new PokemonApiService();

        [Header("References")]
        private readonly IPokemonApiCaller _exampleApiCaller;
        public PokemonApiContainer Data { get; }

        #endregion

        #region Constructors

        private PokemonApiService()
        {
            _exampleApiCaller = new PokemonApiCaller();
            Data = new PokemonApiContainer();
        }

        #endregion

        #region Functionality

        public async Task<bool> UpdatePikachuData(RequestMode mode = RequestMode.Direct)
        {
            SO_NetworkConfiguration networkConfiguration = SO_NetworkSettings.Instance.GetNetworkConfiguration(NetworkConfigurationType.Default);
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
            SO_NetworkConfiguration networkConfiguration = SO_NetworkSettings.Instance.GetNetworkConfiguration(NetworkConfigurationType.Default);
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
