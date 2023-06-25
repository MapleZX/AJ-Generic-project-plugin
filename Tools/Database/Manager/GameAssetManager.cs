using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace AJ.Generic.Tools
{
    public abstract class GameAssetManager<TObject> : BaseObjectManager where TObject : Object
    {
        private AsyncOperationHandle<TObject> handle;
        private TObject gameAsset;
        public TObject GameAsset => gameAsset;
        public event System.Action<Object> GetGameAsset;
        protected override void Initialization()
        {
            LoadGameObjectAsync();
        }
        protected virtual void LoadGameObjectAsync()
        {
            handle = Addressables.LoadAssetAsync<TObject>(Address);
            handle.Completed += Handle_Completed;
        }
        private void Handle_Completed(AsyncOperationHandle<TObject> operation)
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                gameAsset = operation.Result;
                GetGameAsset?.Invoke(gameAsset);
            } else
            {
                Debug.LogError($"Asset for {Address} failed to load.");
            }
        }
        protected override void UnRegister()
        {
            Addressables.Release(handle);
        }
    }
}
