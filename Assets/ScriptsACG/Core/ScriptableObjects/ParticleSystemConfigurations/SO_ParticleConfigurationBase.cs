using System;
using UnityEngine;

namespace ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs
{
    public abstract class SO_ParticleConfigurationBase : ScriptableObject
    {
        #region Methods

        public abstract void ApplyConfiguration(ParticleSystem particleSystem);

        #endregion
    }

    #region Auxiliary Models

    [Serializable]
    public class ParticleConfiguration
    {
        public string ParticleEffectName;
        public SO_ParticleConfigurationBase ParticleConfig;
    }

    #endregion

}