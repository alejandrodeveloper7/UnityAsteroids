using System;
using System.Threading.Tasks;
using ToolsACG.ServicesCreator.Bases;
using UnityEngine;

namespace ToolsACG.Core.Services
{
    public class CursorService : InstancesServiceBase, ICursorService
    {
        #region Fields and Properties

        public bool IsVisible => Cursor.visible;
        public CursorLockMode CurrentLockMode => Cursor.lockState;

        public event Action OnCursorBecomeVisible;
        public event Action OnCursorBecomeInvisible;

        #endregion

        #region Constructors

        public CursorService()
        {
            Initialize();
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

        #region Configuration

        public void SetCursorVisibility(bool value)
        {
            Cursor.visible = value;
            Debug.Log($"- {typeof(CursorService).Name} - Cursor is visible: {value}");

            if (value)
                OnCursorBecomeVisible?.Invoke();
            else
                OnCursorBecomeInvisible?.Invoke();
        }

        public void SetCursorLockMode(CursorLockMode state)
        {
            Cursor.lockState = state;
            Debug.Log($"- {typeof(CursorService).Name} - Cursor lockState is: {state}");
        }

        public void SetCursorTexture(Texture2D texture, Vector2 hotspot, CursorMode mode = CursorMode.Auto)
        {
            Cursor.SetCursor(texture, hotspot, mode);
        }

        #endregion

        #region Management

        public async void ForceCenterCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            await Task.Yield();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void SetGameplayCursor()
        {
            SetCursorVisibility(false);
            SetCursorLockMode(CursorLockMode.Locked);
        }

        public void SetUICursor()
        {
            SetCursorVisibility(true);
            SetCursorLockMode(CursorLockMode.None);
        }

        #endregion
    }
}