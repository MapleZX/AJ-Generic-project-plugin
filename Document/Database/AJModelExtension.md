# AJModelExtension
> 验证数据扩展方法，主要用来查询继承至AJModel列表数据。</br>
### TSource AJDataModel<TSource>(this AJModel aj)
> 将AJmodel转换成子元素。</br>

### TSource ResultElement\<TSource>(this List\<TSource> source, int id)
> 使用唯一ID获取指定值。</br>

|参数|详情|
|:---|----:|
|id|数据ID|
|return|返回对应Id的对象|

### List\<TSource> ResultElements\<TSource>(this List\<TSource> source, string name)
> 获取具有相同名称的新列表。</br>

|参数|详情|
|:---|----:|
|name|数据名称|
|return|返回对应名称的新列表|

### List\<TSource> ResultElements\<TSource>(this List\<TSource> source, Func\<TSource, bool> callBack)
> 获取与传入的回调方法一样的新列表。</br>

|参数|详情|
|:---|----:|
|callBack|数据名称|
|return|返回对应的新列表|

### void void AddElement\<TSource>(this List\<TSource> source, TSource element)
> 将新数据添加入列表。</br>
> > 根据ID进行判断是否添加入列表。</br>

|参数|详情|
|:---|----:|
|element|新元素|

### void AddElements\<TSource>(this List\<TSource> source, List\<TSource> elements)
> 将其他列表插入这个列表。</br>
> > 根据ID进行判断是否添加入列表。</br>

|参数|详情|
|:---|----:|
|elements|其他列表|

### GameObject FindGameObjectWithTagAndName(this MonoBehaviour mono, string tag, string name)
> 使用Tag和GameObject Name寻找GameObject。</br>

|参数|详情|
|:---|----:|
|tag|游戏对象Tag|
|name|游戏对象名称|
|return|返回对应的GameObject|

### TGameObject FindGameObjectWithTagAndName<TGameObject>(this MonoBehaviour mono, string tag, string name)
> 使用Tag和GameObject Name寻找GameObject组件。</br>

|参数|详情|
|:---|----:|
|tag|游戏对象Tag|
|name|游戏对象名称|
|return|返回对应的GameObject组件|

### bool IsDataObject(this MonoBehaviour mono, params object[] datas)
> 验证对象是否存在。</br>

|参数|详情|
|:---|----:|
|object|对象数组|
|return|只要一个对象为空，就返回false|

### string GetDateString(this DateTime dateTime) 
> 将时间搓转换成字符串输出。</br>
> > 输出的字符串的格式是`yyyy-MM-dd HH:mm:ss`。</br>

### DateTime GetDate(this string date)
> 将字符串转换成时间戳。</br>
> > 字符串的时间格式必须是`yyyy-MM-dd HH:mm:ss`。</br>

### int GetTimeDifferenceSeconds(this string current, string last)
> 比较两个时间戳的时间差。</br>

|参数|详情|
|:---|----:|
|last|另外一个时间戳|
|return|返回时间差，以秒为单位|

### int GetTimeDifferenceSeconds(this DateTime current, DateTime last)
> 比较两个时间戳的时间差。</br>

|参数|详情|
|:---|----:|
|last|另外一个时间戳|
|return|返回时间差，以秒为单位|

### int GetTimeDifferenceDays(this string current, string last) 
> 比较两个时间戳的时间差。</br>

|参数|详情|
|:---|----:|
|last|另外一个时间戳|
|return|返回时间差，以日为单位|

### int GetTimeDifferenceDays(this DateTime current, DateTime last)
> 比较两个时间戳的时间差。</br>

|参数|详情|
|:---|----:|
|last|另外一个时间戳|
|return|返回时间差，以日为单位|


### string ToNumberFormat(this double number)
> 使用特殊单位显示数字。</br>

### string Bold(this string str)
> 字符串打印（粗体）。</br>

### string Color(this string str, string clr)
> 字符串打印（颜色）。</br>

### string Italic(this string str)
> 字符串打印（斜体）。</br>

### string Size(this string str, int size)
> 字符串打印（大小）。</br>
