using ACG.Core.Models;
using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using UnityEngine;

namespace Asteroids.MVC.PlayerHealthBarUI.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerHealthBarUIConfiguration", menuName = "ScriptableObjects/ToolsACG/MVCModules/PlayerHealthBarUIConfiguration")]
    public class SO_PlayerHealthBarUIConfiguration : SO_MVCConfigurationBase
    {
        #region Values

        [Header("Feedback")]

        [SerializeField] private Vector3 _panelFeedbackScale;
        public Vector3 PanelFeedbackScale => _panelFeedbackScale;

        [SerializeField] private float _panelFeedbackDuration;
        public float PanelFeedbackDuration => _panelFeedbackDuration;


        [Header("Health")]

        [SerializeField] private GameObject _healthPointPrefab;
        public GameObject HealthPointPrefab => _healthPointPrefab;

        [Space]

        [SerializeField] private Sprite _healthPointSprite;
        public Sprite HealthPointSprite => _healthPointSprite;

        [SerializeField] private Sprite _emptyHealtPointSprite;
        public Sprite EmptyHealtPointSprite => _emptyHealtPointSprite;


        [Header("Shield")]

        [SerializeField] private float _shieldLostSliderTransitionDuration;
        public float ShieldSliderTransitionDuration => _shieldLostSliderTransitionDuration;

        [Space]

        [SerializeField] private float _shieldShineFadeInDuration = 0.5f;
        public float ShieldShineFadeInDuration => _shieldShineFadeInDuration;

        [SerializeField] private float _shieldShineFadeOutDuration = 0.5f;
        public float ShieldShineFadeOutDuration => _shieldShineFadeOutDuration;

        [Space]

        [SerializeField] private FloatRange _shieldShineBlinkAlphaRange;
        public FloatRange ShieldShineBlinkAlphaRange => _shieldShineBlinkAlphaRange;

        [SerializeField] private float _shieldShineBlinkDuration = 0.3f;
        public float ShieldShineBlinkDuration => _shieldShineBlinkDuration;

        #endregion

        #region Methods

        // TODO: Declare your methods here

        #endregion
    }
}
