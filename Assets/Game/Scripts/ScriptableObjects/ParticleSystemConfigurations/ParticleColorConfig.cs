using UnityEngine;

[CreateAssetMenu(fileName = "NewParticleColorConfig", menuName = "ScriptableObjects/ParticleSystemConfigurations/ParticleColorConfig", order = 1)]
public class ParticleColorConfig : ParticleSystemConfig
{
    public Color Color;

    public override void ApplyConfig(ParticleSystem pParticleSystem)
    {
        ParticleSystem.MainModule mainModule = pParticleSystem.main;
        mainModule.startColor = Color;
    }
}
