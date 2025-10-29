using System;
using System.IO;
using System.Linq;
using ToolsACG.EditorTools;
using ToolsACG.MVCModulesCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.MVCModulesCreator.Creators
{
    public class MVCModuleCreator : EditorWindow
    {
        #region Fields

        private static MVCModuleCreator _window;
        private string _moduleName;
        private string _rootNamespace;
               
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
                return;
            }

            _window = EditorWindow.GetWindow<MVCModuleCreator>(_data.WindowTitle);
            _window.minSize = _data.WindowMinSize;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 20) });

            EditorBuilder.Title(_data.WindowTitle);
            EditorBuilder.TextField("Root namespace", ref _rootNamespace);
            EditorBuilder.TextField("Module name", ref _moduleName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_moduleName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateNewModule);
            GUI.enabled = true;

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_moduleName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create configuration instance", CreateConfigurationScriptableObject);
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Creation

        private void CreateNewModule()
        {
            string outputCompletePath = Path.Combine(Application.dataPath, _data.OutputPath);

            DirectoryInfo newModuleDirectory = Directory.CreateDirectory(Path.Combine(outputCompletePath, _moduleName));
            DirectoryInfo newScriptsDirectory = Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, _data.ScriptFolderName));
            DirectoryInfo newControllersDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, _data.ControllerFolderName));
            DirectoryInfo newViewsDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, _data.ViewFolderName));
            DirectoryInfo newModelssDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, _data.ModelFolderName));
            DirectoryInfo newSOsDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, _data.SOFolderName));
            DirectoryInfo newScenessDirectory = Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, _data.ScenessFolderName));

            Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, _data.DesignFolderName));
            Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, _data.PrefabsFolderName));
            Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, _data.OthersFolderName));

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string controllerTemplatePath = string.Concat(templatesCompletePath, _data.ControllerTemplateFileName);
            string controllerInterfaceTemplatePath = string.Concat(templatesCompletePath, _data.ControllerInterfaceTemplateFileName);
            string viewTemplatePath = string.Concat(templatesCompletePath, _data.ViewTemplateFileName);
            string viewInterfaceTemplatePath = string.Concat(templatesCompletePath, _data.ViewInterfaceTemplateFileName);
            string modelTemplatePath = string.Concat(templatesCompletePath, _data.ModelTemplateFileName);
            string configurationScriptableObjectTemplatePath = string.Concat(templatesCompletePath, _data.ConfigurationScriptableObjectFileName);
            string sceneTemplatePath = string.Concat(templatesCompletePath, _data.SceneTemplateFileName);

            string newControllerOutputPath = string.Concat(newControllersDirectory.FullName, "/", string.Concat(_moduleName, _data.ControllerSufix, _data.CsFileExtension));
            string newControllerInterfaceOutputPath = string.Concat(newControllersDirectory.FullName, "/", string.Concat(_data.InterfacePrefix, _moduleName, _data.ControllerSufix, _data.CsFileExtension));
            string newViewOutputPath = string.Concat(newViewsDirectory.FullName, "/", string.Concat(_moduleName, _data.ViewSufix, _data.CsFileExtension));
            string newViewInterfaceOutputPath = string.Concat(newViewsDirectory.FullName, "/", string.Concat(_data.InterfacePrefix, _moduleName, _data.ViewSufix, _data.CsFileExtension));
            string newModelOutputPath = string.Concat(newModelssDirectory.FullName, "/", string.Concat(_moduleName, _data.ModelSufix, _data.CsFileExtension));
            string newConfigurationScriptableObjectPath = string.Concat(newSOsDirectory.FullName, "/", string.Concat(_data.SOPrefix, _moduleName, _data.ConfigurationSufix, _data.CsFileExtension));
            string newSceneOutputPath = string.Concat(newScenessDirectory.FullName, "/", string.Concat(_moduleName, _data.SceneSufix, _data.SceneFileExtension));

            File.Copy(controllerTemplatePath, newControllerOutputPath);
            File.Copy(controllerInterfaceTemplatePath, newControllerInterfaceOutputPath);
            File.Copy(viewTemplatePath, newViewOutputPath);
            File.Copy(viewInterfaceTemplatePath, newViewInterfaceOutputPath);
            File.Copy(modelTemplatePath, newModelOutputPath);
            File.Copy(sceneTemplatePath, newSceneOutputPath);
            File.Copy(configurationScriptableObjectTemplatePath, newConfigurationScriptableObjectPath);

            string newControllerText = ReplaceConstantsInFile(newControllerOutputPath);
            string newControllerInterfaceText = ReplaceConstantsInFile(newControllerInterfaceOutputPath);
            string newViewText = ReplaceConstantsInFile(newViewOutputPath);
            string newViewInterfaceText = ReplaceConstantsInFile(newViewInterfaceOutputPath);
            string newModelText = ReplaceConstantsInFile(newModelOutputPath);
            string newConfigurationScriptableObjectText = ReplaceConstantsInFile(newConfigurationScriptableObjectPath);
            string newSceneText = ReplaceConstantsInFile(newSceneOutputPath);

            File.WriteAllText(newControllerOutputPath, newControllerText);
            File.WriteAllText(newControllerInterfaceOutputPath, newControllerInterfaceText);
            File.WriteAllText(newViewOutputPath, newViewText);
            File.WriteAllText(newViewInterfaceOutputPath, newViewInterfaceText);
            File.WriteAllText(newModelOutputPath, newModelText);
            File.WriteAllText(newConfigurationScriptableObjectPath, newConfigurationScriptableObjectText);
            File.WriteAllText(newSceneOutputPath, newSceneText);

            Debug.Log($"-- {_data.WindowTitle} -- Scripts of the module {_moduleName} created in {newScriptsDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private void CreateConfigurationScriptableObject()
        {
            string className = string.Concat(_data.SOPrefix, _moduleName, _data.ConfigurationSufix);
            string outputCompletePath = string.Concat(_data.AssetsPath, _data.OutputPath, _moduleName, "/");
            string fileName = string.Concat(_moduleName, _data.ConfigurationSufix, _data.AssetFileExtension);
            string path = string.Concat(outputCompletePath, fileName);

            Type scriptableObjectType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == className);

            if (scriptableObjectType == null || typeof(ScriptableObject).IsAssignableFrom(scriptableObjectType) is false)
            {
                Debug.LogError($"-- {_data.WindowTitle} -- ScriptableObject class called {className} not found");
                return;
            }

            ScriptableObject instance = ScriptableObject.CreateInstance(scriptableObjectType);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"-- {_data.WindowTitle} -- ScriptableObject {className} created in {path}");
        }

        private string ReplaceConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstModuleName, _moduleName);
            result = result.Replace(_data.ConstControllerName, string.Concat(_moduleName, _data.ControllerSufix));
            result = result.Replace(_data.ConstControllerInterfaceName, string.Concat(_data.InterfacePrefix, _moduleName, _data.ControllerSufix));
            result = result.Replace(_data.ConstViewName, string.Concat(_moduleName, _data.ViewSufix));
            result = result.Replace(_data.ConstViewInterfaceName, string.Concat(_data.InterfacePrefix, _moduleName, _data.ViewSufix));
            result = result.Replace(_data.ConstModelName, string.Concat(_moduleName, _data.ModelSufix));
            result = result.Replace(_data.ConstSOConfigurationName, string.Concat(_data.SOPrefix + _moduleName, _data.ConfigurationSufix));
            result = result.Replace(_data.ConstSOName, string.Concat(_moduleName, _data.ConfigurationSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        #endregion
    }
}