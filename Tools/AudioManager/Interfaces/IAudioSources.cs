using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Tools
{
    public interface IAudioSources
    {
        AudioSource Source { get; }
        float Volume { get; }
        int GroupId { get; }
    }
}
