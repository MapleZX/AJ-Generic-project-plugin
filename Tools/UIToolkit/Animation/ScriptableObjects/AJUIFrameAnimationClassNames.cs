
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AJ.Generic.Tools
{
    [CreateAssetMenu(fileName = "Frame USS", menuName = "AJ Generic Tools/UI Aniamtion/Frame USS", order = 0)]
    public class AJUIFrameAnimationClassNames : AJUIFrameAnimationObject
    {
        [SerializeField] private StyleSheet styleSheet;
        [SerializeField] private string classPrefixName = "";
        [SerializeField] private int frame = 0;
        private bool completed = false;
        public override int Frame => frame;
        public override bool Completed => completed;
        public override void Init(IUIElementEvent element)
        {
            base.Init(element);
            if (styleSheet != null)
            {
                if (!element.rootVisualElement.styleSheets.Contains(styleSheet))
                {
                    element.rootVisualElement.styleSheets.Add(styleSheet);
                }
            }
            // element.baseElement.style.display = DisplayStyle.Flex;
            completed = true;
        }
        public override void Recover(IUIElementEvent element)
        {
            element.baseElement.AddToClassList(classPrefixName + "0");
        }
        public override void Clear(IUIElementEvent element)
        {
            // element.baseElement.style.display = DisplayStyle.None;
            for (int i = 0; i < frame; i++)
            {
                element.baseElement.RemoveFromClassList(classPrefixName + i);
            }          
        }
        public override void Transform(IUIElementEvent element, int index)
        {
            // element.baseElement.style.display = DisplayStyle.Flex;
            var className = classPrefixName + index;
            var classLastName = classPrefixName + (index - 1);
            if (index <= 0)
            {
                classLastName = classPrefixName + (frame - 1);
            }
            element.baseElement.RemoveFromClassList(classLastName);
            element.baseElement.AddToClassList(className);
        }
    }
}
