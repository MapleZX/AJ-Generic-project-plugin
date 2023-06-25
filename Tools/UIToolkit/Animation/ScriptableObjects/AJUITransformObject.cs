using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AJ.Generic.Tools
{
    public abstract class AJUITransformObject : ScriptableObject, IAJUIFrameAnimation
    {
        [SerializeField] protected List<AJUITransformClass> transformClasses;
        public abstract bool Completed  { get; }
        public virtual void Init(IUIElementEvent element)
        {
            var transitionProperty = new List<StylePropertyName>(); 
            var easingMode = new List<EasingFunction>();
            var transitionDuration = new List<TimeValue>();
            var transitionDelay = new List<TimeValue>();
            foreach (var t in transformClasses)
            {
                transitionProperty.Add(t.TransitionProperty);
                easingMode.Add(t.EasingMode);
                transitionDuration.Add(t.TransitionDuration);
                transitionDelay.Add(t.TransitionDelay);
            }
            element.baseElement.style.transitionProperty = transitionProperty; 
            element.baseElement.style.transitionTimingFunction = easingMode;
            element.baseElement.style.transitionDuration = transitionDuration;
            element.baseElement.style.transitionDelay = transitionDelay;
        }
        public abstract void Recover(IUIElementEvent element);
        public abstract void Clear(IUIElementEvent element);     
        public abstract void Transform(IUIElementEvent element, int index);
    }
}
