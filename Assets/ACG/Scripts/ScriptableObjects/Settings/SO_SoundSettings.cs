using ACG.Scripts.ScriptableObjects.Collections;
using ACG.Scripts.ScriptableObjects.Data;
using ACG.Tools.Runtime.SOCreator.Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ACG.Scripts.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "ScriptableObjects/Settings/Sound")]
    public class SO_SoundSettings : SO_SettingsBase
    {
        #region Singleton

        private static SO_SoundSettings _instance;
        public static SO_SoundSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SO_SoundSettings>("Settings/SoundSettings");
                    if (_instance == null)
                        Debug.LogError($"No {nameof(SO_SoundSettings)} found in Resources");
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        #endregion

        #region Values

        [Header("AudioMixers")]

        [SerializeField] private AudioMixer _musicMixer;
        public AudioMixer MusicMixer=> _musicMixer;

        [SerializeField] private AudioMixer _effectsMixer;
        public AudioMixer EffectsMixer=> _effectsMixer;


        [Header("Music")]

        [SerializeField] private bool _autoPlayMusic = true;
        public bool AutoPlayMusic => _autoPlayMusic;
        
        [SerializeField] private SO_MusicCollection _musicCollecion;
        public SO_MusicCollection MusicCollection => _musicCollecion;

        [SerializeField] private int _musicEndCheckMs = 200;
        public int MusicEndCheckMs => _musicEndCheckMs;


        [Header("Ambient")]
        

        [SerializeField] private bool _useAmbientSound = true;
        public bool UseAmbientSound => _useAmbientSound;

        [SerializeField] private string _ambientParentName = "Ambient_Sounds_Parent";
        public string AmbientParentName => _ambientParentName;

        [SerializeField] private List<SO_SoundData> _ambientSoundsList;
        public List<SO_SoundData> AmbientSoundList => _ambientSoundsList;


        [Header("Sounds")]

        [SerializeField] private string _sound3DParentName = "3DSound_general_parent";
        public string Sound3DParentName => _sound3DParentName;

        #endregion
    }
}