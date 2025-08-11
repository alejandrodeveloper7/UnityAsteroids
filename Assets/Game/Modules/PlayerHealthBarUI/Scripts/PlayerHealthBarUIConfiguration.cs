using UnityEngine;

namespace ToolsACG.Scenes.PlayerHealthBarUI
{ 
    [CreateAssetMenu(fileName = "PlayerHealthBarUIConfiguration", menuName = "ScriptableObjects/MVCModuleCreator/PlayerHealthBarUIConfiguration")]
    public class SO_PlayerHealthBarUIConfiguration : ModuleConfiguration
    {
        [Header("Times")]
        public float DelayBeforeEnter;
        public float DelayBeforeExit; 
        public float FadeTransitionDuration;

        [Header("Health")]
        public Sprite HealthPointSprite;
        public Sprite emptyHealtPointSprite;

        [Header("Shield")]
        public Sprite ShieldBarSprite;
        public Sprite FullShieldBarSprite;
        [Space]
        public float ShielSliderMaxValue=100;
        public float ShieldLostSliderMinValue;
        public float ShieldSliderTransitionDuration;
    }
}
