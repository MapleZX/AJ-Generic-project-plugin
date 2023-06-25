using System;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Tools;

namespace AJ.Generic.Configuration
{
    public class CreateDataKey : EditorWindow
    {       
        [MenuItem(AJConfiguration.PASSWORD, false, 1)]
        public static void ShowExample()
        {
            CreateDataKey wnd = GetWindow<CreateDataKey>();
            wnd.titleContent = new GUIContent("Create Data Password");
        }

        //private string Key = DataPassword.dataKey;
        //private string IV = DataPassword.dataIV;
        private TextField keyField;
        private TextField ivField;
        public StyleSheet styleSheet;
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            // var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Generic project plugin/Editor/CreateDataKey.uxml");
            // VisualElement labelFromUXML = visualTree.Instantiate();
            // root.Add(labelFromUXML);
            var labelFromUXML = new VisualElement();
            root.Add(labelFromUXML);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            // var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Generic project plugin/Editor/CreateDataKey.uss");
            root.styleSheets.Add(styleSheet);
            labelFromUXML.AddToClassList("label-from-UXML");

            var createCotainer = new VisualElement();
            createCotainer.AddToClassList("label-from-UXML");
            labelFromUXML.Add(createCotainer);
            var createDataKeyCotainer = new VisualElement();
            var createDataIVCotainer = new VisualElement();
            createDataKeyCotainer.AddToClassList("data-cotainer");
            createDataIVCotainer.AddToClassList("data-cotainer");
            createCotainer.Add(createDataKeyCotainer);
            createCotainer.Add(createDataIVCotainer);

            var config = AJConfiguration.Configuration();
            if (PlayerPrefs.HasKey("Password Key"))
                config.Key = PlayerPrefs.GetString("Password Key");
            if (PlayerPrefs.HasKey("Password IV"))
                config.IV = PlayerPrefs.GetString("Password IV");

            keyField = new TextField("Key64");
            keyField.value = config.Key;
            // keyField.AddToClassList("label-from-UXML");
            createDataKeyCotainer.Add(keyField);

            ivField = new TextField("IV64");
            ivField.value = config.IV;
            // ivField.AddToClassList("label-from-UXML");
            createDataIVCotainer.Add(ivField);

            var keyBtn = new Button();
            var keyLabel = new Label("Refresh");
            keyBtn.Add(keyLabel);
            keyBtn.clicked += (() =>
            {
                using (AesManaged aesManaged = new AesManaged())
                {
                    config.Key = Convert.ToBase64String(aesManaged.Key);
                    keyField.value = config.Key;
                }
            });
            createDataKeyCotainer.Add(keyBtn);

            var ivBtn = new Button();
            var ivLabel = new Label("Refresh");
            ivBtn.Add(ivLabel);
            ivBtn.clicked += (() =>
            {
                using (AesManaged aesManaged = new AesManaged())
                {
                    config.IV = Convert.ToBase64String(aesManaged.IV);
                    ivField.value = config.IV;
                }
            });
            createDataIVCotainer.Add(ivBtn);
            
            var createBtn = new Button();
            var createLabel = new Label("Create");
            createBtn.Add(createLabel);
            createBtn.clicked += config.OnCreateSaveDataPassword;
        
            createCotainer.Add(createBtn);

            var resetBtn = new Button();
            var resetLabel = new Label("Reset");
            resetBtn.Add(resetLabel);
            resetBtn.clicked += (() =>
            {
                if (PlayerPrefs.HasKey("Password Key"))
                    config.Key = PlayerPrefs.GetString("Password Key");
                if (PlayerPrefs.HasKey("Password IV"))
                    config.IV = PlayerPrefs.GetString("Password IV");
                keyField.value = config.Key;
                ivField.value = config.IV;
            });
            createCotainer.Add(resetBtn);
        }
    }
}