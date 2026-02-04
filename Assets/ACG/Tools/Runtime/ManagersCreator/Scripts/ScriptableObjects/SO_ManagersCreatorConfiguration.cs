using UnityEngine;

namespace ACG.Tools.Runtime.ManagersCreator.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ManagersCreatorConfiguration", menuName = "ToolsACG/ManagersCreator/ManagersCreatorConfiguration")]
    public class SO_ManagersCreatorConfiguration : ScriptableObject
    {
        #region Values

        [Header("Configuration")]
        public string WindowTitle = "Managers Creator";
        public Vector2 WindowMinSize = new(420, 350);

        [Header("Paths")]
        public string TemplatesPath = "ACG/Tools/Editor/ManagersCreator/Templates/";

        [Header("Output Paths")]
        public string OutputPath = "Scripts/Core/Managers";
        public string PrefabsOutputPath = "Resources/Managers/";

        [Header("Affixes")]
        public string ManagerSufix = "Manager";
        public string InterfaceSufix = "I";
        public string PrefabFileExtension = ".prefab";
        public string CsFileExtension = ".cs";

        [Header("Names")]
        public string MonobehaviourAutoSingletonTemplateFileName = "CONST_MonobehaviourAutoSingletonManagerName.cs.template";
        public string ConstMonobehaviourAutoSingletonScriptName = "CONST_MonobehaviourAutoSingletonManagerName";
        [Space]
        public string MonobehaviourLazySingletonTemplateFileName = "CONST_MonobehaviourLazySingletonManagerName.cs.template";
        public string ConstMonobehaviourLazySingletonScriptName = "CONST_MonobehaviourLazySingletonManagerName";
        [Space]
        public string MonobehaviourInstancesTemplateFileName = "CONST_MonobehaviourInstancesManagerName.cs.template";
        public string ConstMonobehaviourInstancesScriptName = "CONST_MonobehaviourInstancesManagerName";
        [Space]
        public string NoMonobehaviourAutoSingletonTemplateFileName = "CONST_NoMonobehaviourAutoSingletonManagerName.cs.template";
        public string ConstNoMonobehaviourAutoSingletonScriptName = "CONST_NoMonobehaviourAutoSingletonManagerName";
        [Space]
        public string NoMonobehaviourLazySingletonTemplateFileName = "CONST_NoMonobehaviourLazySingletonManagerName.cs.template";
        public string ConstNoMonobehaviourLazySingletonScriptName = "CONST_NoMonobehaviourLazySingletonManagerName";
        [Space]
        public string NoMonobehaviourInstancesTemplateFileName = "CONST_NoMonobehaviourInstancesManagerName.cs.template";
        public string ConstNoMonobehaviourInstancesScriptName = "CONST_NoMonobehaviourInstancesManagerName";
        [Space]
        public string InterfaceTemplateFileName = "CONST_I_ManagerName.cs.template";
        public string ConstInterfaceName = "CONST_I_ManagerName";
        [Space]
        public string ConstNamespaceName = "CONST_RootNamespace";

        [Header("Values")]
        public string InfoMessage = "The affixes like 'I' or 'Manager' will be added automatically";
        [Space]
        public string MonoAutoSingletonMessage = "This manager will be a MonoBehaviour that automatically instantiates a prefab in the scene when you press the Play button and does NOT allow more than one instance. Problematic with Zenject!!!";
        public string MonoLazySingletonMessage = "This manager will be a MonoBehaviour that, when you access .Instance for the firs time, instantiates a prefab in the scene and does NOT allow more than one instance. Problematic with Zenject!!!";
        public string MonoInstancesMessage = "This manager will be a MonoBehaviour that, when you call .CreateInstance(), instantiates a prefab in the scene and allows multiple instances. Good for Zenject!!!";
        [Space]
        public string NoMonoAutoSingletonMessage = "This manager will be a NO MonoBehaviour that automatically instantiates itself when you press the Play button and does NOT allow more than one instance. Problematic with Zenject!!!";
        public string NoMonoLazySingletonMessage = "This manager will be a NO MonoBehaviour that, when you access .Instance for the firs time, instantiates itself and does NOT allow more than one instance. Problematic with Zenject!!!";
        public string NoMonoInstancesMessage = "This manager will be a NO MonoBehaviour that allows multiple instances and define constructors with parameters. Good for Zenject!!!";

        #endregion
    }
}
