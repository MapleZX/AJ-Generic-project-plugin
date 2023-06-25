using System;
namespace AJ.Generic.Tools
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class LoadAttribute : Attribute
    {
        public string _name;

        public LoadAttribute(string _name = "")
        {
            this._name = _name;
        }
    }
}
