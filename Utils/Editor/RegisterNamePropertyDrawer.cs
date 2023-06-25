using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AJ.Generic.Utils
{
    [CustomPropertyDrawer(typeof(RegisterNameIngredient))]
    public class RegisterNamePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var isCustom = property.FindPropertyRelative("isCustom");
            var registerName = property.FindPropertyRelative("registerName");
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var customRect = new Rect(position.x, position.y, 90, position.height);
            var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);
            isCustom.boolValue = EditorGUI.ToggleLeft(customRect, "Use Custom", isCustom.boolValue);
            if (isCustom.boolValue) 
                EditorGUI.PropertyField(nameRect, registerName, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}