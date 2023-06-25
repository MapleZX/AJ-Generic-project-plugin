namespace AJ.Generic.Tools
{
    public interface IDataManager
    {
        /// <summary>
        /// 获取游戏数据。
        /// </summary>
        /// <typeparam name="TData">游戏数据类型。</typeparam>
        /// <param name="address">游戏数据地址。</param>
        /// <returns></returns>
        TData DataModel<TData>(string address) where TData : AJModel;
        /// <summary>
        /// 获取游戏数据。
        /// </summary>
        /// <param name="address">游戏数据地址。</param>
        /// <returns></returns>
        AJModel DataModel(string address);
        /// <summary>
        /// 保存单个数据。
        /// </summary>
        /// <param name="key">数据对应的KEY。</param>
        void RunSaved(string key);
        /// <summary>
        /// 保存所有数据。
        /// </summary>
        void RunSaved();
        /// <summary>
        /// 将数据保存到云。
        /// </summary>
        /// <param name="key">数据对应的KEY。</param>
        void RunCloud(string key);
        /// <summary>
        /// 保存所有数据到云。
        /// </summary>
        void RunCloud();
        /// <summary>
        /// 清楚所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        System.DateTime GetAJCurrentDate();
        System.DateTime GetAJExitDate();
    }
}
