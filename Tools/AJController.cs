using System;
using System.Collections.Generic;
using UnityEngine;
using AJ.Generic.Tools.Singleton;
namespace AJ.Generic.Tools
{
    public class AJController : Singleton<AJController>
    {
        private readonly Dictionary<Type, Dictionary<string, object>> controllers = new();
        private readonly Dictionary<string, IAJMessageEvent> messageCallbacks = new();
        #region Register
        public static void Register(Type type, string key, object obj)
        {
            if (Instance == null) Activate();
            if (key == "")
            {
                Debug.LogError($"{type}类型的{obj}对象传入的名称为空!");
                return;
            }
            var haveType = Instance.controllers.TryGetValue(type, out var objects);
            if (!haveType) 
            {
                objects = new();
                Instance.controllers.Add(type, objects);
            }
            objects.Add(key, obj);
        }
        public static void Register<T>(string key, object go)
        {
            Register(typeof(T), key, go);
        }
        public static void Remove(Type type)
        {
            if (Instance == null) return;
            Instance.controllers.Remove(type);
        }
        public static void Remove<T>()
        {
            Remove(typeof(T));
        }
        public static void UnRegister(Type type, string key)
        {
            if (Instance == null) return;
            var haveType = Instance.controllers.TryGetValue(type, out var objects);
            if (haveType) objects.Remove(key);
        }
        public static void UnRegister<T>(string key)
        {
            UnRegister(typeof(T), key);
        }
        #endregion
        #region 资源/数据获取
        public static bool HaveType(Type type)
        {
            if (Instance == null) return false;
            return Instance.controllers.ContainsKey(type);
        }
        public static bool HaveType<T>()
        {
            return HaveType(typeof(T));
        }
        public static bool HaveObject(Type type, string key)
        {
            if (Instance == null) return false;
            var haveType = Instance.controllers.TryGetValue(type, out var objects);
            if (!haveType) return false;
            return objects.ContainsKey(key);
        }
        public static bool HaveObject<T>(string key)
        {
            return HaveObject(typeof(T), key);
        }
        public static GameObject GetAJGameObject(Type type, string key)
        {
            var go = GetAJObject(type, key);
            if (go == null) return null;
            return go as GameObject;
        }
        public static T GetAJGameObject<T>(string key)
        {
            var go = GetAJGameObject(typeof(T), key);
            if (go == null) return default;
            return go.GetComponent<T>();
        }
        public static T GetAJGameObject<T>(Type type, string key)
        {
            var go = GetAJGameObject(type, key);
            if (go == null) return default;
            return go.GetComponent<T>();
        }
        public static object GetAJObject(Type type, string key)
        {
            if (Instance == null) return default;
            var haveType = Instance.controllers.TryGetValue(type, out var objects);
            if (!haveType) return default;
            var haveObject = objects.TryGetValue(key, out var o);
            return o;
        }
        public static T GetAJObject<T>(string key) where T : class
        {
            var go = GetAJObject(typeof(T), key);
            if (go == null) return default;
            return go as T;
        }
        public static T GetAJObject<T>(Type type, string key) where T : class
        {
            var go = GetAJObject(type, key);
            if (go == null) return default;
            return go as T;
        }
        #endregion
        #region 消息处理
        public static void SendAJMessage<T>(string key, T message) where T : AJMessage
        {
            if (Instance == null) Activate();
            var haveMessage = Instance.messageCallbacks.TryGetValue(key, out IAJMessageEvent value);
            if (!haveMessage)
            {
                Debug.LogWarning($"没有找到{key}对应的消息回调!");
                return;
            }
            var @event = (AJMessageEvent<T>)value;
            @event.callback?.Invoke(message);
        }
        public static void SendAJMessageInDestroy<T>(string key, T message) where T : AJMessage
        {
            if (Instance == null) return;
            var haveMessage = Instance.messageCallbacks.TryGetValue(key, out IAJMessageEvent value);
            if (!haveMessage)
            {
                Debug.LogError($"没有找到{key}对应的消息回调!");
                return;
            }
            var @event = (AJMessageEvent<T>)value;
            @event.callback?.Invoke(message);
        }
        public static void RegisterAJMessage<T>(string key, AJMessageAction<T> callback) where T : AJMessage
        {
            if (Instance == null) Activate();
            var haveMessage = Instance.messageCallbacks.TryGetValue(key, out IAJMessageEvent value);
            if (haveMessage)
            {
                var @event = (AJMessageEvent<T>)value;
                @event.callback -= callback;
                @event.callback += callback;
                return;
            }
            value = new AJMessageEvent<T>();
            ((AJMessageEvent<T>)value).callback -= callback;
            ((AJMessageEvent<T>)value).callback += callback;
            Instance.messageCallbacks.Add(key, value);
        }
        public static void UnRegisterAJMessage<T>(string key, AJMessageAction<T> callback) where T : AJMessage
        {
            if (Instance == null) return;
            var haveMessage = Instance.messageCallbacks.TryGetValue(key, out IAJMessageEvent value);
            if (haveMessage)
            {
                var @event = (AJMessageEvent<T>)value;
                @event.callback -= callback;
                if (@event.callback == null)
                {
                    Instance.messageCallbacks.Remove(key);
                }
            }
        }
        #endregion
    }
}