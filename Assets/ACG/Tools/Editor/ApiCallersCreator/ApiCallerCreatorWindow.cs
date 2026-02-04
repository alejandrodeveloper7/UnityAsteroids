using ACG.Tools.Editor.Builder;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.ApiCallersCreator
{
    public class ApiCallerCreatorWindow : EditorWindow
    {
        #region Fields

        [Header("References")]
        private static ApiCallerCreatorWindow _window;
        private string _apiCallerName;
        private string _rootNamespace;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/ApiCaller";

        [Header("Data")]
        private static SO_ApiCallerCreatorConfiguration _data;

        #endregion

        #region Initialization

        [InitializeOnLoadMethod]
        private static void SetupReloadCallback()
        {
            AssemblyReloadEvents.afterAssemblyReload += LoadConfiguration;
        }

        private static void LoadConfiguration()
        {
            _data = Resources.Load<SO_ApiCallerCreatorConfiguration>("ApiCallerCreatorConfiguration");
        }

        #endregion

        #region Editor Window

        [MenuItem(_toolsMenuPath, false)]
        public static void OpenEditorWindow()
        {
            LoadConfiguration();

            if (_data == null)
            {
                Debug.LogWarning("SO_ApiCallerCreatorConfiguration not found in Resources");
            }
            else
            {
                _window = GetWindow<ApiCallerCreatorWindow>(_data.WindowTitle);
                _window.minSize = _data.WindowMinSize;
            }
        }

        #endregion

        #region Window draw

        private void OnGUI()
        {
            DrawWindow();
        }

        private void DrawWindow()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 20) });

            EditorBuilder.Title(_data.WindowTitle);
            EditorBuilder.TextField("Root namespace", ref _rootNamespace);
            EditorBuilder.Space();
            EditorBuilder.TextField("ApiCaller name", ref _apiCallerName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            EditorBuilder.Space();

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_apiCallerName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", () => ApiCallerCreator.CreateNewService(_data, _apiCallerName, _rootNamespace));

            EditorBuilder.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

    }
}