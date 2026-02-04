using UnityEngine;

namespace ACG.Tools.Runtime.SOCreator.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptableObjectsCreatorConfiguration", menuName = "ToolsACG/SOCreator/ScriptableObjectsCreatorConfiguration")]
    public class SO_ScriptableObjectsCreatorConfiguration : ScriptableObject
    {
        #region Values

        [Header("Configuration")]
        public string WindowTitle = "ScriptableObject Creator";
        public Vector2 WindowMinSize = new(400, 245);

        [Header("Folders")]
        public string AssetsFolderName = "Assets";
        public string ResourcesFolderName = "Resources";
        public string SettingsFolderName = "Settings";

        [Header("Paths Names")]
        public string ResourcesPath = "Assets/Resources";
        public string SettingsPath = "Assets/Resources/Settings";

        [Header("Paths")]
        public string AssetsPath = "Assets/";
        public string TemplatesPath = "ACG/Tools/Editor/SOCreator/Templates/";

        [Header("Output Paths")]
        public string CollectionsOutputPath = "Scripts/Core/ScriptableObjects/Collections/";
        [Space]
        public string DataOutputPath = "Scripts/Core/ScriptableObjects/Data/";
        [Space]
        public string ConfigurationOutputPath = "Scripts/Core/ScriptableObjects/Configurations/";
        [Space]
        public string SettingsOutputPath = "Scripts/Core/ScriptableObjects/Settings/";
        public string SettingsInstanceOutputPath = "Resources/Settings/";

        [Header("Affixes")]
        public string SOPrefix = "SO_";
        [Space]
        public string CollectionSufix = "Collection";
        public string DataSufix = "Data";
        public string SettingsSufix = "Settings";
        public string ConfigurationSufix = "Configuration";
        [Space]
        public string TemplateFileExtension = ".template";
        public string AssetFileExtension = ".asset";

        [Header("Names")]
        public string CollectionTemplateFileName = "CONST_SOCollectionName.cs.template";
        public string ConstCollectionsScriptName = "CONST_SOCollectionName";
        [Space]
        public string SettingsTemplateFileName = "CONST_SOSettingsName.cs.template";
        public string ConstSettingsScriptName = "CONST_SOSettingsName";
        [Space]
        public string DataTemplateFileName = "CONST_SODataName.cs.template";
        public string ConstDataScriptName = "CONST_SODataName";
        [Space]
        public string ConfigurationTemplateFileName = "CONST_SOConfigurationName.cs.template";
        public string ConfigurationConstScriptName = "CONST_SOConfigurationName";
        [Space]
        public string ConstSOTypeName = "CONST_SOType";
        public string ConstNewInstanceName = "CONST_NewInstanceName";
        public string ConstName = "CONST_Name";
        public string ConstNamespaceName = "CONST_RootNamespace";

        [Header("Values")]
        public string InfoMessage = "The affixes like 'SO_' or 'Data' will be added automatically";

        #endregion
    }
}