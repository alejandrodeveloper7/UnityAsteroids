using UnityEngine;

namespace ACG.Scripts.ScriptableObjects.ParticleSystemConfigs
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
            ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();

            if (renderer != null && Material != null)
                renderer.material = Material;
        }

        #endregion
    }
}
