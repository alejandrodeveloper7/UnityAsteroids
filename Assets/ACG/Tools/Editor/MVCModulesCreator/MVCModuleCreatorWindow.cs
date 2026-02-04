using ACG.Tools.Editor.Builder;
using ACG.Tools.Runtime.MVCModulesCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.MVCModulesCreator
{
    public class MVCModuleCreatorWindow : EditorWindow
    {
        #region Fields

        [Header("References")]
        private static MVCModuleCreatorWindow _window;
        private string _moduleName;
        private string _rootNamespaceName;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/MVC Module";

        [Header("Data")]
        private static SO_MVCModulesCreatorConfiguration _data;

        #endregion

        #region Initialization

        [InitializeOnLoadMethod]
        private static void SetupReloadCallback()
        {
            AssemblyReloadEvents.afterAssemblyReload += LoadConfiguration;
        }

        private static void LoadConfiguration()
        {
            _data = Resources.Load<SO_MVCModulesCreatorConfiguration>("MVCModulesCreatorConfiguration");
        }

        #endregion

        #region Editor Window

        [MenuItem(_toolsMenuPath, false)]
        public static void OpenEditorWindow()
        {
            LoadConfiguration();

            if (_data == null)
            {
                Debug.LogWarning("SO_MVCModulesCreatorConfiguration not found in Resources");
            }
            else
            {
                _window = GetWindow<MVCModuleCreatorWindow>(_data.WindowTitle);
                _window.minSize = _data.WindowMinSize;
            }
        }

        #endregion

        #region Editor Window

        private void OnGUI()
        {
            DrawWindow();
        }

        #endregion

        #region Window draw

        private void DrawWindow()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 20) });

            EditorBuilder.Title(_data.WindowTitle);
            EditorBuilder.TextField("Root namespace", ref _rootNamespaceName);
            EditorBuilder.TextField("Module name", ref _moduleName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_moduleName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create", () => MVCModuleCreator.CreateNewModule(_data, _moduleName, _rootNamespaceName));
            GUI.enabled = true;

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_moduleName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create configuration instance", () => MVCModuleCreator.CreateConfigurationScriptableObject(_data, _moduleName));
            GUI.enabled = true;

            EditorBuilder.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}