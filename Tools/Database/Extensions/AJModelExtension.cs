using System;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEngine;
using AJ.Generic.Tools;
namespace AJ.Generic.Extension
{
    public static class AJModelExtension
    {
        #region AJModel List 查询与添加
        public static TSource ResultElement<TSource>(
            this List<TSource> source, int id, int index = 0) where TSource : AJModel
        {
            return source.ResultElement(s => s.Id == id, index);
        }
        public static TSource ResultElement<TSource>(
            this List<TSource> source, string name, int index = 0) where TSource : AJModel
        {
            return source.ResultElement(s => s.Name == name, index);
        }
        public static TSource ResultElement<TSource>(
            this List<TSource> source, Func<TSource, bool> verification, int index = 0) where TSource : AJModel
        {
            if (!source.Any()) return default;
            var query = source.Where(s => verification(s));
            if (!query.Any()) return default;
            if (index >= query.ToList().Count) return default;
            return query.ToList()[index];
        }
        public static void AddElement<TSource>(this List<TSource> source, TSource element)
            where TSource : AJModel
        {
            source.AddElement(element, (s, e) => s.Id == e.Id);
        }
        public static void AddElement<TSource>(this List<TSource> source, TSource element, 
            Func<TSource, TSource, bool> verification)
            where TSource : AJModel
        {
            var query = source.Where(s => verification(s, element));
            if (!query.Any()) source.Add(element);
        }
        public static void ItemSort(this List<SlimeItemModel> items)
        {
            if (!items.Any()) return;
            if (items.Count <= 1) return;
            items.Sort((x, y) => {
                if (x.SortLayer < y.SortLayer) return 1;
                else if (x.SortLayer == y.SortLayer)
                {
                    if (x.Id > y.Id) return 1;
                    else if (x.Id == y.Id)
                    {
                        if(x.Amount > y.Amount) return 1;
                        else return -1;
                    }
                    else return -1;
                }
                else return -1;
            });
        }
        #endregion
        #region 使用协程处理数据
        public static void GetAJItemsModel<T, TM>(this MonoBehaviour behaviour, string key, Action<T> callbacks, string managerName, Func<bool> isCompleted) where T : AJModel where TM : IDataManager
        {
            var gameManager = AJController.GetAJGameObject<TM>(typeof(IDataManager), managerName);
            if (gameManager.DataModel<T>(key) != null)
            {
                callbacks.Invoke(gameManager.DataModel<T>(key));
                return;
            }
            behaviour.AJResult<T>(() => gameManager.DataModel<T>(key), callbacks, isCompleted);
        }
        public static void GetAJAsset<T, TM>(this MonoBehaviour behaviour, string key, Action<T> callbacks, string managerName, Func<bool> isCompleted) where T : UnityEngine.Object where TM : IAssetManager
        {
            var gameManager = AJController.GetAJGameObject<TM>(typeof(IDataManager), managerName);
            if (gameManager.GameAsset<T>(key) != null)
            {
                callbacks.Invoke(gameManager.GameAsset<T>(key));
                return;
            }
            behaviour.AJResult<T>(() => gameManager.GameAsset<T>(key), callbacks, isCompleted);
        }
        public static void GetAJAssets<TM>(this MonoBehaviour behaviour, string key, Action<HashSet<UnityEngine.Object>> callbacks, string managerName, Func<bool> isCompleted) where TM : IAssetManager
        {
            var gameManager = AJController.GetAJGameObject<TM>(typeof(IDataManager), managerName);
            if (gameManager.GameAssets(key) != null)
            {
                callbacks.Invoke(gameManager.GameAssets(key));
                return;
            }
            behaviour.AJResult<HashSet<UnityEngine.Object>>(() => gameManager.GameAssets(key), callbacks, isCompleted);
        }
        public static void AJResult<TResult>(this MonoBehaviour behaviour, Func<TResult> result, Action<TResult> callbacks, Func<bool> isCompleted) where TResult : class
        {
            behaviour.StartCoroutine(behaviour.IAJResult(result, callbacks, isCompleted));
        }
        public static void AJResult(this MonoBehaviour behaviour, Func<object> result, Action<object> callbacks, Func<bool> isCompleted)
        {
            behaviour.StartCoroutine(behaviour.IAJResult(result, callbacks, isCompleted));
        }    
        public static IEnumerator IAJResult<TResult>(this MonoBehaviour behaviour, Func<TResult> result, Action<TResult> callbacks, Func<bool> isCompleted) where TResult : class
        {        
            // yield return new WaitUntil(() => result.Invoke() != null);
            // callbacks.Invoke(result.Invoke());
            yield return new WaitUntil(() => isCompleted.Invoke());
            yield return new WaitUntil(() => {
                if (result.Invoke() is TResult tr)
                {
                    callbacks.Invoke(tr);
                    return true;
                }
                return false;
            });
        }
        public static IEnumerator IAJResult(this MonoBehaviour behaviour, Func<object> result, Action<object> callbacks, Func<bool> isCompleted)
        {         
            // yield return new WaitUntil(() => result.Invoke() != null);
            // callbacks.Invoke(result.Invoke());
            yield return new WaitUntil(() => isCompleted.Invoke());
            yield return new WaitUntil(() => {
                if (result.Invoke() is object tr)
                {
                    callbacks.Invoke(tr);
                    return true;
                }
                return false;
            });
        }
        #endregion
        /// <summary>
        /// 判断输入的对象是否为空。
        /// 可以一次判断多个对象。
        /// 只要其中一个为空就返回false。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static bool IsDataObject(this object obj, params object[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                var data = datas[i];
                if (data == null)
                {
                    return false;
                }
            }
            return true;
        }
        public static void GameObjectsFromChild<TClass>(this Transform transform,
            List<Transform> children, ref int layer) where TClass : class
        {
            if (transform.childCount <= 0) return;
            if (layer == 0) return;
            if (layer > 0) layer--;
            var count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                if (child.GetComponent<TClass>() != null)
                {
                    children.Add(child);
                }
                GameObjectsFromChild<TClass>(child, children, ref layer);
            }
        }
        public static T CloneObject<T>(this ScriptableObject scriptableObject, T source) where T : class
        {
            var serialized = JsonUtility.ToJson(source);
            var obj = JsonUtility.FromJson<T>(serialized);
            return obj;
        }

        // [Obsolete]
        // public static TResult AJResult<TResult>(this MonoBehaviour behaviour, Func<TResult> callbacks) where TResult : class
        // {
        //     var result = callbacks?.Invoke();
        //     if (result != null)
        //     {
        //         return callbacks?.Invoke();
        //     }
        //     behaviour.StartCoroutine(behaviour.IAJResult(callbacks));
        //     return default;
        // }
        // [Obsolete]
        // public static object AJResult(this MonoBehaviour behaviour, Func<object> callbacks)
        // {
        //     var result = callbacks?.Invoke();
        //     if (result != null) 
        //     {
        //         return result;
        //     }
        //     behaviour.StartCoroutine(behaviour.IAJResult(callbacks));
        //     return default;
        // }
        // [Obsolete]
        // public static IEnumerator IAJResult<TResult>(this MonoBehaviour behaviour, Func<TResult> callbacks) where TResult : class
        // {        
        //     yield return new WaitUntil(() => callbacks.Invoke() != null);
        // }
        // [Obsolete]
        // public static IEnumerator IAJResult(this MonoBehaviour behaviour, Func<object> callbacks) 
        // {         
        //     yield return new WaitUntil(() => callbacks.Invoke() != null);
        // }
    }
}
