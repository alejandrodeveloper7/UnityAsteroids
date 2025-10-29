using System;
using UnityEngine;

namespace ToolsACG.PlayerPrefsExplorer.Services
{
    public static class PlayerPrefsService
    {
        #region Check Values

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        #endregion

        #region Delete Values

        public static void DeleteKey(string key, bool forceSave = false)
        {
            PlayerPrefs.DeleteKey(key);
            Debug.Log($"- {typeof(PlayerPrefsService).Name} - Key {key} Deleted from PlayerPrefs");

            if (forceSave)
                PlayerPrefs.Save();

#if UNITY_EDITOR
            RemoveKeyFromAllKeys(key);
#endif
        }

        public static void DeleteAllKeys(bool forceSave = false)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"- {typeof(PlayerPrefsService).Name} - All Keys Deleted from PlayerPrefs");

            if (forceSave)
                PlayerPrefs.Save();

#if UNITY_EDITOR
            PlayerPrefs.DeleteKey(AllKeysKey);
#endif
        }

        #endregion

        #region Set Values

        public static void SetFloat(string key, float value, bool forceSave = false)
        {
            PlayerPrefs.SetFloat(key, value);
            Debug.Log($"- {typeof(PlayerPrefsService).Name} - Saved {key} with value {value} in PlayerPrefs");

            if (forceSave)
                PlayerPrefs.Save();

#if UNITY_EDITOR
            AddKeyToAllKeys(key);
#endif
        }

        public static void SetInt(string key, int value, bool forceSave = false)
        {
            PlayerPrefs.SetInt(key, value);
            Debug.Log($"- {typeof(PlayerPrefsService).Name} - Saved {key} with value {value} in PlayerPrefs");

            if (forceSave)
                PlayerPrefs.Save();

#if UNITY_EDITOR
            AddKeyToAllKeys(key);
#endif
        }

        public static void SetString(string key, string value, bool forceSave = false)
        {
            PlayerPrefs.SetString(key, value);
            Debug.Log($"- {typeof(PlayerPrefsService).Name} - Saved {key} with value {value} in PlayerPrefs");

            if (forceSave)
                PlayerPrefs.Save();

#if UNITY_EDITOR
            AddKeyToAllKeys(key);
#endif
        }

        #endregion

        #region Get Values

        public static float GetFloat(string key, float defaultValue = 0f)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetFloat(key);
            }
            else
            {
                SetFloat(key, defaultValue);
                return defaultValue;
            }
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                SetInt(key, defaultValue);
                return defaultValue;
            }
        }

        public static string GetString(string key, string defaultValue = "")
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }
            else
            {
                SetString(key, defaultValue);
                return defaultValue;
            }
        }

        #endregion

        #region PlayerPrefs Explorer

#if UNITY_EDITOR

        private const string AllKeysKey = "allKeys";

        private static void AddKeyToAllKeys(string key)
        {
            string allKeys = PlayerPrefs.GetString(AllKeysKey, "");

            if (allKeys.Contains(key) is false)
            {
                allKeys += string.IsNullOrEmpty(allKeys) ? key : "," + key;
                PlayerPrefs.SetString(AllKeysKey, allKeys);
                PlayerPrefs.Save();
            }
        }

        private static void RemoveKeyFromAllKeys(string key)
        {
            string allKeys = PlayerPrefs.GetString(AllKeysKey, "");
            string[] keysArray = allKeys.Split(',');
            allKeys = string.Join(",", Array.FindAll(keysArray, k => k != key));
            PlayerPrefs.SetString(AllKeysKey, allKeys);
            PlayerPrefs.Save();
        }

        public static string[] GetAllKeys()
        {
            string storedKeys = PlayerPrefs.GetString(AllKeysKey, "");
            string[] keys = storedKeys.Split(',', StringSplitOptions.RemoveEmptyEntries);

            return keys;
        }

#endif

        #endregion
    }
}
