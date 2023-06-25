using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
namespace AJ.Generic.Tools
{
    public abstract class GameObjectManager : BaseObjectManager
    {
        protected abstract InstantiationParameters CreateParameters();
        public virtual void LoadGameObjectAsync(Action<GameObject> GetGameObject, bool isDisable = true)
        {
            var instantiationParameters = CreateParameters();
            var asyncOperationHandle = Addressables.InstantiateAsync(Address, instantiationParameters);
            asyncOperationHandle.Completed += operation => {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    var result = operation.Result;
                    result.AddComponent<GameObjectRelease>();
                    result.SetActive(isDisable);
                    GetGameObject.Invoke(result);
                } else
                {
                    Debug.LogError($"Asset for {Address} failed to load.");
                }
            };
        }
        public virtual void LoadGameObjectAsync(
            Action<GameObject> GetGameObject, InstantiationParameters parameters, bool isDisable = true)
        {
            var instantiationParameters = parameters;
            var asyncOperationHandle = Addressables.InstantiateAsync(Address, instantiationParameters);
            asyncOperationHandle.Completed += operation => {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    var result = operation.Result;
                    result.AddComponent<GameObjectRelease>();
                    result.SetActive(isDisable);
                    GetGameObject.Invoke(result);
                } else
                {
                    Debug.LogError($"Asset for {Address} failed to load.");
                }
            };
        }
        public virtual void LoadGameObjectAsync(string Address,
            Action<GameObject> GetGameObject, InstantiationParameters parameters, bool isDisable = true)
        {
            var instantiationParameters = parameters;
            var asyncOperationHandle = Addressables.InstantiateAsync(Address, instantiationParameters);
            asyncOperationHandle.Completed += operation => {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    var result = operation.Result;
                    result.AddComponent<GameObjectRelease>();
                    result.SetActive(isDisable);
                    GetGameObject.Invoke(result);
                } else
                {
                    Debug.LogError($"Asset for {Address} failed to load.");
                }
            };
        }
    }
}
