using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.Utils.PlayerPrefs
{
    public class PlayerPrefsExplorer : EditorWindow
    {
        #region Fields

        private string[] allKeys;

        #endregion

        #region MenuOptions

        [MenuItem("Tools/ToolsACG/PlayerPrefs Explorer")]
        public static void ShowWindow()
        {
            GetWindow<PlayerPrefsExplorer>("PlayerPrefs Explorer");
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            allKeys = PlayerPrefsManager.GetAllKeys();
        }

        #endregion

        #region Editor

        private Vector2 scrollPos;
        private Dictionary<string, string> editedValues = new();

        private void OnGUI()
        {
            GUILayout.Space(15);
            GUILayout.Label("🔑 PlayerPrefs Explorer", EditorStyles.boldLabel);
            GUILayout.Space(10);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);  

            if (allKeys.Length > 0)
            {
                foreach (var key in allKeys)
                {
                    if (string.IsNullOrEmpty(key))
                        continue;
                    if (UnityEngine.PlayerPrefs.HasKey(key) is false)
                        continue;

                    string value = GetPlayerPrefValue(key);

                    if (editedValues.ContainsKey(key) is false)
                        editedValues[key] = value;

                    GUILayout.BeginVertical("box");
                    GUILayout.Label(key, EditorStyles.boldLabel);

                    editedValues[key] = GUILayout.TextField(editedValues[key]);

                    GUILayout.EndVertical();

                    GUILayout.Space(4);
                }

                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("No PlayerPrefs found.");
            }
            GUILayout.Space(10);


            if (GUILayout.Button("Refresh"))
                allKeys = PlayerPrefsManager.GetAllKeys();

            if (GUILayout.Button("Clean PlayerPrefsManager"))
            {
                PlayerPrefsManager.DeleteAll();
                allKeys = PlayerPrefsManager.GetAllKeys();
            }
        }

        #endregion

        #region Utility

        private string GetPlayerPrefValue(string pKey)
        {
            if (UnityEngine.PlayerPrefs.HasKey(pKey) is false)
                return "Doesnt exist";

            int intVal = UnityEngine.PlayerPrefs.GetInt(pKey, int.MinValue);
            if (intVal != int.MinValue)
                return "int        " + intVal.ToString();

            float floatVal = UnityEngine.PlayerPrefs.GetFloat(pKey, float.MinValue);
            if (Mathf.Abs(floatVal - float.MinValue) > 0.0001f)
                return "float     " + floatVal.ToString("R", System.Globalization.CultureInfo.InvariantCulture);

            return "string   " + UnityEngine.PlayerPrefs.GetString(pKey);
        }

        #endregion
    }
}