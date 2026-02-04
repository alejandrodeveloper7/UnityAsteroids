using ACG.Tools.Runtime.SOCreator.Settings;
using UnityEngine;

namespace ACG.Scripts.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "CursorSettings", menuName = "ScriptableObjects/Settings/Cursor")]
    public class SO_CursorSettings : SO_SettingsBase
    {
        #region Singleton

        private static SO_CursorSettings _instance;
        public static SO_CursorSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SO_CursorSettings>("Settings/CursorSettings");
                    if (_instance == null)
                        Debug.LogError($"No {nameof(SO_CursorSettings)} found in Resources");
                }
                return _instance;
            }
        }

        #endregion

        #region Values

        [Header("InitialConfiguration")]

        [SerializeField] private bool _applyInitialConfiguration = true;
        public bool ApplyInitialConfiguration => _applyInitialConfiguration;

        [Space]

        [SerializeField] private bool _cursorInitialVisibility = true;
        public bool CursorInitialVisibility => _cursorInitialVisibility;

        [SerializeField] private CursorLockMode _cursorInitialLockMode = CursorLockMode.None;
        public CursorLockMode CursorInitialLockMode => _cursorInitialLockMode;


        [Header("Configuration")]

        [SerializeField] private Texture2D _defaultCursor;
        public Texture2D DefaultCursor => _defaultCursor;

        [SerializeField] private Texture2D _clickedCursor;
        public Texture2D ClickedCursor => _clickedCursor;

        [Space]

        [SerializeField] private Vector2 _hotSpot;
        public Vector2 Hotspot => _hotSpot;

        #endregion
    }
}