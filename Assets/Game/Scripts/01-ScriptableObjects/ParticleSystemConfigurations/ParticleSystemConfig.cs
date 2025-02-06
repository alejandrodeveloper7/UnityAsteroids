using UnityEngine;

public abstract class ParticleSystemConfig : ScriptableObject
{
    public abstract void ApplyConfig(ParticleSystem pParticleSystem);       
}

[System.Serializable]
public class ParticleSetup
{
    public string particleEffectName;
    public ParticleSystemConfig particleConfig;
}
