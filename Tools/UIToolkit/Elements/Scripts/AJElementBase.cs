using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
namespace AJ.Generic.Tools
{
    public abstract class AJElementBase : MonoBehaviour
    {
        private AJUIInfo info;
        protected event System.Action<DisplayStyle> _display;
        protected event System.Action<DisplayStyle> _hide;
        public abstract VisualElement AJUIInfoElement { get; } 
        protected AJUIInfo GetUIInfo()
        {
            if (info == null)
            {
                info = new AJUIInfo();

                info.Completed -= LoadCompleted;
                info.Completed += LoadCompleted;

                info.Clear -= ClearUIInfo;
                info.Clear += ClearUIInfo;

                info.UIDisplay -= DisplayUIEvent;
                info.UIDisplay += DisplayUIEvent;
                
                info.UIHide -= HideUIEvent;
                info.UIHide += HideUIEvent;
            }
            return info;
        }
        void OnDestroy() 
        {
            info?.UnRegister();
            DestroyUI();
        }
        protected abstract void LoadCompleted(IUIController controller);
        protected abstract void ClearUIInfo();
        protected virtual void DestroyUI() {}
        protected virtual void DisplayUIEvent()
        {
            _display?.Invoke(DisplayStyle.Flex);
        }
        protected virtual void HideUIEvent()
        {
            _hide?.Invoke(DisplayStyle.None);
        }
        protected virtual void DisplayUI()
        {         
            if (AJUIInfoElement == null)
            {
                StartCoroutine(IUIElementDisplay(DisplayStyle.Flex));
                return;
            }
            AJUIInfoElement.style.display = DisplayStyle.Flex;
            _display?.Invoke(DisplayStyle.Flex);
        }
        protected virtual void HideUI()
        {           
            if (AJUIInfoElement == null)
            {
                StartCoroutine(IUIElementDisplay(DisplayStyle.None));
                return;
            }
            AJUIInfoElement.style.display = DisplayStyle.None;
            _hide?.Invoke(DisplayStyle.None);
        }
        protected IEnumerator IUIElementDisplay(DisplayStyle style)
        {
            yield return new WaitUntil(() => AJUIInfoElement != null);
            AJUIInfoElement.style.display = style;
            if (style == DisplayStyle.Flex) _display?.Invoke(style);
            else _hide?.Invoke(style);
        }
    }
}
