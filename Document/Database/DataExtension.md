# DataExtension
- 持久化数据保存和读取的扩展方法。</br>
## 方法
### void CreateDataDirectory(this object obj, string path)
> 创建一个新目录 </br>
> 
|参数|详情|
|:---|----:|
|path|目录名称|

### bool ExistsDataDirectory(this object obj, string path)
> 验证目录是否存在 </br>

|参数|详情|
|:---|:---:|
|path|目录名称|
|return|目录存在与否|


### bool ExistsDataFile(this object obj, string path)
> 验证文件是否存在 </br>

|参数|详情|
|:---|:---:|
|path|文件名称|
|return|目录存在与否|


### string[] GetDataFile(this object obj, string path, string suffix)
> 获取该目录下含有指定后缀的所有文件名 </br>

|参数|详情|
|:---|:---:|
|path|目录名称|
|suffix|文件名后缀|
|return|所有目录名|


### void DeleteDataFile(this object obj, string file)
> 删除该文件 </br>

|参数|详情|
|:---|:---:|
|file|文件名|


### static string[] DeleteDataFile(this object obj, string path, string suffix, int amount)
> 删除对应计数的文件，返回剩余文件名 </br>

|参数|详情|
|:---|:---:|
|path|目录名称|
|suffix|文件名后缀|
|amount|文件计数|


### string PathCombine(this object obj, params string[] paths)
> 合并目录路径</br>

|参数|详情|
|:---|:---:|
|paths|目录名称|


### void Save<T>(this object obj, T data, string path)
> 将数据永久化使用JSON保存</br>

|参数|详情|
|:---|:---:|
|data|数据文件|
|path|保存的完整目录|


### void SaveEncrypt<T>(this object obj, T data, string path)
> 将数据永久化使用JSON加密保存</br>

|参数|详情|
|:---|:---:|
|data|数据文件|
|path|保存的完整目录|


### T Load<T>(this object obj, string path)
> 读取JSON，转换成数据</br>
|参数|详情|
|:---|:---:|
|path|完整目录|


### T LoadEncrypt<T>(this object obj, string path)
> 读取加密JSON，转换成数据</br>
|参数|详情|
|:---|:---:|
|path|完整目录|
