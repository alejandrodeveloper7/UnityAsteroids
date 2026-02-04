using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;
using System.Threading.Tasks;

namespace Asteroids.ApiCallers.DreamloLeaderboardApiCaller
{
    public interface IDreamloLeaderboardApiService 
    {
        DreamloLeaderboardApiContainer Data { get; }

        Task<bool> SetScore(string name, int score, RequestMode mode = RequestMode.Direct);
        Task<bool> SetScore(string name, int score, SO_NetworkConfiguration networkConfiguration, RequestMode mode = RequestMode.Direct);

        Task<bool> GetScoreRange(int range, RequestMode mode = RequestMode.Direct);
        Task<bool> GetScoreRange(int range, SO_NetworkConfiguration networkConfiguration, RequestMode mode = RequestMode.Direct);
    }
}
