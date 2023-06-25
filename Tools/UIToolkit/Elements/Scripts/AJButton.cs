using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public class AJButton : AJElementBase, IButtonEvent
    {
        #region ILabelEvent
        [SerializeField] protected UnityEvent<string> onClick;
        public event Action OnClick { add => clicked += value; remove => clicked -= value; }
        private event Action clicked;
        [SerializeField, CustomLabel("Button name")] protected string buttonKey = "";
        public virtual string Name { get => buttonKey; protected set => buttonKey = value; }
        public VisualElement rootVisualElement { get; protected set; }    
        AJUIInfo IUIElementEvent.UIInfo => GetUIInfo();
        private Button _element;
        public Button Element => this.GetUIElement(ref _element);
        public VisualElement baseElement => Element;
        public override VisualElement AJUIInfoElement => Element;
        public virtual void RegisterCallback(Button btn) {}
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
        private void LoadUIInfo(IUIController controller)
        {
            if (controller.Status != ControllerStatus.Succeeded)
            {
                Debug.Log($"{name} UI信息加载失败!");
                return;
            }
            rootVisualElement = controller.root;
            RegisterCallback(Element);
            if(Element == null) Debug.Log(name);
            Element.clicked += OnClickEvent;
        }
        protected void OnClickEvent()
        {
            clicked?.Invoke();
            onClick?.Invoke(name);
        }
        #endregion
    }
}
