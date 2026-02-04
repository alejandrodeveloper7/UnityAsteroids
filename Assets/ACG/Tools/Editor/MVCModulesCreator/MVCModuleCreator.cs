using System;
using System.IO;
using System.Linq;
using ACG.Tools.Runtime.MVCModulesCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.MVCModulesCreator
{
    public static class MVCModuleCreator
    {
        #region MVC Creation

        public static void CreateNewModule(SO_MVCModulesCreatorConfiguration data, string moduleName, string rootNamespaceName)
        {
            string outputCompletePath = Path.Combine(Application.dataPath, data.OutputPath);

            DirectoryInfo newModuleDirectory = Directory.CreateDirectory(Path.Combine(outputCompletePath, moduleName));
            DirectoryInfo newScriptsDirectory = Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, data.ScriptFolderName));
            DirectoryInfo newControllersDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, data.ControllerFolderName));
            DirectoryInfo newViewsDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, data.ViewFolderName));
            DirectoryInfo newModelssDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, data.ModelFolderName));
            DirectoryInfo newSOsDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, data.SOFolderName));
            DirectoryInfo newInstallersDirectory = Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, data.InstallersFolderName));
            DirectoryInfo newScenessDirectory = Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, data.ScenessFolderName));

            Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, data.DesignFolderName));
            Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, data.PrefabsFolderName));
            Directory.CreateDirectory(Path.Combine(newScriptsDirectory.FullName, data.OthersFolderName));

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string controllerTemplatePath = string.Concat(templatesCompletePath, data.ControllerTemplateFileName);
            string controllerInterfaceTemplatePath = string.Concat(templatesCompletePath, data.ControllerInterfaceTemplateFileName);
            string viewTemplatePath = string.Concat(templatesCompletePath, data.ViewTemplateFileName);
            string viewInterfaceTemplatePath = string.Concat(templatesCompletePath, data.ViewInterfaceTemplateFileName);
            string modelTemplatePath = string.Concat(templatesCompletePath, data.ModelTemplateFileName);
            string configurationScriptableObjectTemplatePath = string.Concat(templatesCompletePath, data.ConfigurationScriptableObjectFileName);
            string installerTemplatePath = string.Concat(templatesCompletePath, data.InstallerFileName);
            string sceneTemplatePath = string.Concat(templatesCompletePath, data.SceneTemplateFileName);

            string newControllerOutputPath = string.Concat(newControllersDirectory.FullName, "/", string.Concat(moduleName, data.ControllerSufix, data.CsFileExtension));
            string newControllerInterfaceOutputPath = string.Concat(newControllersDirectory.FullName, "/", string.Concat(data.InterfacePrefix, moduleName, data.ControllerSufix, data.CsFileExtension));
            string newViewOutputPath = string.Concat(newViewsDirectory.FullName, "/", string.Concat(moduleName, data.ViewSufix, data.CsFileExtension));
            string newViewInterfaceOutputPath = string.Concat(newViewsDirectory.FullName, "/", string.Concat(data.InterfacePrefix, moduleName, data.ViewSufix, data.CsFileExtension));
            string newModelOutputPath = string.Concat(newModelssDirectory.FullName, "/", string.Concat(moduleName, data.ModelSufix, data.CsFileExtension));
            string newConfigurationScriptableObjectPath = string.Concat(newSOsDirectory.FullName, "/", string.Concat(data.SOPrefix, moduleName, data.ConfigurationSufix, data.CsFileExtension));
            string newInstallerOutputPath = string.Concat(newInstallersDirectory.FullName, "/", string.Concat(moduleName, data.InstallerSufix, data.CsFileExtension));
            string newSceneOutputPath = string.Concat(newScenessDirectory.FullName, "/", string.Concat(moduleName, data.SceneSufix, data.SceneFileExtension));

            File.Copy(controllerTemplatePath, newControllerOutputPath);
            File.Copy(controllerInterfaceTemplatePath, newControllerInterfaceOutputPath);
            File.Copy(viewTemplatePath, newViewOutputPath);
            File.Copy(viewInterfaceTemplatePath, newViewInterfaceOutputPath);
            File.Copy(modelTemplatePath, newModelOutputPath);
            File.Copy(sceneTemplatePath, newSceneOutputPath);
            File.Copy(configurationScriptableObjectTemplatePath, newConfigurationScriptableObjectPath);
            File.Copy(installerTemplatePath, newInstallerOutputPath);

            string newControllerText = ReplaceConstantsInFile(data, moduleName, rootNamespaceName, newControllerOutputPath);
            string newControllerInterfaceText = ReplaceConstantsInFile(data, moduleName, rootNamespaceName, newControllerInterfaceOutputPath);
            string newViewText = ReplaceConstantsInFile(data, moduleName, rootNamespaceName, newViewOutputPath);
            string newViewInterfaceText = ReplaceConstantsInFile(data, moduleName, rootNamespaceName, newViewInterfaceOutputPath);
            string newModelText = ReplaceConstantsInFile(data, moduleName, rootNamespaceName, newModelOutputPath);
            string newConfigurationScriptableObjectText = ReplaceConstantsInFile(data, moduleName, rootNamespaceName, newConfigurationScriptableObjectPath);
            string newInstallerText = ReplaceConstantsInFile(data, moduleName, rootNamespaceName, newInstallerOutputPath);
            string newSceneText = ReplaceConstantsInFile(data, moduleName, rootNamespaceName, newSceneOutputPath);

            File.WriteAllText(newControllerOutputPath, newControllerText);
            File.WriteAllText(newControllerInterfaceOutputPath, newControllerInterfaceText);
            File.WriteAllText(newViewOutputPath, newViewText);
            File.WriteAllText(newViewInterfaceOutputPath, newViewInterfaceText);
            File.WriteAllText(newModelOutputPath, newModelText);
            File.WriteAllText(newConfigurationScriptableObjectPath, newConfigurationScriptableObjectText);
            File.WriteAllText(newInstallerOutputPath, newInstallerText);
            File.WriteAllText(newSceneOutputPath, newSceneText);

            Debug.Log($"-- {data.WindowTitle} -- Scripts of the module {moduleName} created in {newScriptsDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        public static void CreateConfigurationScriptableObject(SO_MVCModulesCreatorConfiguration data, string moduleName)
        {
            string className = string.Concat(data.SOPrefix, moduleName, data.ConfigurationSufix);
            string outputCompletePath = string.Concat(data.AssetsPath, data.OutputPath, moduleName, "/");
            string fileName = string.Concat(moduleName, data.ConfigurationSufix, data.AssetFileExtension);
            string path = string.Concat(outputCompletePath, fileName);

            Type scriptableObjectType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == className);

            if (scriptableObjectType == null || typeof(ScriptableObject).IsAssignableFrom(scriptableObjectType) is false)
            {
                Debug.LogError($"-- {data.WindowTitle} -- ScriptableObject class called {className} not found");
                return;
            }

            ScriptableObject instance = ScriptableObject.CreateInstance(scriptableObjectType);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"-- {data.WindowTitle} -- ScriptableObject {className} created in {path}");
        }

        private static string ReplaceConstantsInFile(SO_MVCModulesCreatorConfiguration data, string moduleName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstModuleName, moduleName);
            result = result.Replace(data.ConstControllerName, string.Concat(moduleName, data.ControllerSufix));
            result = result.Replace(data.ConstControllerInterfaceName, string.Concat(data.InterfacePrefix, moduleName, data.ControllerSufix));
            result = result.Replace(data.ConstViewName, string.Concat(moduleName, data.ViewSufix));
            result = result.Replace(data.ConstViewInterfaceName, string.Concat(data.InterfacePrefix, moduleName, data.ViewSufix));
            result = result.Replace(data.ConstModelName, string.Concat(moduleName, data.ModelSufix));
            result = result.Replace(data.ConstSOConfigurationName, string.Concat(data.SOPrefix + moduleName, data.ConfigurationSufix));
            result = result.Replace(data.ConstSOName, string.Concat(moduleName, data.ConfigurationSufix));
            result = result.Replace(data.ConstInstallerName, string.Concat(moduleName, data.InstallerSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion
    }
}