namespace AJ.Generic.Tools
{
    /// <summary>
    /// 加载UI组件信息。
    /// </summary>
    public interface ILoadUIInfo
    {
        string ControllerKey { get; }
        bool AJActive { get; set; }
        /// <summary>
        /// 加载成功后的事件。
        /// 用来获取UI组件信息。
        /// </summary>
        event System.Action<UIController> Completed;
        /// <summary>
        /// 清楚UI信息事件
        /// </summary>
        event System.Action Clear;
        event System.Action UIDisplay;
        event System.Action UIHide;
        /// <summary>
        /// 内部事件。
        /// </summary>
        /// <param name="controller"></param>
        void CompletedMethod(UIController controller);
        void ClearMethod();
        void UIDisplayMethod(UnityEngine.UIElements.DisplayStyle style);
        /// <summary>
        /// 使用KEY进行注册。
        /// 需要启动AJController。
        /// </summary>
        /// <param name="key"></param>
        void Register(string key);
        /// <summary>
        /// 使用已知的UIController进行注册。
        /// </summary>
        /// <param name="controller"></param>
        void Register(UIController controller);
        /// <summary>
        /// 安全卸载。
        /// </summary>
        void UnRegister();
    }
}
