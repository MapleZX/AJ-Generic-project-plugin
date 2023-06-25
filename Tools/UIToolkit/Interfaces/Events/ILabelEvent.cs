namespace AJ.Generic.Tools
{
    /// <summary>
    /// UI Label组件功能设置。
    /// 该接口一次只提供实现一个Label组件功能的方法。
    /// </summary>
    public interface ILabelEvent : IUIElementEvent<UnityEngine.UIElements.Label>
    {       
        /// <summary>
        /// UI Label Text Value。
        /// </summary>
        /// <value></value>
        string Text { set; }
        void GetTextResult(System.Action<string> result);
    }
}
