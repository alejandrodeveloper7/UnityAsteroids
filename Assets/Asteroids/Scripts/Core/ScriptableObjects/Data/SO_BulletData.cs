using ACG.Core.Models;
using ACG.Scripts.Models;
using ACG.Scripts.ScriptableObjects.Data;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using ACG.Tools.Runtime.SOCreator.Data;
using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "NewBullet", menuName = "ScriptableObjects/Data/Bullet")]
    public class SO_BulletData : SO_DataBase, ISelectorElement, IHasStats
    {
        #region Values

        [SerializeField] private SO_PooledGameObjectData _prefabData;
        public SO_PooledGameObjectData PrefabData => _prefabData;


        [Header("Selector Element")]

        [SerializeField] private bool _isActive = true;
        public bool IsActive => _isActive;

        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;


        [Header("Configuration")]

        [SerializeField] private Color _color;
        public Color Color => _color;

        [SerializeField] private Vector3 _scale;
        public Vector3 Scale => _scale;

        [SerializeField] private float _betweenBulletsTime;
        public float BetweenBulletsTime => _betweenBulletsTime;

        [Space]

        [SerializeField] private float _fullScaleDuration;
        public float FullScaleDuration => _fullScaleDuration;

        [SerializeField] private float _descaleDuration;
        public float DescaleDuration => _descaleDuration;

        public float LifeTime => _fullScaleDuration + _descaleDuration;

        [Space]

        [SerializeField] private float _pushForce = 50;
        public float PushForce => _pushForce;

        [SerializeField] private float _torqueForce = 100;
        public float TorqueForce => _torqueForce;


        [Header("Stats")]

        [SerializeField] private float _speed;
        public float Speed => _speed;

        [SerializeField] private int _damage;
        public int Damage => _damage;


        [Header("Has Stats")]

        [SerializeField] private List<StatData> _stats;
        public List<StatData> Stats => _stats;
        

        [Header("Particles")]

        [SerializeField] private List<ParticleSystemData> _particlesOnDestruction;
        public List<ParticleSystemData> ParticlesOnDestruction => _particlesOnDestruction;


        [Header("Edge Resposition")]

        [SerializeField] private ScreenEdgeTeleportData _screenEdgeTeleportData;
        public ScreenEdgeTeleportData ScreenEdgeTeleportData => _screenEdgeTeleportData;


        [Header("Sound")]

        [SerializeField] private List<SO_SoundData> _soundsOnShoot;
        public List<SO_SoundData> SoundsOnShoot => _soundsOnShoot;

        #endregion
    }
}
