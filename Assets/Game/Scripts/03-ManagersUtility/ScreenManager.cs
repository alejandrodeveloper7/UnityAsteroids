using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScreenManager
{
    public enum AspectRatioType
    {
        AspectRatio_4_3,
        AspectRatio_16_9,
        AspectRatio_16_10,
        AspectRatio_19_9,
        AspectRatio_20_9,
        AspectRatio_21_9,
        AspectRatioUnknown
    }

    private static List<Vector2Int> _cachedResolutions;
    public static List<Vector2Int> AvailableResolutions
    {
        get
        {
            if (_cachedResolutions == null)
            {
                _cachedResolutions = Screen.resolutions
                    .Select(res => new Vector2Int(res.width, res.height))
                    .Distinct()
                    .ToList();
            }

            return _cachedResolutions;
        }
    }

    private static List<string> _cachedAvailableResolutionsOptions;
    public static List<string> AvailableResolutionsOptions
    {
        get
        {
            if (_cachedAvailableResolutionsOptions == null)
            {
                _cachedAvailableResolutionsOptions = new List<string>();

                foreach (Vector2Int resolution in AvailableResolutions)
                    _cachedAvailableResolutionsOptions.Add(string.Format("{0}x{1}", resolution.x, resolution.y));
            }

            return _cachedAvailableResolutionsOptions;
        }
    }

    public static Vector2Int CurrentResolution => new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
    public static bool IsFullScreen => Screen.fullScreenMode == FullScreenMode.Windowed;

    public static event Action<Vector2Int> OnResolutionChanged;


    #region Functionality

    public static void SetTargetFrameRate(int pValue)
    {
        Application.targetFrameRate = pValue;
        Debug.Log("FrameRate Fixed: " + pValue);
    }

    public static void SetFullScreenMode(bool pActive)
    {
        if (pActive)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;

        Debug.Log(string.Format("Full screen mode is {0}", Screen.fullScreenMode));
    }

    public static void ToggleFullScreen()
    {
        bool isFull = Screen.fullScreenMode is not FullScreenMode.Windowed;
        SetFullScreenMode(!isFull);
    }

    public static void SetResolution(int pIndex)
    {
        if (pIndex < 0 || pIndex >= AvailableResolutions.Count)
        {
            Debug.LogWarning("Invalid resolution index");
            return;
        }

        Vector2Int resolution = AvailableResolutions[pIndex];
        Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode);
        Debug.Log(string.Format("Resolution is {0}x{1}", resolution.x, resolution.y));

        OnResolutionChanged?.Invoke(resolution);
    }

    public static void SetResolution(Vector2Int pResolution)
    {
        Screen.SetResolution(pResolution.x, pResolution.y, Screen.fullScreenMode);
        Debug.Log(string.Format("Resolution is {0}x{1}", pResolution.x, pResolution.y));

        OnResolutionChanged?.Invoke(pResolution);
    }

    #endregion
}
