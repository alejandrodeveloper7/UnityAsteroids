using ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs;
using UnityEngine;

namespace ToolsACG.Core.Managers
{
    public interface IVFXManager
    {
        public void PlayParticlesVFX(string name, Vector3 position, Transform parent = null, SO_ParticleConfigurationBase particlesConfig = null);  
    }
}