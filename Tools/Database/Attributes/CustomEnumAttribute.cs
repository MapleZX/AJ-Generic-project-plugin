using System;
namespace AJ.Generic.Tools
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CustomEnumAttribute : Attribute
    {
        public string className;
        public CustomEnumAttribute(string className = "")
        {
            this.className = className;
        }
    }
}
