using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    #region Fields

    private MusicConfiguration _musicConfiguration;
    [Space]
    [SerializeField] private AudioMixer _musicMixer;
    [SerializeField] private AudioMixer _effectsMixer;
    [Space]
    private int _currentMusicIndex = -1;
    [Space]
    private AudioSource _musicSource;
    private AudioSource _uiSource;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.GetUiBus().AddListener<StartGame>(OnStartGame);
        EventManager.GetUiBus().AddListener<MusicVolumeUpdated>(OnMusicVolumeUpdated);
        EventManager.GetUiBus().AddListener<EffectsVolumeUpdated>(OnEffectsVolumeUpdated);
        EventManager.GetGameplayBus().AddListener<GenerateSound>(OnGenerateSound);
        EventManager.GetUiBus().AddListener<GenerateUISound>(OnGenerateUISound);
    }

    private void OnDisable()
    {
        EventManager.GetUiBus().RemoveListener<StartGame>(OnStartGame);
        EventManager.GetUiBus().RemoveListener<MusicVolumeUpdated>(OnMusicVolumeUpdated);
        EventManager.GetUiBus().RemoveListener<EffectsVolumeUpdated>(OnEffectsVolumeUpdated);
        EventManager.GetGameplayBus().RemoveListener<GenerateSound>(OnGenerateSound);
        EventManager.GetUiBus().RemoveListener<GenerateUISound>(OnGenerateUISound);
    }

    #endregion

    #region Bus Callbacks

    private void OnMusicVolumeUpdated(MusicVolumeUpdated pMusicVolumeUpdated)
    {
        float newVolume = Mathf.Log10(pMusicVolumeUpdated.Value) * 20;
        _musicMixer.SetFloat("MasterVolume", newVolume);
        Debug.Log(string.Format("- SETTINGS - Music volume is {0}", pMusicVolumeUpdated.Value));
    }
    private void OnEffectsVolumeUpdated(EffectsVolumeUpdated pEffectsVolumeUpdated)
    {
        float newVolume = Mathf.Log10(pEffectsVolumeUpdated.Value) * 20;
        _effectsMixer.SetFloat("MasterVolume", newVolume);
        Debug.Log(string.Format("- SETTINGS - Effects volume is {0}", pEffectsVolumeUpdated.Value));
    }

    private void OnStartGame(StartGame pStartGame)
    {
        PlayMusicLoop();
    }

    private void OnGenerateSound(GenerateSound pGenerateSound)
    {
        foreach (SO_Sound item in pGenerateSound.SoundsData)
            CreateSound(item);
    }

    private void OnGenerateUISound(GenerateUISound pGenerateUISound)
    {
        CreateUISound(pGenerateUISound.SoundsData);
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _musicConfiguration = ResourcesManager.Instance.MusicConfiguration;
        _musicSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        _uiSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
    }
    
    #endregion

    #region Music

    private async void PlayMusicLoop()
    {
        while (this)
        {
            PlayRandomTrack();
            await WaitForMusicEnd();
        }
    }

    private void PlayRandomTrack()
    {
        int newTrackIndex;
        do
            newTrackIndex = Random.Range(0, _musicConfiguration.Musics.Count);
        while (newTrackIndex == _currentMusicIndex);

        _currentMusicIndex = newTrackIndex;
        SO_Sound currentMusicData = _musicConfiguration.Musics[_currentMusicIndex];
        currentMusicData.ApplyConfig(_musicSource);

        _musicSource.Play();
    }

    private async Task WaitForMusicEnd()
    {
        while (_musicSource && _musicSource.isPlaying)
            await Task.Yield();
    }

    #endregion

    #region Sounds

    private void CreateSound(SO_Sound pData)
    {
        AudioSource audioSource = PoolsManager.Instance.GetInstance(pData.PoolName).GetComponent<AudioSource>();
        pData.ApplyConfig(audioSource);
        audioSource.GetComponent<AudioSourceController>().Play();
    }

    private void CreateUISound(SO_Sound pData)
    {
        _uiSource.Stop();
        pData.ApplyConfig(_uiSource);
        _uiSource.Play();
    }

    #endregion
}

#region IEvents

public class GenerateSound : IEvent
{
    public SO_Sound[] SoundsData { get; set; }
}
public class GenerateUISound : IEvent
{
    public SO_Sound SoundsData { get; set; }
}

#endregion