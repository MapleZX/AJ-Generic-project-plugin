using System;
using UnityEngine;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceManager : MonoBehaviour, IAudioSources, IManager<AudioSourceManager>
    {
        public AudioSourceManager Manager { get => this; protected set {} }
        [SerializeField, CustomLabel("Name")] protected string manager = "";
        [SerializeField, CustomLabel("Group Id")] protected int groupId = -1;
        private AudioSource audioSource;
        public virtual AudioSource Source {
            get {
                if (audioSource == null) audioSource = GetComponent<AudioSource>();
                return audioSource;
            }
        }
        public virtual float Volume {
            get {
                if (audioSource == null) audioSource = GetComponent<AudioSource>();
                return Source.volume * 100;
            }
            set {
                if (audioSource == null) audioSource = GetComponent<AudioSource>();
                Source.volume = value / 100;
            }
        }
        public virtual string Name => manager == "" ? name : manager;
        public virtual int GroupId => groupId;


        public event Action<string> DestroyManagerEvent;

        // public virtual TManager Manager<TManager>() where TManager : Component, IManager
        // {
        //     return GetComponent<TManager>();
        // }

        public virtual void Remove()
        {
            DestroyManagerEvent?.Invoke(Name);
            if (DestroyManagerEvent == null) Destroy(gameObject);
        }

        public virtual void Run()
        {
            Source.Play();
        }
    }
}

