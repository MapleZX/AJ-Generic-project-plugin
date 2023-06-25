using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public sealed class LocaleTextManager : MonoBehaviour
    {
        [SerializeField] private LocalizedStringTable _table;
        [SerializeField] private RegisterNameIngredient registerNameIngredient = new();
        [SerializeField] private bool isLoadAllLocaleText = false;
        [SerializeField, Localization] private string LocalizationTable;
        [SerializeField] private List<string> keys = new();   
        public string RegisterName => !registerNameIngredient.isCustom ? name : registerNameIngredient.registerName;
        private Dictionary<string, string> texts = new();
        private bool isCompleted = false;
        public bool IsCompleted => isCompleted;
        private ControllerStatus status = ControllerStatus.None;
        public ControllerStatus Status => status;
        void Awake() => AJController.Register<LocaleTextManager>(RegisterName, gameObject);
        void OnDestroy() => AJController.UnRegister<LocaleTextManager>(RegisterName);
        void OnEnable()
        {
            _table.TableChanged += OnLoadStrings;
        }
        void OnDisable()
        {
            texts.Clear();
            _table.TableChanged -= OnLoadStrings;
            isCompleted = false;
        }
        #region 获取加载好的本地化文本
        public void LocaleText(Action callbacks)
        {
            if (IsCompleted)
            {
                callbacks.Invoke();
                return;
            }
            StartCoroutine(ILocaleText(callbacks));
        }
        public void LocaleText(Action<string> callbacks, string key)
        {
            if (IsCompleted)
            {
                var text = GetText(key);
                callbacks.Invoke(text);
                return;
            }
            StartCoroutine(ILocaleText(key, callbacks));
        }
        public void LocaleTextFormat(Action<string> callbacks, string key, params string[] characters)
        {
            if (IsCompleted)
            {
                var text = GetTextFormat(key, characters);
                callbacks.Invoke(text);
                return;
            }
            StartCoroutine(ILocaleTextFormat(key, callbacks, characters));
        }
        public string GetText(string key)
        {
            if (!IsCompleted) return "";
            var hasText = texts.TryGetValue(key, out string value);
            if (!hasText)
            {
                Debug.Log($"未找到{key}本地化文本");
            }
            return value;
        }
        public string GetTextFormat(string key, params string[] characters)
        {
            if (!IsCompleted) return "";
            var text = GetText(key);
            return System.String.Format(text, characters);
        }
        private IEnumerator ILocaleText(Action callbacks)
        {
            yield return new WaitUntil(() => IsCompleted);
            callbacks.Invoke();
        }
        private IEnumerator ILocaleText(string key, Action<string> callbacks)
        {
            yield return new WaitUntil(() => IsCompleted);
            var text = GetText(key);
            callbacks.Invoke(text);
        }
        private IEnumerator ILocaleTextFormat(string key, Action<string> callbacks, params string[] characters)
        {
            yield return new WaitUntil(() => IsCompleted);
            var text = GetTextFormat(key, characters);
            callbacks.Invoke(text);
        }
        #endregion
        #region 加载本地化文本
        private void OnLoadStrings(StringTable value)
        {
            isCompleted = false;
            status = ControllerStatus.None;
            var opHandle = _table.GetTableAsync();
            opHandle.Completed -= OnLoadStringsCompleted;
            opHandle.Completed += OnLoadStringsCompleted;
        }
        private void OnLoadStringsCompleted(AsyncOperationHandle<StringTable> operation)
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                var table = operation.Result;
                if (!isLoadAllLocaleText)
                {
                    foreach (var key in keys)
                    {
                        var entry = table[key];
                        if (entry != null)
                        {
                            var text = entry.GetLocalizedString();
                            texts[key] = text;
                        }
                    }
                } else
                {           
                    foreach (var entry in table.Values)
                    {
                        if (entry != null)
                        {
                            var text = entry.GetLocalizedString();
                            texts[entry.Key] = text;
                        }
                    }
                }
                status = ControllerStatus.Succeeded;
                isCompleted = true;
            } else
            {
                isCompleted = false;
                status = ControllerStatus.Failed;
                Addressables.Release(operation);             
            }
        }
        #endregion
    }
}
