using System;
using System.Collections.Generic;
using UnityEngine;
using AJ.Generic.Extension;

namespace AJ.Generic.Tools
{
    [DisallowMultipleComponent]
    public abstract class AudioManager : MonoBehaviour, IAudioManager, IManager<AudioManager>
    {
        public AudioManager Manager { get => this; protected set {} }
        protected readonly Dictionary<string, AudioSourceManager> sources = new();
        protected readonly Dictionary<string, Action<string>> RegisterManagers = new();
        public int SourceCount => sources.Count;
        [SerializeField] protected bool isTag;
        [SerializeField] protected bool isGameObject;
        [SerializeField] protected bool isChild;
        // [SerializeField] protected List<Tags> _tagList = new();
        [SerializeField] protected List<AudioSourceManager> _dynamics = new();
        [SerializeField] protected int _layer = -1;
        public string settingsSavePath => "Audio save :";
        #region 生命周期
        void Awake()
        {
            ReflectionRegisterManager("Register")?.Invoke(name);
            ReflectionRegisterManager("UnRegister");
        }
        // Start is called before the first frame update
        void Start()
        {
            // LoadAttachManager();
        }
        void OnDestroy()
        {
            Action<string> unRegister;
            var hasUn = RegisterManagers.TryGetValue("UnRegister", out unRegister);
            if (hasUn)
                unRegister.Invoke(name);
        }
        private Action<string> ReflectionRegisterManager(string _name)
        {
            var type = GetType();
            var RegisterMethod = type.GetMethod(_name);
            var register = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), this, RegisterMethod);
            RegisterManagers[_name] = register;
            return register;
        }
        #endregion
        #region IAudioManager       
        public virtual AudioSourceManager Source(string _name)
        {
            if (sources.ContainsKey(_name))
            {
                return sources[_name];
            }
            Debug.Log($"未找到{_name.ToString().Bold()}音效对象。");
            return null;
        }
        public virtual ISoure Source<ISoure>(string _name) where ISoure : AudioSourceManager
        {
            if (sources.ContainsKey(_name))
            {
                return sources[_name] as ISoure;
            }
            Debug.Log($"未找到{_name.ToString().Bold()}音效对象。");
            return default;
        }
        public virtual List<AudioSourceManager> Sources(int groupId)
        {
            return Sources<AudioSourceManager>(groupId);
        }
        public virtual List<ISoure> Sources<ISoure>(int groupId) where ISoure : AudioSourceManager
        {
            var group = new List<ISoure>();
            foreach (var value in sources.Values)
            {
                if (value.GroupId == groupId)
                {
                    group.Add(value as ISoure);
                }
            }
            return group;
        }
        public virtual void SetVolume(int groupId, float volume)
        {
            var group = Sources(groupId);
            foreach (var value in group)
            {
                value.Volume = volume;
            }
            PlayerPrefs.SetFloat(settingsSavePath + groupId, volume);
        }
        public virtual float GetVolume(int groupId)
        {
            var volume = 100f;
            if (PlayerPrefs.HasKey(settingsSavePath + groupId))
            {
                volume = PlayerPrefs.GetFloat(settingsSavePath + groupId);
            }
            return volume;
        }
        #endregion
        #region IDynamicManager
        // private void LoadAttachManager()
        // {
        //     //if (isChild)
        //     //    this.LoadAttachManagerFromChild(transform, sources, SetAudioSource, ref _layer);
        //     //if (isTag)
        //     //    this.LoadAttachManagerFromTags(this, _tagList, isTag, sources, SetAudioSource);
        //     //if (isGameObject)
        //     //    this.LoadAttachManagerFromGameObject(_dynamics, sources, SetAudioSource);
        // }
        // private void SetAudioSource(AudioSourceManager source)
        // {
        //     //var parents = "";
        //     //var have = this.HaveParents(ref parents);
        //     //if (have)
        //     //{
        //     //    if (parents != "" && parents != name) return;
        //     //}
        //     var _dynamic = this as IDynamicManager<AudioSourceManager>;
        //     source.DestroyManagerEvent += _dynamic.DestroyDynamicManager;
        //     source.Volume = GetVolume(source.GroupId);
        //     sources[source.Name] = source;
        // }
        // public TGObject DynamicManager<TGObject>(string address) where TGObject : AudioSourceManager
        // {
        //     return Source<TGObject>(address);
        // }

        // public AudioSourceManager DynamicManager(string address)
        // {
        //     return Source(address);
        // }

        // void IDynamicManager<AudioSourceManager>.DestroyDynamicManager(string address)
        // {
        //     if (sources.ContainsKey(address))
        //     {
        //         var so = sources[address];
        //         sources.Remove(address);
        //         Destroy(so);
        //     }
        // }

        // public void RegisterNewDynamicManager(string address, AudioSourceManager manager)
        // {
        //     if (!sources.ContainsKey(address))
        //     {
        //         sources[manager.Name] = manager;
        //         var _dynamic = this as IDynamicManager<AudioSourceManager>;
        //         sources[manager.Name].DestroyManagerEvent += _dynamic.DestroyDynamicManager;
        //     }
        // }
        #endregion

        // public TManager Manager<TManager>() where TManager : Component, IManager
        // {
        //     return GetComponent<TManager>();
        // }
    }
}
