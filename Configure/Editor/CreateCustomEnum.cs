using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using AJ.Generic.Tools;
namespace AJ.Generic.Configuration
{
    public class CreateCustomEnum : EditorWindow
    {
        [MenuItem(AJConfiguration.CUSTOMENUM, false, 4)]
        public static void ShowExample()
        {
            CreateCustomEnum wnd = GetWindow<CreateCustomEnum>();
            wnd.titleContent = new GUIContent("Create Custom Enum");
        }
        private TextField output;
        private TextField outputName;
        private ObjectField input;
        private ObjectField input1;
        private string outputKey = "Custom output path";
        private string outputNameKey = "Custom output name";
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            var createCotainer = new VisualElement();
            // AJConfiguration.CotainerStyle(createCotainer);
            root.Add(createCotainer);

            if (!PlayerPrefs.HasKey(outputKey)) PlayerPrefs.SetString(outputKey, "");
            if (!PlayerPrefs.HasKey(outputNameKey)) PlayerPrefs.SetString(outputNameKey, "");
            input = new ObjectField("Text Input");
            input.objectType = typeof(TextAsset);
            createCotainer.Add(input);
            input1 = new ObjectField("Script Input");
            input1.objectType = typeof(ScriptableObject);
            createCotainer.Add(input1);
            output = new TextField("Output");
            output.RegisterCallback<AttachToPanelEvent>(evt => output.value = PlayerPrefs.GetString(outputKey));
            createCotainer.Add(output);
            outputName = new TextField("Class name");
            outputName.RegisterCallback<AttachToPanelEvent>(evt => outputName.value = PlayerPrefs.GetString(outputNameKey));
            createCotainer.Add(outputName);
            var createBtn = CreateButton("Create");
            createCotainer.Add(createBtn);
            // createBtn.clicked += OnCreate;
            createBtn.clicked += OnCreateFromScript;
            createBtn.clicked += OnCreateFromText;
        }
        // public void OnCreate()
        // {
        //     // Debug.Log(input1.value.GetType());
        // }
        public void OnCreateFromScript()
        {        
            if (input1?.value == null) return;
            var config = AJConfiguration.Configuration();
            var type = input1?.value.GetType();
            var fields = type.GetFields(
                BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var enumsDic = new Dictionary<string, List<string>>();
            var attrType = typeof(CustomEnumAttribute);
            foreach (var field in fields)
            {
                bool isAttribute = Attribute.IsDefined(field, attrType);
                if (isAttribute)
                {
                    var attr = (CustomEnumAttribute)Attribute.GetCustomAttribute(field, attrType);
                    var _className = attr.className == "" ? (input1?.value.name + "Style").Replace(" ", "") : attr.className.Replace(" ", "");
                    var value = field.GetValue(input1?.value);
                    if (typeof(string).IsInstanceOfType(value))
                    {
                        if (enumsDic.ContainsKey(_className))
                        {
                            enumsDic[_className].Add(field.Name);
                        }
                        else
                        {
                            enumsDic[_className] = new List<string>();
                            enumsDic[_className].Add(field.Name);
                        }
                    }
                }
            }
            var op = output.value == "" ? "/" : output.value;
            foreach (var e in enumsDic)
            {
                var path = Application.dataPath + "/" + op + "/" + e.Key + ".cs";
                config.OnCreateCustomEnum(path, e.Key, e.Value);
            }
            // var op = output.value == "" ? "/" : output.value;
            //var className = outputName.value == "" ? (input1?.value.name + "Style").Replace(" ", "") : outputName.value;
            // var path = Application.dataPath + "/" + op + "/" + className + ".cs";
            var className = outputName.value == "" ? (input1?.value.name + "Style").Replace(" ", "") : outputName.value;
            PlayerPrefs.SetString(outputKey, op);
            PlayerPrefs.SetString(outputNameKey, className);
        }
        public void OnCreateFromText()
        {
            if (input?.value == null) return;
            var config = AJConfiguration.Configuration();
            var enums = (input.value as TextAsset).text.Split(new char[] { '\n' }).ToList();
            var op = output.value == "" ? "/" : output.value;
            var className = outputName.value == "" ? (input1?.value.name + "Style").Replace(" ", "") : outputName.value;
            var path = Application.dataPath + "/" + op + "/" + className + ".cs";
            config.OnCreateCustomEnum(path, className, enums);
            PlayerPrefs.SetString(outputKey, op);
            PlayerPrefs.SetString(outputNameKey, className);
        }
        private Button CreateButton(string label)
        {
            var btn = new Button();
            btn.text = label;
            return btn;
        }
    }
}
