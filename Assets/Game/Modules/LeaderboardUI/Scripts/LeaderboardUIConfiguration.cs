using UnityEngine;

namespace ToolsACG.Scenes.LeaderboardUI
{ 
    [CreateAssetMenu(fileName = "LeaderboardUIConfiguration", menuName = "ScriptableObjects/MVCModuleCreator/LeaderboardUIConfiguration")]
    public class SO_LeaderboardUIConfiguration : ModuleConfiguration
    {
        public float DelayBeforeEnter;
        public float DelayBeforeExit;
        public float FadeTransitionDuration;
        [Space]
        public int LeaderboardPositionsAmount;
        public Color LeaderboardPlayerColor;
    }
}
