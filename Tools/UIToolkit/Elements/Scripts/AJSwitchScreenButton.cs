using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public class AJSwitchScreenButton : AJElementBase, IButtonEvent
    {
        [SerializeField] protected UnityEvent<string> onClick;
        public event Action OnClick { add => clicked += value; remove => clicked -= value; }
        private event Action clicked;
        [SerializeField, CustomLabel("Button name")] protected string buttonKey = "";
        [SerializeField, CustomLabel("Open panel name")] protected string panelKey = "";
        [SerializeField, CustomLabel("Cover this panel")] protected bool coverPanel = true;
        public virtual string Name { get => buttonKey; protected set => buttonKey = value; }
        public virtual string SwitchPanelName { get => panelKey; protected set => panelKey = value; }
        public VisualElement rootVisualElement { get; protected set; }
        AJUIInfo IUIElementEvent.UIInfo => GetUIInfo();
        private Button _element;
        public Button Element => this.GetUIElement(ref _element);
        public VisualElement baseElement => Element;
        public override VisualElement AJUIInfoElement => Element;
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
        public virtual void RegisterCallback(Button btn) { }
        #region ILoadUIInfo
        private IScreenSwitch screenSwitch;
        protected override void LoadCompleted(IUIController controller)
        {
            LoadUIInfo(controller);
        }
        protected override void ClearUIInfo()
        {
            _element = null;
            rootVisualElement = null;
        }
        private void LoadUIInfo(IUIController controller)
        {
            if (controller.Status != ControllerStatus.Succeeded)
            {
                Debug.Log($"{name} UI信息加载失败!");
                return;
            }
            rootVisualElement = controller.root;
            screenSwitch = controller.screenSwitch;
            RegisterCallback(Element);
            if (Element == null)
            {
                Debug.LogFormat("{0}游戏对象获取的UI对象为空,{1}", Name, name);
            }
            Element.clicked += OnClickEvent;
            OnClick += SwitchPage;
        }
        protected void OnClickEvent()
        {
            clicked?.Invoke();
            onClick?.Invoke(Name);
        }
        public void SwitchPage()
        {
            screenSwitch?.SwitchPage(SwitchPanelName, coverPanel);
            Debug.Log($"Open {SwitchPanelName} Panel");
        }
        #endregion
    }
}
