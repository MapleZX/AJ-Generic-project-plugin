using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public class AJLabel : AJElementBase, ILabelEvent
    {
        #region ILabelEvent
        [SerializeField, CustomLabel("Label name")] protected string labelKey = "";
        [SerializeField, CustomLabel("Text")] protected string _text = "";
        [SerializeField, CustomLabel("Localization Text")] protected bool isLocalization = true;
        AJUIInfo IUIElementEvent.UIInfo => GetUIInfo();
        protected Label _element;
        protected bool isStartChange = true;
        public virtual string Name { get => labelKey; protected set => labelKey = value; }
        public string Text { set => ChangeText(value); }
        public VisualElement rootVisualElement { get; private set; }
        public Label Element => this.GetUIElement(ref _element);
        public VisualElement baseElement => Element;
        public override VisualElement AJUIInfoElement => Element;
        public void GetTextResult(Action<string> result)
        {
            isStartChange = false;
            if (Element != null)
            {
                result.Invoke(Element.text);
                isStartChange = true;
                return;
            }
            StartCoroutine(IGetTextResult(result));
        }
        protected IEnumerator IGetTextResult(Action<string> result)
        {
            yield return new WaitUntil(() => rootVisualElement != null);
            yield return new WaitUntil(() => Element != null);
            isStartChange = true;
            result.Invoke(Element.text);
        }
        protected void ChangeText(string text)
        {
            if (isLocalization) return;
            if (Element == null)
            {
                StartCoroutine(IChangeLabel(text));
                return;
            }
            Element.text = text;
            _text = Element.text;
        }
        protected IEnumerator IChangeLabel(string text)
        {
            yield return new WaitUntil(() => rootVisualElement != null);
            yield return new WaitUntil(() => Element != null);
            yield return new WaitUntil(() => isStartChange);
            if (!isLocalization)
            {
                if (Element == null) yield break;
                Element.text = text;
                _text = Element.text;
            }
        }
        public virtual void RegisterCallback(Label label) { }
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
        protected void LoadUIInfo(IUIController controller)
        {
            if (controller.Status != ControllerStatus.Succeeded)
            {
                Debug.Log($"{name} UI信息加载失败!");
                return;
            }
            rootVisualElement = controller.root;
            RegisterCallback(Element);
            Element.text = _text == "" ? Element.text : _text;
        }
        #endregion
    }
}
