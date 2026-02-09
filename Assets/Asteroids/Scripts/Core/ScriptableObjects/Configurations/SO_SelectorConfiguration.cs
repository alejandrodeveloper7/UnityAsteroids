using ACG.Tools.Runtime.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
   [CreateAssetMenu(fileName = "SelectorConfiguration", menuName = "ScriptableObjects/Configurations/Selector")]
   public class SO_SelectorConfiguration : SO_ConfigurationBase
   {
        #region Values

        [Header("Configuration")]

        [SerializeField] private float _displayChangeDuration;
        public float DisplayChangeDuration=> _displayChangeDuration;

        [SerializeField] private Vector3 _displayChangeRotationAngle;
        public Vector3 DisplayChangeRotationAngle => _displayChangeRotationAngle;

        #endregion
    }
}
