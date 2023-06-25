using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AJ.Generic.Tools
{
    [RequireComponent(typeof(IUIElementEvent))]
    public class AJUIFrame : MonoBehaviour
    {
        [SerializeField] private AJUIFrameAnimationObject frameAnimationObject;
        [SerializeField] private int componentIndex = -1;     
        [SerializeField] private bool loop = true;
        [SerializeField] private bool playInStart = true;
        [SerializeField] private bool hideInStart = false;
        private IUIElementEvent element;
        private bool stop = false;          
        public bool Loop { get => loop; set => loop = value; }
        public bool TurnOff { get => stop; set => stop = value; }
        public IUIElementEvent Element => GetElementEvent();
        public AJUIFrameAnimationObject FrameAnimationObject { 
            get => frameAnimationObject; 
            set => SetAJUIFrameAnimationObject(value); 
        }
        private int index = 0;
        // Start is called before the first frame update
        void Start()
        {
            GetElementEvent();
            StartCoroutine(IStart());
            if (playInStart) StartCoroutine(INewFrame(null));
        }
        public void Play(bool _Loop = true, AJUIFrameAnimationObject value = null)
        {
            this.Loop = loop;
            TurnOff = false;
            if (Element.baseElement != null) element.baseElement.style.display = DisplayStyle.Flex;
            SetAJUIFrameAnimationObject(value);
        }
        public void Stop()
        {
            TurnOff = true;
        }
        private void SetAJUIFrameAnimationObject(AJUIFrameAnimationObject value)
        {
            if (Element.baseElement == null || !frameAnimationObject.Completed) return;
            Recover(value);
        }
        private IEnumerator INewFrame(AJUIFrameAnimationObject value)
        {
            yield return new WaitUntil(() => Element.baseElement != null);
            yield return new WaitUntil(() => frameAnimationObject.Completed);
            Recover(value);
        }
        private IEnumerator IStart()
        {
            yield return new WaitUntil(() => Element.baseElement != null);
            frameAnimationObject.Init(Element);
            Element.baseElement.RegisterCallback<TransitionEndEvent>(OnTransitionRun);
            if (hideInStart) Element.baseElement.style.display = DisplayStyle.None;
            else Element.baseElement.style.display = DisplayStyle.Flex;
        }
        private IEnumerator IRecover()
        {
            yield return new WaitUntil(() => Element.rootVisualElement.style.display == DisplayStyle.Flex);
            SetAJUIFrameAnimationObject(null);
        }
        private void Recover(AJUIFrameAnimationObject value)
        {
            index = 0;
            frameAnimationObject.Clear(Element);
            frameAnimationObject = value == null ? frameAnimationObject : value;
            frameAnimationObject.Recover(Element);
        }
        private void OnTransitionRun(TransitionEndEvent evt)
        {
            if (!loop || TurnOff) return;
            // if (Element.rootVisualElement.style.display == DisplayStyle.None)
            // {
            //     var newIndex = 0;
            //     newIndex = newIndex == index ? frameAnimationObject.Frame - 1 : index - 1;
            //     frameAnimationObject.Transform(Element, newIndex);
            //     StartCoroutine(IRecover());
            //     return;
            // }
            index++;
            if (index >= frameAnimationObject.Frame)
            {
                index = 0;               
            }
            frameAnimationObject.Transform(Element, index);
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
