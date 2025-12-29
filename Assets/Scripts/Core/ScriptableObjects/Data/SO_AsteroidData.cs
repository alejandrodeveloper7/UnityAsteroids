using System.Collections.Generic;
using ToolsACG.Core.ScriptableObjects.Data;
using ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs;
using ToolsACG.Core.Utilitys;
using ToolsACG.Pooling.ScriptableObjects;
using ToolsACG.SOCreator.Data;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "NewAsteroid", menuName = "ScriptableObjects/Data/Asteroid")]
    public class SO_AsteroidData : SO_DataBase
    {
        #region Values

        [Header("Configuration")]

        [SerializeField] private SO_PooledGameObjectData _prefabData;
        public SO_PooledGameObjectData PrefabData => _prefabData;

        [Space]

        [SerializeField] private bool _isInitialAsteroid;
        public bool IsInitialAsteroid => _isInitialAsteroid;

        [SerializeField] private Sprite[] _possibleSprites;
        public Sprite[] PossibleSprites => _possibleSprites;

        [SerializeField] private Color _color;
        public Color Color => _color;

        [Space]

        [SerializeField] private Vector3 _scale;
        public Vector3 Scale => _scale;

        [SerializeField] private float _asteroidSpawnAngle;
        public float AsteroidSpawnAngle => _asteroidSpawnAngle;

        [SerializeField] private float _posibleTorque;
        public float PosibleTorque => _posibleTorque;

        [SerializeField] private float _alphaAparitionDuration;
        public float AlphaAparitionDuration => _alphaAparitionDuration;


        [Header("Stats")]

        [SerializeField] private int _maxHP;
        public int MaxHP => _maxHP;

        [SerializeField] private float _speed;
        public float Speed => _speed;

        [SerializeField] private int _scoreValue;
        public int ScoreValue => _scoreValue;

        [SerializeField] private int _damage = 1;
        public int Damage => _damage;


        [Header("Damage feedback")]

        [SerializeField] private Color _damageFeedbackColor;
        public Color DamageFeedbackColor => _damageFeedbackColor;

        [SerializeField] private Vector3 _damageFeedbackScale;
        public Vector3 DamageFeedbackScale => _damageFeedbackScale;

        [SerializeField] private float _damageFeedbackDuration;
        public float DamageFeedbackDuration => _damageFeedbackDuration;


        [Header("Destruction")]

        [SerializeField] private int _fragmentsAmountGeneratedOnDestruction;
        public int FragmentsAmountGeneratedOnDestruction => _fragmentsAmountGeneratedOnDestruction;

        [SerializeField] private List<SO_AsteroidData> _fragmentTypesGeneratedOnDestruction;
        public List<SO_AsteroidData> FragmentTypesGeneratedOnDestruction => _fragmentTypesGeneratedOnDestruction;


        [Header("Edge Resposition")]

        [SerializeField] private ScreenEdgeTeleportConfiguration _screenEdgeTeleportConfiguration;
        public ScreenEdgeTeleportConfiguration ScreenEdgeTeleportConfiguration => _screenEdgeTeleportConfiguration;


        [Header("Particles")]

        [SerializeField] private List<ParticleConfiguration> _particlesOnDamage;
        public List<ParticleConfiguration> ParticlesOnDamage => _particlesOnDamage;

        [SerializeField] private List<ParticleConfiguration> _particlesOnDestruction;
        public List<ParticleConfiguration> ParticlesOnDestruction => _particlesOnDestruction;


        [Header("Sound")]

        [SerializeField] private List<SO_SoundData> _soundsOnDamage;
        public List<SO_SoundData> SoundsOnDamage => _soundsOnDamage;

        [SerializeField] private List<SO_SoundData> _soundsOnDestruction;
        public List<SO_SoundData> SoundsOnDestruction => _soundsOnDestruction;

        #endregion
    }
}