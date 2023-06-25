namespace AJ.Generic.Tools
{
    /// <summary>
    /// 屏幕切换接口。
    /// 搭配IScreenSwitch进行屏幕切换。
    /// </summary>
    public interface IPanelSwitch
    {
        /// <summary>
        /// 切换UI方法。
        /// </summary>
        /// <param name="page">界面名字</param>
        /// <param name="cover">覆盖当前页面</param>
        void SwitchPage(string page, bool cover);
    }
}
