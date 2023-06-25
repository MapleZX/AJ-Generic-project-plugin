namespace AJ.Generic.Tools
{
    public enum ControllerStatus { /*未加载*/None = 0, /*加载成功*/Succeeded = 1, /*加载失败*/Failed = 2 }
    /// <summary>
    /// UItoolkit管理器。
    /// 异步加载UI信息（本地化）。
    /// </summary>
    public interface IUIController
    {
        /// <summary>
        /// UI信息加载状态。
        /// 只读。
        /// </summary>
        /// <value></value>
        ControllerStatus Status { get; }
        /// <summary>
        /// 屏幕（页面）切换管理器。
        /// 只读。
        /// </summary>
        /// <value></value>
        IScreenSwitch screenSwitch { get; }
        UnityEngine.UIElements.UIDocument document { get; }
        /// <summary>
        /// UI VisualElement root.
        /// 只读。
        /// </summary>
        /// <value>rootVisualElement</value>
        UnityEngine.UIElements.VisualElement root { get; }
        /// <summary>
        /// UI组件数量。
        /// 只读。
        /// </summary>
        /// <value></value>
        int Count { get; }
    }
    public interface IUIRegister : IUIController
    {
        /// <summary>
        /// 是否进行加载。
        /// </summary>
        /// <param name="completed">是否完成</param>
        /// <param name="status">加载状态</param>
        void Completed(bool completed, ControllerStatus status);
        /// <summary>
        /// 注册UI信息，并保存到管理器里。
        /// </summary>
        /// <param name="info"></param>
        void Register(ILoadUIInfo info);
        /// <summary>
        /// 从管理器卸载对应UI信息。
        /// </summary>
        /// <param name="info"></param>
        void UnRegister(ILoadUIInfo info);
        void ClearUIInfo();
    }
}
