using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Localization.Settings;
using UnityEngine;
using System.IO;
using AJ.Generic.Tools;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Configuration
{
    [CreateAssetMenu(fileName = "AJConfiguration", menuName = "AJ Generic Tools/AJConfiguration", order = 1)]
    public class AJConfiguration : ScriptableObject
    {        
        public static AJConfiguration Configuration()
        {
            var config = (AJConfiguration) AssetDatabase
                .LoadAssetAtPath(_ConfigurationAddress, typeof(AJConfiguration));
            return config;
        }
        [Header("AJ Configuration")]
        public const string ASSETSNAME = "Assets";
        public const string AJNAME = "AJ Generic project plugin";
        public const string _ConfigurationAddress 
            = ASSETSNAME + "/" + AJNAME + "/" + "Configure/Editor/AJConfiguration.asset";

        [Header("AJ Key Address")]
        [SerializeField, CustomLabel("Address")] private string address 
            = AJNAME + "/AJ Keys/";
        [SerializeField, CustomLabel("Scene Key")] private string sceneKey 
            = AJNAME + "/AJ Keys/";
        [SerializeField, CustomLabel("Country Code")] private string countryCode 
            = AJNAME + "/AJ Keys/";
        [SerializeField, CustomLabel("Data password")] private string password 
            = AJNAME + "/AJ Keys/";
        
        [Header("Sceen Configure")]
        [SerializeField, CustomLabel("Demo Sceen")] private DemoScene demoScene;
        public DemoScene DemoScene => demoScene;
        
        [Header("AJ Key MENU")]
        public const string WINDOWMENU = "Window";
        public const string ADDRESSABLE = WINDOWMENU + "/AJ Generic/Create Address";
        public const string COUNTRY = WINDOWMENU + "/AJ Generic/Create Locale";
        public const string PASSWORD = WINDOWMENU + "/AJ Generic/Create Data Password";
        public const string SCENE = WINDOWMENU + "/AJ Generic/Demo Scene Settings";
        public const string CUSTOMENUM = WINDOWMENU + "/AJ Generic/Create Custom Enum";

        private string key;
        private string iv;
        public string AddressPath { 
            get {
                var _path = CreateAJKeys(address);
                return _path + Address + ".cs"; 
            } 
        }
        public string Address => "AddressableAddress";

        public string SceneKeyPath { 
            get {
                var _path = CreateAJKeys(sceneKey);
                return _path + SceneKey + ".cs";
            } 
        }
        public string SceneKey => "SceneKey";

        public string CountryCodePath {
            get {
                var _path = CreateAJKeys(countryCode);
                return _path + CountryCode + ".cs";
            }
        }
        public string CountryCode => "CountryCode";

        public string PasswordPath {
            get {
                var _path = CreateAJKeys(password);
                return _path + Password + ".cs";
            }
        }
        public string Password => "DataPassword";

        public string Key { get => key; set => key = value; }
        public string IV { get => iv; set => iv = value; }

        private string CreateAJKeys(string paths) 
        {
            this.CreateDataDirectory(Application.dataPath + "/" + paths);
            return Application.dataPath + "/" + paths + "/";
        }
        /// <summary>
        /// 创建场景索引。
        /// </summary>
        public void OnCreateSceneKeyButton()
        {
            var scriptPath = SceneKeyPath;
            var classStr = AutoSceneKeyBuildTemplate.KeyClass;
            if (File.Exists(scriptPath))
            {
                File.Delete(scriptPath);
            }
            else
            {
                classStr = AutoSceneKeyBuildTemplate.KeyClass;
            }
            classStr = classStr.Replace("#类名#", SceneKey);
            var enumStr = new System.Text.StringBuilder();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var path = EditorBuildSettings.scenes[i].path;
                if (path != "")
                {
                    var regex = new System.Text.RegularExpressions.Regex("/");
                    var last = regex.Split(path)[regex.Split(path).Length - 1];
                    regex = new System.Text.RegularExpressions.Regex(@"\.");
                    var scene = regex.Split(last)[0];
                    var _tap = "\t\t";
                    var enumBuilder = new System.Text.StringBuilder();
                    enumBuilder.Append(_tap);
                    enumBuilder.Append(scene);
                    enumBuilder.Append(" = ");
                    enumBuilder.Append(i);
                    // enumBuilder.Append(",");
                    if (i != EditorBuildSettings.scenes.Length - 1)
                        enumStr.AppendLine(enumBuilder.ToString() + ",");
                    else
                        enumStr.Append(enumBuilder);
                }
            }
            classStr = classStr.Replace("#枚举#", enumStr.ToString());
            using (FileStream file = new FileStream(scriptPath, FileMode.CreateNew))
            {
                using (StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    fileW.Write(classStr);
                    fileW.Flush();
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 创建可寻址对象地址。
        /// </summary>
        public void OnCreateAddressableKeyButton()
        {
            var scriptPath = AddressPath;
            var classBulid = new System.Text.StringBuilder();
            classBulid.AppendLine("namespace AJ.Generic.Tools.Keys");
            classBulid.AppendLine("{");
            var setting = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            if (setting != null)
            {
                foreach (var group in setting.groups)
                {
                    if (group.name != "Built In Data")
                    {
                        if (!group.name.ToLower().Contains(new string("-").ToLower()))
                        {
                            if (group.name == "Default Local Group")
                            {
                                classBulid.AppendLine("\t" + "public class " + Address);
                                classBulid.AppendLine("\t" + "{");
                                classBulid.AppendLine("" + @"#标签#");
                            }
                            else
                            {
                                classBulid.AppendLine("\t" + "public class " + group.name.Replace(" ", ""));
                                classBulid.AppendLine("\t" + "{");
                            }
                            foreach (var e in group.entries)
                            {
                                if (!e.address.ToLower().Contains(new string("-").ToLower()))
                                {
                                    classBulid.Append("\t\t");
                                    classBulid.Append(@"// ");
                                    classBulid.Append("Address Path:");
                                    classBulid.Append(e.AssetPath);
                                    classBulid.AppendLine(";");
                                    classBulid.Append("\t\t");
                                    classBulid.Append("public const string ");
                                    var field = e.address;
                                    field = field.Replace(" ", "");
                                    field = field.Replace(".", "");
                                    field = field.Replace("-", "");
                                    field = field.Replace("/", "");
                                    field = field.Replace("\\", "");
                                    classBulid.Append(field);
                                    classBulid.Append(" = ");
                                    classBulid.Append("\"");
                                    classBulid.Append(e.address);
                                    classBulid.Append("\"");
                                    classBulid.AppendLine(";");
                                }
                            }
                            classBulid.AppendLine("\t" + "}");
                        }
                    }
                }
                classBulid.Append("}");
                var labels = new System.Text.StringBuilder();
                foreach (var label in setting.GetLabels())
                {
                    if (!label.ToLower().Contains(new string("-").ToLower()))
                    {
                        labels.Append("\t\t");
                        labels.Append("public const string ");
                        var l = label.Replace(" ", "");
                        l = l.Replace(" ", "");
                        l = l.Replace(".", "");
                        l = l.Replace("-", "");
                        l = l.Replace("/", "");
                        l = l.Replace("\\", "");
                        labels.Append(l + "Label");
                        labels.Append(" = ");
                        labels.Append("\"");
                        labels.Append(label);
                        labels.Append("\"");
                        labels.AppendLine(";");
                    }
                }
                classBulid.Replace("#标签#", labels.ToString());
            }
            if (File.Exists(scriptPath))
            {
                File.Delete(scriptPath);
            }
            using (FileStream file = new FileStream(scriptPath, FileMode.CreateNew))
            {
                using (StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    fileW.Write(classBulid.ToString());
                    fileW.Flush();
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 创建加密秘密。
        /// </summary>
        public void OnCreateSaveDataPassword()
        {
            var scriptPath = PasswordPath;
            string classStr = AutoDataKeyBuildTemplate.KeyClass;
            if (File.Exists(scriptPath))
            {
                File.Delete(scriptPath);
            }
            else
            {
                classStr = AutoDataKeyBuildTemplate.KeyClass;
            }
            classStr = classStr.Replace("#类名#", Password);
            classStr = classStr.Replace("#key#", "\"" + Key.ToString() + "\"");
            classStr = classStr.Replace("#IV#", "\"" + IV.ToString() + "\"");
            using (FileStream file = new FileStream(scriptPath, FileMode.CreateNew))
            {
                using (StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    fileW.Write(classStr);
                    fileW.Flush();
                }
            }
            PlayerPrefs.SetString("Password Key", Key);
            PlayerPrefs.SetString("Password IV", IV);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public void OnCreateCustomEnum(string scriptPath, string className, List<string> fields)
        {
            var classStr = AutoSceneKeyBuildTemplate.KeyClass;
            if (File.Exists(scriptPath))
            {
                File.Delete(scriptPath);
            }
            else
            {
                classStr = AutoSceneKeyBuildTemplate.KeyClass;
            }
            classStr = classStr.Replace("#类名#", className);
            var enumStr = new System.Text.StringBuilder();
            var _tap = "\t\t";
            // var noneBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (field != "")
                {
                    var enumBuilder = new System.Text.StringBuilder();
                    enumBuilder.Append(_tap);
                    enumBuilder.Append(field);
                    enumBuilder.Append(" = ");
                    enumBuilder.Append(i);
                    if (i < fields.Count - 1)
                        enumStr.AppendLine(enumBuilder.ToString() + ",");
                    else
                        enumStr.Append(enumBuilder);

                }
            }
            classStr = classStr.Replace("#枚举#", enumStr.ToString());
            using (FileStream file = new FileStream(scriptPath, FileMode.CreateNew))
            {
                using (StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    fileW.Write(classStr);
                    fileW.Flush();
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public void OnCreateLocalEnum()
        {
            var scriptPath = CountryCodePath;
            var classStr = AutoSceneKeyBuildTemplate.KeyClass;
            if (File.Exists(scriptPath))
            {
                File.Delete(scriptPath);
            }
            else
            {
                classStr = AutoSceneKeyBuildTemplate.KeyClass;
            }
            classStr = classStr.Replace("#类名#", CountryCode);
            var enumStr = new System.Text.StringBuilder();
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
            {
                var country = LocalizationSettings.AvailableLocales.Locales[i];
                if (country != null)
                {
                    var codes = country.ToString().Split('(');
                    var last = codes[codes.Length - 1].TrimEnd(')').Split('-');
                    var code = codes[0].TrimEnd(' ') + '_' + last[0];
                    for (int j = 1; j < last.Length; j++)
                    {
                        code += ('_' + last[j]);
                    }
                    var _tap = "\t\t";
                    var enumBuilder = new System.Text.StringBuilder();
                    enumBuilder.Append(_tap);
                    enumBuilder.Append(code);
                    enumBuilder.Append(" = ");
                    enumBuilder.Append(i);
                    if (i != LocalizationSettings.AvailableLocales.Locales.Count - 1)
                        enumStr.AppendLine(enumBuilder.ToString() + ",");
                    else
                        enumStr.Append(enumBuilder);
                }
            }
            classStr = classStr.Replace("#枚举#", enumStr.ToString());
            using (FileStream file = new FileStream(scriptPath, FileMode.CreateNew))
            {
                using (StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    fileW.Write(classStr);
                    fileW.Flush();
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #region Menu Style
        public void CotainerStyle(VisualElement cotainer)
        {
            cotainer.style.flexDirection = FlexDirection.Column;
            cotainer.style.flexGrow = 1;
            cotainer.style.alignItems = Align.Center;
            cotainer.style.justifyContent = Justify.Center;
            cotainer.style.flexWrap = Wrap.Wrap;
        }
        public void ButtonStyle(Button btn)
        {
            btn.style.width = 192;
            btn.style.height = 64;
            btn.style.borderBottomWidth = 2;
            btn.style.borderBottomColor = Color.black;
        }
        #endregion
    }
}
