using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AJ.Generic.Tools
{
    public abstract class AJUIFrameAnimationObject : ScriptableObject, IAJUIFrameAnimation
    {
        [SerializeField] protected EasingMode easingMode = EasingMode.Linear;
        [SerializeField] protected float transitionDuration = .1f;
        [SerializeField] protected float transitionDelay = .05f;
        protected string transitionProperty = "background-image";
        public abstract int Frame { get; }
        public abstract bool Completed  { get; }
        public virtual void Init(IUIElementEvent element)
        {
            element.baseElement.style.transitionProperty = new List<StylePropertyName> { transitionProperty }; 
            element.baseElement.style.transitionTimingFunction = new List<EasingFunction> { easingMode };
            element.baseElement.style.transitionDuration = new List<TimeValue> { transitionDuration };
            element.baseElement.style.transitionDelay = new List<TimeValue> { transitionDelay };
        }
        public abstract void Recover(IUIElementEvent element);
        public abstract void Clear(IUIElementEvent element);    
        public abstract void Transform(IUIElementEvent element, int index);
    }
}