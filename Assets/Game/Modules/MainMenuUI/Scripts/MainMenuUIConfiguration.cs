using UnityEngine;

namespace ToolsACG.Scenes.MainMenuUI
{ 
    [CreateAssetMenu(fileName = "MainMenuUIConfiguration", menuName = "ScriptableObjects/MVCModuleCreator/MainMenuUIConfiguration")]
    public class SO_MainMenuUIConfiguration : ModuleConfiguration
    {
        public float DelayBeforeEnter;
        public float DelayBeforeExit;
        public float FadeTransitionDuration;
    }
}
