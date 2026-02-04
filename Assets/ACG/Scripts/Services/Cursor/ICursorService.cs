using System;
using UnityEngine;

namespace ACG.Scripts.Services
{ 
    public interface ICursorService
    {
        #region Properties

        bool IsVisible { get; }
        CursorLockMode CurrentLockMode { get; }

        #endregion

        #region Events

        public event Action OnCursorBecomeVisible;
        public event Action OnCursorBecomeInvisible;

        #endregion

        #region Methods

        void SetCursorVisibility(bool value);
        void SetCursorLockMode(CursorLockMode state);
        void SetCursorTexture(Texture2D texture, Vector2 hotspot, CursorMode mode = CursorMode.Auto);

        void ForceCenterCursor();

        void SetGameplayCursor();
        void SetUICursor();

        #endregion
    }
}