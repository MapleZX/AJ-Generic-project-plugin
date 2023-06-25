namespace AJ.Generic.Tools
{
    public interface IAssetManager
    {      
        /// <summary>
        /// 游戏资源。
        /// 该资源只有场景切换才会卸载。
        /// 用来存储长期存在的资源。
        /// 对于需要销毁的资源请使用DynamicManager。
        /// </summary>
        /// <typeparam name="TObject">资源类型。</typeparam>
        /// <param name="address">资源地址。</param>
        /// <returns>资源</returns>
        TObject GameAsset<TObject>(string address) where TObject : UnityEngine.Object;
        /// <summary>
        /// 游戏资源。
        /// 该资源只有场景切换才会卸载。
        /// 用来存储长期存在的资源。
        /// 对于需要销毁的资源请使用DynamicManager。
        /// </summary>
        /// <param name="address">资源地址。</param>
        /// <returns>资源</returns>
        UnityEngine.Object GameAsset(string address);
        System.Collections.Generic.HashSet<UnityEngine.Object> GameAssets(string labels);
    }
}
