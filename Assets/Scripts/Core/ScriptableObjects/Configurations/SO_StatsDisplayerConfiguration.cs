using ToolsACG.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
   [CreateAssetMenu(fileName = "StatsDisplayerConfiguration", menuName = "ScriptableObjects/Configurations/StatsDisplayer")]
   public class SO_StatsDisplayerConfiguration : SO_ConfigurationBase
   {
        #region Values

        [Header("References")]

        [SerializeField] private GameObject _statsRowPrefab;
        public GameObject StatsRowPrefab=> _statsRowPrefab;


        [Header("Configuration")]

        [SerializeField] private float _valueChangeTransitionDuration;
        public float ValueChangeTransitionDuration => _valueChangeTransitionDuration;


        [Header("Visuals Configuration")]

        [SerializeField] private float _lowStatThreshold;
        public float LowStatThreshold => _lowStatThreshold;
        
        [SerializeField] private Color _lowStatColor;
        public Color LowStatColor => _lowStatColor;

        [Space]

        [SerializeField] private float _midStatThreshold;
        public float MidStatThreshold => _midStatThreshold;

        [SerializeField] private Color _midStatColor;
        public Color MidStatColor => _midStatColor;

        [Space]

        [SerializeField] private Color _highStatColor;
        public Color HighStatColor => _highStatColor;

        #endregion
    }
}
