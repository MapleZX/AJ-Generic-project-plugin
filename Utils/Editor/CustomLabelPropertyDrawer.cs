using UnityEngine;
using UnityEditor;
namespace AJ.Generic.Utils
{
    [CustomPropertyDrawer(typeof(CustomLabelAttribute))]
    public class CustomLabelPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                var propertyAttribute = this.attribute as CustomLabelAttribute;
                if (IsItBloodyArrayTho(property) == false)
                {
                    label.text = propertyAttribute.label;
                }
                else
                {
                    Debug.LogWarningFormat(
                        "{0}(\"{1}\") doesn't support arrays ",
                        typeof(CustomLabelAttribute).Name,
                        propertyAttribute.label
                    );
                }
                EditorGUI.PropertyField(position, property, label);
            }
            catch (System.Exception ex) { Debug.LogException(ex); }
        }
        private bool IsItBloodyArrayTho(SerializedProperty property)
        {
            string path = property.propertyPath;
            int idot = path.IndexOf('.');
            if (idot == -1) return false;
            string propName = path.Substring(0, idot);
            SerializedProperty p = property.serializedObject.FindProperty(propName);
            return p.isArray;
        }
    }
}
