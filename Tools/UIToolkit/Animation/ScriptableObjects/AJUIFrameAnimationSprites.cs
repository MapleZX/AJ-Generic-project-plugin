using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AJ.Generic.Tools
{
    [CreateAssetMenu(fileName = "Frame Sprite", menuName = "AJ Generic Tools/UI Aniamtion/Frame Sprite", order = 1)]
    public class AJUIFrameAnimationSprites : AJUIFrameAnimationObject
    {
        [SerializeField] private List<Sprite> sprites = new();
        private bool completed = false;   
        public override bool Completed => completed;
        public override int Frame => sprites.Count;
        public override void Init(IUIElementEvent element)
        {
            base.Init(element);
            // element.baseElement.style.display = DisplayStyle.Flex;
            completed = true;
        }
        public override void Recover(IUIElementEvent element)
        {
            element.baseElement.style.backgroundImage = new StyleBackground(sprites[0]);
        }
        public override void Clear(IUIElementEvent element)
        {
            // element.baseElement.style.display = DisplayStyle.None;
            element.baseElement.style.backgroundImage = null;
        }
        public override void Transform(IUIElementEvent element, int index)
        {
            // element.baseElement.style.display = DisplayStyle.Flex;
            if (index >= Frame)
            {
                element.baseElement.style.backgroundImage = new StyleBackground(sprites[0]);
                return;
            }
            element.baseElement.style.backgroundImage = new StyleBackground(sprites[index]);
        }
    }
}
