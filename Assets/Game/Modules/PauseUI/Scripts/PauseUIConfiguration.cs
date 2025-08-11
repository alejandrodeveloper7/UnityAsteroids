using UnityEngine;

namespace ToolsACG.Scenes.PauseUI
{ 
    [CreateAssetMenu(fileName = "PauseUIConfiguration", menuName = "ScriptableObjects/MVCModuleCreator/PauseUIConfiguration")]
    public class SO_PauseUIConfiguration : ModuleConfiguration
    {
        public float DelayBeforeEnter;
        public float DelayBeforeExit;
        public float FadeTransitionDuration;
    }
}
