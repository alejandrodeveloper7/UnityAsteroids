using System;
using System.IO;
using System.Linq;
using ToolsACG.EditorTools;
using ToolsACG.SOCreator.Enums;
using ToolsACG.SOCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.SOCreator.Creators
{
    public class SOCreator : EditorWindow
    {
        #region Fields

        [Header("References")]
        private static SOCreator _window;
        private string _rootNamespace;
        private string _scriptableName;
        [Space]
        private string _collectionSODataTypeName;

        [Header("Data")]
        private static SO_ScriptableObjectsCreatorConfiguration _data;

        [Header("Dropdown")]
        private int _selectedIndex = 0;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/ScriptableObject";

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
                return;
            }

            _window = EditorWindow.GetWindow<SOCreator>(_data.WindowTitle);
            _window.minSize = _data.WindowMinSize;
        }

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

            EditorGUILayout.Space();
        }

        private void DrawCommonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 0) });

            EditorBuilder.Title(_data.WindowTitle);
            EditorBuilder.TextField("Root namespace", ref _rootNamespace);
            EditorBuilder.TextField("ScriptableObject name", ref _scriptableName);
            EditorBuilder.Dropdown<ScriptableObjectType>(ref _selectedIndex, "Type");

            EditorGUILayout.EndVertical();
        }

        private void DrawDataWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_scriptableName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateNewDataScriptableObject);

            EditorGUILayout.EndVertical();
        }

        private void DrawCollectionWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            EditorBuilder.TextField("   --> SO Type", ref _collectionSODataTypeName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_scriptableName) || string.IsNullOrEmpty(_collectionSODataTypeName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateNewCollectionScriptableObject);

            EditorGUILayout.EndVertical();
        }

        private void DrawConfigurationWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_scriptableName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateNewConfigurationScriptableObject);

            EditorGUILayout.EndVertical();
        }

        private void DrawSettingsWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });

            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_scriptableName) || string.IsNullOrEmpty(_rootNamespace)))
            {
                EditorBuilder.Button("Create", CreateNewSettingsScriptableObject);
                EditorBuilder.Button("Create settings instance", CreateSettingsScriptableObjectInstance);
            }

            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Creation

        //Collection
        private void CreateNewCollectionScriptableObject()
        {
            string newTypeName = string.Concat(_data.SOPrefix, _scriptableName, _data.CollectionSufix);
            string newInstanceName = string.Concat(_scriptableName, _data.CollectionSufix);
            string outputCompletePath = Path.Combine(Application.dataPath, _data.CollectionsOutputPath);

            DirectoryInfo newScriptableDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string scriptableTemplatePath = string.Concat(templatesCompletePath, _data.CollectionTemplateFileName);

            string newScriptableOutputPath = string.Concat(newScriptableDirectory.FullName, "/", _data.CollectionTemplateFileName.Replace(_data.ConstCollectionsScriptName, newTypeName).Replace(_data.TemplateFileExtension, string.Empty));

            File.Copy(scriptableTemplatePath, newScriptableOutputPath);

            string newScriptableText = ReplaceCollectionConstantsInFile(newScriptableOutputPath);

            File.WriteAllText(newScriptableOutputPath, newScriptableText);

            Debug.Log(string.Format($"-- {_data.WindowTitle} -- {_scriptableName} collection scriptable created in {newScriptableDirectory.FullName}"));
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceCollectionConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstCollectionsScriptName, string.Concat(_data.SOPrefix, _scriptableName, _data.CollectionSufix));
            result = result.Replace(_data.ConstNewInstanceName, string.Concat(_scriptableName, _data.CollectionSufix));
            result = result.Replace(_data.ConstName, _scriptableName);
            result = result.Replace(_data.ConstSOTypeName, string.Concat(_data.SOPrefix, _collectionSODataTypeName, _data.DataSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }


        //Settings
        private void CreateNewSettingsScriptableObject()
        {
            string newTypeName = string.Concat(_data.SOPrefix, _scriptableName, _data.SettingsSufix);
            string newInstanceName = string.Concat(_scriptableName, _data.SettingsSufix);
            string outputCompletePath = Path.Combine(Application.dataPath, _data.SettingsOutputPath);

            DirectoryInfo newScriptableDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string scriptableTemplatePath = string.Concat(templatesCompletePath, _data.SettingsTemplateFileName);

            string newScriptableOutputPath = string.Concat(newScriptableDirectory.FullName, "/", _data.SettingsTemplateFileName.Replace(_data.ConstSettingsScriptName, newTypeName).Replace(_data.TemplateFileExtension, string.Empty));

            File.Copy(scriptableTemplatePath, newScriptableOutputPath);

            string newScriptableText = ReplaceSettingsConstantsInFile(newScriptableOutputPath);

            File.WriteAllText(newScriptableOutputPath, newScriptableText);

            Debug.Log($"-- {_data.WindowTitle} -- {_scriptableName} Settings scriptable created in {newScriptableDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private void CreateSettingsScriptableObjectInstance()
        {
            string newTypeName = string.Concat(_data.SOPrefix, _scriptableName, _data.SettingsSufix);
            string outputCompletePath = string.Concat(_data.AssetsPath, _data.SettingsInstanceOutputPath);
            string fileName = string.Concat(_scriptableName, _data.SettingsSufix, _data.AssetFileExtension);
            string path = string.Concat(outputCompletePath, fileName);

            Type scriptableObjectType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == newTypeName);

            if (scriptableObjectType == null || typeof(ScriptableObject).IsAssignableFrom(scriptableObjectType) is false)
            {
                Debug.LogError($"-- {_data.WindowTitle} -- Settings instance class called {newTypeName} not found");
                return;
            }

            ScriptableObject instance = ScriptableObject.CreateInstance(scriptableObjectType);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"-- {_data.WindowTitle} -- Settings instance {newTypeName} created in {path}");
        }
        private string ReplaceSettingsConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstSettingsScriptName, string.Concat(_data.SOPrefix, _scriptableName, _data.SettingsSufix));
            result = result.Replace(_data.ConstNewInstanceName, string.Concat(_scriptableName, _data.SettingsSufix));
            result = result.Replace(_data.ConstName, _scriptableName);
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }


        //Data
        private void CreateNewDataScriptableObject()
        {
            string newTypeName = string.Concat(_data.SOPrefix, _scriptableName, _data.DataSufix);
            string newInstanceName = string.Concat(_scriptableName, _data.DataSufix);
            string outputCompletePath = Path.Combine(Application.dataPath, _data.DataOutputPath);

            DirectoryInfo newScriptableDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string scriptableTemplatePath = string.Concat(templatesCompletePath, _data.DataTemplateFileName);

            string newScriptableOutputPath = string.Concat(newScriptableDirectory.FullName, "/", _data.DataTemplateFileName.Replace(_data.ConstDataScriptName, newTypeName).Replace(_data.TemplateFileExtension, string.Empty));

            File.Copy(scriptableTemplatePath, newScriptableOutputPath);

            string newScriptableText = ReplaceDataConstantsInFile(newScriptableOutputPath);

            File.WriteAllText(newScriptableOutputPath, newScriptableText);

            Debug.Log(string.Format($"-- {_data.WindowTitle} -- {_scriptableName} Data scriptable created in {newScriptableDirectory.FullName}"));
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceDataConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstDataScriptName, string.Concat(_data.SOPrefix, _scriptableName, _data.DataSufix));
            result = result.Replace(_data.ConstNewInstanceName, string.Concat(_scriptableName, _data.DataSufix));
            result = result.Replace(_data.ConstName, _scriptableName);
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        //Configuration
        private void CreateNewConfigurationScriptableObject()
        {
            string newTypeName = string.Concat(_data.SOPrefix, _scriptableName, _data.ConfigurationSufix);

            string outputCompletePath = Path.Combine(Application.dataPath, _data.ConfigurationOutputPath);

            DirectoryInfo newScriptableDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string scriptableTemplatePath = string.Concat(templatesCompletePath, _data.ConfigurationTemplateFileName);

            string newScriptableOutputPath = string.Concat(newScriptableDirectory.FullName, "/", _data.ConfigurationTemplateFileName.Replace(_data.ConfigurationConstScriptName, newTypeName).Replace(_data.TemplateFileExtension, string.Empty));

            File.Copy(scriptableTemplatePath, newScriptableOutputPath);

            string newScriptableText = ReplaceConfigurationConstantsInFile(newScriptableOutputPath);

            File.WriteAllText(newScriptableOutputPath, newScriptableText);

            Debug.Log(string.Format($"-- {_data.WindowTitle} -- {_scriptableName} Configuration scriptable created in {newScriptableDirectory.FullName}"));
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceConfigurationConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConfigurationConstScriptName, string.Concat(_data.SOPrefix, _scriptableName, _data.ConfigurationSufix));
            result = result.Replace(_data.ConstNewInstanceName, string.Concat(_scriptableName, _data.ConfigurationSufix));
            result = result.Replace(_data.ConstName, _scriptableName);
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }
        #endregion
    }
}