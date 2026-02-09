using ACG.Tools.Runtime.SOCreator.Settings;
using UnityEngine;

namespace ACG.Scripts.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "PauseSettings", menuName = "ScriptableObjects/Settings/Pause")]
    public class SO_PauseSettings : SO_SettingsBase
    {
        #region Singleton

        private static SO_PauseSettings _instance;
        public static SO_PauseSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SO_PauseSettings>("Settings/PauseSettings");
                    if (_instance == null)
                        Debug.LogError($"No {typeof(SO_PauseSettings).Name} found in Resources");
                }
                return _instance;
            }
        }

        #endregion

        #region Values

        [Header("Configuration")]
        [SerializeField] private bool _autoPauseOnFocusLost;
        public bool AutoPauseOnFocusLost => _autoPauseOnFocusLost;

        #endregion
    }
}