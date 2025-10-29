using ToolsACG.SOCreator.Data;
using UnityEngine;
using UnityEngine.Audio;

namespace ToolsACG.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "NewSound", menuName = "ScriptableObjects/Data/Sound")]
    public class SO_SoundData : SO_DataBase
    {
        #region Values

        [Header("Configuration")]

        [SerializeField] private float _delay;
        public float Delay => _delay;

        [SerializeField] private AudioClip _clip;
        public AudioClip Clip => _clip;

        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        public AudioMixerGroup AudioMixerGroup => _audioMixerGroup;

        [Space]

        [SerializeField] private bool _mute;
        public bool Mute => _mute;

        [SerializeField] private bool _bypassEffects;
        public bool BypassEffects => _bypassEffects;

        [SerializeField] private bool _bypassListenerEffects;
        public bool BypassListenerEffects => _bypassListenerEffects;

        [SerializeField] private bool _bypassReverbZones;
        public bool BypassReverbZones => _bypassReverbZones;

        [SerializeField] private bool _loop;
        public bool Loop => _loop;

        [Space]

        [Range(0, 256f)][SerializeField] private int _priority = 128;
        public int Priority => _priority;

        [Range(0, 1f)][SerializeField] private float _volume = 1;
        public float Volume => _volume;

        [Space]

        [Range(-3, 3f)][SerializeField] private float _pitch = 1;
        public float Pitch => _pitch;

        [SerializeField] private float _pitchVariation;
        public float PitchVariation => _pitchVariation;

        [Space]

        [Range(-1, 1f)][SerializeField] private float _stereoPan = 0;
        public float StereoPan => _stereoPan;

        [Range(0, 1f)][SerializeField] private float _spatialBlend = 0;
        public float SpatialBlend => _spatialBlend;

        [Range(0, 1.1f)][SerializeField] private float _reverbZonMix = 1;
        public float ReverbZonMix => _reverbZonMix;

        [Space]

        [Range(0, 5f)][SerializeField] private float _dopplerLevel = 1;
        public float DopplerLevel => _dopplerLevel;

        [Range(0, 360)][SerializeField] private int _spread = 0;
        public int Spread => _spread;

        [SerializeField] private AudioRolloffMode _volumeRolloff = AudioRolloffMode.Logarithmic;
        public AudioRolloffMode VolumeRolloff => _volumeRolloff;

        [SerializeField] private AnimationCurve _customVolumeRollofCurve;
        public AnimationCurve CustomVolumeRollofCurve => _customVolumeRollofCurve;

        [SerializeField] private float _minDistance = 1;
        public float MinDistance => _minDistance;

        [SerializeField] private float _maxDistance = 500;
        public float MaxDistance => _maxDistance;

        #endregion

        #region Methods

        public void ApplyConfiguration(AudioSource audioSource)
        {
            audioSource.clip = Clip;
            audioSource.outputAudioMixerGroup = AudioMixerGroup;

            audioSource.mute = Mute;
            audioSource.bypassEffects = BypassEffects;
            audioSource.bypassListenerEffects = BypassListenerEffects;
            audioSource.bypassReverbZones = BypassReverbZones;
            audioSource.loop = Loop;

            audioSource.priority = Priority;
            audioSource.volume = Volume;

            float newPitch = Pitch;
            if (PitchVariation is not 0)
                newPitch += Random.Range(-PitchVariation, PitchVariation);
            audioSource.pitch = newPitch;

            audioSource.panStereo = StereoPan;
            audioSource.spatialBlend = SpatialBlend;
            audioSource.reverbZoneMix = ReverbZonMix;

            audioSource.dopplerLevel = DopplerLevel;
            audioSource.spread = Spread;
            audioSource.rolloffMode = VolumeRolloff;
            audioSource.minDistance = MinDistance;
            audioSource.maxDistance = MaxDistance;

            if (VolumeRolloff is AudioRolloffMode.Custom)
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, CustomVolumeRollofCurve);
        }

        #endregion
    }
}