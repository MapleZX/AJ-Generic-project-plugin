using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using AJ.Generic.Tools;
namespace AJ.Generic.Configuration
{
    [CustomEditor(typeof(UIController))]
    public class UIController_Editor : Editor
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
            
            var screenToggle = CreateToggle("Main Screen Switch", "isMainScreenName", createCotainer);
            screenToggle.AddToClassList("asset-toggle-style");
            screenToggle.tooltip = "使用主屏幕UI切换";
            var screenCotainer = new Foldout();
            screenCotainer.text = "Switch UI Screen";
            screenCotainer.tooltip = "使用主屏幕UI切换";
            // screenCotainer.AddToClassList("asset-cotainer");
            createCotainer.Add(screenCotainer);
            var screenTextField = CreateTextField("Name", "screenName", screenCotainer);
            screenTextField.tooltip = "使用主屏幕UI切换名称，如果存在优先使用";
            // screenTextField.AddToClassList("asset-half-width");
            var screenObject = new ObjectField("Switch UI Screen");
            screenObject.objectType = typeof(SwitchUIScreen);
            screenObject.tooltip = "使用主屏幕UI切换对象";
            // screenObject.AddToClassList("asset-half-width");
            screenObject.bindingPath = "screenUISwitch";
            screenCotainer.Add(screenObject);

            screenToggle.RegisterCallback<AttachToPanelEvent>((evt) => {
                if (screenToggle.value)
                {
                    screenCotainer.style.display = DisplayStyle.None;
                }
                else
                {
                    screenCotainer.style.display = DisplayStyle.Flex;
                }
            });
            screenToggle.RegisterValueChangedCallback((evt) => {
                if (evt.newValue)
                {
                    screenCotainer.style.display = DisplayStyle.None;
                } else
                {
                    screenCotainer.style.display = DisplayStyle.Flex;
                }
            });

            var defaultToggle = CreateToggle("UI Display", "isDefaultScreen", createCotainer);
            defaultToggle.tooltip = "显示该UI屏幕";

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
