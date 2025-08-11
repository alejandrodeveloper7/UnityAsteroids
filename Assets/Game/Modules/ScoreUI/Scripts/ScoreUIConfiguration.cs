using UnityEngine;

namespace ToolsACG.Scenes.ScoreUI
{ 
    [CreateAssetMenu(fileName = "ScoreUIConfiguration", menuName = "ScriptableObjects/MVCModuleCreator/ScoreUIConfiguration")]
    public class SO_ScoreUIConfiguration : ModuleConfiguration
    {
        public float DelayBeforeEnter;
        public float DelayBeforeExit; 
        public float FadeTransitionDuration;
    }
}
