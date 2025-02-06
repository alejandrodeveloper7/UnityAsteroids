using UnityEngine;

namespace ToolsACG.Scenes.PlayerHealth
{
    [CreateAssetMenu(fileName = "PlayerHealthBarModel", menuName = "ScriptableObjects/ToolsACG/MVCModuleCreator/PlayerHealthBarModel")]
    public class PlayerHealthBarModel : ModuleModel
    {
        public float FadeTransitionDuration;

        [Header("Health")]
        public Sprite HealthPointSprite;
        public Sprite emptyHealtPointSprite;

        [Header("Shield")]
        public Sprite ShieldBarSprite;
        public Sprite FullShieldBarSprite;
        [Space]
        public float ShieldLostSliderMinValue;
        public float ShieldSliderTransitionDuration;
    }
}