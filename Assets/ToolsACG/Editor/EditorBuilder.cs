using System;
using UnityEditor;
using UnityEngine;

namespace ToolsACG.EditorTools
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
            EditorGUILayout.Space();
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
            EditorGUILayout.Space();
        }

        #endregion

        #region Dropdown

        public static void Dropdown<TEnum>(ref int index, string labelName) where TEnum : Enum
        {
            string[] enumNames = Enum.GetNames(typeof(TEnum));

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(labelName);
            GUILayout.Space(10);
            index = EditorGUILayout.Popup(index, enumNames, GUILayout.MinWidth(50), GUILayout.MaxWidth(5000));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
        public static void Dropdown<TEnum>(ref TEnum currentValue, string labelName) where TEnum : Enum
        {
            string[] enumNames = Enum.GetNames(typeof(TEnum));
            int currentIndex = Array.IndexOf(enumNames, currentValue.ToString());

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(labelName);
            GUILayout.Space(10);
            int newIndex = EditorGUILayout.Popup(currentIndex, enumNames, GUILayout.MinWidth(50), GUILayout.MaxWidth(5000));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            currentValue = (TEnum)Enum.Parse(typeof(TEnum), enumNames[newIndex]);
        }

        #endregion

        #region Fields

        public static void TextField(string title, ref string id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title);
            GUILayout.Space(10);
            id = GUILayout.TextField(id, GUILayout.MinWidth(50), GUILayout.MaxWidth(5000));
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
        public static string TextField(string title, string value)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(title);
            GUILayout.Space(10);
            value = EditorGUILayout.TextField(value, GUILayout.MinWidth(50), GUILayout.MaxWidth(5000));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

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

        #region HelpBox

        public static void HelpBox(string text, MessageType type)
        {
            EditorGUILayout.HelpBox(text, type);
            EditorGUILayout.Space();
        }

        #endregion
    }
}
