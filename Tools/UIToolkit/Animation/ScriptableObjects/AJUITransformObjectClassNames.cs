using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AJ.Generic.Tools
{
    [CreateAssetMenu(fileName = "Transform USS", menuName = "AJ Generic Tools/UI Aniamtion/Transform USS", order = 2)]
    public class AJUITransformObjectClassNames : AJUITransformObject
    {
        [SerializeField] private List<string> initialStyle;
        [SerializeField] private List<string> transformStyle;
        private bool completed = false;
        public override bool Completed => completed;
        public override void Init(IUIElementEvent element)
        {
            base.Init(element);
            Clear(element);     
            completed = true;
        }

        public override void Clear(IUIElementEvent element)
        {
            foreach (var style in initialStyle)
            {
                if (!element.baseElement.ClassListContains(style))
                    element.baseElement.AddToClassList(style);
            }
            foreach (var style in transformStyle)
            {
                element.baseElement.RemoveFromClassList(style);
            }
        }

        public override void Recover(IUIElementEvent element)
        {
            foreach (var style in transformStyle)
            {
                element.baseElement.RemoveFromClassList(style);
            }
        }

        public override void Transform(IUIElementEvent element, int index)
        {
            foreach (var style in transformStyle)
            {
                element.baseElement.AddToClassList(style);
            }
        }
    }
}
