using System.Threading.Tasks;
using ToolsACG.ApiCallersCreator.Enums;
using ToolsACG.ApiCallersCreator.ScriptableObjects;

namespace ToolsACG.ApiCallers.PokemonApiCaller
{
    public interface IPokemonApiService
    {
        PokemonApiContainer Data { get; }

        Task<bool> UpdatePikachuData(RequestMode mode = RequestMode.Direct);
        Task<bool> UpdatePikachuData(SO_NetworkConfiguration networkConfiguration, RequestMode mode = RequestMode.Direct);

        Task<bool> UpdateCharmanderData(RequestMode mode = RequestMode.Direct);
        Task<bool> UpdateCharmanderData(SO_NetworkConfiguration networkConfiguration, RequestMode mode = RequestMode.Direct);
    }
}
