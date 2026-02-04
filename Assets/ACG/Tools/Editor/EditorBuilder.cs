using System;
using UnityEditor;
using UnityEngine;

namespace ACG.Tools.Editor.Builder
{
    public static class EditorBuilder
    {
        #region Text

        public static void Title(string text, GUIStyle style = null)
        {
            style ??= new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                fontSize = 18
            };

            GUILayout.Label(text, style);
            Space();
        }
        public static void Label(string text, GUIStyle style = null)
        {
            style ??= new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12
            };

            GUILayout.Label(text, style);
            Space();
        }

        public static void NameValueRow(int width, string name, string value)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(width));
            Space(10);
            GUILayout.Label(name, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft });
            GUILayout.Label(value, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleRight });
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Dropdown

        public static void Dropdown<TEnum>(ref int index, string labelName) where TEnum : Enum
        {
            string[] enumNames = Enum.GetNames(typeof(TEnum));

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(labelName);
            Space(10);
            index = EditorGUILayout.Popup(index, enumNames, GUILayout.MinWidth(50), GUILayout.MaxWidth(5000));
            EditorGUILayout.EndHorizontal();
            Space();
        }
        public static void Dropdown<TEnum>(ref TEnum currentValue, string labelName) where TEnum : Enum
        {
            string[] enumNames = Enum.GetNames(typeof(TEnum));
            int currentIndex = Array.IndexOf(enumNames, currentValue.ToString());

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(labelName);
            Space(10);
            int newIndex = EditorGUILayout.Popup(currentIndex, enumNames, GUILayout.MinWidth(50), GUILayout.MaxWidth(5000));
            EditorGUILayout.EndHorizontal();
            Space();

            currentValue = (TEnum)Enum.Parse(typeof(TEnum), enumNames[newIndex]);
        }

        #endregion

        #region Fields

        public static void TextField(string title, ref string id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title);
            Space(10);
            id = GUILayout.TextField(id, GUILayout.MinWidth(50), GUILayout.MaxWidth(5000));
            GUILayout.EndHorizontal();
            Space();
        }
        public static string TextField(string title, string value)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(title);
            Space(10);
            value = EditorGUILayout.TextField(value, GUILayout.MinWidth(50), GUILayout.MaxWidth(5000));
            EditorGUILayout.EndHorizontal();
            Space();

            return value;
        }

        #endregion

        #region Buttons

        public static void Button(string button, Action action)
        {
            if (GUILayout.Button(button))
                action.Invoke();
        }

        #endregion

        #region Miscelaneous

        public static void AddHorizontalLine(Color lineColor)
        {
            GUI.backgroundColor = lineColor;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUI.backgroundColor = Color.white;
        }

        public static void Space(int pixels = 0)
        {
            if (pixels == 0)
                EditorGUILayout.Space();
            else
                GUILayout.Space(pixels);
        }

        #endregion

        #region HelpBox

        public static void HelpBox(string text, MessageType type)
        {
            EditorGUILayout.HelpBox(text, type);
            Space();
        }

        #endregion
    }
}
