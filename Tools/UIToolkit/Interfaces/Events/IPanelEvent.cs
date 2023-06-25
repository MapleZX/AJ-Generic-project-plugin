using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Tools
{
    public interface IPanelEvent : IUIElementEvent<UnityEngine.UIElements.VisualElement>
    {     
        /// <summary>
        /// 是否点击在UI Panel上。
        /// </summary>
        /// <value></value>
        bool InPanel { get; }
        /// <summary>
        /// 是否启动UI Panel点击。
        /// </summary>
        /// <value></value>
        bool StartTouch { get; set; }
        /// <summary>
        /// 点击在UI Panel上。
        /// </summary>
        event System.Action OnTouch;
        /// <summary>
        /// 点击在UI Panel外。
        /// </summary>
        event System.Action OutsideTouch;
        /// <summary>
        /// 取消点击事件。
        /// </summary>
        void CancelTouchPanel();
        /// <summary>
        /// 启动点击事件。
        /// </summary>
        void StartPTouchanel();
    }
}
