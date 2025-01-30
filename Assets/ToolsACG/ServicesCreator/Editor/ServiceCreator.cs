using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.Editor
{
    public class ServiceCreator : EditorWindow
    {
        #region Fields

        private static ServiceCreator _window;
        private string _serviceName;

        [Header("Paths")]
        private const string _templatesPath = "ToolsACG/ServicesCreator/Editor/Templates/";
        private const string _outputPath = "Game/Scripts/Services/";

        [Header("Template Name")]
        private const string _serviceTemplateFileName = "CONST_ServiceName.cs.template";

        #endregion

        #region Editor Window Management

        [MenuItem("Tools/ToolsACG/Create Service", false)]
        public static void OpenEditorWindow()
        {
            _window = EditorWindow.GetWindow<ServiceCreator>(true, "Service Creator");
            _window.minSize = new Vector2(300, 125);
            _window.maxSize = new Vector2(300, 125);
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
            DrawField("Service name", ref _serviceName);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUI.enabled = !string.IsNullOrEmpty(_serviceName);
            DrawButton("Create", CreateNewService);
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Creation

        private void CreateNewService()
        {
            string outputCompletePath = Path.Combine(Application.dataPath, _outputPath);

            DirectoryInfo servicesDirectory = Directory.CreateDirectory(Path.Combine(outputCompletePath));

            string templateCompletePath = Path.Combine(Application.dataPath, ServiceCreator._templatesPath);

            string serviceTemplatePath = string.Concat(templateCompletePath, _serviceTemplateFileName);

            string newServiceOutputPath = string.Concat(servicesDirectory.FullName, "/", _serviceTemplateFileName.Replace("CONST_ServiceName", string.Concat(_serviceName, "Service")).Replace(".template", string.Empty));

            File.Copy(serviceTemplatePath, newServiceOutputPath);

            string newServiceText = ReplaceConstantsInFiles(newServiceOutputPath);

            File.Delete(newServiceOutputPath);

            File.WriteAllText(newServiceOutputPath, newServiceText);

            AssetDatabase.Refresh();
        }

        private string ReplaceConstantsInFiles(string pPath)
        {
            string result = File.ReadAllText(pPath);

            result = result.Replace("CONST_ServiceName", _serviceName);

            return result;
        }

        #endregion

        #region Draw Window Elements

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

