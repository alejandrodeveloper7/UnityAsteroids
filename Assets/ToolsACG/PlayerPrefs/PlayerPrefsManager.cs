using UnityEngine;

namespace ToolsACG.Utils.PlayerPrefs
{
    public static class PlayerPrefsManager
    {
        #region Check Value

        public static bool HasKey(string key)
        {
            return UnityEngine.PlayerPrefs.HasKey(key);
        }

        #endregion

        #region Delete Value

        public static void DeleteKey(string pKey)
        {
            UnityEngine.PlayerPrefs.DeleteKey(pKey);

#if UNITY_EDITOR
            RemoveKeyFromAllKeys(pKey);
#endif

            UnityEngine.PlayerPrefs.Save();

#if UNITY_EDITOR
            Debug.Log(string.Format("- PLAYERPREFS - Key {0} Deleted from PlayerPrefs", pKey));
#endif
        }

        public static void DeleteAll()
        {
            UnityEngine.PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
            UnityEngine.PlayerPrefs.DeleteKey(AllKeysKey);
#endif

            UnityEngine.PlayerPrefs.Save();

#if UNITY_EDITOR
            Debug.Log(string.Format("- PLAYERPREFS - All Keys Deleted from PlayerPrefs"));
#endif
        }

        #endregion

        #region Set Values

        public static void SetFloat(string pKey, float pValue)
        {
            UnityEngine.PlayerPrefs.SetFloat(pKey, pValue);

#if UNITY_EDITOR
            AddKeyToAllKeys(pKey);
#endif

            UnityEngine.PlayerPrefs.Save();

#if UNITY_EDITOR
            Debug.Log(string.Format("- PLAYERPREFS - Saved {0} with value {1} in PlayerPrefs", pKey, pValue));
#endif
        }

        public static void SetInt(string pKey, int pValue)
        {
            UnityEngine.PlayerPrefs.SetInt(pKey, pValue);

#if UNITY_EDITOR
            AddKeyToAllKeys(pKey);
#endif

            UnityEngine.PlayerPrefs.Save();

#if UNITY_EDITOR
            Debug.Log(string.Format("- PLAYERPREFS - Saved {0} with value {1} in PlayerPrefs", pKey, pValue));
#endif
        }

        public static void SetString(string pKey, string pValue)
        {
            UnityEngine.PlayerPrefs.SetString(pKey, pValue);

#if UNITY_EDITOR
            AddKeyToAllKeys(pKey);
#endif

            UnityEngine.PlayerPrefs.Save();

#if UNITY_EDITOR
            Debug.Log(string.Format("- PLAYERPREFS - Saved {0} with value {1} in PlayerPrefs", pKey, pValue));
#endif
        }

        #endregion

        #region Get Values

        public static float GetFloat(string pKey, float pDefaultValue = 0f)
        {
            if (UnityEngine.PlayerPrefs.HasKey(pKey))
            {
                return UnityEngine.PlayerPrefs.GetFloat(pKey);
            }
            else
            {
                SetFloat(pKey, pDefaultValue);
                return pDefaultValue;
            }
        }

        public static int GetInt(string pKey, int pDefaultValue = 0)
        {
            if (UnityEngine.PlayerPrefs.HasKey(pKey))
            {
                return UnityEngine.PlayerPrefs.GetInt(pKey);
            }
            else
            {
                SetInt(pKey, pDefaultValue);
                return pDefaultValue;
            }
        }

        public static string GetString(string pKey, string pDefaultValue = "")
        {
            if (UnityEngine.PlayerPrefs.HasKey(pKey))
            {
                return UnityEngine.PlayerPrefs.GetString(pKey);
            }
            else
            {
                SetString(pKey, pDefaultValue);
                return pDefaultValue;
            }
        }

        #endregion

        #region PlayerPrefs Explorer

#if UNITY_EDITOR

        private const string AllKeysKey = "allKeys";

        private static void AddKeyToAllKeys(string pKey)
        {
            string allKeys = UnityEngine.PlayerPrefs.GetString(AllKeysKey, "");
            if (!allKeys.Contains(pKey))
            {
                allKeys += string.IsNullOrEmpty(allKeys) ? pKey : "," + pKey;
                UnityEngine.PlayerPrefs.SetString(AllKeysKey, allKeys);
            }
        }

        private static void RemoveKeyFromAllKeys(string pKey)
        {
            string allKeys = UnityEngine.PlayerPrefs.GetString(AllKeysKey, "");
            string[] keysArray = allKeys.Split(',');
            allKeys = string.Join(",", System.Array.FindAll(keysArray, k => k != pKey));
            UnityEngine.PlayerPrefs.SetString(AllKeysKey, allKeys);
        }

        public static string[] GetAllKeys()
        {
            string storedKeys = UnityEngine.PlayerPrefs.GetString(AllKeysKey, "");
            return storedKeys.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        }

#endif

        #endregion
    }
}
