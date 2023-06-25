using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine;
namespace AJ.Generic.Tools
{
    public interface IAddressableCompleted<TOject>
    {
        public void OnSceneUnloaded(AsyncOperationHandle<TOject> obj);
        public void OnSceneLoaded(AsyncOperationHandle<TOject> obj);
    }
}
