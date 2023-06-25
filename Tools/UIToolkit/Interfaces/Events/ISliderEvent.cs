namespace AJ.Generic.Tools
{
    public interface ISliderEvent : IUIElementEvent<UnityEngine.UIElements.Slider>
    {
        event System.Action<UnityEngine.UIElements.ChangeEvent<float>> changeValueEvent;
        float Volume { get; set; }
    }
}
