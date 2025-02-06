using UnityEngine;

namespace ToolsACG.Scenes.Leaderboard
{
    [CreateAssetMenu(fileName = "LeaderboardModel", menuName = "ScriptableObjects/ToolsACG/MVCModuleCreator/LeaderboardModel")]
    public class LeaderboardModel : ModuleModel
    {
        public float DelayBeforeEnter;
        public float FadeTransitionDuration;
        [Space]
        public int LeaderboardPositionsAmount;
        public Color LeaderboardPlayerColor;
    }
}