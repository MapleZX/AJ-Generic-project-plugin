namespace AJ.Generic.Tools
{
    public interface IToggleEvent : IUIElementEvent<UnityEngine.UIElements.Toggle>
    {
        event System.Action<UnityEngine.UIElements.ChangeEvent<bool>> changeValueEvent;
        bool Value { set; }
        void GetValueResult(System.Action<bool> result);
    }
}