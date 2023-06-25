using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace AJ.Generic.Tools
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIController))]
    public sealed class UILocalization : MonoBehaviour
    {
        [SerializeField] private bool isCustom;
        [SerializeField] private char keyWord = '.';   
        [SerializeField] private LocalizedStringTable _table;
        [SerializeField] private LocalizedAssetTable _asset;
        [SerializeField] private BackgroundType backgroundType;        
        private UIDocument _document;
        private UIController _controller;
        private StringTable stringTable;
        private AssetTable assetTable;
        private bool _assetActive = false;
        private bool _active = false;
        private bool _refresh = false;
        // private int refreshCount = 0;
        public enum BackgroundType { None = 0, Texture = 1 }
        void OnEnable()
        {
            if (_controller == null) _controller = GetComponent<UIController>();
            _document = _controller.document;
            (_controller as IUIRegister).Completed(false, ControllerStatus.None);

            _table.TableChanged -= OnLoadStrings;
            _table.TableChanged += OnLoadStrings;

            _assetActive = true;

            if (backgroundType != BackgroundType.None)
            {
                _assetActive = false;
                _asset.TableChanged -= OnLoadAssets;
                _asset.TableChanged += OnLoadAssets;               
            }
        }
        void OnDisable()
        {
            _table.TableChanged -= OnLoadStrings;
            if (backgroundType != BackgroundType.None)
            {
                _asset.TableChanged -= OnLoadAssets;
            }
        }
        private IEnumerator IRefreshGUI()
        {
            yield return LocalizationSettings.InitializationOperation;
            yield return new WaitUntil(() => _active);
            yield return new WaitUntil(() => _assetActive);
            var root = _document.rootVisualElement;
            var textElements = LocalizeTextElementContainer(root);
            RefreshText(stringTable, textElements);
            if (backgroundType != BackgroundType.None)
            {               
                var imageElements = LocalizeAssetElementContainer(root);
                RefreshAsset(assetTable, imageElements);
            }
            root.MarkDirtyRepaint();
            (_controller as IUIRegister).Completed(true, ControllerStatus.Succeeded);
            _assetActive = false;
            _active = false;
        }
        #region 加载本地化资产
        private void OnLoadAssets(AssetTable asset)
        {
            var opAssetHandle = _asset.GetTableAsync();
            opAssetHandle.Completed -= OnLoadAssetsCompleted;
            opAssetHandle.Completed += OnLoadAssetsCompleted;
        }
        private void OnLoadAssetsCompleted(AsyncOperationHandle<AssetTable> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                assetTable = obj.Result;
                _assetActive = true;
            } else
            {
                (_controller as IUIRegister).Completed(true, ControllerStatus.Failed);
                Addressables.Release(obj);
            }
        }
        private List<VisualElement> LocalizeAssetElementContainer(VisualElement root)
        {
            var imageContainer = root.Query<VisualElement>(className : "localization-image");
            var imageList = imageContainer.ToList();
            return imageList;
        }
        private bool IsLocalizeAsset(VisualElement element)
        {
            return element.ClassListContains("unity-button");
        }
        private void RefreshAsset(AssetTable table, List<VisualElement> imageList)
        {
            for (int i = 0; i < imageList.Count; i++)
            {
                var imageElement = imageList[i];
                var key = imageElement.name;
                var entry = assetTable.GetAssetAsync<Texture2D>(key);
                if (!entry.Equals(null))
                {
                    entry.Completed += (t) => { imageElement.style.backgroundImage = new StyleBackground(t.Result); };
                } else
                {
                    Debug.LogWarning($"{table.LocaleIdentifier.Code}没有找到{key}");
                }
            }
        }
        #endregion
        #region 加载本地化文本
        private void OnLoadStrings(StringTable table)
        {
            var root = _document.rootVisualElement;
            root.Clear();
            _document.visualTreeAsset.CloneTree(root);

            (_controller as IUIRegister).ClearUIInfo();
            (_controller as IUIRegister).Completed(false, ControllerStatus.None);         

            var opStringHandle = _table.GetTableAsync();
            opStringHandle.Completed -= OnLoadStringsCompleted;
            opStringHandle.Completed += OnLoadStringsCompleted;
        }      
        private void OnLoadStringsCompleted(AsyncOperationHandle<StringTable> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                stringTable = obj.Result;
                _active = true;
                if (!_refresh) StartCoroutine(IRefreshGUI());
            } else
            {
                (_controller as IUIRegister).Completed(true, ControllerStatus.Failed);
                Addressables.Release(obj);
            }
        }
        /// <summary>
        /// 从StringTable获取本地化数据
        /// 同时把本地化文本添加到字典中
        /// </summary>
        /// <param name="table">本地化数据</param>
        /// <param name="textList">TEXT集合</param>
        private void RefreshText(StringTable table, List<TextElement> textList)
        {          
            for (int i = 0; i < textList.Count; i++)
            {
                var textElement = textList[i];
                var key = textElement.text.TrimStart(keyWord);
                var entry = table[key];
                if (entry != null)
                {
                    textElement.text = entry.GetLocalizedString();
                } else if (entry == null && textElement.text.IndexOf(keyWord) == 0)
                {
                    Debug.LogWarning($"{table.LocaleIdentifier.Code} -- {name} 游戏对象没有找到 {key}");
                }
            }
        }
        /// <summary>
        /// 使用QueryBuilder查找符合条件的TEXT添加到集合里
        /// </summary>
        /// <param name="element">UI的根</param>
        /// <returns>TEXT集合</returns>
        private List<TextElement> LocalizeTextElementContainer(VisualElement rootElement)
        {       
            var textsContainer = rootElement.Query<TextElement>(className: "localization").Where(IsLocalizeText);
            var noTextsContainer = rootElement.Query<VisualElement>(className: "localization").Where(HasChildIsLabel);
            var texts = textsContainer.ToList();
            if (noTextsContainer.ToList().Any())
            {
                foreach (var noT in noTextsContainer.ToList())
                {
                    var label = noT.Q<Label>();
                    if (!string.IsNullOrEmpty(label.text))
                    {
                        if (label.text[0] == keyWord)
                        {
                            texts.Add(label);
                        }
                    }
                }
            }
            return texts;
        }
        /// <summary>
        /// 判断TEXT是否支持本地化
        /// </summary>
        /// <param name="textElement">UI TEXT元素</param>
        /// <returns>bool</returns>
        private bool IsLocalizeText(TextElement textElement)
        {
            var text = textElement.text;
            if (string.IsNullOrEmpty(text)) return false;
            return textElement.text[0] == keyWord;
        }
        private bool HasChildIsLabel(VisualElement element)
        {
            var label = element.Q<Label>();
            return label != null;
        }
        [Obsolete]
        private void LocalizeTextWithRecursively(List<TextElement> elements, VisualElement element)
        {
            var hierarchy = element.hierarchy;
            for (int i = 0; i < hierarchy.childCount; i++)
            {
                var child = hierarchy.ElementAt(i);
                AddTextToContainer(elements, child);
            }
            for (int i = 0; i < hierarchy.childCount; i++)
            {
                var child = hierarchy.ElementAt(i);
                var childHierarchy = child.hierarchy;
                if (childHierarchy.childCount != 0)
                    LocalizeTextWithRecursively(elements, child);
            }
        }
        [Obsolete]
        private void AddTextToContainer(List<TextElement> elements, VisualElement element)
        {
            if (typeof(TextElement).IsInstanceOfType(element))
            {
                var textElement = (TextElement)element;
                var key = textElement.text;
                if (!string.IsNullOrEmpty(key))
                {
                    if (key[0] == keyWord)
                        elements.Add(textElement);
                }
            }
        }
        #endregion
    }
}
