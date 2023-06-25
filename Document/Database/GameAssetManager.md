# GameAssetManager\<TObject>
包 AJ.Generic.Tools</br>
继承关系 ObjectManager&rarr;GameAssetManager\<TObject></br>
接口 IObjectManager</br>
> - 资源管理类，使用该管理器可以额外加载本地资源。</br>
> - 本管理器受到GameDataManager管理器控制。</br>
> - TManager: GameDataManager管理类。</br>
> - TObject: 资源的对象类型。</br>

## 使用方法
> - 组件脚本继承GameAssetManager，然后挂载GameObject上。</br>
> > ``` C#
> > NewAssetManager: GameAssetManager<GameObject>
> > {
> >     // 设置加载资源的初始值
> >     protected override void SetGameAsset(GameObject asset)
> >     { 
> >         // 设置加载资源的初始值
> >     }
> >
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
> 不使用。</br>

## 属性
### TObject Asset
> 获取加载进来的资源。</br>

## 受保护的方法
### void SetGameAsset(TObject asset)
> 设置加载资源的初始值。</br>

## 继承的方法
### TObject Manager\<TObject>()
> 获取管理器。</br>

|参数|详情|
|:---|----:|
|TObject|管理器类型|

### void Remove()
> 管理器安全卸载，要移除当前管理器请使用该方法。</br>
> > 注：非实例化游戏对象，卸载管理器后，对应的资源如果计数为零也会移除。</br>