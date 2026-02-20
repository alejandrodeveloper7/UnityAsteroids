using ACG.Tools.Runtime.ManagersCreator.ScriptableObjects;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.ManagersCreator
{
    public static class ManagersCreator
    {
        #region Monobehaviour Auto singleton

        public static void CreateNewMonobehaviourAutoSingletonManager(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName)
        {
            string outputCompletePathManager = Path.Combine(Application.dataPath, data.OutputPath, $"{managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePathManager);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, data.MonobehaviourAutoSingletonTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(managerName, data.ManagerSufix, data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix, data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceMonobehaviourAutoSingletonConstantsInFile(data, managerName, rootNamespaceName, newManagerOutputPath);
            string newInterfaceText = ReplaceMonobehaviourAutoSingletonConstantsInFile(data, managerName, rootNamespaceName, newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
       
        private static string ReplaceMonobehaviourAutoSingletonConstantsInFile(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstMonobehaviourAutoSingletonScriptName, string.Concat(managerName, data.ManagerSufix));
            result = result.Replace(data.ConstInterfaceName, string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Monobehaviour Lazy singleton

        public static void CreateNewMonobehaviourLazySingletonManager(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName)
        {
            string outputCompletePathManager = Path.Combine(Application.dataPath, data.OutputPath, $"{managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePathManager);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, data.MonobehaviourLazySingletonTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(managerName, data.ManagerSufix, data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix, data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceMonobehaviourLazySingletonConstantsInFile(data, managerName, rootNamespaceName, newManagerOutputPath);
            string newInterfaceText = ReplaceMonobehaviourLazySingletonConstantsInFile(data, managerName, rootNamespaceName, newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
     
        private static string ReplaceMonobehaviourLazySingletonConstantsInFile(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstMonobehaviourLazySingletonScriptName, string.Concat(managerName, data.ManagerSufix));
            result = result.Replace(data.ConstInterfaceName, string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Monobehaviour MultiInstances

        public static void CreateNewMonobehaviourInstancesManager(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName)
        {
            string outputCompletePathManager = Path.Combine(Application.dataPath, data.OutputPath, $"{managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePathManager);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, data.MonobehaviourInstancesTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(managerName, data.ManagerSufix, data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix, data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceMonobehaviourInstancesConstantsInFile(data, managerName, rootNamespaceName, newManagerOutputPath);
            string newInterfaceText = ReplaceMonobehaviourInstancesConstantsInFile(data, managerName, rootNamespaceName, newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
     
        private static string ReplaceMonobehaviourInstancesConstantsInFile(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstMonobehaviourInstancesScriptName, string.Concat(managerName, data.ManagerSufix));
            result = result.Replace(data.ConstInterfaceName, string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region Monobehaviour Prefab

        public static void CreateMonobehaviourPrefab(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName)
        {
            string newTypeName = string.Concat(managerName, data.ManagerSufix);
            string prefabName = newTypeName;

            string completePrefabsPath = Path.Combine(Application.dataPath, data.PrefabsOutputPath);
            string prefabCompleteFilePath = string.Concat(completePrefabsPath, newTypeName, data.PrefabFileExtension);

            GameObject go = new(prefabName);

            string fullTypeName = $"{rootNamespaceName}.Core.Managers.{newTypeName}";

            Type managerType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == fullTypeName);

            if (managerType != null && managerType.IsSubclassOf(typeof(MonoBehaviour)))
            {
                go.AddComponent(managerType);
            }
            else
            {
                Debug.LogError($"-- {data.WindowTitle} -- {newTypeName} script not found. Prefab not created");
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

            Debug.Log($"Prefab created in {data.PrefabsOutputPath}{newTypeName}{data.PrefabFileExtension}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }

        #endregion

        #region No Monobehaviour Auto singleton

        public static void CreateNewNoMonobehaviourAutoSingletonManager(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName)
        {
            string outputCompletePath = Path.Combine(Application.dataPath, data.OutputPath, $"{managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, data.NoMonobehaviourAutoSingletonTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(managerName, data.ManagerSufix, data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix, data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceNoMonobehaviourAutoSingletonConstantsInFile(data, managerName, rootNamespaceName, newManagerOutputPath);
            string newInterfaceText = ReplaceNoMonobehaviourAutoSingletonConstantsInFile(data, managerName, rootNamespaceName, newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
    
        private static string ReplaceNoMonobehaviourAutoSingletonConstantsInFile(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstNoMonobehaviourAutoSingletonScriptName, string.Concat(managerName, data.ManagerSufix));
            result = result.Replace(data.ConstInterfaceName, string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region No Monobehaviour Lazy singleton

        public static void CreateNewNoMonobehaviourLazySingletonManager(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName)
        {
            string outputCompletePath = Path.Combine(Application.dataPath, data.OutputPath, $"{managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, data.NoMonobehaviourLazySingletonTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(managerName, data.ManagerSufix, data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix, data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceNoMonobehaviourLazySingletonConstantsInFile(data, managerName, rootNamespaceName, newManagerOutputPath);
            string newInterfaceText = ReplaceNoMonobehaviourLazySingletonConstantsInFile(data, managerName, rootNamespaceName, newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
     
        private static string ReplaceNoMonobehaviourLazySingletonConstantsInFile(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstNoMonobehaviourLazySingletonScriptName, string.Concat(managerName, data.ManagerSufix));
            result = result.Replace(data.ConstInterfaceName, string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion

        #region No Monobehaviour MultiInstances

        public static void CreateNewNoMonobehaviourInstancesManager(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName)
        {
            string outputCompletePath = Path.Combine(Application.dataPath, data.OutputPath, $"{managerName}/");

            DirectoryInfo newManagersDirectory = Directory.CreateDirectory(outputCompletePath);

            string templatesCompletePath = Path.Combine(Application.dataPath, data.TemplatesPath);

            string managerTemplatePath = string.Concat(templatesCompletePath, data.NoMonobehaviourInstancesTemplateFileName);
            string interfaceTemplatePath = string.Concat(templatesCompletePath, data.InterfaceTemplateFileName);

            string newManagerOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(managerName, data.ManagerSufix, data.CsFileExtension));
            string newInterfaceOutputPath = string.Concat(newManagersDirectory.FullName, "/", string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix, data.CsFileExtension));

            File.Copy(managerTemplatePath, newManagerOutputPath);
            File.Copy(interfaceTemplatePath, newInterfaceOutputPath);

            string newManagerText = ReplaceNoMonobehaviourInstancesConstantsInFile(data, managerName, rootNamespaceName, newManagerOutputPath);
            string newInterfaceText = ReplaceNoMonobehaviourInstancesConstantsInFile(data, managerName, rootNamespaceName, newInterfaceOutputPath);

            File.WriteAllText(newManagerOutputPath, newManagerText);
            File.WriteAllText(newInterfaceOutputPath, newInterfaceText);

            Debug.Log($"-- {data.WindowTitle} -- {managerName}Manager created in {newManagersDirectory.FullName}");
            EditorApplication.delayCall += () => AssetDatabase.Refresh();
        }
      
        private static string ReplaceNoMonobehaviourInstancesConstantsInFile(SO_ManagersCreatorConfiguration data, string managerName, string rootNamespaceName, string path)
        {
            string result = File.ReadAllText(path);

            result = result.Replace(data.ConstNoMonobehaviourInstancesScriptName, string.Concat(managerName, data.ManagerSufix));
            result = result.Replace(data.ConstInterfaceName, string.Concat(data.InterfaceSufix, managerName, data.ManagerSufix));
            result = result.Replace(data.ConstNamespaceName, rootNamespaceName);

            return result;
        }

        #endregion
    }
}