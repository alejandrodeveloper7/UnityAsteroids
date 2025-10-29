using UnityEngine;

namespace ToolsACG.ServicesCreator.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ServicesCreatorConfiguration", menuName = "ToolsACG/ServicesCreator/ServicesCreatorConfiguration")]
    public class SO_ServicesCreatorConfiguration : ScriptableObject
    {
        #region Values

        [Header("Configuration")]
        public string WindowTitle = "Services Creator";
        public Vector2 WindowMinSize = new(400, 300);

        [Header("Paths")]
        public string AssetsPath = "Assets/";
        public string TemplatesPath = "ToolsACG/ServicesCreator/Editor/Templates/";
        [Space]
        public string OutputPath = "Scripts/Core/Services/";

        [Header("affixes")]
        public string InterfacePrefix = "I";
        public string ServiceSufix = "Service";
        public string CsFileExtension = ".cs";

        [Header("Names")]
        public string StaticServiceTemplateFileName = "CONST_StaticServiceName.cs.template";
        public string ConstStaticServiceScriptName = "CONST_StaticServiceName";
        [Space]
        public string AutoSingletonTemplateFileName = "CONST_AutoSingletonServiceName.cs.template";
        public string ConstAutoSingletonServiceScriptName = "CONST_AutoSingletonServiceName";
        [Space]
        public string LazySingletonTemplateFileName = "CONST_LazySingletonServiceName.cs.template";
        public string ConstLazySingletonServiceScriptName = "CONST_LazySingletonServiceName";
        [Space]
        public string InstancesServiceTemplateFileName = "CONST_InstancesServiceName.cs.template";
        public string ConstInstancesServiceScriptName = "CONST_InstancesServiceName";
        [Space]
        public string ServiceInterfaceTemplateFileName = "CONST_I_ServiceName.cs.template";
        public string ConstServiceInterfaceScriptName = "CONST_I_ServiceName";
        [Space]
        public string ConstNamespaceName = "CONST_RootNamespace";

        [Header("Values")]
        public string InfoMessage = "The affixes like \"Service\" or \"I\" will be added automatically";
        [Space]
        public string staticServiceInfoMessage = "This service will be static, and would not have an interface or inheritance.";
        public string AutoSingletonServiceInfoMessage = "This service will be automatically instanced once you press play button and you can NOT have other instances. Problematic with Zenject!!!";
        public string LazySingletonServiceInfoMessage = "This service will be instanced once you acces .Instance for the first time and you can NOT have other instances. Problematic with Zenject!!!";
        public string InstanceServiceInfoMessage = "This service can define constructors with parameters and you can have several instances. Good for Zenject!!!";

        #endregion
    }
}