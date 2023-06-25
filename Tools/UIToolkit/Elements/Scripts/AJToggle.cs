using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public class AJToggle : AJElementBase, IToggleEvent
    {

        #region IToggleEvent   
        [SerializeField] protected UnityEvent<ChangeEvent<bool>> onChangeValue;
        public event Action<ChangeEvent<bool>> changeValueEvent;
        [SerializeField, CustomLabel("Toggle name")] protected string toggleKey = "";
        public virtual string Name { get => toggleKey; protected set => toggleKey = value; }
        [SerializeField, CustomLabel("Toggle")] protected bool isToggle = true;
        public VisualElement rootVisualElement { get; protected set; }
        AJUIInfo IUIElementEvent.UIInfo => GetUIInfo();
        private bool isStartChange = true;
        private Toggle _element;
        public Toggle Element => this.GetUIElement(ref _element);
        public VisualElement baseElement => Element;
        public override VisualElement AJUIInfoElement => Element;
        public bool Value { set => ChangeValue(value); }
        public void GetValueResult(Action<bool> result)
        {
            isStartChange = false;
            if (Element != null)
            {
                result.Invoke(Element.value);
                isStartChange = true;
                return;
            }
            StartCoroutine(IGetValueResult(result));
        }
        private IEnumerator IGetValueResult(Action<bool> result)
        {
            yield return new WaitUntil(() => rootVisualElement != null);
            yield return new WaitUntil(() => Element != null);
            isStartChange = true;
            result.Invoke(Element.value);
        }
        protected virtual void ChangeValue(bool value)
        {
            if (Element == null)
            {
                StartCoroutine(IChangeValue(value));
                return;
            }
            Element.value = value;
            isToggle = Element.value;                  
        }
        protected IEnumerator IChangeValue(bool value)
        {
            yield return new WaitUntil(() => Element != null);
            Element.value = value;
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
        # endregion
        public void RegisterCallback(Toggle toggle) { }
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
            if (Element == null)
            {
                Debug.LogFormat("{0}游戏对象获取的UI对象为空！", Name);
            }
            Element.RegisterValueChangedCallback(OnValueChange);
            Value = isToggle;
        }
        private void OnValueChange(ChangeEvent<bool> evt)
        {
            changeValueEvent?.Invoke(evt);
            onChangeValue?.Invoke(evt);
        }
        #endregion
    }
}
