using ACG.Tools.Editor.Builder;
using ACG.Tools.Runtime.ManagersCreator.Enums;
using ACG.Tools.Runtime.ManagersCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.ManagersCreator
{
    public class ManagersCreatorWindow : EditorWindow
    {
        #region Fields

        [Header("References")]
        private static ManagersCreatorWindow _window;
        [Space]
        private string _managerName;
        private string _rootNamespace;

        [Header("Dropdown")]
        private ManagerType _selectedManagerKind;
        private InstancePolicy _selectedInstancePolicy;
        private InitMode _selectedInitMode;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/Manager";

        [Header("Data")]
        private static SO_ManagersCreatorConfiguration _data;

        #endregion

        #region Initialization

        [InitializeOnLoadMethod]
        private static void SetupReloadCallback()
        {
            AssemblyReloadEvents.afterAssemblyReload += LoadConfiguration;
        }

        private static void LoadConfiguration()
        {
            _data = Resources.Load<SO_ManagersCreatorConfiguration>("ManagersCreatorConfiguration");
        }

        #endregion

        #region Editor Window

        [MenuItem(_toolsMenuPath, false)]
        public static void ShowWindow()
        {
            LoadConfiguration();

            if (_data == null)
            {
                Debug.LogWarning("SO_ManagersCreatorConfiguration not found in Resources");
            }
            else
            {
                _window = GetWindow<ManagersCreatorWindow>(_data.WindowTitle);
                _window.minSize = _data.WindowMinSize;
            }
        }

        #endregion

        #region Window Draw

        private void OnGUI()
        {
            DrawCommonWindowPart();

            if (_selectedManagerKind is ManagerType.MonoBehaviour)
            {
                if (_selectedInstancePolicy is InstancePolicy.Singleton)
                {
                    if (_selectedInitMode is InitMode.Auto)
                        DrawMonobehaviourAutoSingletonWindowPart();
                    else if (_selectedInitMode is InitMode.Lazy)
                        DrawMonobehaviourLazySingletonWindowPart();
                }
                else if (_selectedInstancePolicy is InstancePolicy.Multiple_instances)
                    DrawMonobehaviourInstancesWindowPart();
            }

            if (_selectedManagerKind is ManagerType.No_Monobehaviour)
            {
                if (_selectedInstancePolicy is InstancePolicy.Singleton)
                {
                    if (_selectedInitMode is InitMode.Auto)
                        DrawNoMonobehaviourAutoSingletonWindowPart();
                    else if (_selectedInitMode is InitMode.Lazy)
                        DrawNoMonobehaviourLazySingletonWindowPart();
                }
                else if (_selectedInstancePolicy is InstancePolicy.Multiple_instances)
                    DrawNoMonobehaviourInstancesWindowPart();
            }
        }

        private void DrawCommonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 0) });

            EditorBuilder.Title(_data.WindowTitle);
            EditorBuilder.Dropdown(ref _selectedManagerKind, "Type");
            EditorBuilder.Dropdown(ref _selectedInstancePolicy, "Instance Policy");

            if (_selectedInstancePolicy is InstancePolicy.Singleton)
                EditorBuilder.Dropdown(ref _selectedInitMode, "   --> InitMode");

            EditorBuilder.TextField("Root namespace", ref _rootNamespace);
            EditorBuilder.TextField("Manager name", ref _managerName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            EditorGUILayout.EndVertical();
        }

        private void DrawMonobehaviourAutoSingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.MonoAutoSingletonMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
            {
                EditorBuilder.Button("Create", () => ManagersCreator.CreateNewMonobehaviourAutoSingletonManager(_data, _managerName, _rootNamespace));
                EditorBuilder.Button("Create Prefab", () => ManagersCreator.CreateMonobehaviourPrefab(_data, _managerName, _rootNamespace));
            }

            EditorGUILayout.EndVertical();
        }
        private void DrawMonobehaviourLazySingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.MonoLazySingletonMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
            {
                EditorBuilder.Button("Create", () => ManagersCreator.CreateNewMonobehaviourLazySingletonManager(_data, _managerName, _rootNamespace));
                EditorBuilder.Button("Create Prefab", () => ManagersCreator.CreateMonobehaviourPrefab(_data, _managerName, _rootNamespace));
            }

            EditorGUILayout.EndVertical();
        }
        private void DrawMonobehaviourInstancesWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.MonoInstancesMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
            {
                EditorBuilder.Button("Create", () => ManagersCreator.CreateNewMonobehaviourInstancesManager(_data, _managerName, _rootNamespace));
                EditorBuilder.Button("Create Prefab", () => ManagersCreator.CreateMonobehaviourPrefab(_data, _managerName, _rootNamespace));
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawNoMonobehaviourAutoSingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.NoMonoAutoSingletonMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", () => ManagersCreator.CreateNewNoMonobehaviourAutoSingletonManager(_data, _managerName, _rootNamespace));

            EditorGUILayout.EndVertical();
        }
        private void DrawNoMonobehaviourLazySingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.NoMonoLazySingletonMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", () => ManagersCreator.CreateNewNoMonobehaviourLazySingletonManager(_data, _managerName, _rootNamespace));

            EditorGUILayout.EndVertical();
        }
        private void DrawNoMonobehaviourInstancesWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.NoMonoInstancesMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", () => ManagersCreator.CreateNewNoMonobehaviourInstancesManager(_data, _managerName, _rootNamespace));

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}