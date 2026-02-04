using ACG.Core.Models;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRange))]
public class RangeFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, label);

        var minProp = property.FindPropertyRelative("Min");
        var maxProp = property.FindPropertyRelative("Max");

        float width = position.width / 2f;
        var minRect = new Rect(position.x, position.y, width - 2, position.height);
        var maxRect = new Rect(position.x + width + 2, position.y, width - 2, position.height);

        EditorGUI.PropertyField(minRect, minProp, GUIContent.none);
        EditorGUI.PropertyField(maxRect, maxProp, GUIContent.none);

        EditorGUI.EndProperty();
    }
}