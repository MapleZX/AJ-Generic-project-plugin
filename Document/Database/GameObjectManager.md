# GameObjectManager
包 AJ.Generic.Tools</br>
继承关系 ObjectManager&rarr;GameObjectManager</br>
接口 IObjectManager</br>
> - GameObject实例化管理类，使用该管理器可以快速实例化游戏对象。</br>
> - 本管理器受到GameDataManager管理器控制。</br>
> - TManager: GameDataManager管理类。</br>

## 使用方法
> - 组件脚本继承GameObjectManager，然后挂载GameObject上。</br>
> > ``` C#
> > NewGameObjectManager: GameObjectManager
> > {
> >     // 加载对象加载成功后的设置。
> >     protected override void SetGameObject(GameObject go)
> >     { 
> >         
> >     }
> >     // 初始化的对象设置。
> >     protected override void SetParameters(out InstantiationParameters parameters)
> >     {
> >         // 初始化GameObject实例化。
> >         parameters = new InstantiationParameters(Vector3.zero, transform.rotation, transform);
> >     }
> > }
> > ```

## 管理器配置
### Name
> 管理器名称，也是GameDataManager中GetGameObjectManager()获取该管理器需要的KEY值。</br>
> 如果为空，将会使用Hierarchy中的名称。</br>

### Address
> 资源的地址。</br>

### Game Manager Tag
> GameDataManager管理器对象的Tag。</br>

### Game Manager Name
> GameDataManager管理器对象的名称。</br>

### Disable
> 实例化的对象是否处于活动状态。</br>

## 属性
### GameObject Prefab
> 实例化的对象。</br>

## 受保护的方法
### void SetGameObject(GameObject go)
> 加载对象加载成功后的设置。</br>

### void SetParameters(out InstantiationParameters parameters)
> 初始化的对象设置。</br>

## 继承的方法
### TObject Manager\<TObject>()
> 获取管理器。</br>

|参数|详情|
|:---|----:|
|TObject|管理器类型|

### void Remove()
> 管理器安全卸载，要移除当前管理器请使用该方法。</br>
> > 注：非实例化游戏对象，卸载管理器后，对应的资源如果计数为零也会移除。</br>

## 方法
### TComponent GetGameObject\<TComponent>()
> 获取实例化的组件。</br>