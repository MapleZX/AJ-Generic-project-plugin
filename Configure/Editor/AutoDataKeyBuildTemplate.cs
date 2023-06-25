namespace AJ.Generic.Configuration
{  
    public static class AutoDataKeyBuildTemplate
    {
        public static string KeyClass =
@"using System;
namespace AJ.Generic.Tools
{
    public static class #类名#
    {
        public const string dataKey = #key#;
        public const string dataIV = #IV#;
    }
}";
    }
}