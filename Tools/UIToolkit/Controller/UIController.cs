using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Extension;

namespace AJ.Generic.Tools
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIDocument))]
    public sealed class UIController : MonoBehaviour, IUIRegister, IPanelSwitch, IManager<UIController>
    {
        [SerializeField] private bool isDefaultScreen = false;
        [SerializeField] private bool isCustomRegisterName = false;
        [SerializeField] private bool isMainScreenName = true;
        [SerializeField] private string registerName = "";
        [SerializeField] private string screenName = "";
        [SerializeField] private SwitchUIScreen screenUISwitch;
        public string RegisterName => !isCustomRegisterName ? name : registerName;
        public string ScreenName => isMainScreenName ? SwitchUIScreen.MainScreenName : screenName;
        private ControllerStatus status;
        private IScreenSwitch _screenSwitch;
        private UIDocument _document;
        private readonly List<ILoadUIInfo> infos = new();
        private readonly Dictionary<string, GameObject> elements = new();
        private bool loading = false;
        private int layer = -1;     
        public int Count => infos.Count;
        public ControllerStatus Status => status;
        public IScreenSwitch screenSwitch {
            get {
                if (_screenSwitch != null) return _screenSwitch;
                if (isMainScreenName) _screenSwitch = AJController.GetAJGameObject<SwitchUIScreen>(ScreenName);
                else
                {
                    if (ScreenName != "") _screenSwitch = AJController.GetAJGameObject<SwitchUIScreen>(ScreenName);
                    else _screenSwitch = screenUISwitch;
                }
                return _screenSwitch;
            }
        }
        public UIDocument document {
            get {
                if (_document != null) return _document;
                if (_document == null) _document = GetComponentInParent<UIDocument>(includeInactive: true);
                return _document;
            }
        }
        public VisualElement root {
            get {
                if (status != ControllerStatus.Succeeded) return null;
                return document.rootVisualElement;
            }
        }
        public bool IsLocalization => GetComponent<UILocalization>() != null;
        void Awake() => AJController.Register<UIController>(RegisterName, gameObject);
        void OnDestroy()
        {
            if (AJController.HaveObject<SwitchUIScreen>(ScreenName))
            {
                var screen = AJController.GetAJGameObject<SwitchUIScreen>(ScreenName) as IScreenSwitch;
                screen.SwitchEvent -= (this as IPanelSwitch).SwitchPage;
            }
            AJController.UnRegister<UIController>(RegisterName);
        }
        void OnEnable()
        {
            var defaultName = "-";
            if (isDefaultScreen) defaultName = RegisterName;
            (this as IPanelSwitch).SwitchPage(defaultName, true);
            RegisterUIInfoFromChild();
            StartCoroutine(ISwitchEvent());
            if (IsLocalization) return;
            (this as IUIRegister).Completed(true, ControllerStatus.Succeeded);
        }
        UIController IManager<UIController>.Manager => this;
        void IPanelSwitch.SwitchPage(string page, bool cover)
        {
            var root = document.rootVisualElement;
            var style = cover ? SwitchPageStyle.Cover : SwitchPageStyle.Overlay;
            this.SwitchPage(root, page, RegisterName, style);
            // var style = this.SwitchPage(page, RegisterName, root, cover);
            foreach (var info in infos)
            {
                info?.UIDisplayMethod(root.style.display.value);
            }
        }
        void IUIRegister.ClearUIInfo()
        {
            foreach (var info in infos)
            {
                info.ClearMethod();
            }
        }
        void IUIRegister.Register(ILoadUIInfo info)
        {
            if (!infos.Contains(info))
            {
                infos.Add(info);
                LoadInfo(info);
            }
        }
        void IUIRegister.UnRegister(ILoadUIInfo info)
        {
            if (!infos.Contains(info)) return;
            infos.Remove(info);
        }
        void IUIRegister.Completed(bool completed, ControllerStatus status)
        {
            loading = completed;
            this.status = status;
            if (!loading) return;
            foreach (var info in infos)
            {
                LoadInfo(info);
            }
        }
        public GameObject UIElement(string name)
        {
            var has = elements.TryGetValue(name, out GameObject element);
            if (!has) Debug.LogFormat("{0}管理器内没有{0}元素", RegisterName, name);
            return element;
        }
        public TUI UIElement<TUI>(string name) where TUI : Component, IUIElementEvent
        {
            return UIElement(name).GetComponent<TUI>();
        }
        private void RegisterUIInfoFromChild()
        {
            var children = new List<Transform>();
            layer = -1;
            transform.GameObjectsFromChild<IUIElementEvent>(children, ref layer);
            foreach (var child in children)
            {
                var components = child.GetComponents<IUIElementEvent>();
                foreach (var info in components)
                {
                    elements[child.name] = child.gameObject;
                    (info.UIInfo as ILoadUIInfo).Register(this);
                }
            }
        }
        private void LoadInfo(ILoadUIInfo info)
        {
            if (info.AJActive) return;
            StartCoroutine(ILoadUIInfos(info));
        }
        private IEnumerator ILoadUIInfos(ILoadUIInfo info)
        {
            info.AJActive = true;
            yield return new WaitUntil(() => loading);
            info.CompletedMethod(this);
            info.AJActive = false;
        }
        private IEnumerator ISwitchEvent()
        {
            yield return new WaitUntil(() => screenSwitch != null);
            screenSwitch.SwitchEvent -= (this as IPanelSwitch).SwitchPage;
            screenSwitch.SwitchEvent += (this as IPanelSwitch).SwitchPage;
        }
    }
}
