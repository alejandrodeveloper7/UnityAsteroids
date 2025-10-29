using System;
using System.IO;
using ToolsACG.EditorTools;
using ToolsACG.ManagersCreator.Enums;
using ToolsACG.ManagersCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.ManagersCreator.Creators
{
    public class ManagersCreator : EditorWindow
    {
        #region Fields

        private static ManagersCreator _window;
        private string _managerName;
        private string _rootNamespace;

        [Header("Data")]
        private static SO_ManagersCreatorConfiguration _data;

        [Header("Dropdown")]
        private ManagerType _selectedManagerKind;
        private InstancePolicy _selectedInstancePolicy;
        private InitMode _selectedInitMode;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/Manager";

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
                return;
            }

            _window = GetWindow<ManagersCreator>(_data.WindowTitle);
            _window.minSize = _data.WindowMinSize;
        }

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
                EditorBuilder.Button("Create", CreateNewMonobehaviourAutoSingletonManager);
                EditorBuilder.Button("Create Prefab", CreateMonobehaviourPrefab);
            }

            EditorGUILayout.EndVertical();
        }
        private void DrawMonobehaviourLazySingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.MonoLazySingletonMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
            {
                EditorBuilder.Button("Create", CreateNewMonobehaviourLazySingletonManager);
                EditorBuilder.Button("Create Prefab", CreateMonobehaviourPrefab);
            }

            EditorGUILayout.EndVertical();
        }
        private void DrawMonobehaviourInstancesWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.MonoInstancesMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
            {
                EditorBuilder.Button("Create", CreateNewMonobehaviourInstancesManager);
                EditorBuilder.Button("Create Prefab", CreateMonobehaviourPrefab);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawNoMonobehaviourAutoSingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.NoMonoAutoSingletonMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateNewNoMonobehaviourAutoSingletonManager);

            EditorGUILayout.EndVertical();
        }
        private void DrawNoMonobehaviourLazySingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.NoMonoLazySingletonMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateNewNoMonobehaviourLazySingletonManager);

            EditorGUILayout.EndVertical();
        }
        private void DrawNoMonobehaviourInstancesWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.NoMonoInstancesMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_managerName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateNewNoMonobehaviourInstancesManager);

            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Creation

        private void CreateMonobehaviourPrefab()
        {
            string newTypeName = string.Concat(_managerName, _data.ManagerSufix);
            string prefabName = newTypeName;

            string completePrefabsPath = Path.Combine(Application.dataPath, _data.PrefabsPath);
            string prefabCompleteFilePath = string.Concat(completePrefabsPath, newTypeName, _data.PrefabFileExtension);

            GameObject go = new(prefabName);

            Type managerType = Type.GetType($"{_rootNamespace}.Core.Managers.{newTypeName}, Assembly-CSharp");

            if (managerType != null && managerType.IsSubclassOf(typeof(MonoBehaviour)))
            {
                go.AddComponent(managerType);
            }
            else
            {
                Debug.LogError($"-- {_data.WindowTitle} -- {newTypeName} script not found. Prefab not created");
                return;
            }

            GameObjectUtility.SetStaticEditorFlags(go, (StaticEditorFlags)(-1));


            string folderPath = Path.GetDirectoryName(prefabCompleteFilePath);
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }

            PrefabUtility.SaveAsPrefabAsset(go, prefabCompleteFilePath);
            GameObject.DestroyImmediate(go);

            Debug.Log($"Prefab created in {_data.PrefabsPath}{newTypeName}{_data.PrefabFileExtension}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        // Mono Singleton Auto
        private void CreateNewMonobehaviourAutoSingletonManager()
        {
            string outputCompletePathManager = Path.Combine(Application.dataPath, _data.OutputPath,$"{_managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePathManager);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, _data.MonobehaviourAutoSingletonTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, _data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_managerName, _data.ManagerSufix, _data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix, _data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceMonobehaviourAutoSingletonConstantsInFile(newManagerOutputPath);
            string newInterfaceText = ReplaceMonobehaviourAutoSingletonConstantsInFile(newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceMonobehaviourAutoSingletonConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstMonobehaviourAutoSingletonScriptName, string.Concat(_managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstInterfaceName, string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        // Mono Singleton Lazy
        private void CreateNewMonobehaviourLazySingletonManager()
        {
            string outputCompletePathManager = Path.Combine(Application.dataPath, _data.OutputPath, $"{_managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePathManager);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, _data.MonobehaviourLazySingletonTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, _data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_managerName, _data.ManagerSufix, _data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix, _data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceMonobehaviourLazySingletonConstantsInFile(newManagerOutputPath);
            string newInterfaceText = ReplaceMonobehaviourLazySingletonConstantsInFile(newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceMonobehaviourLazySingletonConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstMonobehaviourLazySingletonScriptName, string.Concat(_managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstInterfaceName, string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        // Mono MultiInstances
        private void CreateNewMonobehaviourInstancesManager()
        {
            string outputCompletePathManager = Path.Combine(Application.dataPath, _data.OutputPath, $"{_managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePathManager);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, _data.MonobehaviourInstancesTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, _data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_managerName, _data.ManagerSufix, _data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix, _data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceMonobehaviourInstancesConstantsInFile(newManagerOutputPath);
            string newInterfaceText = ReplaceMonobehaviourInstancesConstantsInFile(newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceMonobehaviourInstancesConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstMonobehaviourInstancesScriptName, string.Concat(_managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstInterfaceName, string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }


        // NoMono Singleton Auto
        private void CreateNewNoMonobehaviourAutoSingletonManager()
        {
            string outputCompletePath = Path.Combine(Application.dataPath, _data.OutputPath, $"{_managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, _data.NoMonobehaviourAutoSingletonTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, _data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_managerName, _data.ManagerSufix, _data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix, _data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceNoMonobehaviourAutoSingletonConstantsInFile(newManagerOutputPath);
            string newInterfaceText = ReplaceNoMonobehaviourAutoSingletonConstantsInFile(newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceNoMonobehaviourAutoSingletonConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstNoMonobehaviourAutoSingletonScriptName, string.Concat(_managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstInterfaceName, string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        // NoMono Singleton Lazy
        private void CreateNewNoMonobehaviourLazySingletonManager()
        {
            string outputCompletePath = Path.Combine(Application.dataPath, _data.OutputPath, $"{_managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, _data.NoMonobehaviourLazySingletonTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, _data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_managerName, _data.ManagerSufix, _data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix, _data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceNoMonobehaviourLazySingletonConstantsInFile(newManagerOutputPath);
            string newInterfaceText = ReplaceNoMonobehaviourLazySingletonConstantsInFile(newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceNoMonobehaviourLazySingletonConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstNoMonobehaviourLazySingletonScriptName, string.Concat(_managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstInterfaceName, string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        // NoMono MultiInstances
        private void CreateNewNoMonobehaviourInstancesManager()
        {
            string outputCompletePath = Path.Combine(Application.dataPath, _data.OutputPath, $"{_managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, _data.NoMonobehaviourInstancesTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, _data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_managerName, _data.ManagerSufix, _data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix, _data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceNoMonobehaviourInstancesConstantsInFile(newManagerOutputPath);
            string newInterfaceText = ReplaceNoMonobehaviourInstancesConstantsInFile(newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceNoMonobehaviourInstancesConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstNoMonobehaviourInstancesScriptName, string.Concat(_managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstInterfaceName, string.Concat(_data.InterfaceSufix, _managerName, _data.ManagerSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        #endregion
    }
}