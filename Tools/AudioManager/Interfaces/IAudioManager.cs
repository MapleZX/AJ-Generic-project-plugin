using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AJ.Generic.Tools
{
    public interface IAudioManager
    {
        int SourceCount { get; }
        AudioSourceManager Source(string _name);
        ISoure Source<ISoure>(string _name) where ISoure : AudioSourceManager;
        List<AudioSourceManager> Sources(int groupId);
        List<ISoure> Sources<ISoure>(int groupId) where ISoure : AudioSourceManager;
        void SetVolume(int groupId, float volume);
        float GetVolume(int groupId);
    }
}
