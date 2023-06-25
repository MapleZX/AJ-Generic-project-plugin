using System;
namespace AJ.Generic.Tools
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]

    public sealed class AddressableAddressAttribute : Attribute
    {
        // public bool isLabelKey;
        // public AddressableAddressAttribute(bool isLabelKey = false)
        // {
        //     this.isLabelKey = isLabelKey;
        // }
        public string LabelKey;
        public AddressableAddressAttribute(string LabelKey = "")
        {
            this.LabelKey = LabelKey;
        }
    }
}
