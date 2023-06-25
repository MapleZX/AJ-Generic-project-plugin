namespace AJ.Generic.Tools
{
    public interface IManager<T> where T : UnityEngine.MonoBehaviour
    {
        T Manager { get; }
    }
}
