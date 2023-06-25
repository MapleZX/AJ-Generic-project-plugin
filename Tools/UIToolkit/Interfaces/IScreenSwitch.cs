using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Tools
{
    /// <summary>
    /// 屏幕切换控制器。
    /// </summary>
    public interface IScreenSwitch
    {
        string SelectedScreen { get; }
        bool IsMainScreenSwitch { get; }
        /// <summary>
        /// 进行屏幕切换。
        /// </summary>
        /// <param name="panel">对应屏幕的游戏对象名称</param>
        void SwitchPage(string page, bool cover);
        event System.Action<string, bool> SwitchEvent;
    }
}
