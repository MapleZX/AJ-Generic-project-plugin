using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    [DisallowMultipleComponent]
    public abstract class GameDataManager<T> : MonoBehaviour, IDataManager, IAssetManager, IManager<T> where T : UnityEngine.MonoBehaviour 
    {
        protected readonly Dictionary<string, UObject> assets = new(); // 游戏资源数据字典列表。
        protected readonly Dictionary<string, HashSet<UObject>> assetList = new(); // 游戏资源数据字典列表。
        protected readonly Dictionary<string, List<string>> labels = new(); // Addressables Key。
        protected readonly Dictionary<string, AsyncOperationHandle<IList<UObject>>> loadAssetHandles = new(); // Addressables 异步句柄。
        public int AssetCount => assets.Count + assetList.Count; // 加载了多少游戏资源。
        protected readonly Dictionary<string, AJModel> datas = new(); // 持久化数据字典列表。
        public int DataCount => datas.Count; // 游戏数据数量。
        protected int loadModelHandleAmount = 0; // 异步加载资源计数。
        protected readonly Dictionary<string, Func<AJModel>> LoadedAsyncEvents = new();
        protected readonly Dictionary<string, Action<string>> SavedEvents = new(); // 保存事件委托保存用
        [SerializeField] protected RegisterNameIngredient registerNameIngredient = new();
        public string RegisterName => !registerNameIngredient.isCustom ? name : registerNameIngredient.registerName;
        private bool isCompleted = false;
        public bool IsCompleted => isCompleted;
        #region Unity 生命周期
        void Awake() => Register();
        void Start() => Initialization();
        void OnDestroy() => UnRegister();
        private void Register()
        {
            AJController.Register<IDataManager>(RegisterName, gameObject);
            this.CreateDataDirectory(DataPath);
        }      
        private void Initialization()
        {
            ReflectionAddress();
            ReflectionData();
            LoadAsset();
        }
        private void UnRegister()
        {
            if (loadAssetHandles.Any())
            {
                foreach (var handle in loadAssetHandles.Values)
                {
                    Addressables.Release(handle);
                }
            }
            AJController.UnRegister<IDataManager>(RegisterName);
            (this as IDataManager).Clear();
        }
        #endregion
        #region ITManager
        public abstract string DataPath { get; protected set; }
        public abstract T Manager { get; protected set; }
        public virtual TObject GameAsset<TObject>(string address) where TObject : UObject
        {
            return GameAsset(address) as TObject;
        }
        public virtual UObject GameAsset(string address)
        {
            var hasAsset = assets.TryGetValue(address, out var uObject);
            if (!hasAsset)
            {
                Debug.Log($"未找到{address.ToString().Bold()}游戏资产。");
            }
            return uObject;
        }
        public virtual HashSet<UObject> GameAssets(string labels)
        {
            var hasAsset = assetList.TryGetValue(labels, out var uObjects);
            if (!hasAsset)
            {
                Debug.LogWarning($"未找到{labels.ToString().Bold()}游戏资产。");
            }
            return uObjects;
        }
        #endregion
        #region IDataManager
        public virtual TData DataModel<TData>(string address) where TData : AJModel
        {
            return DataModel(address) as TData;
        }
        public virtual AJModel DataModel(string address)
        {
            var hasData = datas.TryGetValue(address, out AJModel model);
            if (!hasData)
            {
                Debug.LogWarning($"未找到{address.ToString().Bold()}游戏数据!");
            }
            return model;
        }
        public virtual void RunSaved(string address)
        {
            StartCoroutine(ISaveData(address));
        }
        public virtual void RunSaved()
        {
            StartCoroutine(ISaveData());
        }
        public virtual void RunCloud(string address) { }
        public virtual void RunCloud() { }
        void IDataManager.Clear()
        {
            foreach (var data in datas.Values)
            {
                if (data != null)
                {
                    if (data is ISaveEvent value)
                    {
                        value.SavedEvent -= RunSaved;
                        value.SavedAllEvent -= RunSaved;
                        value.CloudEvent -= RunCloud;
                        value.CloudAllEvent -= RunCloud;
                    }
                }
            }
            SavedEvents.Clear();
            datas.Clear();
        }
        public virtual void Remove(string key)
        {
            var haveData = datas.TryGetValue(key, out var data);
            if (!haveData)
            {
                return;
            }
            if (data == null)
            {
                datas.Remove(key);
                SavedEvents.Remove(key);
                return;
            }
            if (data is ISaveEvent value)
            {
                value.SavedEvent -= RunSaved;
                value.SavedAllEvent -= RunSaved;
                value.CloudEvent -= RunCloud;
                value.CloudAllEvent -= RunCloud;
            }
            datas.Remove(key);
            SavedEvents.Remove(key);
        }
        public virtual System.DateTime GetAJCurrentDate()
        {
            return System.DateTime.Now;
        }
        public virtual System.DateTime GetAJExitDate()
        {
            return System.DateTime.Now;
        }
        #endregion
        #region 资源数据加载
        protected virtual void LoadAsset()
        {
            var isCompletedKeys = LoadAssetFromKeys();
            if (!isCompletedKeys)
            {
                Debug.Log($"未添加资源加载地址!");
            }
        }      
        private bool LoadAssetFromKeys()
        {       
            if (!labels.Any()) return false;
            loadModelHandleAmount = labels.Count;
            foreach (var key in labels.Keys)
            {              
                var value = labels[key];
                var handle = Addressables.LoadAssetsAsync<UObject>(
                value, model =>
                {
                    if (model != null)
                    {
                        var haveModel = assetList.TryGetValue(key, out var models);
                        if (!haveModel)
                        {
                            var newModels = new HashSet<UObject>();
                            newModels.Add(model);
                            assetList.Add(key, newModels);                 
                        } else
                        {
                            models.Add(model);
                        }
                        assets.Add(model.name, model);
                    }
                }, Addressables.MergeMode.Union, false);
                handle.Completed += LoadModelHandle_Completed;
                loadAssetHandles.Add(key, handle);
            }
            return true;
        }
        private void LoadModelHandle_Completed(AsyncOperationHandle<IList<UObject>> operation)
        {
            if (operation.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning("Some assets did not load.");
            }
            else if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                loadModelHandleAmount--;
                if (loadModelHandleAmount <= 0)
                {
                    Debug.LogFormat("资源加载计数:{0}", AssetCount.ToString().Color("red"));
                    LoadedAsyncEvents["Load Log"] = () =>
                    {
                        Debug.LogFormat($"数据加载完成,数据数量 {0}", DataCount.ToString().Color("red"));                     
                        return new();
                    };
                    StartCoroutine(ILoadData());
                }
            }
        }
        protected virtual void OnLoadedCompleted() {}
        #endregion
        #region 协程异步加载数据。
        /// <summary>
        /// 本地化存储。
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        private IEnumerator ISaveData(string key)
        {
            yield return new WaitUntil(() => SavedEvents[key] != null);
            var path = this.PathCombine(DataPath, key);
            SavedEvents[key].Invoke(path);
        }
        /// <summary>
        /// 本地化存储。
        /// </summary>
        /// <returns></returns>
        private IEnumerator ISaveData()
        {
            foreach (var key in SavedEvents.Keys)
            {
                yield return null;
                yield return new WaitUntil(() => SavedEvents[key] != null);
                var path = this.PathCombine(DataPath, key);
                SavedEvents[key].Invoke(path);
            }
        }
        /// <summary>
        /// 加载基础数据。
        /// </summary>
        /// <returns></returns>
        private IEnumerator ILoadData()
        {
            yield return new WaitUntil(() => loadModelHandleAmount <= 0);
            foreach (var key in LoadedAsyncEvents.Keys)
            {
                yield return new WaitForFixedUpdate();
                var value = LoadedAsyncEvents[key];
                yield return new WaitUntil(() => value != null);
                yield return new WaitUntil(() => {
                    if (key == "Load Log") return true;
                    if (value.Invoke() is AJModel data && key != "Load Log")
                    {
                        datas.Add(key, data);
                        CreatSavedEvent(key, data);
                        return data != null;
                    }
                    return false;
                });
            }
            isCompleted = true;
            OnLoadedCompleted();
        }
        private void CreatSavedEvent(string key, AJModel model)
        {
            if (model is not ISaveEvent value) return;
            value.SavedEvent -= RunSaved;
            value.SavedEvent += RunSaved;

            value.SavedAllEvent -= RunSaved;
            value.SavedAllEvent += RunSaved;

            value.CloudEvent -= RunCloud;
            value.CloudEvent += RunCloud;

            value.CloudAllEvent -= RunCloud;
            value.CloudAllEvent += RunCloud;

            if (!SavedEvents.ContainsKey(key) && datas.ContainsKey(key))
            {
                SavedEvents.Add(key, path =>
                {
                    var _data = datas[key];
                    _data.CurrentDate = GetAJCurrentDate();
                    _data.ExitDate = GetAJExitDate();
                    this.Saved(_data, path);
                });
            }
        }
        #endregion
        #region 反射获取数据
        /// <summary>
        /// 使用反射获取需要加载的资源地址（Address）。
        /// </summary>
        private void ReflectionAddress()
        {
            var dataType = GetType();
            var binding = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var fields = dataType.GetFields(binding);
            var fieldAttributeType = typeof(AddressableAddressAttribute);
            foreach (var field in fields)
            {
                bool isAttribute = Attribute.IsDefined(field, fieldAttributeType);
                if (isAttribute)
                {
                    var attr = (AddressableAddressAttribute)Attribute.GetCustomAttribute(field, fieldAttributeType);
                    var value = field.GetValue(this);
                    if (value is string key)
                    {
                        AddLabelKey(attr.LabelKey, key);
                    } else if (value is List<string> keyList)
                    {
                        AddLabelKeys(attr.LabelKey, keyList);
                    }
                }
            }
        }
        private void AddLabelKey(string key, string value)
        {
            var newKey = key != "" ? key : value;
            var have = labels.TryGetValue(newKey, out var labelkeys);
            if (!have)
            {
                labelkeys = new();
            }
            if (!labelkeys.Contains(value)) labelkeys.Add(value);
            labels[newKey] = labelkeys;
        }
        private void AddLabelKeys(string key, List<string> values)
        {
            if (key == "")
            {
                Debug.Log("未给加载的资源组设置KEY!".Color("red"));
                return;
            }
            var newKey = key;
            var have = labels.TryGetValue(newKey, out var keys);
            if (!have)
            {
                keys = values;
                labels[newKey] = keys;
            } else
            {
                var result = keys.Union(values).ToList<string>();
                labels[newKey] = result;
            }
        }
        /// <summary>
        /// 使用反射加载数据资源。
        /// </summary>
        private void ReflectionData()
        {
            var dataType = GetType();
            var binding = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var methods = dataType.GetMethods(binding);
            var methodAttributeType = typeof(LoadAttribute);
            foreach (var method in methods)
            {
                bool isAttribute = Attribute.IsDefined(method, methodAttributeType);
                if (isAttribute)
                {
                    var attr = (LoadAttribute)Attribute.GetCustomAttribute(method, methodAttributeType);
                    var handler = Delegate.CreateDelegate(typeof(Func<AJModel>), this, method);
                    var func = (Func<AJModel>)handler;
                    if (func != null)
                    {
                        var key = attr._name == "" ? method.Name : attr._name;
                        LoadedAsyncEvents[key] = func;
                    }
                }
            }
        }
        #endregion
    }
}
