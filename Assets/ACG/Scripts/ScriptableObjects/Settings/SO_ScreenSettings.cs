using ACG.Tools.Runtime.SOCreator.Settings;
using UnityEngine;

namespace ACG.Scripts.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "ScreenSettings", menuName = "ScriptableObjects/Settings/Screen")]
    public class SO_ScreenSettings : SO_SettingsBase
    {
        #region Singleton

        private static SO_ScreenSettings _instance;
        public static SO_ScreenSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SO_ScreenSettings>("Settings/ScreenSettings");
                    if (_instance == null)
                        Debug.LogError($"No {nameof(SO_ScreenSettings)} found in Resources");
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        #endregion

        #region Values

        [Header("Frame Rate")]

        [SerializeField] private bool _applyFrameRateLimit = false;
        public bool ApplyFrameRateLimit => _applyFrameRateLimit;

        [SerializeField] private int _frameRate = 60;
        public int FrameRate => _frameRate;


        [Header("Orientation")]

        [SerializeField] private bool _applyInitialOrientation = true;
        public bool ApplyInitialOrientation => _applyInitialOrientation;

        [SerializeField] private ScreenOrientation _initialOrientarion = ScreenOrientation.Portrait;
        public ScreenOrientation InitialOrientarion => _initialOrientarion;


        [Header("Rotation")]

        [SerializeField] private bool _applyRotationConfiguration = true;
        public bool ApplyRotationConfiguration => _applyRotationConfiguration;

        [SerializeField] private bool _autoRotatePortrait = false;
        public bool AutoRotatePortrait => _autoRotatePortrait;

        [SerializeField] private bool _autoRotatePortraitUpsideDown = false;
        public bool AutoRotatePortraitUpsideDown => _autoRotatePortraitUpsideDown;

        [SerializeField] private bool _autoRotateLandscapeLeft = false;
        public bool AutoRotateLandscapeLeft => _autoRotateLandscapeLeft;

        [SerializeField] private bool _autoRotateLandscapeRight = false;
        public bool AutoRotateLandscapeRight => _autoRotateLandscapeRight;


        [Header("Sleep / Screen Timeout")]

        [SerializeField] private bool _preventSleep = true;
        public bool PreventSleep => _preventSleep;

        #endregion
    }
}