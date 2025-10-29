using System.Threading.Tasks;
using ToolsACG.ApiCallersCreator.Core;
using ToolsACG.ApiCallersCreator.Enums;
using ToolsACG.ApiCallersCreator.Models;
using ToolsACG.ApiCallersCreator.ScriptableObjects;
using UnityEngine;

namespace ToolsACG.ApiCallers.PokemonApiCaller
{
    public class PokemonApiCaller : ApiCallerBase<PokemonApiCaller>, IPokemonApiCaller
    {
        #region Fields

        [Header("Values")]
        private readonly int _localHostPort = 0;

        #endregion

        #region Pikachu Data

        private readonly string _pikachuDataURL = "pokemon/Pikachu";
        public async Task<Pokemon> GetPikachuData(SO_NetworkConfiguration configuration, RequestEmpty request, RequestMode mode = RequestMode.Direct)
        {
            string url = BuildUrl(configuration, _pikachuDataURL, _localHostPort);

            switch (mode)
            {
                case RequestMode.Direct:
                    return await SendJsonRequest<Pokemon>(configuration, url, request, HttpMethod.Get);

                case RequestMode.Queued:
                    return await EnqueueJsonRequest<Pokemon>(configuration, url, request, HttpMethod.Get);

                case RequestMode.GlobalQueue:
                    return await ApiCallerGlobalQueue.Instance.AddJsonRequest<Pokemon>(configuration, url, request, HttpMethod.Get);

                default:
                    Debug.LogError($"Request mode {mode} not controlled");
                    return null;
            }
        }

        private readonly string _charmanderDataURL = "pokemon/Charmander";
        public async Task<Pokemon> GetCharmanderData(SO_NetworkConfiguration configuration, RequestEmpty request, RequestMode mode = RequestMode.Direct)
        {
            string url = BuildUrl(configuration, _charmanderDataURL, _localHostPort);

            switch (mode)
            {
                case RequestMode.Direct:
                    return await SendJsonRequest<Pokemon>(configuration, url, request, HttpMethod.Get);

                case RequestMode.Queued:
                    return await EnqueueJsonRequest<Pokemon>(configuration, url, request, HttpMethod.Get);

                case RequestMode.GlobalQueue:
                    return await ApiCallerGlobalQueue.Instance.AddJsonRequest<Pokemon>(configuration, url, request, HttpMethod.Get);

                default:
                    Debug.LogError($"Request mode {mode} not controlled");
                    return null;
            }
        }

        #endregion
    }
}
