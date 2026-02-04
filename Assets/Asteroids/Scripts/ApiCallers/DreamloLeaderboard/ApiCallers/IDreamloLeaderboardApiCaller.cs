using System.Threading.Tasks;
using ToolsACG.ApiCallersCreator.Models;
using ToolsACG.ApiCallersCreator.Enums;
using ToolsACG.ApiCallersCreator.ScriptableObjects;

namespace Asteroids.ApiCallers.DreamloLeaderboardApiCaller
{
    public interface IDreamloLeaderboardApiCaller
    {
        Task<string> SetScore(string name, int score, SO_NetworkConfiguration networkConfiguration, RequestEmpty request, RequestMode mode = RequestMode.Direct);
        Task<Response> GetScoreRange(int range, SO_NetworkConfiguration networkConfiguration, RequestEmpty request, RequestMode mode = RequestMode.Direct);
    }
}
