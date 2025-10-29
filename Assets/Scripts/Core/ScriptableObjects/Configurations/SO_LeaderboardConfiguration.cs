using ToolsACG.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
    [CreateAssetMenu(fileName = "LeaderboardConfiguration", menuName = "ScriptableObjects/Configurations/Leaderboard")]
    public class SO_LeaderboardConfiguration : SO_ConfigurationBase
    {
        #region Values

        [Header("Configuration")]

        [SerializeField] private int _positionsAmount = 8;
        public int PositionsAmount => _positionsAmount;

        #endregion
    }
}
