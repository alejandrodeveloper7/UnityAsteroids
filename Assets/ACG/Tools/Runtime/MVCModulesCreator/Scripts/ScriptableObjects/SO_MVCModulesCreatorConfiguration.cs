using UnityEngine;

namespace ACG.Tools.Runtime.MVCModulesCreator.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MVCModulesCreatorConfiguration", menuName = "ToolsACG/MVCModulesCreator/MVCModulesCreatorConfiguration")]
    public class SO_MVCModulesCreatorConfiguration : ScriptableObject
    {
        #region Values

        [Header("Configuration")]
        public string WindowTitle = "MVC Module Creator";
        public Vector2 WindowMinSize = new(345, 215); 
        
        [Header("Paths")]
        public string AssetsPath = "Assets/";
        public string TemplatesPath = "ACG/Tools/Editor/MVCModulesCreator/Templates/";

        [Header("Output Paths")]
        public string OutputPath = "MVC Modules/";

        [Header("Template Names")]
        public string ControllerTemplateFileName = "CONST_ControllerName.cs.template";
        public string ControllerInterfaceTemplateFileName = "CONST_IControllerName.cs.template";
        public string ViewTemplateFileName = "CONST_ViewName.cs.template";
        public string ViewInterfaceTemplateFileName = "CONST_IViewName.cs.template";
        public string ModelTemplateFileName = "CONST_ModelName.cs.template";
        public string ConfigurationScriptableObjectFileName = "CONST_ConfigurationScriptableObject.cs.template";
        public string InstallerFileName = "CONST_SceneInstaller.cs.template";
        public string SceneTemplateFileName = "CONST_SceneName.unity";

        [Header("Names")]
        public string ConstModuleName = "CONST_ModuleName";
        public string ConstControllerName = "CONST_ControllerName";
        public string ConstControllerInterfaceName = "CONST_IControllerName";
        public string ConstViewName = "CONST_ViewName";
        public string ConstViewInterfaceName = "CONST_IViewName";
        public string ConstModelName = "CONST_ModelName";
        public string ConstNamespaceName = "CONST_RootNamespace";
        public string ConstSOConfigurationName = "CONST_SO_ConfigurationScriptableObject";
        public string ConstInstallerName = "CONST_SceneInstaller";
        public string ConstSOName = "CONST_ConfigurationScriptableObject";

        [Header("Affixes")]
        public string SOPrefix = "SO_";
        public string InterfacePrefix = "I";
        public string ViewSufix = "View";
        public string ControllerSufix = "Controller";
        public string ModelSufix = "Model";
        public string SceneSufix = "Scene";
        public string InstallerSufix = "SceneInstaller";
        public string ConfigurationSufix = "Configuration";
        public string CsFileExtension = ".cs";
        public string AssetFileExtension = ".asset";
        public string SceneFileExtension = ".unity";

        [Header("Folders")]
        public string ScriptFolderName = "Scripts";
        public string ControllerFolderName = "Controllers";
        public string ViewFolderName = "Views";
        public string ModelFolderName = "Models";
        public string SOFolderName = "ScriptableObjects";
        public string InstallersFolderName = "Installers";
        public string DesignFolderName = "Design";
        public string PrefabsFolderName = "Prefabs";
        public string ScenessFolderName = "Scenes";
        public string OthersFolderName = "Others";

        [Header("Values")]
        public string InfoMessage = "The affixes of each script will be added automatically";
      
        #endregion
    }
}