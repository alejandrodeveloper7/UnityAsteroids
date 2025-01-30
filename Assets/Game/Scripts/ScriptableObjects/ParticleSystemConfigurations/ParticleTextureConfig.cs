using UnityEngine;

[CreateAssetMenu(fileName = "NewParticleTextureConfig", menuName = "ScriptableObjects/ParticleSystemConfigurations/ParticleTextureConfig", order = 0)]
public class ParticleTextureConfig : ParticleSystemConfig
{
    public Texture2D Texture;

    public override void ApplyConfig(ParticleSystem pParticleSystem)
    {
        ParticleSystemRenderer renderer = pParticleSystem.GetComponent<ParticleSystemRenderer>();
        if (renderer != null && Texture != null)
        {
            renderer.material.SetTexture("_MainTex", Texture);
        }
    }
}
