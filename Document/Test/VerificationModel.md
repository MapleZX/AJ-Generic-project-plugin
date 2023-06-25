# VerificationModel
## 属性
### int _id
> 数据唯一标识ID </br>

### string _name
> 数据唯一标识ID </br>

> ### string _verification
> 验证用数据，主要运用在云存档上面 </br>

> ### int _verificationAmount
> 验证用数据，主要运用在云存档上面 </br>

## 公共方法
### void Verification(DateTime dateTime, int amount)
|参数|详情|
|:---|----:|
|dateTime|时间|
|amount|计数器|
> 验证用数据，主要运用在云存档上面 </br>

### string GetDate(DateTime dateTime)
|参数|详情|
|:---|----:|
|dateTime|时间|
> 将时间转换成"yyyy-MM-dd HH:mm:ss"格式的字符串。</br>
> > return "yyyy-MM-dd HH:mm:ss"</br>

### DateTime GetDate(string date)
|参数|详情|
|:---|----:|
|dateTime|时间字符串|
> 将"yyyy-MM-dd HH:mm:ss"格式的字符串转换成时间。</br>
> > return DateTime</br>

### int GetTimeDifferenceSeconds(string last, string next)
|参数|详情|
|:---|----:|
|last|上一次时间|
|next|下一次时间|
> 计算两个时间的时间差。</br>
> 字符串时间格式必须是"yyyy-MM-dd HH:mm:ss"</br>
> > return Seconds</br>

### int GetTimeDifferenceSeconds(DateTime last, DateTime next)
|参数|详情|
|:---|----:|
|last|上一次时间|
|next|下一次时间|
> 计算两个时间的时间差。</br>
> > return Seconds</br>

### int GetTimeDifferenceDays(string last, string next)
|参数|详情|
|:---|----:|
|last|上一次时间|
|next|下一次时间|
> 计算两个时间的时间差。</br>
> 字符串时间格式必须是"yyyy-MM-dd HH:mm:ss"</br>
> > return Days</br>

### int GetTimeDifferenceDays(DateTime last, DateTime next)
|参数|详情|
|:---|----:|
|last|上一次时间|
|next|下一次时间|
> 计算两个时间的时间差。</br>
> > return Days</br>