using System;
using UnityEngine;
using AJ.Generic.Extension;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    [Serializable]
    public class AJModel
    {
        public AJModel() {}
        public AJModel(DateTime firstData)
        {
            this.firstDate = firstData.GetDateString();
        }
        public AJModel(long _id, string _name, DateTime firstData)
        {
            this._id = _id;
            this._name = _name;
            this.firstDate = firstData.GetDateString();
            this.currentDate = firstData.GetDateString();
            this.exitDate = firstData.GetDateString();
            this.saveCount = 1;
        }
        [SerializeField, CustomLabel("ID")] protected long _id = -1;
        [SerializeField, CustomLabel("Name")] protected string _name;
        [SerializeField, HideInInspector] protected string firstDate;
        [SerializeField, HideInInspector] protected string currentDate;
        [SerializeField, HideInInspector] protected string exitDate;
        [SerializeField, HideInInspector] protected long saveCount;
        public virtual long Id { get => _id; protected set => _id = value; }
        public virtual string Name { get => _name; set => _name = value; }
        public virtual DateTime FirstDate { 
            get {
                if (firstDate == "") return default(DateTime);
                if (firstDate == null) return default(DateTime);
                return firstDate.GetDate(); 
            }
            set {
                if (firstDate == "" || firstDate == null || firstDate.Equals("") || firstDate.Equals(null))
                    firstDate = value.GetDateString();
            }         
        }
        public virtual DateTime CurrentDate { 
            get {
                if (currentDate == "") return default(DateTime);
                if (currentDate == null) return default(DateTime);
                return currentDate.GetDate(); 
            }
            set => currentDate = value.GetDateString(); 
        }
        public virtual DateTime ExitDate { 
            get {
                if (exitDate == "") return default(DateTime);
                if (exitDate == null) return default(DateTime);
                return exitDate.GetDate(); 
            }
            set => exitDate = value.GetDateString(); }
        public virtual long SaveCount { get => saveCount; set => saveCount = value; }
    }
}
