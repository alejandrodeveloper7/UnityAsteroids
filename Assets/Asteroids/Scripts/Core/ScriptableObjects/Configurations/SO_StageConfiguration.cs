using ACG.Tools.Runtime.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
    [CreateAssetMenu(fileName = "StageConfiguration", menuName = "ScriptableObjects/Configurations/Stage")]
    public class SO_StageConfiguration : SO_ConfigurationBase
    {
        #region Values

        [Header("Asteroids Amount")]

        [SerializeField] private int _decorationAsteroidsAmount = 12;
        public int DecorationAsteroidsAmount => _decorationAsteroidsAmount;

        [Space]

        [SerializeField] private int _initialAsteroidsAmount = 4;
        public int InitialAsteroidsAmount => _initialAsteroidsAmount;

        [SerializeField] private int _asteroidsIncrementPerRound;
        public int AsteroidsIncrementPerRound => _asteroidsIncrementPerRound;

        [SerializeField] private int _maxPosibleAsteroids;
        public int MaxPosibleAsteroids => _maxPosibleAsteroids;


        [Header("Times")]

        [SerializeField] private float _delayBeforeFirstRound;
        public float DelayBeforeFirstRound => _delayBeforeFirstRound;

        [SerializeField] private float _delayBetweenRounds;
        public float DelayBetweenRounds => _delayBetweenRounds;

        [Space]

        [SerializeField] private float _delayAfterPlayerDead;
        public float DelayAfterPlayerDead => _delayAfterPlayerDead;

        #endregion
    }
}
