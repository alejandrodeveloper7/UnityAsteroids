using UnityEngine;

namespace ToolsACG.Scenes.MainMenu
{
    [CreateAssetMenu(fileName = "MainMenuModel", menuName = "ScriptableObjects/ToolsACG/MVCModuleCreator/MainMenuModel")]
    public class MainMenuModel : ModuleModel
    {
        public float DelayBeforeEnter;
        public float FadeTransitionDuration;
    }
}