using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Tools
{
    public interface IAJUIFrameAnimation
    {     
        public void Init(IUIElementEvent element);
        public void Recover(IUIElementEvent element);
        public void Clear(IUIElementEvent element);        
        public void Transform(IUIElementEvent element, int index);
    }
}
