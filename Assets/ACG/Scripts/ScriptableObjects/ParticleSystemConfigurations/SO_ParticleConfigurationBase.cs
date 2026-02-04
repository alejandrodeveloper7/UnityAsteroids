using UnityEngine;

namespace ACG.Scripts.ScriptableObjects.ParticleSystemConfigs
{
    public abstract class SO_ParticleConfigurationBase : ScriptableObject
    {
        #region Methods

        public abstract void ApplyConfiguration(ParticleSystem particleSystem);

        #endregion
    }
}