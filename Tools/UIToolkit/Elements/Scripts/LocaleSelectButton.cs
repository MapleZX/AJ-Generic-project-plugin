using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization.Settings;
using AJ.Generic.Extension;
using AJ.Generic.Utils;
using AJ.Generic.Tools.Keys;

namespace AJ.Generic.Tools
{
    [System.Serializable]
    public sealed class LocaleSelectButton : AJElementBase, ILabelEvent
    {
        
        #region IToggleEvent
        [SerializeField, CustomLabel("Button name")] private string labelBtnKey;
        [SerializeField, CustomLabel("Country code")] private CountryCode countryCode;     
        // [SerializeField, CustomLabel("UIController tag")] private List<Tags> tags;
        [SerializeField] private UnityEngine.Events.UnityEvent<int> onClick; 
        private string choose = "locale-tab-background-select";
        private string unChoose = "locale-tab-background";      
        private List<UIController> controllers;
        public event Action<CountryCode> clicked;
        string IUIElementEvent<Label>.Name => labelBtnKey;
        string ILabelEvent.Text { set {} }
        void ILabelEvent.GetTextResult(Action<string> result) {}
        private Label _element;
        AJUIInfo IUIElementEvent.UIInfo => GetUIInfo();
        private bool isFirst = true;
        Label IUIElementEvent<Label>.Element => this.GetUIElement(ref _element);
        VisualElement IUIElementEvent.baseElement { get; }
        public override VisualElement AJUIInfoElement { get; }
        public void RegisterCallback(Label label)
        {           
            ChangeStyle(label);
            label.RegisterCallback<ClickEvent>(TabOnClick);
        }
        public event Action<DisplayStyle> UIDisplay { add => _display += value; remove => _display -= value; }
        public event Action<DisplayStyle> UIHide { add => _hide += value; remove => _hide -= value; }
        public void Hide() 
        {
            HideUI();
        }
        public void Display()
        {
            DisplayUI();
        }
        #endregion
        #region ILoadUIInfo
        protected override void LoadCompleted(IUIController controller)
        {
            LoadUIInfo(controller);
        }
        protected override void ClearUIInfo()
        {
            _element = null;
            rootVisualElement = null;
        }
        private bool _active = false;
        public VisualElement rootVisualElement { get; private set; }
        private void LoadUIInfo(IUIController controller)
        {
            if (controller.Status != ControllerStatus.Succeeded)
            {
                Debug.Log($"{name} UI信息加载失败!");
                return;
            }
            rootVisualElement = controller.root;
            if (!isFirst) rootVisualElement.style.display = DisplayStyle.Flex;            
            // controllers = this.GameObjectsFromTag<UIController>(tags, true);
            RegisterCallback((this as IUIElementEvent<Label>).Element);
        }
        private void TabOnClick(ClickEvent evt)
        {
            if (_active) return;
            if (LocalizationSettings.SelectedLocale 
                == LocalizationSettings.AvailableLocales.Locales[(int)countryCode]) return;
            ResetUI();   
            StartCoroutine(SetLocale((int)countryCode));           
        }
        private void ResetUI()
        {
            if (rootVisualElement != null) rootVisualElement.style.display = DisplayStyle.None;
            // AJController.ClearUIInfo();
        }
        private void ChangeStyle(VisualElement ve)
        {
            var localeCode = PlayerPrefs.GetInt(LocaleInitialization.LocaleSettingsKey);           
            if ((int)countryCode == localeCode) 
            {
                ve.ChangeUIStyleClass(choose, unChoose);
            } else {
                ve.ChangeUIStyleClass(unChoose, choose);
            }
        }
        IEnumerator SetLocale(int code)
        {
            _active = true;
            isFirst = false;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[code];
            PlayerPrefs.SetInt(LocaleInitialization.LocaleSettingsKey, code);
            _active = false;         
            clicked?.Invoke(countryCode);
            onClick?.Invoke((int)countryCode);
        }
        #endregion
    }
}
