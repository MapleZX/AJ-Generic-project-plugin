using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public class AJTextField : AJElementBase, ITextFieldEvent
    {
        #region ILabelEvent
        [SerializeField, CustomLabel("Label name")] protected string labelKey = "";
        [SerializeField, CustomLabel("Text")] protected string _text = "";
        public virtual string Name { get => labelKey; protected set => labelKey = value; }
        public string Text {
            get => Element?.value;
            set {
                if (Element != null)
                {
                    Element.value = value;
                    _text = Element.text;
                }
                else
                    StartCoroutine(IChangeLabel(value));
            }
        }
        public VisualElement rootVisualElement { get; private set; }
        AJUIInfo IUIElementEvent.UIInfo => GetUIInfo();
        private TextField _element;
        public TextField Element => this.GetUIElement(ref _element);
        public VisualElement baseElement => Element;
        public override VisualElement AJUIInfoElement => Element;
        public IEnumerator IChangeLabel(string text)
        {
            yield return new WaitUntil(() => rootVisualElement != null);
            yield return new WaitUntil(() => Element != null);
            if (Element == null) yield break;
            Element.value = text;
            _text = Element.text;
        }
        public virtual void RegisterCallback(TextField label) {}
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
            // Text = _text;
        }
        #endregion
    }
}
