using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using AJ.Generic.Tools;
namespace AJ.Generic.Configuration
{
    [CustomEditor(typeof(AudioManager), true)]
    public class AudioManager_Editor : Editor
    {
        private SerializedProperty tagPropertyList;
        private SerializedProperty goPropertyList;
        private Toggle childToggle;
        // private Toggle tagToggle;
        private Toggle goToggle;
        // private Foldout tagFoldout;
        private Foldout goFoldout;
        private ListView tagListView;
        private ListView goListView;
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            serializedObject.Update();
            tagPropertyList = serializedObject.FindProperty("_tagList");
            goPropertyList = serializedObject.FindProperty("_dynamics");
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new();
            var createCotainer = new VisualElement();
            myInspector.Add(createCotainer);

            childToggle = CreateToggle("Children", "isChild", createCotainer);
            var layerText = new IntegerField("Layer")
            {
                bindingPath = "_layer"
            };
            createCotainer.Add(layerText);
            childToggle.RegisterCallback<AttachToPanelEvent>(evt =>
            {
                if (!childToggle.value)
                {
                    layerText.style.display = DisplayStyle.None;
                }
                else
                {
                    layerText.style.display = DisplayStyle.Flex;
                }
            });
            childToggle.RegisterValueChangedCallback(evt =>
            {
                if (!evt.newValue)
                {
                    layerText.style.display = DisplayStyle.None;
                }
                else
                {
                    layerText.style.display = DisplayStyle.Flex;
                }
            });

            // tagToggle = CreateToggle("Tag", "isTag", createCotainer);
            // tagToggle.RegisterValueChangedCallback(HandleCallbackWithTag);

            // tagFoldout = new Foldout
            // {
            //     // text = "Tags"
            // };
            // // tagFoldout.style.width = new Length(100, LengthUnit.Percent);
            // createCotainer.Add(tagFoldout);
            // tagToggle.RegisterCallback<AttachToPanelEvent>(HandleAttachToPanelEvent);

            // tagListView = CreateTageListView(tagFoldout, GetPropertyFields(tagPropertyList), tagPropertyList);
            // var tagListViewChild = tagListView.Query<Scroller>().ToList()[1].Query().ToList();
            // foreach (var child in tagListViewChild)
            // {
            //     child.style.height = 0;
            //     child.style.width = 0;
            //     child.style.display = DisplayStyle.None;
            // }

            // var tagButtonPanel = new VisualElement();
            // tagFoldout.Add(tagButtonPanel);
            // tagButtonPanel.style.flexGrow = 1;
            // tagButtonPanel.style.flexDirection = FlexDirection.Row;
            // tagButtonPanel.style.alignItems = Align.FlexEnd;
            // tagButtonPanel.style.justifyContent = Justify.FlexEnd;

            // var addBtn = CreateButton(tagButtonPanel, "+");
            // addBtn.clicked += HandleAddTagItem;
            // var removeBtn = CreateButton(tagButtonPanel, "-");
            // removeBtn.clicked += HandleRemoveTagItem;

            goToggle = CreateToggle("GameObject", "isGameObject", createCotainer);
            goToggle.RegisterValueChangedCallback(HandleCallbackWithGo);
            goFoldout = new Foldout
            {
                // text = "GameObjects"
            };
            createCotainer.Add(goFoldout);
            goToggle.RegisterCallback<AttachToPanelEvent>(HandleAttachToPanelEventGo);
            goListView = CreateTageListView(goFoldout, GetPropertyFields(goPropertyList), goPropertyList);
            var goListViewChild = goListView.Query<Scroller>().ToList()[1].Query().ToList();
            foreach (var child in goListViewChild)
            {
                child.style.height = 0;
                child.style.width = 0;
                child.style.display = DisplayStyle.None;
            }

            var goButtonPanel = new VisualElement();
            goFoldout.Add(goButtonPanel);
            goButtonPanel.style.flexGrow = 1;
            goButtonPanel.style.flexDirection = FlexDirection.Row;
            goButtonPanel.style.alignItems = Align.FlexEnd;
            goButtonPanel.style.justifyContent = Justify.FlexEnd;

            var addgoBtn = CreateButton(goButtonPanel, "+");
            addgoBtn.clicked += HandleAddGoItem;
            var removegoBtn = CreateButton(goButtonPanel, "-");
            removegoBtn.clicked += HandleRemoveGoItem;

