using System;
using System.Threading.Tasks;
using UnityEngine;

public static class CursorManager
{
    #region Properties

    public static bool IsVisible => Cursor.visible;
    public static CursorLockMode CurrentLockMode => Cursor.lockState;

    public static event Action OnCursorBecomeVisible;

    #endregion

    #region Configuration

    public static void SetCursorVisibility(bool pValue)
    {
        Cursor.visible = pValue;
        Debug.Log("Cursor is visible: " + pValue);

        if (pValue)
            OnCursorBecomeVisible?.Invoke();
    }

    public static void SetCursorLockMode(CursorLockMode pState)
    {
        Cursor.lockState = pState;
        Debug.Log("Cursor lockState is: " + pState);
    }

    public static void SetCursorTexture(Texture2D pTexture, Vector2 pHotspot, CursorMode pMode = CursorMode.Auto)
    {
        Cursor.SetCursor(pTexture, pHotspot, pMode);
    }

    #endregion

    #region Management

    public static async void ResetCursorToCenter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        await Task.Yield();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        OnCursorBecomeVisible?.Invoke();
    }

    public static void SetGameplayCursor()
    {
        SetCursorVisibility(false);
        SetCursorLockMode(CursorLockMode.Locked);
    }

    public static void SetUICursor()
    {
        SetCursorVisibility(true);
        SetCursorLockMode(CursorLockMode.None);
    }

    #endregion
}
