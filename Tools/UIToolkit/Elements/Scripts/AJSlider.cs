using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public class AJSlider : AJElementBase, ISliderEvent
    {
        
        #region ISliderEvent
        [SerializeField] protected UnityEvent<ChangeEvent<float>> onChangeValue;
        public event System.Action<ChangeEvent<float>> changeValueEvent;      
        [SerializeField, CustomLabel("Slider name")] protected string sliderKey = "";
        public virtual string Name => sliderKey;
        [SerializeField, CustomLabel("Slider Volume"), Range(-1, 100)] protected float sliderValue = -1;
        public VisualElement rootVisualElement { get; protected set; }
        AJUIInfo IUIElementEvent.UIInfo => GetUIInfo();
        private Slider _element;       
        public Slider Element => this.GetUIElement(ref _element);
        public VisualElement baseElement => Element;
        public override VisualElement AJUIInfoElement => Element;
        public float Volume { get => GetVolume(); set =>SetVolume(value); }
        protected virtual float GetVolume()
        {
            if (Element != null) return Element.value;
            Debug.LogError($"{Name} Slider not loaded!");
            return -1;
        }
        protected virtual void SetVolume(float value)
        {
            if (rootVisualElement == null || Element == null)
            {
                StartCoroutine(IChangeValue(value));
                return;
            }
            var newValue = value == -1 ? Element.value : value;
            Element.value = newValue;
        }
        protected IEnumerator IChangeValue(float value)
        {
            yield return new WaitUntil(() => Element != null);
            Element.value = value == -1 ? Element.value : value;
        }
        public virtual void RegisterCallback(Slider slider) {}
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
            Volume = sliderValue;
            Element.RegisterValueChangedCallback(OnVolumeChange);         
        }
        protected void OnVolumeChange(ChangeEvent<float> evt)
        {
            changeValueEvent?.Invoke(evt);
            onChangeValue?.Invoke(evt);
        }
        #endregion
    }
}