            return myInspector;
        }
        private List<PropertyField> GetPropertyFields(SerializedProperty serializedP)
        {
            var PropertyFields = new List<PropertyField>();
            for (int i = 0; i < serializedP.arraySize; i++)
            {
                var property = serializedP.GetArrayElementAtIndex(i);
                var propertyField = new PropertyField(property);
                PropertyFields.Add(propertyField);
            }
            return PropertyFields;
        }
        private void RefreshListView(ListView listView, List<PropertyField> visualElements, SerializedProperty serialized)
        {
            listView.itemsSource = visualElements;
            listView.bindItem = (e, i) => (e as PropertyField).BindProperty(serialized.GetArrayElementAtIndex(i));
            listView.RefreshItems();
        }
        private ListView CreateTageListView(VisualElement Cotainer, List<PropertyField> visualElements, SerializedProperty serialized)
        {
            var listView = new ListView();
            listView.itemsSource = visualElements;
            listView.makeItem = () => new PropertyField();
            listView.bindItem = (e, i) => (e as PropertyField).BindProperty(serialized.GetArrayElementAtIndex(i));
            listView.fixedItemHeight = 20;
            listView.selectionType = SelectionType.Single;
            listView.style.flexDirection = FlexDirection.Row;
            listView.style.width = new Length(100, LengthUnit.Percent);
            listView.style.maxHeight = 200;
            Cotainer.Add(listView);
            return listView;
        }
        private Toggle CreateToggle(string label, string bindingPath, VisualElement createCotainer)
        {
            var toggle = new Toggle(label)
            {
                bindingPath = bindingPath
            };
            createCotainer.Add(toggle);
            return toggle;
        }
        private Button CreateButton(VisualElement cotainer, string Label)
        {
            var btn = new Button
            {
                text = Label
            };
            cotainer.Add(btn);
            return btn;
        }
        private void HandleAddTagItem()
        {
            tagPropertyList.arraySize += 1;
            tagPropertyList.serializedObject.ApplyModifiedProperties();
            RefreshListView(tagListView, GetPropertyFields(tagPropertyList), tagPropertyList);
        }
        // private void HandleRemoveTagItem()
        // {
        //     if (tagPropertyList.arraySize > tagListView.selectedIndex && tagListView.selectedIndex != -1)
        //     {
        //         tagPropertyList.DeleteArrayElementAtIndex(tagListView.selectedIndex);
        //     }
        //     else
        //     {
        //         if (tagPropertyList.arraySize > 0)
        //             tagPropertyList.DeleteArrayElementAtIndex(tagPropertyList.arraySize - 1);
        //     }
        //     tagPropertyList.serializedObject.ApplyModifiedProperties();
        //     RefreshListView(tagListView, GetPropertyFields(tagPropertyList), tagPropertyList);
        //     if (tagPropertyList.arraySize <= 0)
        //     {
        //         tagToggle.value = false;
        //     }
        // }
        // private void HandleCallbackWithTag(ChangeEvent<bool> evt)
        // {
        //     if (evt.newValue)
        //     {
        //         tagFoldout.style.display = DisplayStyle.Flex;
        //         if (tagPropertyList.arraySize <= 0)
        //         {
        //             tagPropertyList.arraySize += 1;
        //             tagPropertyList.serializedObject.ApplyModifiedProperties();
        //             RefreshListView(tagListView, GetPropertyFields(tagPropertyList), tagPropertyList);
        //         }
        //     }
        //     else
        //     {
        //         tagFoldout.style.display = DisplayStyle.None;
        //     }
        // }
        // private void HandleAttachToPanelEvent(AttachToPanelEvent evt)
        // {
        //     if (!tagToggle.value)
        //     {
        //         tagFoldout.style.display = DisplayStyle.None;
        //     }
        //     else
        //     {
        //         tagFoldout.style.display = DisplayStyle.Flex;
        //     }
        // }
        private void HandleCallbackWithGo(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
            {
                goFoldout.style.display = DisplayStyle.Flex;
                if (goPropertyList.arraySize <= 0)
                {
                    goPropertyList.arraySize += 1;
                    goPropertyList.serializedObject.ApplyModifiedProperties();
                    RefreshListView(goListView, GetPropertyFields(goPropertyList), goPropertyList);
                }
            }
            else
            {
                goFoldout.style.display = DisplayStyle.None;
            }
        }
        private void HandleAttachToPanelEventGo(AttachToPanelEvent evt)
        {
            if (!goToggle.value)
            {
                goFoldout.style.display = DisplayStyle.None;
            }
            else
            {
                goFoldout.style.display = DisplayStyle.Flex;
            }
        }
        private void HandleAddGoItem()
        {
            goPropertyList.arraySize += 1;
            goPropertyList.serializedObject.ApplyModifiedProperties();
            RefreshListView(goListView, GetPropertyFields(goPropertyList), goPropertyList);
        }
        private void HandleRemoveGoItem()
        {
            if (goPropertyList.arraySize > goListView.selectedIndex && goListView.selectedIndex != -1)
            {
                goPropertyList.DeleteArrayElementAtIndex(goListView.selectedIndex);
            }
            else
            {
                if (goPropertyList.arraySize > 0)
                    goPropertyList.DeleteArrayElementAtIndex(goPropertyList.arraySize - 1);
            }
            goPropertyList.serializedObject.ApplyModifiedProperties();
            RefreshListView(goListView, GetPropertyFields(goPropertyList), goPropertyList);
            if (goPropertyList.arraySize <= 0)
            {
                goToggle.value = false;
            }
        }
        private Button CreateButton(VisualElement cotainer, string lable, string tooltip = "")
        {
            var btn = new Button();
            btn.text = lable;
            btn.tooltip = tooltip;
            cotainer.Add(btn);
            return btn;
        }
    }
}

