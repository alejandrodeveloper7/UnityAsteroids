using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScreenManager
{
    public static List<Vector2Int> AvailableResolutions { get { return Screen.resolutions.Select(res => new Vector2Int(res.width, res.height)).Distinct().ToList(); }  }
    public static List<string> AvailableResolutionsOptions 
    { 
        get 
        {
            List<string> options = new List<string>();

            foreach (Vector2Int resolution in AvailableResolutions)
                options.Add(string.Format("{0}x{1}", resolution.x, resolution.y));
            return options; 
        }
    }

    #region Functionality

    public static void SetTargetFrameRate(int pValue)
    {
        Application.targetFrameRate = pValue;
    }

    public static void UpdateFullScreenMode(bool pActive)
    {
        if (pActive)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;

        Debug.Log(string.Format("- SETTINGS - Full screen mode is {0}", Screen.fullScreenMode));
    }

    public static void UpdateResolution(int pIndex)
    {
        List<Vector2Int> availableResolutions = Screen.resolutions.Select(res => new Vector2Int(res.width, res.height)).Distinct().ToList();
        Screen.SetResolution(availableResolutions[pIndex].x, availableResolutions[pIndex].y, Screen.fullScreenMode);

        Debug.Log(string.Format("- SETTINGS - Resolution is {0}x{1}", Screen.currentResolution.width, Screen.currentResolution.height));
    }
    
    #endregion
}
