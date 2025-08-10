using UnityEngine;

public class GeneralConfigurationManager
{
    #region Fields

    [Header("Data")]
    private SO_GeneralSettings _generalSetting;
    private static GeneralConfigurationManager _instance;

    #endregion

    #region Initialization

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        _instance = new GeneralConfigurationManager();
        _instance.GetReferences();
        _instance.ApplyConfiguration();
    }

    private void GetReferences()
    {
        _generalSetting = ResourcesManager.Instance.GetScriptableObject<SO_GeneralSettings>(ScriptableObjectKeys.GENERAL_SETTINGS_KEY);
    }

    private void ApplyConfiguration()
    {
        if (_generalSetting.ApplyDebugLogConfiguration)
            ApplyDebugLogConfiguration();

        if (_generalSetting.ApplyFrameRateLimit)
            ApplyFrameRate();

        if (_generalSetting.ApplyMouseCongifuration)
            ApplyMouseConfiguration();

        if (_generalSetting.ApplyTimeScale)
            ApplyTimeScale();
    }

    #endregion

    #region Functionality

    private void ApplyDebugLogConfiguration()
    {
        Debug.unityLogger.logEnabled = _generalSetting.DebugLogActive;
        Debug.Log("DebugLog is active: " + _generalSetting.DebugLogActive);
    }

    private void ApplyFrameRate()
    {
        ScreenManager.SetTargetFrameRate(_generalSetting.FrameRate);
    }

    private void ApplyMouseConfiguration()
    {
        CursorManager.SetCursorVisibility(_generalSetting.CursorIsVisible);
        CursorManager.SetCursorLockMode(_generalSetting.CursorLockMode);
    }

    private void ApplyTimeScale()
    {
        Time.timeScale = _generalSetting.TimeScale;
        Debug.Log("TimeScale is: " + _generalSetting.TimeScale);
    }

    #endregion
}
