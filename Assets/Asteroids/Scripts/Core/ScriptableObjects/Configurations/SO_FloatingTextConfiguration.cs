using ACG.Core.Models;
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

        [SerializeField] private bool _useFloatingText = true;
        public bool UseFloatingText => _useFloatingText;

        [SerializeField] private SO_PooledGameObjectData _prefabData;
        public SO_PooledGameObjectData PrefabData => _prefabData;


        [Header("Configuration")]

        [SerializeField] private Color _textColor;
        public Color TextColor => _textColor;

        [Space]

        [SerializeField] private FloatRange _movementDistanceRange = new(0.5f, 0.8f);
        public FloatRange MovementDistanceRange => _movementDistanceRange;


        [Header("Animation")]

        [SerializeField] private float _enterTransitionDuration;
        public float EnterTransitionDuration => _enterTransitionDuration;

        [SerializeField] private float _exitTransitionDuration;
        public float ExitTransitionDuration => _exitTransitionDuration;

        [SerializeField] private float _duration;
        public float Duration => _duration;

        public float TotalLifeTime => _enterTransitionDuration + _exitTransitionDuration + _duration;

        #endregion
    }
}
