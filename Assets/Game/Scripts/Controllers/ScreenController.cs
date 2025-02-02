using System.Collections.Generic;
using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.GetUiBus().AddListener<FullScreenModeUpdated>(OnFullScreenModeupdated);
        EventManager.GetUiBus().AddListener<ResolutionUpdated>(OnResolutionUpdated);
    }

    private void OnDisable()
    {
        EventManager.GetUiBus().RemoveListener<FullScreenModeUpdated>(OnFullScreenModeupdated);
        EventManager.GetUiBus().RemoveListener<ResolutionUpdated>(OnResolutionUpdated);
    }

    private void OnFullScreenModeupdated(FullScreenModeUpdated pFullScreenModeupdated)
    {
        if (pFullScreenModeupdated.Active)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;

        Debug.Log(string.Format("Full screen mode is {0}", Screen.fullScreenMode));
    }

    private void OnResolutionUpdated(ResolutionUpdated pResolutionUpdated)
    {
        List<Vector2Int> availableResolutions = Screen.resolutions.Select(res => new Vector2Int(res.width, res.height)).Distinct().ToList();
        Screen.SetResolution(availableResolutions[pResolutionUpdated.Index].x, availableResolutions[pResolutionUpdated.Index].y,Screen.fullScreenMode);

        Debug.Log(string.Format("Resolution is {0}x{1}", Screen.currentResolution.width, Screen.currentResolution.height));
    }
}
