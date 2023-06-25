using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
namespace AJ.Generic.Service
{
    /// <summary>
    /// 谷歌云服务。
    /// </summary>
    public class AJGoogleService
    {
        private AJGoogleService(DataSource dataSource)
        {
            if (platform == null)
            {
                platform = PlayGamesPlatform.Activate();
            }
            this.dataSource = dataSource;
        }
        private static AJGoogleService _instance;
        private PlayGamesPlatform platform;
        private Dictionary<string, object> clouds = new();
        private string _token;
        private string _error;
        private int dataCount = 0;
        private bool isAutoCloud = true;
        private bool hasGoogleService = false;
        public bool HasGoogleService => hasGoogleService;
        private DataSource dataSource;
        public DataSource DataSource => dataSource;
        public int CloudCount => dataCount;
        public bool CloudLoadComplete => dataCount <= 0;
        public string Token => _token;
        public string Error => _error;
        public bool IsAutoCloud { get => isAutoCloud; set => isAutoCloud = value; }
        public PlayGamesPlatform Platform => platform;
        public static AJGoogleService Instance => _instance;
        private event Action<CloudStatus, ConflictCallback> dataOpenCustomConflict;
        private event Action<CloudStatus> dataOpen;
        #region 初始化
        /// <summary>
        /// 初始化Google Service服务。
        /// </summary>
        /// <returns></returns>
        public static AJGoogleService Activate(DataSource dataSource)
        {
            if (_instance != null) return _instance;
            _instance = new(dataSource);
            return _instance;
        }
        /// <summary>
        /// 谷歌账户登入。
        /// </summary>
        public void LoginGooglePlayGames(Action<SignInStatus> callback)
        {
            platform.Authenticate(success =>
            {
                hasGoogleService = success == SignInStatus.Success;
                callback.Invoke(success);
            });
        }
        public void RequestServerSideAccess()
        {
            platform.Authenticate((success) =>
            {
                if (success == SignInStatus.Success)
                {
                    hasGoogleService = true;
                    AJFirebase.Log("Login with Google Play games successful.");
                    platform.RequestServerSideAccess(true, code =>
                    {
                        AJFirebase.Log("Authorization code: " + code);
                        _token = code;
                        // This token serves as an example to be used for SignInWithGooglePlayGames
                    });
                }
                else
                {
                    hasGoogleService = false;
                    _error = "Failed to retrieve Google play games authorization code";
                    AJFirebase.Log($"Login Unsuccessful {success}");
                }
            });
        }
        #endregion
        #region 云保存/读取/删除
        /// <summary>
        /// 打开云存档UI界面
        /// </summary>
        /// <param name="callback"></param>
        public void ShowSelectUI(Action<SelectUIStatus, ISavedGameMetadata> callback)
        {
            uint maxNumToDisplay = 5;
            bool allowCreateNew = false;
            bool allowDelete = true;
            ISavedGameClient savedGameClient = platform.SavedGame;
            savedGameClient?.ShowSelectSavedGameUI("Select saved game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                callback);
        }
        /// <summary>
        /// 创建新的谷歌云存档。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        public AJGoogleServiceCloud<T> Cloud<T>(string fileName) where T : class
        {
            if (clouds.ContainsKey(fileName)) return clouds[fileName] as AJGoogleServiceCloud<T>;
            var cloud = new AJGoogleServiceCloud<T>(fileName, platform, dataSource);
            dataCount++;

            cloud.loaded -= CloudCountEvent;
            cloud.loaded += CloudCountEvent;

            dataOpenCustomConflict -= cloud.OpenSavedGame;
            dataOpenCustomConflict += cloud.OpenSavedGame;

            dataOpen -= cloud.OpenSavedGame;
            dataOpen += cloud.OpenSavedGame;

            clouds.Add(fileName, cloud);
            return cloud;
        }
        /// <summary>
        /// 保存数据到谷歌云。
        /// </summary>
        /// <param name="callback">自定义冲突解决方案</param>
        public void SaveCloud()
        {
            dataOpen?.Invoke(CloudStatus.Saved);
        }
        public void SaveAutoCloud()
        {
            if (!IsAutoCloud) return;
            SaveCloud();
        }
        public void SaveCloud(ConflictCallback callback)
        {
            dataOpenCustomConflict?.Invoke(CloudStatus.Saved, callback);
        }
        public void SaveAutoCloud(ConflictCallback callback)
        {
            if (!IsAutoCloud) return;
            SaveCloud(callback);
        }
        /// <summary>
        /// 从谷歌云读取数据。
        /// </summary>
        /// <param name="callback">自定义冲突解决方案</param>
        public void LoadCloud()
        {
            dataOpen?.Invoke(CloudStatus.Loaded);
        }
        public void LoadCloud(ConflictCallback callback)
        {
            dataOpenCustomConflict?.Invoke(CloudStatus.Loaded, callback);
        }
        private void CloudCountEvent(SavedGameRequestStatus status, object metaData)
        {
            dataCount--;
        }
        /// <summary>
        /// 移除谷歌云存档事件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RemoveAJGoogleServiceCloud<T>(AJGoogleServiceCloud<T> cloud) where T : class
        {
            if (Instance == null) return;
            Instance.dataOpenCustomConflict -= cloud.OpenSavedGame;
            Instance.dataOpen -= cloud.OpenSavedGame;
            cloud.loaded -= Instance.CloudCountEvent;
            Instance.clouds.Remove(cloud.FileName);
        }
        #endregion
    }
}
