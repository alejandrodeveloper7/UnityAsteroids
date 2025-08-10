using UnityEngine;

[CreateAssetMenu(fileName = "NewParticleMaterialConfig", menuName = "ScriptableObjects/ParticleSystemConfigurations/ParticleMaterialConfig")]
public class ParticleMaterialConfig : ParticleSystemConfig
{
    public Material Material;

    public override void ApplyConfig(ParticleSystem pParticleSystem)
    {
        ParticleSystemRenderer renderer = pParticleSystem.GetComponent<ParticleSystemRenderer>();

        if (renderer != null && Material != null)        
            renderer.material=Material;        
    }
}
