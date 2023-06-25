using System;
using System.Text;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using AJ.Generic.Extension;

namespace AJ.Generic.Service
{
    /// <summary>
    /// 云存档执行状态。
    /// </summary>
    public enum CloudStatus { /*保存*/Saved, /*读取*/Loaded, /*删除*/Deleted, /*打开*/Open }
    /// <summary>
    /// 谷歌云存档事件类。
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class AJGoogleServiceCloud<TData> where TData : class
    {
        private PlayGamesPlatform platform;
        private ISavedGameClient savedGameClient;
        private DataSource dataSource = DataSource.ReadCacheOrNetwork;
        private ConflictResolutionStrategy conflictResolutionStrategy = ConflictResolutionStrategy.UseLongestPlaytime;
        private string fileName;
        private CloudStatus _cloudStatus;
        private byte[] datas;
        private bool isCloudOpen = false;
        private DateTime lastModifiedTimestamp;
        public DateTime LastModifiedTimestamp => lastModifiedTimestamp;
        public bool IsCloudOpen => isCloudOpen;
        public ISavedGameClient SavedGameClient => savedGameClient;
        public DataSource DataSource => dataSource;
        public TData Datas { get => FromByte<TData>(datas); set => datas = ToByte(value); }
        public string FileName => fileName;
        public ConflictResolutionStrategy ConflictResolutionStrategy => conflictResolutionStrategy;
        public CloudStatus CloudStatus => _cloudStatus;
        /// <summary>
        /// 存档事件。
        /// </summary>
        public event Action<SavedGameRequestStatus> saved;
        /// <summary>
        /// 加载事件。
        /// </summary>
        public event Action<SavedGameRequestStatus, TData> loaded;
        /// <summary>
        /// 删除事件。
        /// </summary>
        public event Action<bool> deleted;
        /// <summary>
        /// 初始化谷歌云保存服务。
        /// </summary>
        /// <param name="fileName">保存文件名称</param>
        /// <param name="dataSource"></param>
        public AJGoogleServiceCloud(string fileName)
        {
            this.fileName = fileName;
            this.platform = PlayGamesPlatform.Instance;
            this.savedGameClient = platform.SavedGame;
        }
        public AJGoogleServiceCloud(string fileName, DataSource dataSource)
        {
            this.fileName = fileName;
            this.platform = PlayGamesPlatform.Instance;
            this.savedGameClient = platform.SavedGame;
            this.dataSource = dataSource;
        }
        public AJGoogleServiceCloud(string fileName, DataSource dataSource, ConflictResolutionStrategy conflictResolutionStrategy)
        {
            this.fileName = fileName;
            this.platform = PlayGamesPlatform.Instance;
            this.savedGameClient = platform.SavedGame;
            this.dataSource = dataSource;
            this.conflictResolutionStrategy = conflictResolutionStrategy;
        }
        /// <summary>
        /// 初始化谷歌云保存服务。
        /// </summary>
        /// <param name="fileName">保存文件名称</param>
        /// <param name="platform"></param>
        /// <param name="dataSource"></param>
        public AJGoogleServiceCloud(string fileName, PlayGamesPlatform platform)
        {
            this.fileName = fileName;
            this.platform = platform;
            this.savedGameClient = platform.SavedGame;
        }
        public AJGoogleServiceCloud(string fileName, PlayGamesPlatform platform, DataSource dataSource)
        {
            this.fileName = fileName;
            this.platform = platform;
            this.savedGameClient = platform.SavedGame;
            this.dataSource = dataSource;
        }
        public AJGoogleServiceCloud(string fileName, PlayGamesPlatform platform, DataSource dataSource, ConflictResolutionStrategy conflictResolutionStrategy)
        {
            this.fileName = fileName;
            this.platform = platform;
            this.savedGameClient = platform.SavedGame;
            this.dataSource = dataSource;
            this.conflictResolutionStrategy = conflictResolutionStrategy;
        }
        /// <summary>
        /// 打开保存文件。
        /// </summary>
        /// <param name="cloudStatus">执行状态保存/加载/删除</param>
        public void OpenSavedGame(CloudStatus cloudStatus)
        {
            _cloudStatus = cloudStatus;
            if (savedGameClient == null) savedGameClient = platform.SavedGame;
            savedGameClient?.OpenWithAutomaticConflictResolution(FileName, dataSource,
                conflictResolutionStrategy, OnSavedGameOpened);
        }
        /// <summary>
        /// 打开保存文件。
        /// </summary>
        /// <param name="cloudStatus">执行状态保存/加载/删除</param>
        /// <param name="ResolverConflict">自定义冲突解决方案</param>
        public void OpenSavedGame(CloudStatus cloudStatus, ConflictCallback ResolverConflict)
        {
            _cloudStatus = cloudStatus;
            if (savedGameClient == null) savedGameClient = platform.SavedGame;
            savedGameClient?.OpenWithManualConflictResolution(FileName, dataSource,
                    true, ResolverConflict, OnSavedGameOpened);
        }
        /// <summary>
        /// 保存数据到谷歌云。
        /// </summary>
        /// <param name="datas"></param>
        public void SavedGame(TData datas)
        {
            _cloudStatus = CloudStatus.Saved;
            this.datas = ToByte(datas);
            if (savedGameClient == null) savedGameClient = platform.SavedGame;
            savedGameClient?.OpenWithAutomaticConflictResolution(FileName, dataSource,
                conflictResolutionStrategy, OnSavedGameOpened);
        }
        /// <summary>
        /// 保存数据到谷歌云。
        /// </summary>
        /// <param name="ResolverConflict">自定义冲突解决方案</param>
        /// <param name="datas"></param>
        public void SavedGame(ConflictCallback ResolverConflict, TData datas)
        {
            _cloudStatus = CloudStatus.Saved;
            this.datas = ToByte(datas);
            if (savedGameClient == null) savedGameClient = platform.SavedGame;
            savedGameClient?.OpenWithManualConflictResolution(FileName, dataSource,
                    true, ResolverConflict, OnSavedGameOpened);
        }
        private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata metaData)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                AJFirebase.Log($"打开谷歌云存档成功!");
                lastModifiedTimestamp = metaData.LastModifiedTimestamp;
                isCloudOpen = true;
                if (CloudStatus == CloudStatus.Saved)
                {
                    SaveCloud(metaData);
                }
                else if (CloudStatus == CloudStatus.Loaded)
                {
                    LoadCloud(metaData);
                }
                else if (CloudStatus == CloudStatus.Deleted)
                {
                    DeleteCloud(metaData);
                }
                else if (CloudStatus == CloudStatus.Open)
                {
                    OnpenCloud(metaData);
                }
                else
                {
                    AJFirebase.Log($"状态 : {status}");
                }
            }
            else
            {
                isCloudOpen = false;
                AJFirebase.Log($"打开谷歌云存档失败!错误信息:{status}");
            }
        }
        private void SaveCloud(ISavedGameMetadata metaData)
        {
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            if (savedGameClient == null) savedGameClient = platform.SavedGame;
            savedGameClient?.CommitUpdate(metaData, updatedMetadata, datas, (status, _metaData) =>
            {
                saved?.Invoke(status);
                if (status == SavedGameRequestStatus.Success)
                {
                    AJFirebase.Log($"数据{datas}保存到谷歌云成功{status}");
                }
                else
                {
                    AJFirebase.Log($"数据{datas}保存到谷歌云失败{status}".Bold().Color("blue"));
                }
                isCloudOpen = false;
            });
            if (savedGameClient == null) isCloudOpen = false;
        }
        private void LoadCloud(ISavedGameMetadata metaData)
        {
            if (savedGameClient == null) savedGameClient = platform.SavedGame;
            savedGameClient?.ReadBinaryData(metaData, (status, metaData) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    AJFirebase.Log($"数据从谷歌云读取成功 {status}");
                    loaded?.Invoke(status, FromByte<TData>(metaData));
                }
                else
                {
                    AJFirebase.Log($"数据从谷歌云读取失败 {status}".Bold().Color("blue"));
                    loaded?.Invoke(status, null);
                }
                isCloudOpen = false;
            });
            if (savedGameClient == null) isCloudOpen = false;
        }
        private void DeleteCloud(ISavedGameMetadata metaData)
        {
            isCloudOpen = false;
            if (savedGameClient == null) savedGameClient = platform.SavedGame;
            savedGameClient?.Delete(metaData);
            deleted?.Invoke(true);
        }
        private void OnpenCloud(ISavedGameMetadata metaData)
        {
            if (savedGameClient == null) savedGameClient = platform.SavedGame;
            savedGameClient?.ReadBinaryData(metaData, (status, metaData) =>
            {
                isCloudOpen = false;
            });
            if (savedGameClient == null) isCloudOpen = false;
        }
        #region 数据转换
        public static byte[] ToByte<T>(T data) where T : class
        {
            if (data.Equals(null) || data == null) return default;
            var json = JsonUtility.ToJson(data);
            return ToByte(json);
        }
        public static byte[] ToByte(string json)
        {
            if (json.Equals(null) || json == null || json == "") return null;
            var bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }
        public static T FromByte<T>(byte[] bytes) where T : class
        {
            if (bytes.Equals(null) || bytes == null) return null;
            var json = FromByte(bytes);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        public static string FromByte(byte[] bytes)
        {
            if (bytes.Equals(null) || bytes == null) return null;
            var json = Encoding.UTF8.GetString(bytes);
            return json;
        }
        #endregion
    }
}