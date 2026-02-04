using ACG.Tools.Editor.Builder;
using System.Collections.Generic;
using ACG.Tools.Runtime.PlayerPrefsExplorer.ScriptableObjects;
using ACG.Tools.Runtime.PlayerPrefsExplorer.Services;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.PlayerPrefsExplorer.Explorers
{
    public class PlayerPrefsExplorerWindow : EditorWindow
    {
        #region Fields

        [Header("References")]
        private static PlayerPrefsExplorerWindow _window;

        [Header("Values")]
        private string[] _allKeys;
        private readonly Dictionary<string, string> _editedValues = new();
        [Space]
        private const string _toolsMenuPath = "Tools/ToolsACG/PlayerPrefs Explorer";
        [Space]
        private Vector2 _scrollPos;

        [Header("Data")]
        private static SO_PlayerPrefsExplorerConfiguration _data;

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
            }
            else
            {
                _window = GetWindow<PlayerPrefsExplorerWindow>(_data.WindowTitle);
                _window.minSize = _data.WindowMinSize;
            }
        }

        #endregion

        #region Window draw

        private void OnGUI()
        {
            EditorBuilder.Space(15);
            if (_allKeys.Length > 0)
            {
                EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

                GUILayout.BeginHorizontal();
                GUIStyle boldStyle = new(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 13
                };

                GUILayout.Label("KEY NAME", boldStyle, GUILayout.Width(200));
                GUILayout.Label("VALUE TYPE", boldStyle, GUILayout.Width(140));
                GUILayout.Label("VALUE", boldStyle, GUILayout.Width(140));
                GUILayout.EndHorizontal();

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

            EditorBuilder.Button("Refresh", () => { UpdateKeys(); });
            EditorBuilder.Button("Clean PlayerPrefsManager", () => { PlayerPrefsService.DeleteAllKeys(); UpdateKeys(); });

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
                GUILayout.BeginHorizontal();
                GUILayout.Label(key, GUILayout.Width(200));
                GUILayout.Label(GetValueType(key), GUILayout.Width(140));
                GUILayout.Label(_editedValues[key], GUILayout.Width(140));
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                EditorBuilder.Space();
            }
        }

        #endregion

        #region Utility

        private void UpdateKeys()
        {
            _allKeys = PlayerPrefsService.GetAllKeys();
            Debug.Log($"- {_data.WindowTitle} - {_allKeys.Length} keys found in the all keys Key");
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