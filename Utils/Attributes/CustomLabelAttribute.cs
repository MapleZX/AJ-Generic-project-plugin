using UnityEngine;
namespace AJ.Generic.Utils
{
    public class CustomLabelAttribute : PropertyAttribute
    {
        public string label;
        public CustomLabelAttribute(string label)
        {
            this.label = label;
        }
    }
}