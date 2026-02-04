using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using ACG.Tools.Runtime.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
    [CreateAssetMenu(fileName = "FloatingTextConfiguration", menuName = "ScriptableObjects/Configurations/FloatingText")]
    public class SO_FloatingTextConfiguration : SO_ConfigurationBase
    {
        #region Values

        [Header("General")]

        [SerializeField] private bool _isActive;
        public bool IsActive => _isActive;

        [SerializeField] private SO_PooledGameObjectData _prefabData;
        public SO_PooledGameObjectData PrefabData => _prefabData;


        [Header("Configuration")]

        [SerializeField] private Color _textColor;
        public Color TextColor => _textColor;

        [Space]

        [SerializeField] private float _offsetDistance = 0.4f;
        public float OffsetDistance => _offsetDistance;

        [Space]

        [SerializeField] private float _minMovementDistance = 0.5f;
        public float MinMovementDistance => _minMovementDistance;

        [SerializeField] private float _maxMovementDistance = 0.8f;
        public float MaxMovementDistance => _maxMovementDistance;

        [Space]

        [SerializeField] private float _lifeTime;
        public float LifeTime => _lifeTime;

        [SerializeField] private float _transitionDuration;
        public float TransitionDuration => _transitionDuration;

        #endregion
    }
}
