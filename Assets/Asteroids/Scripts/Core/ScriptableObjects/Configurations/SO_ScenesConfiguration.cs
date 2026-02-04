using ACG.Tools.Runtime.SOCreator.Configurations;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
    [CreateAssetMenu(fileName = "ScenesConfiguration", menuName = "ScriptableObjects/Configurations/Scenes")]
    public class SO_ScenesConfiguration : SO_ConfigurationBase
    {
        #region Values

        [Header("Scene Dependencys")]

        [SerializeField] private List<string> _desktopSceneDependencies;
        public List<string> DesktopSceneDependencies => _desktopSceneDependencies;

        #endregion
    }
}
