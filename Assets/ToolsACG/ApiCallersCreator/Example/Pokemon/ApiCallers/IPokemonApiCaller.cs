using System.Threading.Tasks;
using ToolsACG.ApiCallersCreator.Enums;
using ToolsACG.ApiCallersCreator.Models;
using ToolsACG.ApiCallersCreator.ScriptableObjects;

namespace ToolsACG.ApiCallers.PokemonApiCaller
{
    public interface IPokemonApiCaller
    {
        Task<Pokemon> GetPikachuData(SO_NetworkConfiguration networkConfiguration, RequestEmpty request, RequestMode mode = RequestMode.Direct);
        Task<Pokemon> GetCharmanderData(SO_NetworkConfiguration networkConfiguration, RequestEmpty request, RequestMode mode = RequestMode.Direct);
    }
}
