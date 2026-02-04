using System.Threading.Tasks;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;

namespace ACG.Tools.Runtime.ApiCallers.Pokemon
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
