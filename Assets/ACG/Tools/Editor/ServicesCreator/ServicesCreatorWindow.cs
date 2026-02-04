using ACG.Tools.Editor.Builder;
using ACG.Tools.Runtime.ServicesCreator.Enums;
using ACG.Tools.Runtime.ServicesCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.ServicesCreator
{
    public class ServicesCreatorWindow : EditorWindow
    {
        #region Fields

        [Header("References")]
        private static ServicesCreatorWindow _window;
        private string _serviceName;
        private string _rootNamespaceName;

        [Header("Dropdown")]
        private InstancePolicy _selectedInstancePolicy;
        private InitMode _selectedInitMode;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/Service";
        
        [Header("Data")]
        private static SO_ServicesCreatorConfiguration _data;

        #endregion

        #region Initialization

        [InitializeOnLoadMethod]
        private static void SetupReloadCallback()
        {
            AssemblyReloadEvents.afterAssemblyReload += LoadConfiguration;
        }

        private static void LoadConfiguration()
        {
            _data = Resources.Load<SO_ServicesCreatorConfiguration>("ServicesCreatorConfiguration");
        }

        #endregion

        #region Editor window

        [MenuItem(_toolsMenuPath, false)]
        public static void ShowWindow()
        {
            LoadConfiguration();

            if (_data == null)
            {
                Debug.LogWarning("SO_ServicesCreatorConfiguration not found in Resources");
            }
            else
            {
                _window = GetWindow<ServicesCreatorWindow>(_data.WindowTitle);
                _window.minSize = _data.WindowMinSize;
            }
        }

        #endregion

        #region Window draw

        private void OnGUI()
        {
            DrawCommonWindowPart();

            switch (_selectedInstancePolicy)
            {
                case InstancePolicy.Static:
                    DrawStaticServiceWindowPart();
                    break;


                case InstancePolicy.Singleton:

                    if (_selectedInitMode is InitMode.Auto)
                        DrawAutoSingletonWindowPart();
                    else if (_selectedInitMode is InitMode.Lazy)
                        DrawLazySingletonWindowPart();
                    break;


                case InstancePolicy.Multiple_instances:
                    DrawMultiInstancesServiceWindowPart();
                    break;


                default:
                    break;
            }
        }

        private void DrawCommonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 0) });

            EditorBuilder.Title(_data.WindowTitle);
            EditorBuilder.Dropdown(ref _selectedInstancePolicy, "Type");

            if (_selectedInstancePolicy is InstancePolicy.Singleton)
                EditorBuilder.Dropdown(ref _selectedInitMode, "   --> InitMode");

            EditorBuilder.TextField("Root namespace", ref _rootNamespaceName);
            EditorBuilder.TextField("Service name", ref _serviceName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            EditorGUILayout.EndVertical();
        }

        private void DrawStaticServiceWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.staticServiceInfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_serviceName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create", () => ServicesCreator.CreateStaticService(_data, _serviceName, _rootNamespaceName));

            EditorGUILayout.EndVertical();
        }
        private void DrawAutoSingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.AutoSingletonServiceInfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_serviceName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create", () => ServicesCreator.CreateAutoSingletonService(_data, _serviceName, _rootNamespaceName));

            EditorGUILayout.EndVertical();
        }
        private void DrawLazySingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.LazySingletonServiceInfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_serviceName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create", () => ServicesCreator.CreateLazySingletonService(_data, _serviceName, _rootNamespaceName));

            EditorGUILayout.EndVertical();
        }
        private void DrawMultiInstancesServiceWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.InstanceServiceInfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_serviceName) || string.IsNullOrEmpty(_rootNamespaceName)))
                EditorBuilder.Button("Create", () => ServicesCreator.CreateMultiInstancesService(_data, _serviceName, _rootNamespaceName));

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}