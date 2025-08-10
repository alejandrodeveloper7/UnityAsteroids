using UnityEngine;

[CreateAssetMenu(fileName = "GeneralSettings", menuName = "ScriptableObjects/Settings/GeneralSettings")]
public class SO_GeneralSettings : ScriptableObject
{
    [Header("Screen")]
    public bool ApplyFrameRateLimit = false;
    public int FrameRate = 60;

    [Header("Mouse")]
    public bool ApplyMouseCongifuration = true;
    public bool CursorIsVisible = true;
    public CursorLockMode CursorLockMode = CursorLockMode.None;

    [Header("TimeScale")]
    public bool ApplyTimeScale = true;
    public float TimeScale = 1;

    [Header("Debug Log")]
    public bool ApplyDebugLogConfiguration = true;
    public bool DebugLogActive = true;
}

