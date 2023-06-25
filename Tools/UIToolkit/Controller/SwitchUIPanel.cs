using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public abstract class SwitchUIPanel : AJPanel, IPanelSwitch
    {
        [SerializeField, CustomLabel("Ignore this panel")] protected bool ignorePanel = false;
        [SerializeField, CustomLabel("Display this panel")] protected bool displayPanel = true;
        public event System.Action<string> SwitchPageEvent;
        private bool _active = false;
        public override void RegisterCallback(VisualElement element)
        {
            screenSwitch = transform.parent.GetComponent<IScreenSwitch>();
            screenSwitch.SwitchEvent -= (this as IPanelSwitch).SwitchPage;
            screenSwitch.SwitchEvent += (this as IPanelSwitch).SwitchPage;
            var displayName = "-";
            displayName = displayPanel ? name : "-";
            StartCoroutine(ISwitchPage(displayName));
        }
        void IPanelSwitch.SwitchPage(string page, bool cover)
        {        
            if (ignorePanel) return;
            if (Element == null) return;
            var style = cover ? SwitchPageStyle.Cover : SwitchPageStyle.Overlay;
            this.SwitchPage(Element, page, name, style);
            // var style = this.SwitchPage(page, name, Element, cover);
            SwitchPageEvent?.Invoke(page); 
        }
        private IEnumerator ISwitchPage(string displayName)
        {
            _active = true;
            yield return new WaitUntil(() => Element != null && screenSwitch != null);
            var style = SwitchPageStyle.Cover;
            this.SwitchPage(Element, displayName, name, style);
            // var style = this.SwitchPage(displayName, name, Element, true);
            SwitchPageEvent?.Invoke(displayName); 
        }
    }
}
