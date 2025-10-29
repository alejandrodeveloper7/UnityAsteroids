using System.Collections.Generic;
using ToolsACG.EditorTools;
using ToolsACG.PlayerPrefsExplorer.ScriptableObjects;
using ToolsACG.PlayerPrefsExplorer.Services;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.PlayerPrefsExplorer.Explorers
{
    public class PlayerPrefsExplorer : EditorWindow
    {
        #region Fields

        [Header("References")]
        private static PlayerPrefsExplorer _window;

        [Header("Data")]
        private static SO_PlayerPrefsExplorerConfiguration _data;

        [Header("Values")]
        private string[] _allKeys;
        private readonly Dictionary<string, string> _editedValues = new();
        [Space]
        private const string _windowTitle = "🔑 PlayerPrefs Explorer";
        private const string _toolsMenuPath = "Tools/ToolsACG/PlayerPrefs Explorer";
        [Space]
        private Vector2 _scrollPos;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            UpdateKeys();
        }

        #endregion

        #region Initialization

        [InitializeOnLoadMethod]
        private static void SetupReloadCallback()
        {
            AssemblyReloadEvents.afterAssemblyReload += LoadConfiguration;
        }

        private static void LoadConfiguration()
        {
            _data = Resources.Load<SO_PlayerPrefsExplorerConfiguration>("PlayerPrefsExplorerConfiguration");
        }

        #endregion

        #region Editor Window

        [MenuItem(_toolsMenuPath)]
        public static void ShowWindow()
        {
            LoadConfiguration();

            if (_data == null)
            {
                Debug.LogWarning("SO_PlayerPrefsExplorerConfiguration not found in Resources");
                return;
            }

            _window = GetWindow<PlayerPrefsExplorer>(_windowTitle);
            _window.minSize = _data.WindowMinSize;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 10) });
            EditorBuilder.Title(_windowTitle);
            GUILayout.EndVertical();

            if (_allKeys.Length > 0)
            {
                EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                DrawKeys();
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
                EditorBuilder.Label("No PlayerPrefs found.");
                GUILayout.EndVertical();
            }

            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            GUILayout.BeginHorizontal();
            EditorBuilder.Button("Refresh", () => { UpdateKeys(); });
            EditorBuilder.Button("Clean PlayerPrefsManager", () => { PlayerPrefsService.DeleteAllKeys(); UpdateKeys(); });
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void DrawKeys()
        {
            foreach (var key in _allKeys)
            {
                if (string.IsNullOrEmpty(key))
                    continue;
                if (PlayerPrefs.HasKey(key) is false)
                    continue;

                string value = GetValueFormated(key);

                if (_editedValues.ContainsKey(key) is false)
                    _editedValues[key] = value;

                GUILayout.BeginVertical("box");
                EditorBuilder.TextField(string.Format(_data.keyDisplayFormat, key, GetValueType(key)), _editedValues[key]);
                GUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }

        #endregion

        #region Utility

        private void UpdateKeys() 
        {
            _allKeys = PlayerPrefsService.GetAllKeys();
            Debug.Log($"- {_windowTitle} - {_allKeys.Length} keys found in the all keys Key");
        }

        private string GetValueType(string key)
        {
            if (PlayerPrefs.HasKey(key) is false)
                return "";

            int intVal = PlayerPrefs.GetInt(key, int.MinValue);
            if (intVal != int.MinValue)
                return "int";

            float floatVal = PlayerPrefs.GetFloat(key, float.MinValue);
            if (Mathf.Abs(floatVal - float.MinValue) > 0.0001f)
                return "float";

            return "string";
        }

        private string GetValueFormated(string key)
        {
            if (PlayerPrefs.HasKey(key) is false)
                return "Doesnt exist";

            int intVal = PlayerPrefs.GetInt(key, int.MinValue);
            if (intVal != int.MinValue)
                return intVal.ToString();

            float floatVal = PlayerPrefs.GetFloat(key, float.MinValue);
            if (Mathf.Abs(floatVal - float.MinValue) > 0.0001f)
                return floatVal.ToString("R", System.Globalization.CultureInfo.InvariantCulture);

            return PlayerPrefs.GetString(key);
        }

        #endregion
    }
}