using ACG.Scripts.ScriptableObjects.Settings;
using ACG.Scripts.Services;
using ACG.Tools.Runtime.ManagersCreator.Bases;
using UnityEngine;
using Zenject;

namespace ACG.Scripts.Managers
{
    public class CursorManager : MonobehaviourInstancesManagerBase<CursorManager>, ICursorManager
    {
        #region Fields

        [Header("References")]
        [Inject] private readonly ICursorService _cursorService;

        [Header("Data")]
        [Inject] private readonly SO_CursorSettings _cursorSettings;

        [Header("States")]
        private bool _isClicked = false;

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

            if (_cursorSettings == null)
            {
                Debug.LogWarning("- CursorManager - Cursor configuration not set, Manager disabled");
                enabled = false;
                return;
            }

            _cursorService.OnCursorBecomeVisible += SetCursorToDefault;

            ApplyCursorInitialConfiguration();
        }

        public override void Dispose()
        {
            base.Dispose();

            _cursorService.OnCursorBecomeVisible -= SetCursorToDefault;
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

        private void Update()
        {
            if (_cursorService.IsVisible is false)
                return;

            if (Input.GetMouseButtonDown(0) && _isClicked is false)
            {
                _isClicked = true;
                _cursorService.SetCursorTexture(_cursorSettings.ClickedCursor, _cursorSettings.Hotspot);
            }
            else if (Input.GetMouseButtonUp(0) && _isClicked)
            {
                _isClicked = false;
                _cursorService.SetCursorTexture(_cursorSettings.DefaultCursor, _cursorSettings.Hotspot);
            }
        }

        #endregion

        #region Functionality

        private void ApplyCursorInitialConfiguration()
        {
            SetCursorToDefault();

            if (_cursorSettings.ApplyInitialConfiguration)
            {
                _cursorService.SetCursorVisibility(_cursorSettings.CursorInitialVisibility);
                _cursorService.SetCursorLockMode(_cursorSettings.CursorInitialLockMode);
            }
        }

        public void SetCursorToDefault()
        {
            _cursorService.SetCursorTexture(_cursorSettings.DefaultCursor, _cursorSettings.Hotspot);
        }

        public void SetCursorToClicked()
        {
            _cursorService.SetCursorTexture(_cursorSettings.ClickedCursor, _cursorSettings.Hotspot);
        }

        public void SetUICursor()
        {
            _cursorService.SetUICursor();
        }
        public void SetGameplayCursor()
        {
            _cursorService.SetGameplayCursor();
        }

        #endregion
    }
}
