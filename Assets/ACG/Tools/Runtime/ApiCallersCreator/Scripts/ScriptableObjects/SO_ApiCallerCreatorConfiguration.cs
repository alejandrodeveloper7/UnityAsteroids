using UnityEngine;

namespace ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ApiCallerCreatorConfiguration", menuName = "ToolsACG/ApiCallerCreator/ApiCallerCreatorConfiguration")]
    public class SO_ApiCallerCreatorConfiguration : ScriptableObject
    {
        #region Values

        [Header("Configuration")]
        public string WindowTitle = "ApiCaller Creator";
        public Vector2 WindowMinSize = new(345, 205); 
        
        [Header("Paths")]
        public string TemplatesPath = "ACG/Tools/Editor/ApiCallersCreator/Templates/";

        [Header("Output Paths")]
        public string OutputPath = "Scripts/ApiCallers/";

        [Header("Affixes")]
        public string InterfacePrefix = "I";
        public string ApiCallerSufix = "ApiCaller";
        public string ServiceSufix = "ApiService";
        public string ContainerSufix = "ApiContainer";
        public string ModelsSufix = "Models";
        public string RequestsSufix = "Requests";
        public string ResponsesSufix = "Responses";
        public string CsFileExtension = ".cs";

        [Header("Names")]
        public string CallerTemplateFileName = "CONST_ApiCallerName.cs.template";
        public string CallerInterfaceTemplateFileName = "CONST_IApiCallerName.cs.template";
        public string ServiceTemplateFileName = "CONST_ApiServiceName.cs.template";
        public string ServiceInterfaceTemplateFileName = "CONST_IApiServiceName.cs.template";
        public string ContainerTemplateFileName = "CONST_ApiContainerName.cs.template";
        public string ModelsTemplateFileName = "CONST_ApiCallerModelsName.cs.template";
        public string RequestsTemplateFileName = "CONST_ApiCallerRequestsName.cs.template";
        public string ResponsesTemplateFileName = "CONST_ApiCallerResponsesName.cs.template";

        public string ConstApiCallerScriptName = "CONST_ApiCallerName";
        public string ConstApiCallerInterfaceScriptName = "CONST_IApiCallerName";
        public string ConstApiServiceScriptName = "CONST_ApiServiceName";
        public string ConstApiServiceInterfaceScriptName = "CONST_IApiServiceName";
        public string ConstApiContainerScriptName = "CONST_ApiContainerName";
        public string ConstNamespaceName = "CONST_RootNamespace";

        public string ServicesFolderName = "Services";
        public string ApiCallersFolderName = "ApiCallers";
        public string ApiContainersFolderName = "Containers";
        public string ModelsFolderName = "Models";

        [Header("Values")]
        public string InfoMessage = "The affixes of each script will be added automatically";

        #endregion
    }
}