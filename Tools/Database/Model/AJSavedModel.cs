using System;
using UnityEngine;
using AJ.Generic.Extension;

namespace AJ.Generic.Tools
{
    public class AJSavedModel : AJModel, ISaveEvent
    {
        public AJSavedModel() : base() {}
        public AJSavedModel(DateTime firstData) : base(firstData) {}
        public AJSavedModel(long _id, string _name, DateTime firstData) : base(_id, _name, firstData) {}
        #region ISaveEvent
        protected event Action<string> savedEvent;
        protected event Action<string> cloudEvent;
        protected event Action savedAllEvent;
        protected event Action cloudAllEvent;
        event Action<string> ISaveEvent.SavedEvent { add => savedEvent += value; remove => savedEvent -= value; }
        event Action<string> ISaveEvent.CloudEvent { add => cloudEvent += value; remove => cloudEvent -= value; }
        event Action ISaveEvent.SavedAllEvent { add => savedAllEvent += value; remove => savedAllEvent -= value; }
        event Action ISaveEvent.CloudAllEvent { add => cloudAllEvent += value; remove => cloudAllEvent -= value; }
        public virtual void SaveData(string fileName)
        {
            SaveCount++;
            savedEvent?.Invoke(fileName);           
        }
        public virtual void SaveData()
        {
            SaveCount++;
            savedAllEvent?.Invoke();
        }
        public virtual void CloudData(string fileName)
        {
            cloudEvent?.Invoke(fileName);
        }
        public virtual void CloudData()
        {
            cloudAllEvent?.Invoke();
        }
        #endregion
    }
}