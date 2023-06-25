using System;
using UnityEngine;
namespace AJ.Generic.Tools.Singleton
{
    /// <summary>
    /// 单例基础类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T _instance;
        public static T Instance => _instance;
        void Awake() => _Awake();
        protected void _Awake(Action _awake = null)
        {
            if (_instance == null)
            {
                _instance = (T)this;
                DontDestroyOnLoad(_instance);
                _awake?.Invoke();
            } else
            {
                Destroy(gameObject);
            }          
        }
        /// <summary>
        /// 激活单例
        /// 如果内存中已经存在，就返回该单例
        /// 如果内存中不存在该单例，就创建一个新的单例
        /// </summary>
        /// <returns></returns>
        public static T Activate()
        {
            if (_instance != null) return _instance;
            Debug.LogFormat("构建新的{0}单例管理器", typeof(T).Name);
            var aj = new GameObject(typeof(T).Name);
            aj.AddComponent<T>();
            aj.transform.position = Vector3.zero;
            aj.transform.parent = null;
            Instantiate(aj);
            return _instance;
        }
    }
}
