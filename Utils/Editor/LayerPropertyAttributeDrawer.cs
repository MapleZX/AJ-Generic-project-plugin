using UnityEditor;
using UnityEngine;

namespace AJ.Generic.Utils
{
    [CustomPropertyDrawer(typeof(LayerPropertyAttribute))]
    public class LayerPropertyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}