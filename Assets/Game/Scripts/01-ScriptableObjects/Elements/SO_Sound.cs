using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSound", menuName = "ScriptableObjects/Elements/Sound")]
public class SO_Sound : SO_Base
{
    [Space]
    public string PoolName;
    [Space]
    [Header("Clip")]
    public AudioClip Clip;
    [Range(0, 1f)] public float Volume;

    [Header("Configuration")]
    public bool PlayOnAwake;
    public bool Loop;
    public AudioMixerGroup AudioMixerGroup;
    public float PitchVariation;


    public void ApplyConfig(AudioSource pAudioSource)
    {
        pAudioSource.clip = Clip;
        pAudioSource.volume = Volume;
        pAudioSource.outputAudioMixerGroup = AudioMixerGroup;
        pAudioSource.loop = Loop;
        pAudioSource.playOnAwake = PlayOnAwake;

        float newPitch = 1;
        if (PitchVariation != 0)
            newPitch += Random.Range(-PitchVariation, PitchVariation);

        pAudioSource.pitch = newPitch;
    }
}
