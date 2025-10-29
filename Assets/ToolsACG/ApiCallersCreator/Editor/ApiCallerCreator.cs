using System.IO;
using ToolsACG.ApiCallersCreator.ScriptableObjects;
using ToolsACG.EditorTools;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.ApiCallersCreator.Creators
{
    public class ApiCallerCreator : EditorWindow
    {
        #region Fields

        private static ApiCallerCreator _window;
        private string _apiCallerName;
        private string _rootNamespace;

        [Header("Data")]
        private static SO_ApiCallerCreatorConfiguration _data;

        [Header("Values")]
        private const string _toolsMenuPath = "Tools/ToolsACG/Create/ApiCaller";

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
                return;
            }

            _window = EditorWindow.GetWindow<ApiCallerCreator>(_data.WindowTitle);
            _window.minSize =_data.WindowMinSize;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 20) });

            EditorBuilder.Title(_data.WindowTitle);
            EditorBuilder.TextField("Root namespace", ref _rootNamespace);
            EditorGUILayout.Space();
            EditorBuilder.TextField("ApiCaller name", ref _apiCallerName);
            EditorBuilder.HelpBox(_data.InfoMessage, MessageType.Info);

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_apiCallerName) || string.IsNullOrEmpty(_rootNamespace)))
                EditorBuilder.Button("Create", CreateNewService);

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Creation

        private void CreateNewService()
        {
            string outputCompletePath = Path.Combine(Application.dataPath, _data.OutputPath, _apiCallerName);

            DirectoryInfo GeneralDirectory = Directory.CreateDirectory(outputCompletePath);
            DirectoryInfo ApiCallersDirectory = Directory.CreateDirectory(Path.Combine(GeneralDirectory.FullName, _data.ApiCallersFolderName));
            DirectoryInfo ServiceDirectory = Directory.CreateDirectory(Path.Combine(GeneralDirectory.FullName, _data.ServicesFolderName));
            DirectoryInfo ModelsDirectory = Directory.CreateDirectory(Path.Combine(GeneralDirectory.FullName, _data.ModelsFolderName));
            DirectoryInfo ContainersDirectory = Directory.CreateDirectory(Path.Combine(GeneralDirectory.FullName, _data.ApiContainersFolderName));

            string templatesCompletePath = Path.Combine(Application.dataPath, _data.TemplatesPath);

            string ApiCallerTemplatePath = string.Concat(templatesCompletePath, _data.CallerTemplateFileName);
            string ApiCallerInterfaceTemplatePath = string.Concat(templatesCompletePath, _data.CallerInterfaceTemplateFileName);
            string ApiServiceTemplatePath = string.Concat(templatesCompletePath, _data.ServiceTemplateFileName);
            string ApiServiceInterfaceTemplatePath = string.Concat(templatesCompletePath, _data.ServiceInterfaceTemplateFileName);
            string ApiContainersTemplatePath = string.Concat(templatesCompletePath, _data.ContainerTemplateFileName);
            string modelsTemplatePath = string.Concat(templatesCompletePath, _data.ModelsTemplateFileName);
            string requestsTemplatePath = string.Concat(templatesCompletePath, _data.RequestsTemplateFileName);
            string responsesTemplatePath = string.Concat(templatesCompletePath, _data.ResponsesTemplateFileName);

            string newApiCallerOutputPath = string.Concat(ApiCallersDirectory.FullName, "/", string.Concat(_apiCallerName, _data.ApiCallerSufix, _data.CsFileExtension));
            string newApiCallerInterfaceOutputPath = string.Concat(ApiCallersDirectory.FullName, "/", string.Concat(_data.InterfacePrefix, _apiCallerName, _data.ApiCallerSufix, _data.CsFileExtension));
            string newApiServiceOutputPath = string.Concat(ServiceDirectory.FullName, "/", string.Concat(_apiCallerName, _data.ServiceSufix, _data.CsFileExtension));
            string newApiServiceInterfaceOutputPath = string.Concat(ServiceDirectory.FullName, "/", string.Concat(_data.InterfacePrefix, _apiCallerName, _data.ServiceSufix, _data.CsFileExtension));
            string newApiContainerOutputPath = string.Concat(ContainersDirectory.FullName, "/", string.Concat(_apiCallerName, _data.ContainerSufix, _data.CsFileExtension));
            string newModelsOutputPath = string.Concat(ModelsDirectory.FullName, "/", string.Concat(_apiCallerName, _data.ModelsSufix, _data.CsFileExtension));
            string newRequestsOutputPath = string.Concat(ModelsDirectory.FullName, "/", string.Concat(_apiCallerName, _data.RequestsSufix, _data.CsFileExtension));
            string newResponseOutputPath = string.Concat(ModelsDirectory.FullName, "/", string.Concat(_apiCallerName, _data.ResponsesSufix, _data.CsFileExtension));

            File.Copy(ApiCallerTemplatePath, newApiCallerOutputPath);
            File.Copy(ApiCallerInterfaceTemplatePath, newApiCallerInterfaceOutputPath);
            File.Copy(ApiServiceTemplatePath, newApiServiceOutputPath);
            File.Copy(ApiServiceInterfaceTemplatePath, newApiServiceInterfaceOutputPath);
            File.Copy(ApiContainersTemplatePath, newApiContainerOutputPath);
            File.Copy(modelsTemplatePath, newModelsOutputPath);
            File.Copy(requestsTemplatePath, newRequestsOutputPath);
            File.Copy(responsesTemplatePath, newResponseOutputPath);

            string newApiCallerText = ReplaceConstantsInFile(newApiCallerOutputPath);
            string newApiCallerInterfaceText = ReplaceConstantsInFile(newApiCallerInterfaceOutputPath);
            string newApiServiceText = ReplaceConstantsInFile(newApiServiceOutputPath);
            string newApiServiceInterfaceText = ReplaceConstantsInFile(newApiServiceInterfaceOutputPath);
            string newApiContainerText = ReplaceConstantsInFile(newApiContainerOutputPath);
            string newModelsText = ReplaceConstantsInFile(newModelsOutputPath);
            string newRequestsText = ReplaceConstantsInFile(newRequestsOutputPath);
            string newResponsesText = ReplaceConstantsInFile(newResponseOutputPath);

            File.WriteAllText(newApiCallerOutputPath, newApiCallerText);
            File.WriteAllText(newApiCallerInterfaceOutputPath, newApiCallerInterfaceText);
            File.WriteAllText(newApiServiceOutputPath, newApiServiceText);
            File.WriteAllText(newApiServiceInterfaceOutputPath, newApiServiceInterfaceText);
            File.WriteAllText(newApiContainerOutputPath, newApiContainerText);
            File.WriteAllText(newModelsOutputPath, newModelsText);
            File.WriteAllText(newRequestsOutputPath, newRequestsText);
            File.WriteAllText(newResponseOutputPath, newResponsesText);

            Debug.Log($"-- {_data.WindowTitle} -- Api Caller {_apiCallerName}ApiCaller created in {newApiCallerOutputPath}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private string ReplaceConstantsInFile(string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(_data.ConstApiCallerScriptName, string.Concat(_apiCallerName, _data.ApiCallerSufix));
            result = result.Replace(_data.ConstApiCallerInterfaceScriptName, string.Concat(_data.InterfacePrefix, _apiCallerName, _data.ApiCallerSufix));
            result = result.Replace(_data.ConstApiServiceScriptName, string.Concat(_apiCallerName, _data.ServiceSufix));
            result = result.Replace(_data.ConstApiServiceInterfaceScriptName, string.Concat(_data.InterfacePrefix, _apiCallerName, _data.ServiceSufix));
            result = result.Replace(_data.ConstApiContainerScriptName, string.Concat(_apiCallerName, _data.ContainerSufix));
            result = result.Replace(_data.ConstNamespaceName, _rootNamespace);

            return result;
        }

        #endregion
    }
}

