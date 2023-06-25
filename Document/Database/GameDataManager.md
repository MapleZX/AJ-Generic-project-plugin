# GameDataManager
包 AJ.Generic.Tools</br>
实现接口 IDataManager</br>
> - 数据管理类，使用该管理器可以快速的读取和保存游戏必要数据。</br>
> - 数据管理器需要加载Unity Addressable包。</br>
> - 序列化数据必须继承[AJModel](../Database/AJModel.md)。</br>
> - 保存数据必须实现接口[ISaveEvent](../Database/ISaveEvent.md)。</br>

## 使用方法
> - 组件脚本继承GameDataManager，然后挂载GameObject上。</br>
> - 加载的资源不要选择GameObject资源（虽然也能加载进来。）。</br>
> - 加载GameObject资源或者其他需要卸载的资源请使用额外的IObjectManager。</br>
> > > 详情查看[GameAssetManager](./GameAssetManager.md)、[GameObjectManager](./GameObjectManager.md)或[GameObjectsManager](./GameObjectsManager.md)。</br>
> > ``` C#
> > NewGameDataManager : GameDataManager
> > {
> >     // 获取当前管理器。
> >     public override NewGameDataManager dataMangeger => this;
> >     // 设置持久化数据地址。
> >     protected override string DataPath => this.PathCombine(Application.persistentDataPath, "data folder")
> >     // 加载单个资源，
> >     [AddressableKey] private string Key = "Key";
> >     // 加载复数资源。
> >     [AddressableKey(true)] private List<string> LabelKeys = new(){ "Label Key" };
> >     [Load("Method")]
> >     TData Method() // TData是继承AJModel的数据, "Method"是该方法的方法名, 不填写默认使用Method名称。
> >     {
> >         // 这里处理数据加载。
> >         // 将获取的数据存入管理器中，并返回该数据。
> >         SetData<TData>(TData data);
> >         Return TData;
> >     }
> >
> > }
> > ```
> > ``` C#
> > GetDataClass : MonoBehaviour
> > {
> >     private IDataManager gameManager;
> >     void Start()
> >     {
> >         // managerTag管理器游戏对象的Tag,managerName管理器游戏对象名字。
> >         gameManager = this.FindGameObjectWithTagAndName<IDataManager>(managerTag, managerName);
> >         gameManager.IResult(
> >            () =>
> >            {
> >                // 这里可以获取资源数据/持久化数据/。
> >                // 在这里使用GetGameAsset()或者GetDataModel();
> >                return data;
> >            });
> >     }
> > }
> > ``` 

## 继承属性

### string DataPath { get; }
> 游戏本地保存路径 </br> 

## 受保护的方法
### void SetData\<TData>(TData data)
> 将序列化数据存入管理器中</br>
> 序列化数据必须继承[AJModel](../Database/AJModel.md)。</br>
> - 保存数据必须实现接口[ISaveEvent](../Database/ISaveEvent.md)。</br>

|参数|详情|
|:---|----:|
|TData|序列化数据|

## 公共方法
### TManager Mangeger\<TManager>()
> 当前数据管理组件 </br>

### TGObject GetGameObjectManager\<TGObject>(string address)
> 获取额外的GameObject（Assets）管理器[ObjectManagerquest;](../Database/ObjectManager.Md)。</br>
> 详情请查看[ObjectManagerquest;](../Database/ObjectManager.Md)。</br>

|参数|详情|
|:---|----:|
|address|预制件位于Addressable的地址|
|return|返回预制件实例的管理器|

### IObjectManager GetGameObjectManager(string address)
> 获取额外的GameObject（Assets）管理器[ObjectManagerquest;](../Database/ObjectManager.Md)。</br>
> 详情请查看[ObjectManagerquest;](../Database/ObjectManager.Md)。</br>

|参数|详情|
|:---|----:|
|address|预制件位于Addressable的地址|
|return|返回预制件实例的管理器接口(基类)|

### TObject GetGameAsset\<TObject>(string address)
> 获取加载进游戏里的资产。</br>
> > 请勿在`Start()`上直接调用该方法，请使用协程[IResult()quest;](#Result)中回调方法获取数据。

|参数|详情|
|:---|----:|
|address|Addressable的地址|
|return|返回指定资产|

### UnityEngine.Object GetGameAsset(string address)
> 获取加载进游戏里的资产。</br>
> > 请勿在`Start()`上直接调用该方法，请使用协程[IResult()quest;](#Result)中回调方法获取数据。

|参数|详情|
|:---|----:|
|address|Addressable的地址|
|return|返回指定资产(基类)|

### TData GetDataModel\<TData>(string address)
> 获取加载进游戏里的数据。</br>
> > 请勿在`Start()`上直接调用该方法，请使用协程[IResult()](#Result)中回调方法获取数据。

|参数|详情|
|:---|----:|
|address|数据名称，VerificationModel._name|
|return|返回指定数据|

### [AJModel](../Database/AJModel.md)) GetDataModel(string address)
> 获取加载进游戏里的数据。</br>
> > 请勿在`Start()`上直接调用该方法，请使用协程[IResult()](#Result)中回调方法获取数据。

|参数|详情|
|:---|----:|
|address|数据名称，VerificationModel._name|
|return|返回指定VerificationModel|

### <a name="Result">IEnumerator IResult\<TData>(Func\<TData> call_back)</a>
> 获取数据资源的回调函数。</br>

|参数|详情|
|:---|----:|
|call_data|获取管理数据的回调函数|

### void RunSaved\<TData>(string key)
|参数|详情|
|:---|----:|
|key|数据名称，VerificationModel._name，也是数据保存的最终地址|
> 保存对应名称的序列化数据。</br>

### void RunSaved()
> 保存所有数据。</br>

### void RunCloud\<TData>(string key)
|参数|详情|
|:---|----:|
|key|数据key|
> 保存当前云数据。</br>

### void RunCloud()
> 保存云数据。</br>
> 