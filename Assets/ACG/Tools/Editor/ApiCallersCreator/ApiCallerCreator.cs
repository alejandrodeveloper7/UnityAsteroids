using System.IO;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.ApiCallersCreator
{
    public static class ApiCallerCreator
    {
        #region Api Caller Creation

        public static void CreateNewService(SO_ApiCallerCreatorConfiguration data, string apiCallerName, string rootNamespaceName)
        {
            string outputCompletePath = Path.Combine(Application.dataPath, data.OutputPath, apiCallerName);

            DirectoryInfo GeneralDirectory = Directory.CreateDirectory(outputCompletePath);
            DirectoryInfo ApiCallersDirectory = Directory.CreateDirectory(Path.Combine(GeneralDirectory.FullName, data.ApiCallersFolderName));
            DirectoryInfo ServiceDirectory = Directory.CreateDirectory(Path.Combine(GeneralDirectory.FullName, data.ServicesFolderName));
            DirectoryInfo ModelsDirectory = Directory.CreateDirectory(Path.Combine(GeneralDirectory.FullName, data.ModelsFolderName));
            DirectoryInfo ContainersDirectory = Directory.CreateDirectory(Path.Combine(GeneralDirectory.FullName, data.ApiContainersFolderName));

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string ApiCallerTemplatePath = string.Concat(templatesCompletePath, data.CallerTemplateFileName);
            string ApiCallerInterfaceTemplatePath = string.Concat(templatesCompletePath, data.CallerInterfaceTemplateFileName);
            string ApiServiceTemplatePath = string.Concat(templatesCompletePath, data.ServiceTemplateFileName);
            string ApiServiceInterfaceTemplatePath = string.Concat(templatesCompletePath, data.ServiceInterfaceTemplateFileName);
            string ApiContainersTemplatePath = string.Concat(templatesCompletePath, data.ContainerTemplateFileName);
            string modelsTemplatePath = string.Concat(templatesCompletePath, data.ModelsTemplateFileName);
            string requestsTemplatePath = string.Concat(templatesCompletePath, data.RequestsTemplateFileName);
            string responsesTemplatePath = string.Concat(templatesCompletePath, data.ResponsesTemplateFileName);

            string newApiCallerOutputPath = string.Concat(ApiCallersDirectory.FullName, "/", string.Concat(apiCallerName, data.ApiCallerSufix, data.CsFileExtension));
            string newApiCallerInterfaceOutputPath = string.Concat(ApiCallersDirectory.FullName, "/", string.Concat(data.InterfacePrefix, apiCallerName, data.ApiCallerSufix, data.CsFileExtension));
            string newApiServiceOutputPath = string.Concat(ServiceDirectory.FullName, "/", string.Concat(apiCallerName, data.ServiceSufix, data.CsFileExtension));
            string newApiServiceInterfaceOutputPath = string.Concat(ServiceDirectory.FullName, "/", string.Concat(data.InterfacePrefix, apiCallerName, data.ServiceSufix, data.CsFileExtension));
            string newApiContainerOutputPath = string.Concat(ContainersDirectory.FullName, "/", string.Concat(apiCallerName, data.ContainerSufix, data.CsFileExtension));
            string newModelsOutputPath = string.Concat(ModelsDirectory.FullName, "/", string.Concat(apiCallerName, data.ModelsSufix, data.CsFileExtension));
            string newRequestsOutputPath = string.Concat(ModelsDirectory.FullName, "/", string.Concat(apiCallerName, data.RequestsSufix, data.CsFileExtension));
            string newResponseOutputPath = string.Concat(ModelsDirectory.FullName, "/", string.Concat(apiCallerName, data.ResponsesSufix, data.CsFileExtension));

            File.Copy(ApiCallerTemplatePath, newApiCallerOutputPath);
            File.Copy(ApiCallerInterfaceTemplatePath, newApiCallerInterfaceOutputPath);
            File.Copy(ApiServiceTemplatePath, newApiServiceOutputPath);
            File.Copy(ApiServiceInterfaceTemplatePath, newApiServiceInterfaceOutputPath);
            File.Copy(ApiContainersTemplatePath, newApiContainerOutputPath);
            File.Copy(modelsTemplatePath, newModelsOutputPath);
            File.Copy(requestsTemplatePath, newRequestsOutputPath);
            File.Copy(responsesTemplatePath, newResponseOutputPath);

            string newApiCallerText = ReplaceConstantsInFile(data, apiCallerName, rootNamespaceName, newApiCallerOutputPath);
            string newApiCallerInterfaceText = ReplaceConstantsInFile(data, apiCallerName, rootNamespaceName, newApiCallerInterfaceOutputPath);
            string newApiServiceText = ReplaceConstantsInFile(data, apiCallerName, rootNamespaceName, newApiServiceOutputPath);
            string newApiServiceInterfaceText = ReplaceConstantsInFile(data, apiCallerName, rootNamespaceName, newApiServiceInterfaceOutputPath);
            string newApiContainerText = ReplaceConstantsInFile(data, apiCallerName, rootNamespaceName, newApiContainerOutputPath);
            string newModelsText = ReplaceConstantsInFile(data, apiCallerName, rootNamespaceName, newModelsOutputPath);
            string newRequestsText = ReplaceConstantsInFile(data, apiCallerName, rootNamespaceName, newRequestsOutputPath);
            string newResponsesText = ReplaceConstantsInFile(data, apiCallerName, rootNamespaceName, newResponseOutputPath);

            File.WriteAllText(newApiCallerOutputPath, newApiCallerText);
            File.WriteAllText(newApiCallerInterfaceOutputPath, newApiCallerInterfaceText);
            File.WriteAllText(newApiServiceOutputPath, newApiServiceText);
            File.WriteAllText(newApiServiceInterfaceOutputPath, newApiServiceInterfaceText);
            File.WriteAllText(newApiContainerOutputPath, newApiContainerText);
            File.WriteAllText(newModelsOutputPath, newModelsText);
            File.WriteAllText(newRequestsOutputPath, newRequestsText);
            File.WriteAllText(newResponseOutputPath, newResponsesText);

            Debug.Log($"-- {data.WindowTitle} -- Api Caller {apiCallerName}ApiCaller created in {newApiCallerOutputPath}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private static string ReplaceConstantsInFile(SO_ApiCallerCreatorConfiguration data, string apiCallerName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstApiCallerScriptName, string.Concat(apiCallerName, data.ApiCallerSufix));
            result = result.Replace(data.ConstApiCallerInterfaceScriptName, string.Concat(data.InterfacePrefix, apiCallerName, data.ApiCallerSufix));
            result = result.Replace(data.ConstApiServiceScriptName, string.Concat(apiCallerName, data.ServiceSufix));
            result = result.Replace(data.ConstApiServiceInterfaceScriptName, string.Concat(data.InterfacePrefix, apiCallerName, data.ServiceSufix));
            result = result.Replace(data.ConstApiContainerScriptName, string.Concat(apiCallerName, data.ContainerSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion
    }
}

