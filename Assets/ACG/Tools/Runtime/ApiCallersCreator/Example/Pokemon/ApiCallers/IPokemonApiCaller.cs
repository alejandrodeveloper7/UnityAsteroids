using System.Threading.Tasks;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.Models;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;

namespace ACG.Tools.Runtime.ApiCallers.Pokemon
{
    public interface IPokemonApiCaller
    {
        Task<Pokemon> GetPikachuData(SO_NetworkConfiguration networkConfiguration, RequestEmpty request, RequestMode mode = RequestMode.Direct);
        Task<Pokemon> GetCharmanderData(SO_NetworkConfiguration networkConfiguration, RequestEmpty request, RequestMode mode = RequestMode.Direct);
    }
}
