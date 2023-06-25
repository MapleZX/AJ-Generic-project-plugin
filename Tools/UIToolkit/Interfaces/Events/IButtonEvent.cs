namespace AJ.Generic.Tools
{
    public interface IButtonEvent : IUIElementEvent<UnityEngine.UIElements.Button>
    {
        event System.Action OnClick;
    }
}
