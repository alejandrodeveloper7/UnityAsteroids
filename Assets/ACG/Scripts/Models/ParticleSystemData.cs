using ACG.Scripts.ScriptableObjects.ParticleSystemConfigs;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using System;
using UnityEngine;

namespace ACG.Scripts.Models
{
    [Serializable]
    public class ParticleSystemData
    {
        public SO_PooledGameObjectData PrefabData;
        public SO_ParticleConfigurationBase ParticleConfiguration;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
    }
}
