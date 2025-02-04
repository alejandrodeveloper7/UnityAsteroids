using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSound", menuName = "ScriptableObjects/Sound", order = 0)]
public class SO_Sound : SO_Base
{
    [Space]
    public string PoolName;
    public AudioClip Clip;
    public AudioMixerGroup AudioMixerGroup;
    [Range(0, 1f)] public float Volume;
    public float PitchVariation;


    public void ApplyConfig(AudioSource pAudioSource)
    {
        pAudioSource.clip = Clip;
        pAudioSource.volume = Volume;
        pAudioSource.outputAudioMixerGroup = AudioMixerGroup;

        float newPitch = 1;
        if (PitchVariation != 0)
            newPitch += Random.Range(-PitchVariation, PitchVariation);

        pAudioSource.pitch = newPitch;
    }
}
