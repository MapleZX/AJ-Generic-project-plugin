using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace AJ.Generic.Configuration
{
    public static class AutoSceneKeyBuildTemplate
    {
        public static string KeyClass =
    @"namespace AJ.Generic.Tools.Keys
{
    public enum #类名#
    {
#枚举#
    }
}";
    }
    public class CreateAddress : EditorWindow
    {
        [MenuItem(AJConfiguration.ADDRESSABLE, false, 0)]
        public static void ShowExample()
        {
            CreateAddress wnd = GetWindow<CreateAddress>();
            wnd.titleContent = new GUIContent("Create Address");
        }
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
            var createCotainer = new VisualElement();
            var config = AJConfiguration.Configuration();
            config.CotainerStyle(createCotainer);
            root.Add(createCotainer);

            var createBtn = CreateButton("Scene Key");
            createBtn.clicked += config.OnCreateSceneKeyButton;
            createCotainer.Add(createBtn);

            var createKeyWithAddressableBtn = CreateButton("Addressable Address");
            createKeyWithAddressableBtn.clicked += config.OnCreateAddressableKeyButton;
            createCotainer.Add(createKeyWithAddressableBtn);
        }
        private Button CreateButton(string label)
        {
            var btn = new Button();
            var config = AJConfiguration.Configuration();
            btn.text = label;
            config.ButtonStyle(btn);
            return btn;
        }
    }
}