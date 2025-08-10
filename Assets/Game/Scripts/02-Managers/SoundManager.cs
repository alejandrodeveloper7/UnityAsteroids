using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolsACG.Utils.Pooling;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    #region Fields

    [Header("Data")]
    private SO_MusicCollection _musicCollecion;

    [Header("Music")]
    [SerializeField] private AudioMixer _musicMixer;
    [SerializeField] private AudioMixer _effectsMixer;
    [Space]
    [SerializeField] private bool _autoPlayMusic;
    private bool _musicPlaying;
    private AudioSource _musicSource;
    private int _currentMusicIndex = -1;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
        Initialize();
    }

    private void Start()
    {
        if (_autoPlayMusic)
            PlayMusicLoop();
    }

    private void OnEnable()
    {
        EventManager.SoundBus.AddListener<MusicVolumeUpdated>(OnMusicVolumeUpdated);
        EventManager.SoundBus.AddListener<EffectsVolumeUpdated>(OnEffectsVolumeUpdated); 
        EventManager.SoundBus.AddListener<Generate2DSound>(OnGenerate2DSound);
        EventManager.SoundBus.AddListener<Generate3DSound>(OnGenerate3DSound);
        EventManager.SoundBus.AddListener<PlayMusic>(OnPlayMusic);
        EventManager.SoundBus.AddListener<StopMusic>(OnStopMusic);
    }

    private void OnDisable()
    {
        EventManager.SoundBus.RemoveListener<MusicVolumeUpdated>(OnMusicVolumeUpdated);
        EventManager.SoundBus.RemoveListener<EffectsVolumeUpdated>(OnEffectsVolumeUpdated); 
        EventManager.SoundBus.RemoveListener<Generate2DSound>(OnGenerate2DSound);
        EventManager.SoundBus.RemoveListener<Generate3DSound>(OnGenerate3DSound);
        EventManager.SoundBus.RemoveListener<PlayMusic>(OnPlayMusic);
        EventManager.SoundBus.RemoveListener<StopMusic>(OnStopMusic);
    }

    #endregion

    #region Bus Callbacks   

    private void OnGenerate2DSound(Generate2DSound pGenerate2DSound)
    {
        foreach (SO_Sound item in pGenerate2DSound.SoundsData)
            Create2DSound(item);
    }

    private void OnGenerate3DSound(Generate3DSound pGenerate3DSound)
    {
        foreach (SO_Sound item in pGenerate3DSound.SoundsData)
            Create3DSound(item, pGenerate3DSound.Position);
    }

    private void OnPlayMusic(PlayMusic pPlayMusic)
    {
        if (_musicPlaying is false)
            PlayMusicLoop();
    }

    private void OnStopMusic(StopMusic pStopMusic)
    {
        if (_musicPlaying)
            StopMusicLoop(pStopMusic.ProgressivelyStopDuration);
    }

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
    #endregion

        #region Initialization

    private void GetReferences()
    {
        _musicCollecion = ResourcesManager.Instance.GetScriptableObject<SO_MusicCollection>(ScriptableObjectKeys.MUSIC_COLLECTION_KEY);
    }

    private void Initialize()
    {
        CreateMusicSource();
    }

    #endregion

    #region Music

    private void CreateMusicSource()
    {
        _musicSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
    }

    private async void PlayMusicLoop()
    {
        if (_musicCollecion.Musics.Count == 0)
            return;

        _musicPlaying = true;

        while (this)
        {
            PlayRandomTrack();
            await WaitForMusicEnd();
        }
    }

    private void StopMusicLoop(float pProgressivelyStopDuration)
    {
        if (pProgressivelyStopDuration > 0)
        {
            float startVolume = _musicSource.volume;

            _musicSource.DOFade(0f, pProgressivelyStopDuration)
                .OnComplete(() =>
                {
                    _musicSource.Stop();
                    _musicPlaying = false;
                });
        }
        else
        {
            _musicSource.Stop();
            _musicPlaying = false;
        }
    }

    private void PlayRandomTrack()
    {
        int newTrackIndex;
        do
            newTrackIndex = Random.Range(0, _musicCollecion.Musics.Count);
        while (newTrackIndex == _currentMusicIndex && _musicCollecion.Musics.Count > 1);

        _currentMusicIndex = newTrackIndex;
        SO_Sound currentMusicData = _musicCollecion.Musics[_currentMusicIndex];
        currentMusicData.ApplyConfig(_musicSource);

        _musicSource.Play();
    }

    private async Task WaitForMusicEnd()
    {
        while (_musicPlaying && _musicSource!=null && _musicSource.isPlaying)
            await Task.Delay(100);
    }

    #endregion

    #region Sounds

    private void Create2DSound(SO_Sound pData)
    {
        AudioSource audioSource = FactoryManager.Instance.Get2DAudioSource();
        pData.ApplyConfig(audioSource);

        if (pData.Delay > 0)
            audioSource.PlayDelayed(pData.Delay);
        else
            audioSource.Play();
    }

    private void Create3DSound(SO_Sound pData, Vector3 pPosition)
    {
        GameObject new3DSound = FactoryManager.Instance.GetGameObjectInstance(pData.PoolName);
        new3DSound.transform.position = pPosition;

        AudioSource audioSource = new3DSound.GetComponent<AudioSource>();
        pData.ApplyConfig(audioSource);

        Sound3DController sound3DController = new3DSound.GetComponentInParent<Sound3DController>();
        sound3DController.Play(pData.Delay);
    }

    #endregion
}

#region IEvents

public sealed class Generate2DSound : IEvent
{
    public List<SO_Sound> SoundsData { get; set; }

    public Generate2DSound(List<SO_Sound> soundsData)
    {
        SoundsData = soundsData;
    }
}

public sealed class Generate3DSound : IEvent
{
    public List<SO_Sound> SoundsData { get; set; }
    public Vector3 Position { get; set; }

    public Generate3DSound(List<SO_Sound> soundsData, Vector3 position)
    {
        SoundsData = soundsData;
        Position = position;
    }
}

public readonly struct PlayMusic : IEvent
{
}

public readonly struct StopMusic : IEvent
{
    public readonly float ProgressivelyStopDuration;

    public StopMusic(float progressivelyStopDuration)
    {
        ProgressivelyStopDuration = progressivelyStopDuration;
    }
}

#endregion

