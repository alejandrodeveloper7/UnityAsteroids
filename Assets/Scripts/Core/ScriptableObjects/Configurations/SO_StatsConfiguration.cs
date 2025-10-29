using Asteroids.Core.Interfaces.Models;
using System.Collections.Generic;
using ToolsACG.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
    [CreateAssetMenu(fileName = "StatsConfiguration", menuName = "ScriptableObjects/Configurations/Stats")]
    public class SO_StatsConfiguration : SO_ConfigurationBase
    {
        #region Values

        [Header("Configuration")]
        [SerializeField] private List<StatConfiguration> _statsConfiguration;
        public List<StatConfiguration> StatsConfiguration => _statsConfiguration;

        #endregion
    }
}
