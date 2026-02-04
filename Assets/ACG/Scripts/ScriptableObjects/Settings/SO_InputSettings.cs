using ACG.Tools.Runtime.SOCreator.Settings;
using UnityEngine;

namespace ACG.Scripts.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "InputSettings", menuName = "ScriptableObjects/Settings/Input")]
    public class SO_InputSettings : SO_SettingsBase
    {
        #region Singleton

        private static SO_InputSettings _instance;
        public static SO_InputSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SO_InputSettings>("Settings/InputSettings");
                    if (_instance == null)
                        Debug.LogError($"No {nameof(SO_InputSettings)} found in Resources");
                }
                return _instance;
            }
        }

        #endregion

        #region Values

        [Header("Controls")]

        [SerializeField] private KeyCode _turnLeftKey;
        public KeyCode TurnLeftKey => _turnLeftKey;

        [SerializeField] private KeyCode _turnRightKey;
        public KeyCode TurnRightKey => _turnRightKey;

        [Space]

        [SerializeField] private KeyCode _moveForwardKey;
        public KeyCode MoveForwardKey => _moveForwardKey;

        [Space]

        [SerializeField] private KeyCode _shootKey;
        public KeyCode ShootKey => _shootKey;

        [Space]

        [SerializeField] private KeyCode _pauseKey;
        public KeyCode PauseKey => _pauseKey;

        #endregion
    }
}