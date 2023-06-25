using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace AJ.Generic.Tools
{
    public class GameObjectRelease : MonoBehaviour
    {
        void OnDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}
