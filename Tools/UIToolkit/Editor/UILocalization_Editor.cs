using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using AJ.Generic.Tools;
namespace AJ.Generic.Configuration
{
    [CustomEditor(typeof(UILocalization))]
    public class UILocalization_Editor : Editor
    {
        public StyleSheet styleSheet;
        public TextField customField;
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new VisualElement();
            var createCotainer = new VisualElement();
            myInspector.styleSheets.Add(styleSheet);
            myInspector.Add(createCotainer);

            var customCotainer = new VisualElement();
            customCotainer.AddToClassList("asset-cotainer");
            createCotainer.Add(customCotainer);
            var customToggle = CreateToggle("Custom Prefix", "isCustom", customCotainer, HandleCallbackWithCreate);
            customToggle.AddToClassList("asset-toggle-style");
            customField = new TextField();
            customField.bindingPath = "keyWord";
            customField.maxLength = 1;
            customField.AddToClassList("prefix-style");
            customToggle.RegisterCallback<AttachToPanelEvent>((evt) => {
                customField.isReadOnly = !customToggle.value;
                customField.focusable = customToggle.value;
                if (!customToggle.value)
                {
                    customField.AddToClassList("prefix-style-undisplay");
                }
                else
                {
                    customField.RemoveFromClassList("prefix-style-undisplay");
                }
            });
            customCotainer.Add(customField);

            var tableCotainer = new VisualElement();
            tableCotainer.AddToClassList("asset-cotainer");
            createCotainer.Add(tableCotainer);
            var table = new PropertyField();
            table.label = "Localized String Table";
            table.bindingPath = "_table";
            table.AddToClassList("asset-style");
            tableCotainer.Add(table);
            var openLocalizationBtn = new Button();
            openLocalizationBtn.clicked += () => { EditorApplication.ExecuteMenuItem("Window/Asset Management/Localization Tables"); };
            openLocalizationBtn.Add(new Label("L"));
            tableCotainer.Add(openLocalizationBtn);

            var assetCotainer = new VisualElement();
            assetCotainer.AddToClassList("asset-cotainer");
            createCotainer.Add(assetCotainer);
            var asset = new PropertyField();
            asset.label = "Localized Asset Table";
            asset.bindingPath = "_asset";
            asset.AddToClassList("asset-style");
            assetCotainer.Add(asset);
            var backgroundType = new EnumField();
            backgroundType.bindingPath = "backgroundType";
            assetCotainer.Add(backgroundType);
;
            var foldout = new Foldout();
            createCotainer.Add(foldout);
            var localeBtn = new Button();
            localeBtn.Add(new Label("Locale"));
            localeBtn.clicked += OnCreateLocalEnum;
            foldout.Add(localeBtn);

            // Return the finished inspector UI
            return myInspector;
        }
        private void OnCreateLocalEnum()
        {
            EditorApplication.ExecuteMenuItem(AJConfiguration.COUNTRY);
        }
        private void HandleCallbackWithCreate(ChangeEvent<bool> evt)
        {
            customField.isReadOnly = !evt.newValue;
            customField.focusable = evt.newValue;
            if (!evt.newValue) customField.AddToClassList("prefix-style-undisplay");
            else customField.RemoveFromClassList("prefix-style-undisplay");
        }

        private Toggle CreateToggle(string label, string bindingPath, VisualElement createCotainer, EventCallback<ChangeEvent<bool>> callback)
        {
            var toggle = new Toggle(label);
            createCotainer.Add(toggle);
            toggle.bindingPath = bindingPath;
            toggle.RegisterValueChangedCallback(callback);
            return toggle;
        }
    }
}
