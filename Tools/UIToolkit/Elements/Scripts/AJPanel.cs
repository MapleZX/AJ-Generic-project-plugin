using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    /// <summary>
    /// UI Panel(VisualElement)组件功能设置。
    /// 该接口一次只提供实现一个Panel组件功能的方法。
    /// </summary>
    public class AJPanel : AJElementBase, IPanelEvent
    {  
        #region IPanelEvent
        public virtual string Name { get => panelKey; protected set => panelKey = value; }
        [SerializeField] protected UnityEvent<string> onTouch;
        [SerializeField] protected UnityEvent<string> outsideTouch;
        [SerializeField, CustomLabel("Panel name")] protected string panelKey = "";
        [SerializeField, CustomLabel("Touch")] protected bool startTouch = true;
        public VisualElement rootVisualElement { get; protected set; }
        AJUIInfo IUIElementEvent.UIInfo => GetUIInfo();
        private VisualElement _element;
        public VisualElement Element => this.GetUIElement(ref _element);
        public VisualElement baseElement => Element;
        public override VisualElement AJUIInfoElement => Element;
        private event Action _onTouch;
        private event Action _outsideTouch;
        public event Action OnTouch { add => _onTouch += value; remove => _onTouch -= value; }
        public event Action OutsideTouch { add => _outsideTouch += value; remove => _outsideTouch -= value; }
        protected bool isClose = true;
        protected bool isInPanel = false;
        protected bool close = false;
        protected float delta = 0;
        public bool StartTouch { get => startTouch; set => startTouch = value; }
        public bool InPanel => isInPanel;
        public virtual void RegisterCallback(VisualElement element) {}
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
        public IScreenSwitch screenSwitch { get; protected set; }
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
            isInPanel = false;
            OnTouchEvent();
        }
        protected virtual void OnTouchEvent()
        {
            if (Element == null) Debug.Log(name + "空的");
            Element.RegisterCallback<PointerOverEvent>((evt) => {
                if(!startTouch) return;              
                isInPanel = true;
            });
            rootVisualElement.RegisterCallback<PointerOverEvent>((evt) => {
                if(!startTouch) return;
                close = true;                    
            });
            rootVisualElement.RegisterCallback<PointerUpEvent>((evt) => {
                if(!startTouch) return;
                delta++;
                if (close && delta > 0)
                {
                    close = false;
                    StartCoroutine(IDisplayStyle());
                }
            });
        }
        protected IEnumerator IDisplayStyle()
        {            
            yield return new WaitUntil(() => !close);
            if (!isInPanel)
            {
                _outsideTouch?.Invoke();
                outsideTouch?.Invoke("");                
            } else
            {
                _onTouch?.Invoke();
                onTouch?.Invoke("");
            }
            delta = 0;
            isInPanel = false;
            close = false;         
        }
        public void CancelTouchPanel()
        {
            startTouch = false;
        }
        public void StartPTouchanel()
        {
            startTouch = true;
        }
        #endregion
    }
}
