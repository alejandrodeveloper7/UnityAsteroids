using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolsACG.Core.ScriptableObjects.Collections;
using ToolsACG.Core.ScriptableObjects.Data;
using ToolsACG.Core.Utils;
using ToolsACG.ManagersCreator.Bases;
using ToolsACG.Pooling.Core;
using ToolsACG.Pooling.Gameplay;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace ToolsACG.Core.Managers
{
    public class SoundManager : MonobehaviourInstancesManagerBase<SoundManager>, ISoundManager
    {
        #region Fields

        [Header("Data")]
        [Inject] private readonly SO_MusicCollection _musicCollecion;

        [Header("Audio Mixers")]
        [SerializeField] private AudioMixer _musicMixer;
        [SerializeField] private AudioMixer _effectsMixer;

        [Header("Gameplay References")]
        private Transform _3dSoundsGeneralParent;

        [Header("Music")]
        [SerializeField] private bool _autoPlayMusic;
        [Space]
        private AudioSource _musicSource;
        private int _currentMusicIndex = -1;
        private bool _isMusicPlaying;

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            base.GetReferences();
            // TODO: Get your references here
        }

        public void Setup()
        {
            // TODO: Manual method to set parameters
        }

        public override void Initialize()
        {
            base.Initialize();

            Create3DSoundsGeneralParent();
            CreateMusicSource();

            if (_autoPlayMusic)
                PlayMusicLoop();
        }

        public override void Dispose()
        {
            base.Dispose();

            _musicSource.Stop();
            Destroy(_musicSource);
            _currentMusicIndex = -1;
            _isMusicPlaying = false;
        }

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
        }

        protected override void Start()
        {
            base.Start();

            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        #region Volume

        public void SetMusicVolume(float newValue)
        {
            float newVolume = Mathf.Log10(newValue) * 20;
            _musicMixer.SetFloat("MasterVolume", newVolume);
            Debug.Log($"- Volume - Music volume is {newValue}");
        }

        public void SetEffectsVolume(float newValue)
        {
            float newVolume = Mathf.Log10(newValue) * 20;
            _effectsMixer.SetFloat("MasterVolume", newVolume);
            Debug.Log($"- Volume - Effects volume is {newValue}");
        }

        #endregion

        #region Music

        private void CreateMusicSource()
        {
            _musicSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        }

        private void PlayRandomTrack()
        {
            int newTrackIndex;
            do
                newTrackIndex = Random.Range(0, _musicCollecion.Elements.Count);
            while (newTrackIndex == _currentMusicIndex && _musicCollecion.Elements.Count > 1);

            _currentMusicIndex = newTrackIndex;
            SO_SoundData currentMusicData = _musicCollecion.Elements[_currentMusicIndex];
            currentMusicData.ApplyConfiguration(_musicSource);

            _musicSource.Play();
        }
        public async void PlayMusicLoop()
        {
            if (_musicCollecion == null)
            {
                Debug.LogWarning($"- {typeof(SoundManager).Name} - SO_MusicCollection is null");
                    return;
            }

            if (_musicCollecion.Elements.Count is 0)
                return;

            _isMusicPlaying = true;

            while (this)
            {
                PlayRandomTrack();
                await WaitForMusicEnd();
            }
        }
        public void StopMusicLoop(float progressivelyStopDuration)
        {
            if (progressivelyStopDuration is not 0)
            {
                float startVolume = _musicSource.volume;

                _musicSource.DOFade(0f, progressivelyStopDuration)
                    .OnComplete(() =>
                    {
                        _musicSource.Stop();
                        _isMusicPlaying = false;
                    });
            }
            else
            {
                _musicSource.Stop();
                _isMusicPlaying = false;
            }
        }
        private async Task WaitForMusicEnd()
        {
            while (_isMusicPlaying && _musicSource != null && _musicSource.isPlaying)
                await Task.Delay(200);
        }

        #endregion

        #region Sound

        private void Create3DSoundsGeneralParent()
        {
            GameObject newGameObject = new("3DSound_general_parent");
            GameObject.DontDestroyOnLoad(newGameObject);
            _3dSoundsGeneralParent = newGameObject.transform;
        }

        public void Play2DSounds(List<SO_SoundData> data)
        {
            foreach (SO_SoundData soundData in data)
                _ = Play2DSound(soundData);
        }
        public async Task Play2DSound(SO_SoundData data)
        {
            if (data.Delay > 0)
                await TimingUtils.WaitSeconds(data.Delay);

            AudioSource audioSource = FactoryManager.Instance.Get2DAudioSourceInstance();
            data.ApplyConfiguration(audioSource);
            audioSource.Play();
        }

        public void Play3DSounds(List<SO_SoundData> data, Vector3 position, Transform parent)
        {
            foreach (SO_SoundData soundData in data)
                _ = Play3DSound(soundData, position, parent);
        }
        public async Task Play3DSound(SO_SoundData data, Vector3 position, Transform parent)
        {
            if (data.Delay > 0)
                await TimingUtils.WaitSeconds(data.Delay);

            GameObject new3DSound = FactoryManager.Instance.Get3DSoundInstance();

            if (parent == null)
            {
                new3DSound.transform.SetParent(_3dSoundsGeneralParent, false);
                new3DSound.transform.position = position;
            }
            else
            {
                new3DSound.transform.SetParent(parent, false);
                new3DSound.transform.localPosition = position;
            }

            AudioSource audioSource = new3DSound.GetComponent<AudioSource>();
            data.ApplyConfiguration(audioSource);

            new3DSound.GetComponent<Pooled3DSound>().Play();
        }

        #endregion
    }
}
