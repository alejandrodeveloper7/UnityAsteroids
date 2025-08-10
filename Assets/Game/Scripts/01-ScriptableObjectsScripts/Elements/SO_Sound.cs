using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSound", menuName = "ScriptableObjects/Elements/Sound")]
public class SO_Sound : ScriptableObject
{
    public string PoolName;
    public float Delay;
    [Header("Configuration")]
    [Space]
    public AudioClip Clip;
    public AudioMixerGroup AudioMixerGroup;
    [Space]
    public bool Mute;
    public bool BypassEffects;
    public bool BypassListenerEffects;
    public bool BypassReverbZones;
    public bool Loop;
    [Space]
    [Range(0, 256f)] public int Priority = 128;
    [Range(0, 1f)] public float Volume = 1;
    [Space]
    [Range(-3, 3f)] public float Pitch = 1;
    public float PitchVariation;
    [Space]
    [Range(-1, 1f)] public float StereoPan = 0;
    [Range(0, 1f)] public float SpatialBlend = 0;
    [Range(0, 1.1f)] public float ReverbZonMix = 1;
    [Space]
    [Range(0, 5f)] public float DopplerLevel = 1;
    [Range(0, 360)] public int Spread = 0;
    public AudioRolloffMode VolumeRolloff = AudioRolloffMode.Logarithmic;
    public AnimationCurve CustomVolumeRollofCurve;
    public float MinDistance = 1;
    public float MaxDistance = 500;


    public void ApplyConfig(AudioSource pAudioSource)
    {
        pAudioSource.clip = Clip;
        pAudioSource.outputAudioMixerGroup = AudioMixerGroup;

        pAudioSource.mute = Mute;
        pAudioSource.bypassEffects = BypassEffects;
        pAudioSource.bypassListenerEffects = BypassListenerEffects;
        pAudioSource.bypassReverbZones = BypassReverbZones;
        pAudioSource.loop = Loop;

        pAudioSource.priority = Priority;
        pAudioSource.volume = Volume;

        float newPitch = Pitch;
        if (PitchVariation != 0)
            newPitch += Random.Range(-PitchVariation, PitchVariation);
        pAudioSource.pitch = newPitch;

        pAudioSource.panStereo = StereoPan;
        pAudioSource.spatialBlend = SpatialBlend;
        pAudioSource.reverbZoneMix = ReverbZonMix;

        pAudioSource.dopplerLevel = DopplerLevel;
        pAudioSource.spread = Spread;
        pAudioSource.rolloffMode = VolumeRolloff;
        pAudioSource.minDistance = MinDistance;
        pAudioSource.maxDistance = MaxDistance;
      
        if (VolumeRolloff is AudioRolloffMode.Custom)
            pAudioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, CustomVolumeRollofCurve);      
    }
}
