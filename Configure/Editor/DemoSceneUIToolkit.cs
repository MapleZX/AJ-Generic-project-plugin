using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace AJ.Generic.Configuration
{
    public class DemoSceneUIToolkit : EditorWindow
    {
        [MenuItem(AJConfiguration.SCENE)]
        public static void ShowExample()
        {
            DemoSceneUIToolkit wnd = GetWindow<DemoSceneUIToolkit>();
            wnd.titleContent = new GUIContent("Demo Scene Settings");
        }
        private Foldout demoFoldout;
        private List<ObjectField> demos = new List<ObjectField>();
        private DemoScene demoScene;
        private ObjectField initializeScene;
        public StyleSheet styleSheet;
        public void CreateGUI()
        {         
            VisualElement root = rootVisualElement;           
            var labelFromUXML = new VisualElement();
            root.Add(labelFromUXML);
            root.styleSheets.Add(styleSheet);
            labelFromUXML.AddToClassList("label-from-UXML");

            var scrollContainer = new ScrollView();
            labelFromUXML.Add(scrollContainer);
            scrollContainer.AddToClassList("scroll-view");
            var config = AJConfiguration.Configuration();
            demoScene = config.DemoScene;
            initializeScene = new ObjectField("Initialize Scene");
            initializeScene.objectType = typeof(SceneAsset);
            initializeScene.value = demoScene.initializeScene;
            initializeScene.RegisterCallback<ChangeEvent<UnityEngine.Object>>((e) => {
                demoScene.initializeScene = initializeScene.value as SceneAsset;
                EditorUtility.SetDirty(demoScene);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            });
            scrollContainer.Add(initializeScene);

            demoFoldout = new Foldout();
            demoFoldout.text = "Demo Scene";
            scrollContainer.Add(demoFoldout);
            if (demoScene.demos.Any())
            {
                for (int i = 0; i < demoScene.demos.Count; i++)
                {
                    var objectField = new ObjectField("Demo Scene " + i);
                    objectField.objectType = typeof(SceneAsset);
                    var scene = demoScene.demos[i];
                    objectField.value = scene;
                    demos.Add(objectField);
                    objectField.RegisterCallback<ChangeEvent<UnityEngine.Object>> ((e) => {
                        // scene = e.newValue as SceneAsset;
                        for (int j = 0; j < demos.Count; j++)
                        {
                            demoScene.demos[j] = demos[j].value as SceneAsset;
                        }
                        EditorUtility.SetDirty(demoScene);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    });
                    demoFoldout.contentContainer.Add(objectField);
                }
            }

            var addObjectCotainer = new VisualElement();
            labelFromUXML.Add(addObjectCotainer);
            addObjectCotainer.AddToClassList("add-object-cotainer");
            var addLeftCotainer = new VisualElement();
            addLeftCotainer.AddToClassList("add-left-cotainer");
            var addRightCotainer = new VisualElement();
            addRightCotainer.AddToClassList("add-right-cotainer"); 
            addObjectCotainer.Add(addLeftCotainer);
            addObjectCotainer.Add(addRightCotainer);
            var addBtn = new Button();
            var addLabel = new Label("+");
            addBtn.Add(addLabel);
            addBtn.clicked += OnAddObjectButton;
            var removeBtn = new Button();
            var removeLabel = new Label("-");
            removeBtn.Add(removeLabel);
            removeBtn.clicked += OnRemoveObjectButton;
            addRightCotainer.Add(addBtn);
            addRightCotainer.Add(removeBtn);          
        }
        private void OnRemoveObjectButton()
        {
            if (demos.Any() && demoScene.demos.Any())
            {
                demoFoldout.contentContainer.Remove(demos[demos.Count - 1]);
                demos.RemoveAt(demos.Count - 1);
                demoScene.demos.RemoveAt(demoScene.demos.Count - 1);
            }
            EditorUtility.SetDirty(demoScene);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        private void OnAddObjectButton()
        {
            var objectField = new ObjectField("Demo Scene " + demos.Count);
            objectField.objectType = typeof(SceneAsset);
            // objectField.value = null;
            demos.Add(objectField);
            demoScene.demos.Add(null);
            objectField.RegisterCallback<ChangeEvent<UnityEngine.Object>>((e) => {
                // scene = e.newValue as SceneAsset;
                for (int j = 0; j < demos.Count; j++)
                {
                    demoScene.demos[j] = demos[j].value as SceneAsset;
                }
                EditorUtility.SetDirty(demoScene);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            });
            demoFoldout.contentContainer.Add(objectField);
            EditorUtility.SetDirty(demoScene);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}