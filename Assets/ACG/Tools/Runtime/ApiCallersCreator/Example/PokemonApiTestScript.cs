using System.Threading.Tasks;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallers.Pokemon;
using UnityEngine;

namespace ACG.Tools.Runtime.ApiCallers.Test
{
    public class PokemonApiTestScript : MonoBehaviour
    {
        #region Fields

        [Header("Configuration")]
        [SerializeField] private bool _testService;
        [SerializeField] private bool _testServiceEnqueue;
        [SerializeField] private bool _testServiceGeneralEnqueue;

        [Header("References")]
        private IPokemonApiService _pokemonService;

        #endregion

        #region Initialization

        private void TestPokeApiService()
        {
            _pokemonService = PokemonApiService.Instance;

            if (_testService)
                _ = TestService();
            else if (_testServiceEnqueue)
                TestServiceEnqueue();
            else if (_testServiceGeneralEnqueue)
                TestServiceGeneralEnqueue();
        }

        #endregion

        #region Monobehaviour

        private void Start()
        {
            TestPokeApiService();
        }

        #endregion

        #region Funcionality

        private async Task TestService()
        {
            //This call will print true in console, call the Api and after receive the response, if the call works, will print false in console

            Debug.Log($"PikachuData is null: {_pokemonService.Data.PikachuData is null}");
            await _pokemonService.UpdatePikachuData();
            Debug.Log($"PikachuData is null: {_pokemonService.Data.PikachuData is null}");
        }

        private void TestServiceEnqueue()
        {
            //This will call to 5 Apis in order, without overlap any call

            _ = _pokemonService.UpdatePikachuData(RequestMode.Queued);
            _ = _pokemonService.UpdateCharmanderData(RequestMode.Queued);
            _ = _pokemonService.UpdatePikachuData(RequestMode.Queued);
            _ = _pokemonService.UpdateCharmanderData(RequestMode.Queued);
            _ = _pokemonService.UpdatePikachuData(RequestMode.Queued);
        }

        private void TestServiceGeneralEnqueue()
        {
            //This will call to 5 Apis in order, without overlap any call, and could be use with apis in diferent ApiCallers

            _ = _pokemonService.UpdatePikachuData(RequestMode.GlobalQueue);
            _ = _pokemonService.UpdateCharmanderData(RequestMode.GlobalQueue);
            _ = _pokemonService.UpdatePikachuData(RequestMode.GlobalQueue);
            _ = _pokemonService.UpdateCharmanderData(RequestMode.GlobalQueue);
            _ = _pokemonService.UpdatePikachuData(RequestMode.GlobalQueue);
        }

        #endregion
    }
}