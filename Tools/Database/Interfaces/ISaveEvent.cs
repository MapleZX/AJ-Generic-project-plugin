using System;
namespace AJ.Generic.Tools
{
    public interface ISaveEvent
    {
        event Action<string> SavedEvent;
        event Action<string> CloudEvent;
        event Action SavedAllEvent;
        event Action CloudAllEvent;
    }
}
