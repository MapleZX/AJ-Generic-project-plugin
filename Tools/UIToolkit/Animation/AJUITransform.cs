using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace AJ.Generic.Tools
{
    [RequireComponent(typeof(IUIElementEvent))]
    public class AJUITransform : MonoBehaviour
    {
        [SerializeField] private AJUITransformObject transformObject;
        [SerializeField] private int componentIndex = -1;
        public event Action<TransitionRunEvent> OnTransitionRun;
        public event Action<TransitionStartEvent> OnTransitionStart;
        public event Action<TransitionEndEvent> OnTransitionEnd;
        public event Action<TransitionCancelEvent> OnTransitionCancel;
        private IUIElementEvent element;
        public IUIElementEvent Element => GetElementEvent();
        // Start is called before the first frame update
        void Start()
        {
            GetElementEvent();
            StartCoroutine(ITransformEvent());
        }
        public void StartTransform()
        {
            if (Element.baseElement == null)
            {
                return;
            }
            StartT();
        }
        public void RecoverTransform()
        {
            if (Element.baseElement == null)
            {
                return;
            }
            RecoverT();
        }
        private IEnumerator ITransformEvent()
        {
            yield return new WaitUntil(() => Element.baseElement != null);
            transformObject.Init(Element);
            Element.baseElement.RegisterCallback<TransitionRunEvent>(evt => OnTransitionRun?.Invoke(evt));
            Element.baseElement.RegisterCallback<TransitionStartEvent>(evt => OnTransitionStart?.Invoke(evt));
            Element.baseElement.RegisterCallback<TransitionEndEvent>(evt => OnTransitionEnd?.Invoke(evt));
            Element.baseElement.RegisterCallback<TransitionCancelEvent>(evt => OnTransitionCancel?.Invoke(evt));
        }
        private IEnumerator IStartTransform()
        {
            yield return new WaitUntil(() => Element.baseElement != null);
            StartT();
        }
        private IEnumerator IRecoverTransform()
        {
            yield return new WaitUntil(() => Element.baseElement != null);
            RecoverT();
        }
        private void StartT()
        {
            transformObject.Transform(Element, 0);
        }
        private void RecoverT()
        {
            transformObject.Recover(Element);
        }
        private IUIElementEvent GetElementEvent()
        {
            if (element != null) return element;
            element = componentIndex < 0 
                ? GetComponent<IUIElementEvent>() : GetComponents<IUIElementEvent>()[componentIndex];
            return element;
        }
    }
}
