using UnityEngine;
using System.Collections;
using AJ.Generic.Utils;

namespace AJ.Generic.Tools
{
    public abstract class BaseObjectManager : MonoBehaviour, IManager<BaseObjectManager>
    {
        [SerializeField] protected RegisterNameIngredient registerNameIngredient = new();
        public string RegisterName => !registerNameIngredient.isCustom ? name : registerNameIngredient.registerName;
        [SerializeField, CustomLabel("Address")] protected string address = "";
        public virtual string Address => address;
        public BaseObjectManager Manager => this;
        void Awake() => Register();
        void Start() => Initialization();
        void OnDestroy()
        {
            AJController.UnRegister<BaseObjectManager>(RegisterName);
            UnRegister();
        }
        protected virtual void Register()
        {
            AJController.Register<BaseObjectManager>(RegisterName, gameObject);
        }
        protected virtual void UnRegister() {}
        protected abstract void Initialization();
        public virtual void Remove()
        {
            Destroy(gameObject);
        }
    }
}
