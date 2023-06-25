# VerificationExtension
> 验证数据扩展方法，主要用来查询继承至VerificationModel列表数据
## 方法
### TSource ResultElement<TSource>(this List<TSource> source, int _id)
|参数|详情|
|:---|:---:|
|_id|VerificationModel._id|
> 使用唯一ID进行数据查询。</br>
> > return VerificationModel

### TSource ResultElement<TSource>(this List<TSource> source, string _name)
|参数|详情|
|:---|:---:|
|_name|VerificationModel._name|
> 使用唯一_name进行数据查询。</br>
> > return VerificationModel

### void AddElement<TSource>(this List<TSource> source, TSource element)
|参数|详情|
|:---|:---:|
|element|VerificationModel元素|
> 添加新的VerificationModel到列表。</br>
> 列表内存在VerificationModel._id或VerificationModel._name数据的时候不添加该数据。

### void AddElements<TSource>(this List<TSource> source, List<TSource> elements)
|参数|详情|
|:---|:---:|
|elements|完整目录|
> 从其他列表里添加数据。</br>

### string ToNumberFormat(this double number)
> 对大数字进行格式化字符串输出。</br>
> > return number + 单位符号。

### string Bold(this string str)
> 进行格式化字符串"Debug"输出。</br>
> > return 加粗字体。

### string Color(this string str, string clr)
> 进行格式化字符串"Debug"输出。</br>
> > return 颜色字体。

### string Italic(this string str)
> 进行格式化字符串"Debug"输出。</br>
> > return 斜体字体。

### string Size(this string str, int size)
> 进行格式化字符串"Debug"输出。</br>
> > return 字体大小。