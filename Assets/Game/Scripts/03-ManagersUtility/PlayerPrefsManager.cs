using UnityEngine;

public static class PlayerPrefsManager
{
    #region Set Values

    public static void SetFloat(string pKey, float pValue)
    {
        PlayerPrefs.SetFloat(pKey, pValue);
        PlayerPrefs.Save();
        Debug.Log(string.Format("- PLAYERPREFS - Saved {0} with value {1} in PlayerPrefs", pKey, pValue));
    }
    public static void SetInt(string pKey, int pValue)
    {
        PlayerPrefs.SetInt(pKey, pValue);
        PlayerPrefs.Save();
        Debug.Log(string.Format("- PLAYERPREFS - Saved {0} with value {1} in PlayerPrefs", pKey, pValue));
    }
    public static void SetString(string pKey, string pValue)
    {
        PlayerPrefs.SetString(pKey, pValue);
        PlayerPrefs.Save();
        Debug.Log(string.Format("- PLAYERPREFS - Saved {0} with value {1} in PlayerPrefs", pKey, pValue));
    }

    #endregion

    #region Get Values

    public static float GetFloat(string pKey, float pDefaultValue = 0f)
    {
        if (PlayerPrefs.HasKey(pKey))
            return PlayerPrefs.GetFloat(pKey);
        else
            return pDefaultValue;
    }
    public static int GetInt(string pKey, int pDefaultValue = 0)
    {
        if (PlayerPrefs.HasKey(pKey))
            return PlayerPrefs.GetInt(pKey);
        else
            return pDefaultValue;
    }
    public static string GetString(string pKey, string pDefaultValue = "")
    {
        if (PlayerPrefs.HasKey(pKey))
            return PlayerPrefs.GetString(pKey);
        else
            return pDefaultValue;
    }
    
    #endregion
}
