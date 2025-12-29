using ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs;
using ToolsACG.Pooling.ScriptableObjects;
using UnityEngine;

namespace ToolsACG.Core.Managers
{
    public interface IVFXManager
    {
        void PlayParticlesVFX(SO_PooledGameObjectData pooledGameObjectData, Vector3 position, Quaternion rotation, Transform parent = null, SO_ParticleConfigurationBase particlesConfig = null);
    }
}