namespace AJ.Generic.Tools
{
    public delegate void AJMessageAction<in TM>(TM message);
    public class AJMessageEvent<T> : IAJMessageEvent where T : AJMessage
    {
        public AJMessageAction<T> callback;
    }
    public interface IAJMessageEvent
    {

    }
}