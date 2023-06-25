using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace AJ.Generic.Configuration
{
    public class CreateLocale : EditorWindow
    {
        public StyleSheet styleSheet;
        [MenuItem(AJConfiguration.COUNTRY, false, 3)]
        public static void ShowExample()
        {
            CreateLocale wnd = GetWindow<CreateLocale>();
            wnd.titleContent = new GUIContent("CreateLocale");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            var createCotainer = new VisualElement();
            root.Add(createCotainer);

            var config = AJConfiguration.Configuration();

            var localeBtn = new Button();
            localeBtn.Add(new Label("Locale"));
            localeBtn.clicked += config.OnCreateLocalEnum;
            createCotainer.Add(localeBtn);
        }
    }
}