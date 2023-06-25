using UnityEditor;
using UnityEngine.UIElements;
using AJ.Generic.Tools;
namespace AJ.Generic.Configuration
{
    [CustomEditor(typeof(SwitchUIScreen))]
    public class SwitchUIScreen_Editor : Editor
    {      
        public StyleSheet styleSheet;
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement myInspector = new VisualElement();
            var createCotainer = new VisualElement();
            myInspector.styleSheets.Add(styleSheet);
            myInspector.Add(createCotainer);
            
            var registerCotainer = new VisualElement();
            registerCotainer.tooltip = "自定义注册名称，默认为游戏对象名称";
            registerCotainer.AddToClassList("asset-cotainer");
            createCotainer.Add(registerCotainer);
            var registerToggle = CreateToggle("Register Custom Name", "isRegister", registerCotainer);
            registerToggle.AddToClassList("asset-toggle-style");
            var registerTextField = CreateTextField("", "registerName", registerCotainer);
            registerTextField.AddToClassList("asset-half-width");

            registerToggle.RegisterCallback<AttachToPanelEvent>((evt) => {
                if (!registerToggle.value)
                {
                    registerTextField.style.display = DisplayStyle.None;
                }
                else
                {
                    registerTextField.style.display = DisplayStyle.Flex;
                }
            });
            registerToggle.RegisterValueChangedCallback((evt) => {
                if (!evt.newValue)
                {
                    registerTextField.style.display = DisplayStyle.None;
                } else
                {
                    registerTextField.style.display = DisplayStyle.Flex;
                }
            });

            var defaultToggle = CreateToggle("Main Screen Switch", "isMainScreenSwitch", createCotainer);
            defaultToggle.tooltip = "设置为主UI屏幕切换器";

            return myInspector;
        }
        private Toggle CreateToggle(string label, string bindingPath, VisualElement cotainer)
        {
            var toggle = new Toggle(label);
            toggle.bindingPath = bindingPath;
            cotainer.Add(toggle);           
            return toggle;
        }
        private TextField CreateTextField(string label, string bindingPath, VisualElement cotainer)
        {
            var textField = new TextField(label);
            textField.bindingPath = bindingPath;
            cotainer.Add(textField);
            return textField;
        }
    }
}
