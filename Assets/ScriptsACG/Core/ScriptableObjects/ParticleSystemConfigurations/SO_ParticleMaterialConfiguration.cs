using UnityEngine;

namespace ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs
{
    [CreateAssetMenu(fileName = "NewParticleMaterialConfig", menuName = "ScriptableObjects/ParticleSystemConfigurations/Material")]
    public class SO_ParticleMaterialConfiguration : SO_ParticleConfigurationBase
    {
        #region Values

        [Header("References")]

        [SerializeField] private Material _material;
        public Material Material => _material;

        #endregion

        #region Methods

        public override void ApplyConfiguration(ParticleSystem particleSystem)
        {
            if (Material == null)
                return;
            
            if (particleSystem.TryGetComponent(out ParticleSystemRenderer renderer))
                renderer.material = Material;
        }

        #endregion
    }
}
