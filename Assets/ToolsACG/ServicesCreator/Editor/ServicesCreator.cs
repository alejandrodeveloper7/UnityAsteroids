using System.IO;
using ToolsACG.EditorTools;
using ToolsACG.ServicesCreator.Enums;
using ToolsACG.ServicesCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.ServicesCreator.Creators
{
    public class ServicesCreator : EditorWindow
    {
        #region Fields

        private static ServicesCreator _window;
        private string _serviceName;
        private string _rootNamespace;

        [Header("Data")]
        private static SO_ServicesCreatorConfiguration _data;

        [Header("Dropdown")]
        private InstancePolicy _selectedInstancePolicy;
        private InitMode _selectedInitMode;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/Service";

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

        #region Window

        [MenuItem(_toolsMenuPath, false)]
        public static void ShowWindow()
        {
            LoadConfiguration();

            if (_data == null)
            {
                Debug.LogWarning("SO_ServicesCreatorConfiguration not found in Resources");
                return;
            }

            _window = GetWindow<ServicesCreator>(_data.WindowTitle);
            _window.minSize = _data.WindowMinSize;
        }

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
                    else
                    { }

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

            EditorBuilder.TextField("Root namespace", ref _rootNamespace);
            EditorBuilder.TextField("Service name", ref _serviceName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            EditorGUILayout.EndVertical();
        }

        private void DrawStaticServiceWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.staticServiceInfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_serviceName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateStaticService);

            EditorGUILayout.EndVertical();
        }
        private void DrawAutoSingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.AutoSingletonServiceInfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_serviceName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateAutoSingletonService);

            EditorGUILayout.EndVertical();
        }
        private void DrawLazySingletonWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.LazySingletonServiceInfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_serviceName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateLazySingletonService);

            EditorGUILayout.EndVertical();
        }
        private void DrawMultiInstancesServiceWindowPart()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 0, 20) });
            EditorBuilder.HelpBox(_data.InstanceServiceInfoMessage, MessageType.Info);

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_serviceName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateMultiInstancesService);

            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Creation

        // Static
        private void CreateStaticService()
        {
            string outputCompletePathService = Path.Combine(Application.dataPath, _data.OutputPath, $"{_serviceName}/");

            DirectoryInfo newServiceDirectory = Directory.CreateDirectory(outputCompletePathService);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string serviceTemplatePath = string.Concat(templatesCompletePath, _data.StaticServiceTemplateFileName);

            string newServiceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(_serviceName, _data.ServiceSufix, _data.CsFileExtension));

            File.Copy(serviceTemplatePath, newServiceOutputPath);

            string newServiceText = ReplaceStaticServiceConstantsInFile(newServiceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_serviceName}Service created in {newServiceDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceStaticServiceConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstStaticServiceScriptName, string.Concat(_serviceName, _data.ServiceSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        // Auto Singleton
        private void CreateAutoSingletonService()
        {
            string outputCompletePathService = Path.Combine(Application.dataPath, _data.OutputPath, $"{_serviceName}/");

            DirectoryInfo newServiceDirectory = Directory.CreateDirectory(outputCompletePathService);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string serviceTemplatePath = string.Concat(templatesCompletePath, _data.AutoSingletonTemplateFileName);
            string serviceInterfaceTemplatePath = string.Concat(templatesCompletePath, _data.ServiceInterfaceTemplateFileName);

            string newServiceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(_serviceName, _data.ServiceSufix, _data.CsFileExtension));
            string newServiceInterfaceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(_data.InterfacePrefix, _serviceName, _data.ServiceSufix, _data.CsFileExtension));

            File.Copy(serviceTemplatePath, newServiceOutputPath);
            File.Copy(serviceInterfaceTemplatePath, newServiceInterfaceOutputPath);

            string newServiceText = ReplaceAutoSingletonServiceConstantsInFile(newServiceOutputPath);
            string newServiceInterfaceText = ReplaceAutoSingletonServiceConstantsInFile(newServiceInterfaceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);
            File.WriteAllText(newServiceInterfaceOutputPath, newServiceInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_serviceName}Service created in {newServiceDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceAutoSingletonServiceConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstAutoSingletonServiceScriptName, string.Concat(_serviceName, _data.ServiceSufix));
            result = result.Replace(_data.ConstServiceInterfaceScriptName, string.Concat(_data.InterfacePrefix, _serviceName, _data.ServiceSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        // Lazy Singleton
        private void CreateLazySingletonService()
        {
            string outputCompletePathService = Path.Combine(Application.dataPath, _data.OutputPath, $"{_serviceName}/");

            DirectoryInfo newServiceDirectory = Directory.CreateDirectory(outputCompletePathService);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string serviceTemplatePath = string.Concat(templatesCompletePath, _data.LazySingletonTemplateFileName);
            string serviceInterfaceTemplatePath = string.Concat(templatesCompletePath, _data.ServiceInterfaceTemplateFileName);

            string newServiceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(_serviceName, _data.ServiceSufix, _data.CsFileExtension));
            string newServiceInterfaceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(_data.InterfacePrefix, _serviceName, _data.ServiceSufix, _data.CsFileExtension));

            File.Copy(serviceTemplatePath, newServiceOutputPath);
            File.Copy(serviceInterfaceTemplatePath, newServiceInterfaceOutputPath);

            string newServiceText = ReplaceLazySingletonServiceConstantsInFile(newServiceOutputPath);
            string newServiceInterfaceText = ReplaceLazySingletonServiceConstantsInFile(newServiceInterfaceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);
            File.WriteAllText(newServiceInterfaceOutputPath, newServiceInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_serviceName}Service created in {newServiceDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceLazySingletonServiceConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstLazySingletonServiceScriptName, string.Concat(_serviceName, _data.ServiceSufix));
            result = result.Replace(_data.ConstServiceInterfaceScriptName, string.Concat(_data.InterfacePrefix, _serviceName, _data.ServiceSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        // MultiInstances
        private void CreateMultiInstancesService()
        {
            string outputCompletePathService = Path.Combine(Application.dataPath, _data.OutputPath, $"{_serviceName}/");

            DirectoryInfo newServiceDirectory = Directory.CreateDirectory(outputCompletePathService);

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string serviceTemplatePath = string.Concat(templatesCompletePath, _data.InstancesServiceTemplateFileName);
            string serviceInterfaceTemplatePath = string.Concat(templatesCompletePath, _data.ServiceInterfaceTemplateFileName);

            string newServiceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(_serviceName, _data.ServiceSufix, _data.CsFileExtension));
            string newServiceInterfaceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(_data.InterfacePrefix, _serviceName, _data.ServiceSufix, _data.CsFileExtension));

            File.Copy(serviceTemplatePath, newServiceOutputPath);
            File.Copy(serviceInterfaceTemplatePath, newServiceInterfaceOutputPath);

            string newServiceText = ReplaceMultiInstancesServiceConstantsInFile(newServiceOutputPath);
            string newServiceInterfaceText = ReplaceMultiInstancesServiceConstantsInFile(newServiceInterfaceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);
            File.WriteAllText(newServiceInterfaceOutputPath, newServiceInterfaceText);

            Debug.Log($"-- {_data.WindowTitle} -- {_serviceName}Service created in {newServiceDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
        private string ReplaceMultiInstancesServiceConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstInstancesServiceScriptName, string.Concat(_serviceName, _data.ServiceSufix));
            result = result.Replace(_data.ConstServiceInterfaceScriptName, string.Concat(_data.InterfacePrefix, _serviceName, _data.ServiceSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        #endregion
    }
}