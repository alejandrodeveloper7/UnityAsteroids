using System.Threading.Tasks;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;
using ACG.Tools.Runtime.ApiCallersCreator.Models;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;

namespace Asteroids.ApiCallers.DreamloLeaderboardApiCaller
{
    public interface IDreamloLeaderboardApiCaller
    {
        Task<string> SetScore(string name, int score, SO_NetworkConfiguration networkConfiguration, RequestEmpty request, RequestMode mode = RequestMode.Direct);
        Task<Response> GetScoreRange(int range, SO_NetworkConfiguration networkConfiguration, RequestEmpty request, RequestMode mode = RequestMode.Direct);
    }
}
