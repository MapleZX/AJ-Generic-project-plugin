using UnityEngine;
using UnityEngine.UIElements;

namespace AJ.Generic.Tools
{
    [System.Serializable]
    public class AJUITransformClass
    {
        [SerializeField] private string transitionProperty = "";
        [SerializeField] private EasingMode easingMode = EasingMode.Linear;
        [SerializeField] private float transitionDuration = 2f;
        [SerializeField] private float transitionDelay = .5f;

        public string TransitionProperty => transitionProperty;
        public EasingMode EasingMode => easingMode;
        public float TransitionDuration => transitionDuration;
        public float TransitionDelay => transitionDelay;
    }
}
