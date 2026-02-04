using ACG.Tools.Editor.Builder;
using ACG.Tools.Runtime.SOCreator.Enums;
using ACG.Tools.Runtime.SOCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.SOCreator
{
    public class SOCreatorWindow : EditorWindow
    {
        #region Fields

        [Header("References")]
        private static SOCreatorWindow _window;
        [Space]
        private string _rootNamespaceName;
        private string _scriptableName;
        [Space]
        private string _collectionSODataTypeName;

        [Header("Dropdown")]
        private int _selectedIndex = 0;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/ScriptableObject";

        [Header("Data")]
        private static SO_ScriptableObjectsCreatorConfiguration _data;

        #endregion

        #region Initialization

        [InitializeOnLoadMethod]
        private static void SetupReloadCallback()
        {
            AssemblyReloadEvents.afterAssemblyReload += LoadConfiguration;
        }

        private static void LoadConfiguration()
        {
            _data = Resources.Load<SO_ScriptableObjectsCreatorConfiguration>("ScriptableObjectsCreatorConfiguration");
        }

        #endregion

        #region Editor Window

        [MenuItem(_toolsMenuPath, false)]
        public static void OpenEditorWindow()
        {
            LoadConfiguration();

            if (_data == null)
            {
                Debug.LogWarning("SO_ScriptableObjectsCreatorConfiguration not found in Resources");
            }
            else
            {
                _window = GetWindow<SOCreatorWindow>(_data.WindowTitle);
                _window.minSize = _data.WindowMinSize;
            }
        }

        #endregion

        #region Window draw

        private void OnGUI()
        {
            DrawCommonWindowPart();

            ScriptableObjectType selectedEnum = (ScriptableObjectType)_selectedIndex;
            switch (selectedEnum)
            {
                case ScriptableObjectType.Data:
                    DrawDataWindowPart();
                    break;

                case ScriptableObjectType.Collection:
                    DrawCollectionWindowPart();
                    break;

                case ScriptableObjectType.Configuration:
                    DrawConfigurationWindowPart();
                    break;

                case ScriptableObjectType.Setting:
                    DrawSettingsWindowPart();
                    break;

                default:
                    break;
            }

            EditorBuilder.Space();
        }

        private void DrawCommonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 0) });

            EditorBuilder.Title(_data.WindowTitle);
            EditorBuilder.TextField("Root namespace", ref _rootNamespaceName);
            EditorBuilder.TextField("ScriptableObject name", ref _scriptableName);
            EditorBuilder.Dropdown<ScriptableObjectType>(ref _selectedIndex, "Type");

            EditorGUILayout.EndVertical();
        }

        private void DrawDataWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_scriptableName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create", () => SOCreator.CreateNewDataScriptableObject(_data, _scriptableName, _rootNamespaceName));

            EditorGUILayout.EndVertical();
        }

        private void DrawCollectionWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            EditorBuilder.TextField("   --> SO Type", ref _collectionSODataTypeName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_scriptableName) || string.IsNullOrEmpty(_collectionSODataTypeName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create", () => SOCreator.CreateNewCollectionScriptableObject(_data, _scriptableName, _rootNamespaceName, _collectionSODataTypeName));

            EditorGUILayout.EndVertical();
        }

        private void DrawConfigurationWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_scriptableName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create", () => SOCreator.CreateNewConfigurationScriptableObject(_data, _scriptableName, _rootNamespaceName));

            EditorGUILayout.EndVertical();
        }

        private void DrawSettingsWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_scriptableName) || string.IsNullOrEmpty(_rootNamespaceName)))
            {
                EditorBuilder.Button("Create", () => SOCreator.CreateNewSettingsScriptableObject(_data, _scriptableName, _rootNamespaceName));
                EditorBuilder.Button("Create settings instance", () => SOCreator.CreateSettingsScriptableObjectInstance(_data, _scriptableName));
            }

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}