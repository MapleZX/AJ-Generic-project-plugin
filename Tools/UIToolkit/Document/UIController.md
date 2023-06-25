# UIController
interface: ILocalization>IUIController>IUIDestroy
> 订阅取消UI事件</br>
> 需要挂载在UIDocument组件上</br>
### Children
> 勾选后，将从子组件里面订阅UI事件
### Tag
> 勾选后，将Tag组件里面订阅UI事件
### Localization
> 启动本地化配置
## 事件
### LoadUIInfoAction()
> 运行订阅的事件。
### void UIDestroyAll()
> 将UI组件从游戏中全部移除
### void UIDestroy(GameObject obj)
> 移除参数里的ILoadUIInfo接口组件</br>
> 
# UILocalization
> 本地化设置 </br>
> 在需要本地化的资源上设置localization，TextElement元素在Text上的文本设置Custom Prefix
### Custom Prefix
> 本地化key前缀，在TextElement上设置该前缀就说明这个元素支持本地化设置。
### Localized String Tabel
> 本地化文本库
### Localized Asset Tabel
> 本地化资源</br>
> 设置NONE表示不启动本地化资源

# ILoadUIInfo
> 注册UI事件接口</br>
> 在组件上实现该接口，将该组件挂载UIController子组件，或者设定对应Tag。
### DestroyUIEvent
> 移除事件，如果要提前卸载该组件，请优先调用该事件取消订阅。
### void LoadUIInfo(VisualElement root)
> root就是UI的根。

 