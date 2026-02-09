using ACG.Scripts.ScriptableObjects.Settings;
using ACG.Tools.Runtime.ServicesCreator.Bases;
using System;
using UnityEngine;
using Zenject;

namespace ACG.Scripts.Services
{
    public class SoundService : InstancesServiceBase, ISoundService
    {
        #region Fields and Events

        public event Action<bool> MusicMuteStateChanged;
        public event Action<float> MusicVolumeChanged;

        public event Action<bool> EffectsMuteStateChanged;
        public event Action<float> EffectsVolumeChanged;

        //[Header( "Values")]
        public float MusicVolume { get; private set; }
        public bool MusicMuted { get; private set; }

        public float EffectsVolume { get; private set; }
        public bool EffectsMuted { get; private set; }

        [Header("Data")]
        private readonly SO_SoundSettings _soundSettings;

        #endregion

        #region Constructors

        [Inject]
        public SoundService(SO_SoundSettings soundSettings)
        {
            Initialize();

            _soundSettings = soundSettings;
        }

        #endregion

        #region Initialization     

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method called in the constructor to initialize the Service
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the elements that need be clean when the Service is destroyed
        }

        #endregion

        #region Music management

        public void SetMusicVolume(float newValue)
        {
            MusicVolume = newValue;
            float newVolume = Mathf.Log10(newValue) * 20;
            _soundSettings.MusicMixer.SetFloat("MasterVolume", newVolume);
            MusicVolumeChanged?.Invoke(MusicVolume);

            Debug.Log($"- Volume - Music volume is {newValue}");
        }

        public void SetMuteMusicState(bool state)
        {
            MusicMuted = state;

            if (MusicMuted)
            {
                _soundSettings.MusicMixer.SetFloat("MasterVolume", 0);
            }
            else
            {
                float newVolume = Mathf.Log10(MusicVolume) * 20;
                _soundSettings.MusicMixer.SetFloat("MasterVolume", newVolume);
            }

            MusicMuteStateChanged?.Invoke(MusicMuted);

            Debug.Log($"- Volume - Music is muted: {state}");
        }

        #endregion

        #region Effect management

        public void SetEffectsVolume(float newValue)
        {
            EffectsVolume = newValue;
            float newVolume = Mathf.Log10(newValue) * 20;
            _soundSettings.EffectsMixer.SetFloat("MasterVolume", newVolume);
            EffectsVolumeChanged?.Invoke(EffectsVolume);

            Debug.Log($"- Volume - Effects volume is {newValue}");
        }

        public void SetMuteEffectsState(bool state)
        {
            EffectsMuted = state;

            if (EffectsMuted)
            {
                _soundSettings.EffectsMixer.SetFloat("MasterVolume", 0);
            }
            else
            {
                float newVolume = Mathf.Log10(EffectsVolume) * 20;
                _soundSettings.EffectsMixer.SetFloat("MasterVolume", newVolume);
            }

            EffectsMuteStateChanged?.Invoke(EffectsMuted);

            Debug.Log($"- Volume - Effects ara muted: {state}");
        }

        #endregion
    }
}