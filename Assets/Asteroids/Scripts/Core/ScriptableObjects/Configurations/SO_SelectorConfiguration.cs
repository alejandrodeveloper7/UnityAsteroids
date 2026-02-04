using ACG.Tools.Runtime.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
   [CreateAssetMenu(fileName = "SelectorConfiguration", menuName = "ScriptableObjects/Configurations/Selector")]
   public class SO_SelectorConfiguration : SO_ConfigurationBase
   {
        #region Values

        [Header("Configuration")]

        [SerializeField] private float _displayedItemChangeAnimationTotalDuration;
        public float DisplayedItemChangeAnimationTotalDuration=> _displayedItemChangeAnimationTotalDuration;

        [SerializeField] private Vector3 _displayedItemAnimationRotationAngle;
        public Vector3 DisplayedItemAnimationRotationAngle => _displayedItemAnimationRotationAngle;

        #endregion
    }
}
