using UnityEditor;
using UnityEngine;

namespace AJ.Generic.Utils
{
    [CustomPropertyDrawer(typeof(TagPropertyAttribute))]
    public class TagPropertyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
}
