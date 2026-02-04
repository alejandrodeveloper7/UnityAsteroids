using System;
using System.IO;
using System.Linq;
using ACG.Tools.Runtime.SOCreator.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.SOCreator
{
    public static class SOCreator 
    {
        #region Collection creation

        public static void CreateNewCollectionScriptableObject(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName, string rootNamespaceName, string collectionSODataName)
        {
            string newTypeName = string.Concat(data.SOPrefix, scriptableName, data.CollectionSufix);
            string newInstanceName = string.Concat(scriptableName, data.CollectionSufix);
            string outputCompletePath = Path.Combine(Application.dataPath, data.CollectionsOutputPath);

            DirectoryInfo newScriptableDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string scriptableTemplatePath = string.Concat(templatesCompletePath, data.CollectionTemplateFileName);

            string newScriptableOutputPath = string.Concat(newScriptableDirectory.FullName, "/", data.CollectionTemplateFileName.Replace(data.ConstCollectionsScriptName, newTypeName).Replace(data.TemplateFileExtension, string.Empty));

            File.Copy(scriptableTemplatePath, newScriptableOutputPath);

            string newScriptableText = ReplaceCollectionConstantsInFile(data, scriptableName, rootNamespaceName, collectionSODataName, newScriptableOutputPath);

            File.WriteAllText(newScriptableOutputPath, newScriptableText);

            Debug.Log(string.Format($"-- {data.WindowTitle} -- {scriptableName} collection scriptable created in {newScriptableDirectory.FullName}"));
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private static string ReplaceCollectionConstantsInFile(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName, string rootNamespaceName, string collectionSODataTypeName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstCollectionsScriptName, string.Concat(data.SOPrefix, scriptableName, data.CollectionSufix));
            result = result.Replace(data.ConstNewInstanceName, string.Concat(scriptableName, data.CollectionSufix));
            result = result.Replace(data.ConstName, scriptableName);
            result = result.Replace(data.ConstSOTypeName, string.Concat(data.SOPrefix, collectionSODataTypeName, data.DataSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Settings creation

        public static void CreateNewSettingsScriptableObject(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName, string rootNamespaceName)
        {
            string newTypeName = string.Concat(data.SOPrefix, scriptableName, data.SettingsSufix);
            string newInstanceName = string.Concat(scriptableName, data.SettingsSufix);
            string outputCompletePath = Path.Combine(Application.dataPath, data.SettingsOutputPath);

            DirectoryInfo newScriptableDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string scriptableTemplatePath = string.Concat(templatesCompletePath, data.SettingsTemplateFileName);

            string newScriptableOutputPath = string.Concat(newScriptableDirectory.FullName, "/", data.SettingsTemplateFileName.Replace(data.ConstSettingsScriptName, newTypeName).Replace(data.TemplateFileExtension, string.Empty));

            File.Copy(scriptableTemplatePath, newScriptableOutputPath);

            string newScriptableText = ReplaceSettingsConstantsInFile(data, scriptableName, rootNamespaceName, newScriptableOutputPath);

            File.WriteAllText(newScriptableOutputPath, newScriptableText);

            Debug.Log($"-- {data.WindowTitle} -- {scriptableName} Settings scriptable created in {newScriptableDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        public static void CreateSettingsScriptableObjectInstance(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName)
        {
            string newTypeName = string.Concat(data.SOPrefix, scriptableName, data.SettingsSufix);
            string outputCompletePath = string.Concat(data.AssetsPath, data.SettingsInstanceOutputPath);
            string fileName = string.Concat(scriptableName, data.SettingsSufix, data.AssetFileExtension);
            string path = string.Concat(outputCompletePath, fileName);

            EnsureResourcesFolder(data);
            EnsureSettingsFolder(data);

            Type scriptableObjectType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == newTypeName);

            if (scriptableObjectType == null || typeof(ScriptableObject).IsAssignableFrom(scriptableObjectType) is false)
            {
                Debug.LogError($"-- {data.WindowTitle} -- Settings instance class called {newTypeName} not found");
                return;
            }

            ScriptableObject instance = ScriptableObject.CreateInstance(scriptableObjectType);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"-- {data.WindowTitle} -- Settings instance {newTypeName} created in {path}");
        }

        private static string ReplaceSettingsConstantsInFile(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstSettingsScriptName, string.Concat(data.SOPrefix, scriptableName, data.SettingsSufix));
            result = result.Replace(data.ConstNewInstanceName, string.Concat(scriptableName, data.SettingsSufix));
            result = result.Replace(data.ConstName, scriptableName);
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Data creation

        public static void CreateNewDataScriptableObject(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName, string rootNamespaceName)
        {
            string newTypeName = string.Concat(data.SOPrefix, scriptableName, data.DataSufix);
            string newInstanceName = string.Concat(scriptableName, data.DataSufix);
            string outputCompletePath = Path.Combine(Application.dataPath, data.DataOutputPath);

            DirectoryInfo newScriptableDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string scriptableTemplatePath = string.Concat(templatesCompletePath, data.DataTemplateFileName);

            string newScriptableOutputPath = string.Concat(newScriptableDirectory.FullName, "/", data.DataTemplateFileName.Replace(data.ConstDataScriptName, newTypeName).Replace(data.TemplateFileExtension, string.Empty));

            File.Copy(scriptableTemplatePath, newScriptableOutputPath);

            string newScriptableText = ReplaceDataConstantsInFile(data, scriptableName, rootNamespaceName, newScriptableOutputPath);

            File.WriteAllText(newScriptableOutputPath, newScriptableText);

            Debug.Log(string.Format($"-- {data.WindowTitle} -- {scriptableName} Data scriptable created in {newScriptableDirectory.FullName}"));
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private static string ReplaceDataConstantsInFile(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstDataScriptName, string.Concat(data.SOPrefix, scriptableName, data.DataSufix));
            result = result.Replace(data.ConstNewInstanceName, string.Concat(scriptableName, data.DataSufix));
            result = result.Replace(data.ConstName, scriptableName);
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Configuration creation

        public static void CreateNewConfigurationScriptableObject(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName, string rootNamespaceName)
        {
            string newTypeName = string.Concat(data.SOPrefix, scriptableName, data.ConfigurationSufix);

            string outputCompletePath = Path.Combine(Application.dataPath, data.ConfigurationOutputPath);

            DirectoryInfo newScriptableDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string scriptableTemplatePath = string.Concat(templatesCompletePath, data.ConfigurationTemplateFileName);

            string newScriptableOutputPath = string.Concat(newScriptableDirectory.FullName, "/", data.ConfigurationTemplateFileName.Replace(data.ConfigurationConstScriptName, newTypeName).Replace(data.TemplateFileExtension, string.Empty));

            File.Copy(scriptableTemplatePath, newScriptableOutputPath);

            string newScriptableText = ReplaceConfigurationConstantsInFile(data, scriptableName, rootNamespaceName, newScriptableOutputPath);

            File.WriteAllText(newScriptableOutputPath, newScriptableText);

            Debug.Log(string.Format($"-- {data.WindowTitle} -- {scriptableName} Configuration scriptable created in {newScriptableDirectory.FullName}"));
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        private static string ReplaceConfigurationConstantsInFile(SO_ScriptableObjectsCreatorConfiguration data, string scriptableName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConfigurationConstScriptName, string.Concat(data.SOPrefix, scriptableName, data.ConfigurationSufix));
            result = result.Replace(data.ConstNewInstanceName, string.Concat(scriptableName, data.ConfigurationSufix));
            result = result.Replace(data.ConstName, scriptableName);
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Functionality

        private static void EnsureResourcesFolder(SO_ScriptableObjectsCreatorConfiguration data)
        {
            if (AssetDatabase.IsValidFolder(data.ResourcesPath) is false)
            {
                AssetDatabase.CreateFolder(data.AssetsFolderName, data.ResourcesFolderName);
                AssetDatabase.Refresh();
            }
        }

        private static void EnsureSettingsFolder(SO_ScriptableObjectsCreatorConfiguration data)
        {
            if (AssetDatabase.IsValidFolder(data.SettingsPath) is false)
            {
                AssetDatabase.CreateFolder(data.ResourcesPath, data.SettingsFolderName);
                AssetDatabase.Refresh();
            }
        }

        #endregion
    }
}