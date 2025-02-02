using ToolsACG.Utils.Events;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _musicMixer;
    [SerializeField] private AudioMixer _effectsMixer;


    private void OnEnable()
    {
        EventManager.GetUiBus().AddListener<MusicVolumeUpdated>(OnMusicVolumeUpdated);
        EventManager.GetUiBus().AddListener<EffectsVolumeUpdated>(OnEffectsVolumeUpdated);
    }

    private void OnDisable()
    {
        EventManager.GetUiBus().RemoveListener<MusicVolumeUpdated>(OnMusicVolumeUpdated);
        EventManager.GetUiBus().RemoveListener<EffectsVolumeUpdated>(OnEffectsVolumeUpdated);
    }

    private void OnMusicVolumeUpdated(MusicVolumeUpdated pMusicVolumeUpdated) 
    {
        float newVolume = Mathf.Lerp(-80f, 0f, pMusicVolumeUpdated.Value);
        _musicMixer.SetFloat("MasterVolume", newVolume);
    }
    private void OnEffectsVolumeUpdated(EffectsVolumeUpdated pEffectsVolumeUpdated) 
    {
        float newVolume = Mathf.Lerp(-80f, 0f, pEffectsVolumeUpdated.Value);
        _effectsMixer.SetFloat("MasterVolume", newVolume);
    }

}
