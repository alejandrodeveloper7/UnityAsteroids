using ACG.Core.Utils;
using ACG.Scripts.ScriptableObjects.Data;
using ACG.Scripts.ScriptableObjects.Settings;
using ACG.Tools.Runtime.ManagersCreator.Bases;
using ACG.Tools.Runtime.Pooling.Core;
using ACG.Tools.Runtime.Pooling.Interfaces;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace ACG.Scripts.Managers
{
    public class SoundManager : MonobehaviourInstancesManagerBase<SoundManager>, ISoundManager
    {
        #region Fields

        [Header("Data")]
        [Inject] private readonly SO_SoundSettings _soundSettings;

        [Header("Gameplay References")]
        private Transform _3dSoundsGeneralParent;
        private Transform _ambiendSourcesParent;
        private AudioSource _musicSource;

        [Header("Music")]
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

            if (_soundSettings.AutoPlayMusic)
                PlayMusicLoop();

            if (_soundSettings.UseAmbientSound)
                PlayAmbientMusic();
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
            _soundSettings.MusicMixer.SetFloat("MasterVolume", newVolume);
            Debug.Log($"- Volume - Music volume is {newValue}");
        }

        public void SetEffectsVolume(float newValue)
        {
            float newVolume = Mathf.Log10(newValue) * 20;
            _soundSettings.EffectsMixer.SetFloat("MasterVolume", newVolume);
            Debug.Log($"- Volume - Effects volume is {newValue}");
        }

        #endregion

        #region Ambient Sound

        private void PlayAmbientMusic()
        {
            _ambiendSourcesParent = new GameObject(_soundSettings.AmbientParentName).transform;

            foreach (var soundData in _soundSettings.AmbientSoundList)
            {
                AudioSource newSource = _ambiendSourcesParent.gameObject.AddComponent<AudioSource>();
                soundData.ApplyConfiguration(newSource);
                newSource.loop = true;
                newSource.Play();
            }
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
                newTrackIndex = Random.Range(0, _soundSettings.MusicCollection.Elements.Count);
            while (newTrackIndex == _currentMusicIndex && _soundSettings.MusicCollection.Elements.Count > 1);

            _currentMusicIndex = newTrackIndex;
            SO_SoundData currentMusicData = _soundSettings.MusicCollection.Elements[_currentMusicIndex];
            currentMusicData.ApplyConfiguration(_musicSource);

            _musicSource.Play();
        }
        public async void PlayMusicLoop()
        {
            if (_soundSettings.MusicCollection == null)
            {
                Debug.LogWarning($"- {typeof(SoundManager).Name} - SO_MusicCollection is null");
                return;
            }

            if (_soundSettings.MusicCollection.Elements.Count is 0)
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
                await Task.Delay(_soundSettings.MusicEndCheckMs);
        }

        #endregion

        #region Sound

        private void Create3DSoundsGeneralParent()
        {
            GameObject newGameObject = new(_soundSettings.Sound3DParentName);
            GameObject.DontDestroyOnLoad(newGameObject);
            _3dSoundsGeneralParent = newGameObject.transform;
        }

        public void Play2DSounds(List<SO_SoundData> data)
        {
            if (data is null || data.Count == 0)
                return;

            foreach (SO_SoundData soundData in data)
                _ = Play2DSound(soundData);
        }
        public async Task Play2DSound(SO_SoundData data)
        {
            if (data == null)
                return;

            if (data.Delay > 0)
                await TimingUtils.WaitSeconds(data.Delay);

            AudioSource audioSource = FactoryManager.Instance.Get2DAudioSourceInstance();
            data.ApplyConfiguration(audioSource);
            audioSource.Play();
        }

        public void Play3DSounds(List<SO_SoundData> data, Vector3 position, Transform parent = null)
        {
            if (data is null || data.Count == 0)
                return;

            foreach (SO_SoundData soundData in data)
                _ = Play3DSound(soundData, position, parent);
        }
        public async Task Play3DSound(SO_SoundData data, Vector3 position, Transform parent = null)
        {
            if (data == null)
                return;

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

            new3DSound.GetComponent<IPooledDetonable>().Detonate();
        }

        #endregion
    }
}
