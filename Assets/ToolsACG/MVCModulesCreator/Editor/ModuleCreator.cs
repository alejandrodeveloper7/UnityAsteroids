using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.Editor
{
    public class ModuleCreator : EditorWindow
    {
        #region Fields

        private static ModuleCreator _window;
        private string _moduleName;

        [Header("Paths")]
        private const string _templatesPath = "ToolsACG/MVCModulesCreator/Editor/Templates/";
        private const string _outputPath = "Game/Modules/";

        [Header("Template Names")]
        private const string _controllerTemplateFileName = "CONST_ControllerName.cs.template";
        private const string _viewTemplateFileName = "CONST_ViewName.cs.template";
        private const string _modelTemplateFileName = "CONST_ModelName.cs.template";
        private const string _sceneTemplateFileName = "CONST_SceneName.unity";

        #endregion

        #region Editor Window Management

        [MenuItem("Tools/ToolsACG/Create MVC Module", false)]
        public static void OpenEditorWindow()
        {
            _window = EditorWindow.GetWindow<ModuleCreator>(true, "MVC Module Creator");
            _window.minSize = new Vector2(300, 160);
            _window.maxSize = new Vector2(300, 160);
        }

        private void OnGUI()
        {
            GUIStyle _style = new GUIStyle();
            _style.fontSize = 14;
            _style.normal.textColor = Color.white;
            _style.fontStyle = FontStyle.Bold;
            _style.margin = new RectOffset(10, 10, 10, 10);
            _style.padding = new RectOffset(10, 10, 10, 10);

            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(20, 20, 20, 20) });

            DrawTitle("Configuration");
            DrawField("Module name", ref _moduleName);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUI.enabled = !string.IsNullOrEmpty(_moduleName);
            DrawButton("Create", CreateNewModule);
            GUI.enabled = true;

            EditorGUILayout.Space();

            GUI.enabled = !string.IsNullOrEmpty(_moduleName);
            DrawButton("Create Scriptable", CreateAssetFromScript);
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Creation

        private void CreateNewModule()
        {
            string outputCompletePath = Path.Combine(Application.dataPath, _outputPath);

            DirectoryInfo newModuleDirectory = Directory.CreateDirectory(Path.Combine(outputCompletePath, _moduleName));
            DirectoryInfo newScriptsDirectory = Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, "Scripts"));
            Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, "Design"));
            Directory.CreateDirectory(Path.Combine(newModuleDirectory.FullName, "Prefabs"));

            string templatesCompletePath = Path.Combine(Application.dataPath, _templatesPath);

            string controllerTemplatePath = string.Concat(templatesCompletePath, _controllerTemplateFileName);
            string viewTemplatePath = string.Concat(templatesCompletePath, _viewTemplateFileName);
            string modelTemplatePath = string.Concat(templatesCompletePath, _modelTemplateFileName);
            string sceneTemplatePath = string.Concat(templatesCompletePath, _sceneTemplateFileName);

            string newControllerOutputPath = string.Concat(newScriptsDirectory.FullName, "/", _controllerTemplateFileName.Replace("CONST_ControllerName", string.Concat(_moduleName, "Controller")).Replace(".template", string.Empty));
            string newViewOutputPath = string.Concat(newScriptsDirectory.FullName, "/", _viewTemplateFileName.Replace("CONST_ViewName", string.Concat(_moduleName, "View")).Replace(".template", string.Empty));
            string newModelOutputPath = string.Concat(newScriptsDirectory.FullName, "/", _modelTemplateFileName.Replace("CONST_ModelName", string.Concat(_moduleName, "Model")).Replace(".template", string.Empty));
            string newSceneOutputPath = string.Concat(newModuleDirectory.FullName, "/", _sceneTemplateFileName.Replace("CONST_SceneName", string.Concat(_moduleName, "Scene")));

            File.Copy(controllerTemplatePath, newControllerOutputPath);
            File.Copy(viewTemplatePath, newViewOutputPath);
            File.Copy(modelTemplatePath, newModelOutputPath);
            File.Copy(sceneTemplatePath, newSceneOutputPath);

            string newControllerText = ReplaceConstantsInFiles(newControllerOutputPath);
            string newViewText = ReplaceConstantsInFiles(newViewOutputPath);
            string newModelText = ReplaceConstantsInFiles(newModelOutputPath);
            string newSceneText = ReplaceConstantsInFiles(newSceneOutputPath);

            File.Delete(newControllerOutputPath);
            File.Delete(newViewOutputPath);
            File.Delete(newModelOutputPath);
            File.Delete(newSceneOutputPath);

            File.WriteAllText(newControllerOutputPath, newControllerText);
            File.WriteAllText(newViewOutputPath, newViewText);
            File.WriteAllText(newModelOutputPath, newModelText);
            File.WriteAllText(newSceneOutputPath, newSceneText);

            AssetDatabase.Refresh();
        }

        private void CreateAssetFromScript()
        {
            string className = string.Concat(_moduleName, "Model");
            string outputCompletePath = string.Concat("Assets/", _outputPath, _moduleName, "/");
            string fileName = string.Concat(_moduleName, "Model.asset");
            string path = string.Concat(outputCompletePath, fileName);

            Type scriptableObjectType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == className);

            if (scriptableObjectType == null || typeof(ScriptableObject).IsAssignableFrom(scriptableObjectType) is false)
            {
                Debug.LogError(string.Format("ScriptableObject class called {0} not found", className));
                return;
            }

            ScriptableObject instance = ScriptableObject.CreateInstance(scriptableObjectType);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(string.Format("ScriptableObject {0} created in {1}", className, path));
        }

        private string ReplaceConstantsInFiles(string pPath)
        {
            string result = File.ReadAllText(pPath);

            result = result.Replace("CONST_ModuleName", _moduleName);
            result = result.Replace("CONST_ControllerName", string.Concat(_moduleName, "Controller"));
            result = result.Replace("CONST_ViewName", string.Concat(_moduleName, "View"));
            result = result.Replace("CONST_ModelName", string.Concat(_moduleName, "Model"));
            result = result.Replace("CONST_InterfaceName", string.Concat("I", _moduleName, "View"));

            return result;
        }

        #endregion

        #region Draw Windows Elements

        private void DrawButton(string pButton, Action pMethod)
        {
            if (GUILayout.Button(pButton))
                pMethod.Invoke();

            GUILayout.Space(5);
        }

        private void DrawField(string pTitle, ref string pId)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(pTitle);
                GUILayout.Space(10);
                pId = GUILayout.TextField(pId, GUILayout.MinWidth(150), GUILayout.MaxWidth(150));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(2.5f);
        }

        private void DrawTitle(string pText)
        {
            GUILayout.Label(pText, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleLeft, fontSize = 18 });
            EditorGUILayout.Space();
        }

        #endregion
    }
}