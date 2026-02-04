using ACG.Tools.Runtime.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
   [CreateAssetMenu(fileName = "LeaderboardRowConfiguration", menuName = "ScriptableObjects/Configurations/LeaderboardRow")]
   public class SO_LeaderboardRowConfiguration : SO_ConfigurationBase
   {
        #region Values

        [Header("Visuals")]

        [SerializeField] private Color _playerRowColor;
        public Color PlayerRowColor => _playerRowColor;

        [SerializeField] private Color _defaultRowColor;
        public Color DefaultRowColor => _defaultRowColor;

        [Space]

        [SerializeField] private Sprite _firstPositionMedal;
        public Sprite FirstPositionMedal => _firstPositionMedal;

        [SerializeField] private Sprite _secondPositionMedal;
        public Sprite SecondPositionMedal => _secondPositionMedal;

        [SerializeField] private Sprite _thirdPositionMedal;
        public Sprite ThirdPositionMedal => _thirdPositionMedal;


        [Header("Configuration")]

        [SerializeField] private Vector3 _firstPositionScale;
        public Vector3 FirstPositionScale => _firstPositionScale;

        [SerializeField] private Vector3 _secondPositionScale;
        public Vector3 SecondPositionScale => _secondPositionScale;

        [SerializeField] private Vector3 _thirdPositionScale;
        public Vector3 ThirdPositionScale => _thirdPositionScale;

        [SerializeField] private Vector3 _defaultPositionScale;
        public Vector3 DefaultPositionScale => _defaultPositionScale;

        [Space]

        [SerializeField] private float _enterRowAnimationDuration;
        public float EnterRowAnimationDuration => _enterRowAnimationDuration;

        [SerializeField] private float _enterRowAnimationDelay;
        public float EnterRowAnimationDelay => _enterRowAnimationDelay;

        #endregion
    }
}
