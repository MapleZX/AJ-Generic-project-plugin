namespace AJ.Generic.Tools
{
    /// <summary>
    /// 设置UI组件功能。
    /// 该接口一次只提供实现一个UI组件功能的方法。
    /// </summary>
    /// <typeparam name="T">UI组件类型</typeparam>
    public interface IUIElementEvent<TUI> : IUIElementEvent where TUI : UnityEngine.UIElements.VisualElement
    {
        /// <summary>
        /// UI位于UXML上的Name(名称)。
        /// </summary>
        /// <value></value>
        string Name { get; }
        /// <summary>
        /// 隐藏当前UI。
        /// </summary>
        void Hide();
        /// <summary>
        /// 显示当前UI。
        /// </summary>
        void Display();
        /// <summary>
        /// UI组件。
        /// </summary>
        /// <value></value>
        TUI Element { get; }
        /// <summary>
        /// 设置当前UI回调。
        /// </summary>
        /// <param name="t"></param>
        void RegisterCallback(TUI t);
        event System.Action<UnityEngine.UIElements.DisplayStyle> UIDisplay;
        event System.Action<UnityEngine.UIElements.DisplayStyle> UIHide;
    }
    public interface IUIElementEvent
    {
        /// <summary>
        /// UI结构树中的root。
        /// </summary>
        /// <value>root</value>
        UnityEngine.UIElements.VisualElement rootVisualElement { get; }
        UnityEngine.UIElements.VisualElement baseElement { get; }
        /// <summary>
        /// UI 注册信息。
        /// </summary>
        /// <value></value>
        AJUIInfo UIInfo { get; }
    }
}
