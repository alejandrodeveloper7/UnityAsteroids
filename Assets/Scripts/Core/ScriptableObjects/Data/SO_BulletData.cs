using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using System.Collections.Generic;
using ToolsACG.Core.ScriptableObjects.Data;
using ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs;
using ToolsACG.Core.Utilitys;
using ToolsACG.Pooling.ScriptableObjects;
using ToolsACG.SOCreator.Data;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "NewBullet", menuName = "ScriptableObjects/Data/Bullet")]
    public class SO_BulletData : SO_DataBase, ISelectorElement, IHasStats
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


        [Header("Configuration")]

        [SerializeField] private SO_PooledGameObjectData _prefabData;
        public SO_PooledGameObjectData PrefabData => _prefabData;

        [SerializeField] private Color _color;
        public Color Color => _color;

        [SerializeField] private Vector3 _scale;
        public Vector3 Scale => _scale;


        [Header("Stats")]

        [SerializeField] private float _speed;
        public float Speed => _speed;

        [SerializeField] private int _damage;
        public int Damage => _damage;

        [SerializeField] private float _betweenBulletsTime;
        public float BetweenBulletsTime => _betweenBulletsTime;

        [Space]

        [SerializeField] private float _lifeDuration;
        public float LifeDuration => _lifeDuration;

        [SerializeField] private float _descaleDuration;
        public float DescaleDuration => _descaleDuration;

        [Space]

        [SerializeField] private float _pushForce = 50;
        public float PushForce => _pushForce;

        [SerializeField] private float _torqueForce = 100;
        public float TorqueForce => _torqueForce;


        [Header("Particles")]

        [SerializeField] private List<ParticleConfiguration> _particlesOnDestruction;
        public List<ParticleConfiguration> ParticlesOnDestruction => _particlesOnDestruction;


        [Header("Edge Resposition")]

        [SerializeField] private ScreenEdgeTeleportConfiguration _screenEdgeTeleportConfiguration;
        public ScreenEdgeTeleportConfiguration ScreenEdgeTeleportConfiguration => _screenEdgeTeleportConfiguration;


        [Header("Sound")]

        [SerializeField] private List<SO_SoundData> _soundsOnShoot;
        public List<SO_SoundData> SoundsOnShoot => _soundsOnShoot;

        #endregion
    }
}
