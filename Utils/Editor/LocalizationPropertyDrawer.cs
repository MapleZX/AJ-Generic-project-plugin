using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AJ.Generic.Utils
{
    [CustomPropertyDrawer(typeof(LocalizationAttribute))]
    public class LocalizationPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (EditorGUI.LinkButton(position, label))
            {
                EditorApplication.ExecuteMenuItem("Window/Asset Management/Localization Tables");
            }
        }
    }
}