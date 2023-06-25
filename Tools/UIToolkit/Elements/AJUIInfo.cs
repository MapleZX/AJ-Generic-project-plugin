using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace AJ.Generic.Tools
{
    public class AJUIInfo : ILoadUIInfo
    {
        public string ControllerKey => _controllerKey;
        bool ILoadUIInfo.AJActive { get; set; } = false;
        private string _controllerKey;
        private Action<UIController> _completed;
        public event Action<UIController> Completed { add => _completed += value; remove => _completed -= value; }
        private event Action _clear;
        public event Action Clear { add => _clear += value; remove => _clear -= value; }
        private event Action _display;
        public event System.Action UIDisplay { add => _display += value; remove => _display -= value; }
        private event Action _hide;
        public event System.Action UIHide { add => _hide += value; remove => _hide -= value; }
        void ILoadUIInfo.ClearMethod()
        {
            _clear?.Invoke();
        }
        void ILoadUIInfo.CompletedMethod(UIController controller)
        {
            _completed?.Invoke(controller);
        }
        void ILoadUIInfo.UIDisplayMethod(DisplayStyle style)
        {
            if (style == DisplayStyle.Flex)
            {
                _display?.Invoke();
            }
            else if (style == DisplayStyle.None)
            {
                _hide?.Invoke();
            }
        }
        void ILoadUIInfo.Register(string key)
        {
            var controller = AJController.GetAJGameObject<UIController>(key);
            _controllerKey = controller.RegisterName;
            (controller as IUIRegister).Register(this);
        }
        void ILoadUIInfo.Register(UIController controller)
        {
            _controllerKey = controller.RegisterName;
            (controller as IUIRegister).Register(this);
        }
        public void UnRegister()
        {
            if (!AJController.HaveObject<UIController>(ControllerKey))
            {
                Debug.LogWarning($"{ControllerKey} 已经提前卸载!");
                return;
            }
            var controller = AJController.GetAJGameObject<UIController>(ControllerKey);
            (controller as IUIRegister).UnRegister(this);
        }
        public static AJUIInfo CreateAJElement(string key)
        {
            var aj = new AJUIInfo();
            (aj as ILoadUIInfo).Register(key);
            return aj;
        }
        public static AJUIInfo CreateAJElement(UIController controller)
        {
            var aj = new AJUIInfo();
            (aj as ILoadUIInfo).Register(controller);
            return aj;
        }
    }
}