using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AJ.Generic.Tools;
namespace AJ.Generic.Extension
{
    public static class AJUIExtensions
    {
        #region UI对象
        public static T AddUIToDictionary<T>(this Dictionary<string, T> elements, string name, Transform transform)
            where T : IUIElementEvent
        {
            elements[name] = transform.Find(name).GetComponent<T>();
            return elements[name];
        }
        public static GameObject UIGameObject(this MonoBehaviour behaviour, string name, string controllerName)
        {
            var controller = AJController.GetAJGameObject<UIController>(controllerName);
            return controller.UIElement(name);
        }
        public static T UIGameObject<T>(this MonoBehaviour behaviour, string name, string controllerName)
            where T : Component, IUIElementEvent
        {
            var controller = AJController.GetAJGameObject<UIController>(controllerName);
            return controller.UIElement<T>(name);
        }
        public static void UIGameObject(this MonoBehaviour behaviour, 
            string name, string controllerName, Action<GameObject> result)
        {
            var controller = AJController.GetAJGameObject<UIController>(controllerName);
            if (controller.UIElement(name) != null)
            {
                result.Invoke(controller.UIElement(name));
                return;
            }
            behaviour.AJResult<GameObject>(() => {
                var element = controller.UIElement(name);
                return element;
            }, result, () => controller.Status == ControllerStatus.Succeeded);
        }
        public static void UIGameObject<T>(this MonoBehaviour behaviour, 
            string name, string controllerName, Action<T> result)
            where T : Component, IUIElementEvent
        {
            var controller = AJController.GetAJGameObject<UIController>(controllerName);
            if (controller.UIElement(name) != null)
            {
                result.Invoke(controller.UIElement<T>(name));
                return;
            }
            behaviour.AJResult<T>(() => {
                var element = controller.UIElement<T>(name);
                return element;
            }, result, () => controller.Status == ControllerStatus.Succeeded);
        }
        #endregion
        #region 坐标转换
        public static Vector2 ViewportPoint<TUI>(this IUIElementEvent<TUI> element) where TUI : VisualElement
        {
            var Element = element.Element;
            var root = element.rootVisualElement;
            var point = Element.worldTransform.GetPosition();
            var viewport = new Vector2(point.x / root.layout.width, 
                ((point.y - root.layout.height) / root.layout.height) * - 1);
            return viewport;
        }
        public static Vector2 ViewportPoint(this VisualElement element, VisualElement root)
        {
            var point = element.worldTransform.GetPosition();
            var viewport = new Vector2(point.x / root.layout.width, 
                ((point.y - root.layout.height) / root.layout.height) * - 1);
            return viewport;
        }
        public static Vector3 WorldPoint<TUI>(this IUIElementEvent<TUI> element, 
            Camera camera, float pointZ = 0) where TUI : VisualElement
        {
            var point = camera.ViewportToWorldPoint(element.ViewportPoint<TUI>());
            var newWorld = new Vector3(point.x, point.y, pointZ);
            return newWorld;
        }
        public static Vector3 WorldPoint(this VisualElement element, VisualElement root, Camera camera, float pointZ = 0)
        {
            var point = camera.ViewportToWorldPoint(element.ViewportPoint(root));
            var newWorld = new Vector3(point.x, point.y, pointZ);
            return newWorld;
        }
        public static Vector2 WorldToPanel<TUI>(this IUIElementEvent<TUI> element, 
            Transform transform, Camera camera) where TUI : VisualElement
        {
            var Element = element.Element;
            var root = element.rootVisualElement;
            var newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
                Element.panel, transform.position, camera);
            newPosition.x = newPosition.x - (root.layout.width / 2);
            newPosition.y = newPosition.y - (Element.layout.height / 2);
            Element.transform.position = newPosition;
            return newPosition;
        }
        public static Vector2 WorldToPanel(this VisualElement element, 
            Transform transform, VisualElement root, Camera camera)
        {
            var newPosition = RuntimePanelUtils.CameraTransformWorldToPanel(
                element.panel, transform.position, camera);
            newPosition.x = newPosition.x - (root.layout.width / 2);
            newPosition.y = newPosition.y - (element.layout.height / 2);
            element.transform.position = newPosition;
            return newPosition;
        }
        #endregion
        /// <summary>
        /// UI页面切换。
        /// </summary>
        /// <param name="panelSwitch"></param>
        /// <param name="page">切换页面</param>
        /// <param name="name">当前页面</param>
        /// <param name="root"></param>
        /// <param name="cover">是否覆盖当前页面</param>
        /// <returns></returns>
        public static SwitchPageStyle SwitchPage(this IPanelSwitch panelSwitch,
            string page, string name, VisualElement root, bool cover)
        {
            var style = SwitchPageStyle.None;
            if (page == "") return style;
            style = cover ? SwitchPageStyle.Cover : SwitchPageStyle.Overlay;
            if (page != name)
            {              
                root.style.display = cover ? DisplayStyle.None : root.style.display;
                return style;
            }
            root.style.display = DisplayStyle.Flex;
            return style;
        }
        public static void SwitchPage(this IPanelSwitch panelSwitch, 
            VisualElement page, 
            string switchPage, string currentPage, 
            SwitchPageStyle status)
        {
            if (switchPage == "") return;
            page.style.display = switchPage == currentPage
                ? DisplayStyle.Flex 
                : status == SwitchPageStyle.Cover ? DisplayStyle.None : page.style.display;
        }
        // /// <summary>
        // /// 修改UI元素显示。
        // /// </summary>
        // /// <param name="element"></param>
        // /// <param name="display"></param>
        // /// <typeparam name="TUI"></typeparam>
        // public static void UIElementDisplay<TUI>(this IUIElementEvent<TUI> element, bool display)
        //     where TUI : VisualElement
        // {
        //     var hide = !display ? DisplayStyle.None : DisplayStyle.Flex;
        //     if (element.Element != null) element.Element.style.display = hide;
        // }
        public static void Hide(this VisualElement element)
        {
            element.style.display = DisplayStyle.None;
        }
        public static void Display(this VisualElement element)
        {
            element.style.display = DisplayStyle.Flex;
        }
        /// <summary>
        /// 更改UI CSS。
        /// </summary>
        /// <param name="element"></param>
        /// <param name="newClass"></param>
        /// <param name="oldClass"></param>
        public static void ChangeUIStyleClass(this VisualElement element, string newClass, string oldClass)
        {
            if (element.ClassListContains(newClass)) return;
            element.AddToClassList(newClass);
            element.RemoveFromClassList(oldClass);
        }
        public static void ChangeUIStyle<TUI>(this IUIElementEvent<TUI> element, Action<VisualElement> callbacks) 
            where TUI : VisualElement
        {
            if(element.Element != null)
            {
                callbacks?.Invoke(element.Element);
                return;
            }
            var mono = element as MonoBehaviour;
            if (mono != null)
            {
                mono.StartCoroutine(element.IChangeUIStyle(callbacks));
            }
        }
        public static IEnumerator IChangeUIStyle<TUI>(this IUIElementEvent<TUI> element, 
            Action<VisualElement> callbacks) where TUI : VisualElement
        {
            yield return new WaitUntil(() => element.Element != null);
            callbacks?.Invoke(element.Element);
        }
        /// <summary>
        /// 获取UI元素。
        /// </summary>
        /// <param name="elementEvent"></param>
        /// <param name="element"></param>
        /// <typeparam name="TUI"></typeparam>
        /// <returns></returns>
        public static TUI GetUIElement<TUI>(this IUIElementEvent<TUI> elementEvent, ref TUI element)
            where TUI : VisualElement
        {
            if (element != null) return element;
            else if (element == null && elementEvent.rootVisualElement != null)
            {
                element = elementEvent.rootVisualElement.Q<TUI>(elementEvent.Name);
                return element;
            }
            // Debug.LogErrorFormat("UI名称错误,名称{0},UI类型{1}", elementEvent.Name, typeof(TUI));
            return null;
        }
        #region 社交
        public static void SendEmail<TUI>(this IUIElementEvent<TUI> info, 
            string email, string subject = "", string bodys = "")
            where TUI : VisualElement
        {
            var uri = new System.Uri($"mailto:{email}?subject={subject}&body={bodys}");
            UnityEngine.Application.OpenURL(uri.AbsoluteUri);
        }
        public static void SendTwitter<TUI>(this IUIElementEvent<TUI> info, string id = "")
            where TUI : VisualElement
        {
            var uri = new System.Uri($"twitter://user?user_id={id}");
            UnityEngine.Application.OpenURL(uri.AbsoluteUri);
        }
        public static void SendYoutub<TUI>(this IUIElementEvent<TUI> info, string username = "")
            where TUI : VisualElement
        {
            var uri = new System.Uri($"https://youtube.com/{username}");
            UnityEngine.Application.OpenURL(uri.AbsoluteUri);
        }
        #endregion
    }
}