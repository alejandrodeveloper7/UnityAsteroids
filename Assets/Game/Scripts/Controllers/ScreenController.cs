using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScreenController
{
    public static void FixFrameRate(int pValue)
    {
        Application.targetFrameRate = pValue;
    }

    public static void UpdateFullScreenMode(bool pActive)
    {
        if (pActive)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;

        Debug.Log(string.Format("Full screen mode is {0}", Screen.fullScreenMode));
    }

    public static void UpdateResolution(int pIndex)
    {
        List<Vector2Int> availableResolutions = Screen.resolutions.Select(res => new Vector2Int(res.width, res.height)).Distinct().ToList();
        Screen.SetResolution(availableResolutions[pIndex].x, availableResolutions[pIndex].y, Screen.fullScreenMode);

        Debug.Log(string.Format("Resolution is {0}x{1}", Screen.currentResolution.width, Screen.currentResolution.height));
    }
}
