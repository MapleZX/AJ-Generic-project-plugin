# AJModel
> 通用类数据模型，要使用数据管理器，数据就要继承该模型。</br>
> > 需要进行存储的数据需要实现接口[ISaveEvent](./ISaveEvent.md)

## 属性
### int Id { get; }
> 数据唯一标识ID。 </br>
> > 备注：只有id值为0以下才能进行设置。</br>

### string Name { get; }
> 数据名称（可以复数）。</br>

### string FirstDate { get; }
> 第一次游戏时间。</br>

### string ExitDate { get; }
> 关闭游戏时间（或游戏发生重大改变）。 </br>

### string NowDate { get; }
> 当前游戏时间。</br>

### long SaveCount { get; set; }
> 游戏保存计数。</br>

## 公共方法
### void SetFirstDate(DateTime firstDate)
> 设置第一次游戏时间。</br>

### void SetFirstDate(string firstDate)
> 设置第一次游戏时间。</br>

### void SetExitDate(DateTime exitDate)
> 设置游戏结束时间。</br>

### void SetExitDate(string exitDate)
> 设置游戏结束时间。</br>

### void SetNowDate(DateTime nowDate)
> 设置游戏当前时间。</br>

### void SetNowDate(string nowDate)
> 设置游戏当前时间。</br>