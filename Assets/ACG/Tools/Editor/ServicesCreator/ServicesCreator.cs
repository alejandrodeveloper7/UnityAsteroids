using System.IO;
using ACG.Tools.Runtime.ServicesCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.ServicesCreator
{
    public static class ServicesCreator
    {
        #region Static

        public static void CreateStaticService(SO_ServicesCreatorConfiguration data, string serviceName, string rootNamespaceName)
        {
            string outputCompletePathService = Path.Combine(Application.dataPath, data.OutputPath, $"{serviceName}/");

            DirectoryInfo newServiceDirectory = Directory.CreateDirectory(outputCompletePathService);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string serviceTemplatePath = string.Concat(templatesCompletePath, data.StaticServiceTemplateFileName);

            string newServiceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(serviceName, data.ServiceSufix, data.CsFileExtension));

            File.Copy(serviceTemplatePath, newServiceOutputPath);

            string newServiceText = ReplaceStaticServiceConstantsInFile(data, serviceName, rootNamespaceName, newServiceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);

            Debug.Log($"-- {data.WindowTitle} -- {serviceName}Service created in {newServiceDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private static string ReplaceStaticServiceConstantsInFile(SO_ServicesCreatorConfiguration data, string serviceName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstStaticServiceScriptName, string.Concat(serviceName, data.ServiceSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Auto Singleton

        public static void CreateAutoSingletonService(SO_ServicesCreatorConfiguration data, string serviceName, string rootNamespaceName)
        {
            string outputCompletePathService = Path.Combine(Application.dataPath, data.OutputPath, $"{serviceName}/");

            DirectoryInfo newServiceDirectory = Directory.CreateDirectory(outputCompletePathService);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string serviceTemplatePath = string.Concat(templatesCompletePath, data.AutoSingletonTemplateFileName);
            string serviceInterfaceTemplatePath = string.Concat(templatesCompletePath, data.ServiceInterfaceTemplateFileName);

            string newServiceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(serviceName, data.ServiceSufix, data.CsFileExtension));
            string newServiceInterfaceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(data.InterfacePrefix, serviceName, data.ServiceSufix, data.CsFileExtension));

            File.Copy(serviceTemplatePath, newServiceOutputPath);
            File.Copy(serviceInterfaceTemplatePath, newServiceInterfaceOutputPath);

            string newServiceText = ReplaceAutoSingletonServiceConstantsInFile(data, serviceName, rootNamespaceName, newServiceOutputPath);
            string newServiceInterfaceText = ReplaceAutoSingletonServiceConstantsInFile(data, serviceName, rootNamespaceName, newServiceInterfaceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);
            File.WriteAllText(newServiceInterfaceOutputPath, newServiceInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {serviceName}Service created in {newServiceDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private static string ReplaceAutoSingletonServiceConstantsInFile(SO_ServicesCreatorConfiguration data, string serviceName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstAutoSingletonServiceScriptName, string.Concat(serviceName, data.ServiceSufix));
            result = result.Replace(data.ConstServiceInterfaceScriptName, string.Concat(data.InterfacePrefix, serviceName, data.ServiceSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Lazy singleton

        public static void CreateLazySingletonService(SO_ServicesCreatorConfiguration data, string serviceName, string rootNamespaceName)
        {
            string outputCompletePathService = Path.Combine(Application.dataPath, data.OutputPath, $"{serviceName}/");

            DirectoryInfo newServiceDirectory = Directory.CreateDirectory(outputCompletePathService);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string serviceTemplatePath = string.Concat(templatesCompletePath, data.LazySingletonTemplateFileName);
            string serviceInterfaceTemplatePath = string.Concat(templatesCompletePath, data.ServiceInterfaceTemplateFileName);

            string newServiceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(serviceName, data.ServiceSufix, data.CsFileExtension));
            string newServiceInterfaceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(data.InterfacePrefix, serviceName, data.ServiceSufix, data.CsFileExtension));

            File.Copy(serviceTemplatePath, newServiceOutputPath);
            File.Copy(serviceInterfaceTemplatePath, newServiceInterfaceOutputPath);

            string newServiceText = ReplaceLazySingletonServiceConstantsInFile(data, serviceName, rootNamespaceName, newServiceOutputPath);
            string newServiceInterfaceText = ReplaceLazySingletonServiceConstantsInFile(data, serviceName, rootNamespaceName, newServiceInterfaceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);
            File.WriteAllText(newServiceInterfaceOutputPath, newServiceInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {serviceName}Service created in {newServiceDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private static string ReplaceLazySingletonServiceConstantsInFile(SO_ServicesCreatorConfiguration data, string serviceName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstLazySingletonServiceScriptName, string.Concat(serviceName, data.ServiceSufix));
            result = result.Replace(data.ConstServiceInterfaceScriptName, string.Concat(data.InterfacePrefix, serviceName, data.ServiceSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region MultiInstances

        public static void CreateMultiInstancesService(SO_ServicesCreatorConfiguration data, string serviceName, string rootNamespaceName)
        {
            string outputCompletePathService = Path.Combine(Application.dataPath, data.OutputPath, $"{serviceName}/");

            DirectoryInfo newServiceDirectory = Directory.CreateDirectory(outputCompletePathService);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string serviceTemplatePath = string.Concat(templatesCompletePath, data.InstancesServiceTemplateFileName);
            string serviceInterfaceTemplatePath = string.Concat(templatesCompletePath, data.ServiceInterfaceTemplateFileName);

            string newServiceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(serviceName, data.ServiceSufix, data.CsFileExtension));
            string newServiceInterfaceOutputPath = string.Concat(newServiceDirectory.FullName, "/", string.Concat(data.InterfacePrefix, serviceName, data.ServiceSufix, data.CsFileExtension));

            File.Copy(serviceTemplatePath, newServiceOutputPath);
            File.Copy(serviceInterfaceTemplatePath, newServiceInterfaceOutputPath);

            string newServiceText = ReplaceMultiInstancesServiceConstantsInFile(data, serviceName, rootNamespaceName, newServiceOutputPath);
            string newServiceInterfaceText = ReplaceMultiInstancesServiceConstantsInFile(data, serviceName, rootNamespaceName, newServiceInterfaceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);
            File.WriteAllText(newServiceInterfaceOutputPath, newServiceInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {serviceName}Service created in {newServiceDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private static string ReplaceMultiInstancesServiceConstantsInFile(SO_ServicesCreatorConfiguration data, string serviceName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstInstancesServiceScriptName, string.Concat(serviceName, data.ServiceSufix));
            result = result.Replace(data.ConstServiceInterfaceScriptName, string.Concat(data.InterfacePrefix, serviceName, data.ServiceSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion
    }
}