using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using System.Collections.Generic;
using ToolsACG.Core.Models;
using ToolsACG.Core.ScriptableObjects.Data;
using ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs;
using ToolsACG.Core.Utilitys;
using ToolsACG.Pooling.ScriptableObjects;
using ToolsACG.SOCreator.Data;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "NewShip", menuName = "ScriptableObjects/Data/Ship")]
    public class SO_ShipData : SO_DataBase, ISelectorElement, IHasStats
    {
        #region Values

        [Header("Selector Element")]

        [SerializeField] private bool _isActive = true;
        public bool IsActive => _isActive;

        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;


        [Header("Has Stats")]

        [SerializeField] private List<StatData> _stats;
        public List<StatData> Stats => _stats;


        [Header("General")]

        [SerializeField] private SO_PooledGameObjectData _prefabData;
        public SO_PooledGameObjectData PrefabData => _prefabData;

        [SerializeField] private float _timeBeforeRecicle = 0.4f;
        public float TimeBeforeRecicle => _timeBeforeRecicle;


        [Header("Stats")]

        [Range(1, 5)][SerializeField] private int _healthPoints;
        public int HealthPoints => _healthPoints;

        [SerializeField] private float _movementSpeed;
        public float MovementSpeed => _movementSpeed;

        [SerializeField] private float _rotationSpeed;
        public float RotationSpeed => _rotationSpeed;

        [SerializeField] private float _shieldRecoveryTime;
        public float ShieldRecoveryTime => _shieldRecoveryTime;


        [Header("Configuration")]

        [SerializeField] private Color _color;
        public Color Color => _color;

        [Space]

        [SerializeField] private float _propulsionFireTransitionDuration;
        public float PropulsionFireTransitionDuration => _propulsionFireTransitionDuration;

        [SerializeField] private Vector3 _propulsionFireLocalPosition;
        public Vector3 PropulsionFireLocalPosition => _propulsionFireLocalPosition;

        [Space]

        [SerializeField] private Vector3 _bulletsSpawnPointsLocalPosition;
        public Vector3 BulletsSpawnPointsLocalPosition => _bulletsSpawnPointsLocalPosition;


        [Header("Invulnerability")]

        [SerializeField] private int _invulnerabilityDuration;
        public int InvulnerabilityDuration => _invulnerabilityDuration;

        [Space]

        [SerializeField] private int _invulnerabilityBlinksPerSecond;
        public int InvulnerabilityBlinksPerSecond => _invulnerabilityBlinksPerSecond;


        [Header("Shield")]

        [SerializeField] private Color _shieldColor;
        public Color ShieldColor => _shieldColor;

        [Space]

        [SerializeField] private float _shieldFadeInDuration = 0.5f;
        public float ShieldFadeInDuration => _shieldFadeInDuration;

        [SerializeField] private float _shieldFadeOutDuration = 0.5f;
        public float ShieldFadeOutDuration => _shieldFadeOutDuration;

        [Space]

        [SerializeField] private float _shieldBlinkDuration = 0.3f;
        public float ShieldBlinkDuration => _shieldBlinkDuration;

        [SerializeField] private float _shieldBlinkMinAlpha = 0.5f;
        public float ShieldBlinkMinAlpha => _shieldBlinkMinAlpha;

        [SerializeField] private float _shieldBlinkMaxAlpha = 0.8f;
        public float ShieldBlinkMaxAlpha => _shieldBlinkMaxAlpha;

        [Space]

        [SerializeField] private float _shieldSliderMinValue = 20;
        public float ShieldSliderMinValue => _shieldSliderMinValue;

        [SerializeField] private float _shielSliderMaxValue = 100;
        public float ShielSliderMaxValue => _shielSliderMaxValue;

        [Space]

        [SerializeField] private Color _shieldSliderRecoveringColor;
        public Color ShieldSliderRecoveringColor => _shieldSliderRecoveringColor;

        [SerializeField] private Color _shieldSliderFullColor;
        public Color ShieldSliderFullColor => _shieldSliderFullColor;

        [Space]

        [SerializeField] private Color _shieldShineColor;
        public Color ShieldShineColor => _shieldShineColor;

        [Header("Edge Resposition")]

        [SerializeField] private ScreenEdgeTeleportConfiguration _screenEdgeTeleportConfiguration;
        public ScreenEdgeTeleportConfiguration ScreenEdgeTeleportConfiguration => _screenEdgeTeleportConfiguration;


        [Header("CameraShake")]

        [SerializeField] private CameraShakeConfiguration _shieldLostCameraShakeConfig;
        public CameraShakeConfiguration ShieldLostCameraShakeConfig => _shieldLostCameraShakeConfig;

        [SerializeField] private CameraShakeConfiguration _damageTakedCameraShakeConfig;
        public CameraShakeConfiguration DamageTakedCameraShakeConfig => _damageTakedCameraShakeConfig;

        [SerializeField] private CameraShakeConfiguration _deadCameraShakeConfig;
        public CameraShakeConfiguration DeadCameraShakeConfig => _deadCameraShakeConfig;


        [Header("Particles")]

        [SerializeField] private List<ParticleConfiguration> _particlesOnDead;
        public List<ParticleConfiguration> ParticlesOnDead => _particlesOnDead;


        [Header("Sound")]

        [SerializeField] private List<SO_SoundData> _soundsOnShieldUp;
        public List<SO_SoundData> SoundsOnShieldUp => _soundsOnShieldUp;

        [SerializeField] private List<SO_SoundData> _soundsOnShieldDown;
        public List<SO_SoundData> SoundsOnShieldDown => _soundsOnShieldDown;

        [Space]

        [SerializeField] private List<SO_SoundData> _soundsOnDamage;
        public List<SO_SoundData> SoundsOnDamage => _soundsOnDamage;

        [SerializeField] private List<SO_SoundData> _soundsOnDestruction;
        public List<SO_SoundData> SoundsOnDestruction => _soundsOnDestruction;

        #endregion
    }
}