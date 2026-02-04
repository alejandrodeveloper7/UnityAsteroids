using ACG.Tools.Runtime.SOCreator.Settings;
using UnityEngine;

namespace ACG.Scripts.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "DebugSettings", menuName = "ScriptableObjects/Settings/Debug")]
    public class SO_DebugSettings : SO_SettingsBase
    {
        #region Singleton

        private static SO_DebugSettings _instance;
        public static SO_DebugSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SO_DebugSettings>("Settings/DebugSettings");
                    if (_instance == null)
                        Debug.LogError($"No {nameof(SO_DebugSettings)} found in Resources");
                }
                return _instance;
            }
        }

        #endregion

        #region Values

        [Header("Configuration")]

        [SerializeField] private string _topicIndicators = "---";
        public string TopicIndicators => _topicIndicators;

        [Space]

        [SerializeField] private string _warningMessage = "[WARNING]";
        public string WarningMessage => _warningMessage;

        [SerializeField] private Color _warningMessageColor;
        public Color WarningMessageColor => _warningMessageColor;

        [Space]

        [SerializeField] private string _errorMessage = "[ERROR] [ERROR] [ERROR]";
        public string ErrorMessage => _errorMessage;

        [SerializeField] private Color _errorMessageColor;
        public Color ErrorMessageColor => _errorMessageColor;

        #endregion
    }
}